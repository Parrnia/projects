using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Onyx.Application.Common.Exceptions;
using Onyx.Application.Settings;
using Onyx.Domain.Entities.LayoutsCluster.HeaderCluster;
using Onyx.Domain.Entities.ProductsCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductAttributesCluster;
using Onyx.Domain.Entities.ProductsCluster.ProductOptionsCluster.Value;
using Onyx.Domain.Entities.RequestsCluster;
using Onyx.Domain.Entities.UserProfilesCluster;
using Onyx.Domain.Enums;
using Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster;
using Onyx.Infrastructure.Persistence;
using Brand = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.BrandsCluster.Brand;
using DbVehicleBrand = Onyx.Domain.Entities.BrandsCluster.VehicleBrand;
using DbProductBrand = Onyx.Domain.Entities.BrandsCluster.ProductBrand;
using VehicleType = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.BrandsCluster.VehicleType;
using DbFamily = Onyx.Domain.Entities.BrandsCluster.Family;
using Vehicle = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.BrandsCluster.Vehicle;
using DbModel = Onyx.Domain.Entities.BrandsCluster.Model;
using VehicleModel = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.BrandsCluster.VehicleModel;
using DbKind = Onyx.Domain.Entities.BrandsCluster.Kind;
using PartGroup = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.CategoriesCluster.PartGroup;
using DbProductCategory = Onyx.Domain.Entities.CategoriesCluster.ProductCategory;
using CostType = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.InfoCluster.CostType;
using DbCostType = Onyx.Domain.Entities.InfoCluster.CostType;
using Country = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.InfoCluster.Country;
using DbCountry = Onyx.Domain.Entities.InfoCluster.Country;
using CountingUnitType = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster.CountingUnitType;
using DbCountingUnitType = Onyx.Domain.Entities.ProductsCluster.CountingUnitType;
using CountingUnit = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster.CountingUnit;
using DbCountingUnit = Onyx.Domain.Entities.ProductsCluster.CountingUnit;
using PartStatus = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster.PartStatus;
using DbProductStatus = Onyx.Domain.Entities.ProductsCluster.ProductStatus;
using PartType = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster.PartType;
using DbProductType = Onyx.Domain.Entities.ProductsCluster.ProductType;
using Provider = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster.Provider;
using DbProvider = Onyx.Domain.Entities.ProductsCluster.Provider;
using Part = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster.Part;
using DbProduct = Onyx.Domain.Entities.ProductsCluster.Product;
using PartVehicleModel = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster.PartVehicleModel;
using DbProductKind = Onyx.Domain.Entities.ProductsCluster.ProductKind;
using JsonSerializer = System.Text.Json.JsonSerializer;
using ProductCount = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster.ProductCount;
using ProductPrice = Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.ProductsCluster.ProductPrice;
using System.Text.RegularExpressions;
using Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData._7SoftEntities.BrandsCluster;

namespace Onyx.Infrastructure.BackgroundJobs.Features.Handlers.SyncData;
public class DataMigrationHandler
{
    private readonly ApplicationDbContext _context;
    private string? _accessToken;
    private readonly ApplicationSettings _applicationSettings;
    private readonly SharedService _sharedService;

    public DataMigrationHandler(
        ApplicationDbContext context,
        IOptions<ApplicationSettings> applicationSettings,
        SharedService sharedService)
    {
        _context = context;
        _applicationSettings = applicationSettings.Value;
        _sharedService = sharedService;
    }

    #region MapData

    public async Task<bool> MapData()
    {
        _accessToken = await _sharedService.Authenticate();
        MapCustomerTypes();

        var productCategories = await MapPartGroups();
        var costTypes = await MapCostTypes();
        var countries = await MapCountries();
        var productStatusList = await MapPartStatuses();
        var productTypes = await MapPartTypes();
        var providers = await MapProviders();
        var countingUnitTypes = await MapCountingUnitTypes();

        await _context.SaveChangesAsync();

        var productBrands = await MapProductBrands();
        var vehicleBrands = await MapVehicleBrands();
        var countingUnits = await MapCountingUnits(countingUnitTypes);

        await _context.SaveChangesAsync();


        var families = await MapVehicleTypes(vehicleBrands);

        await _context.SaveChangesAsync();


        var models = await MapVehicles(families);

        await _context.SaveChangesAsync();


        var kinds = await MapVehicleModels(models);
        var products = await MapParts(providers, productCategories, countries, productBrands, countingUnits,
            productTypes, productStatusList);

        await _context.SaveChangesAsync();

        var allProducts = await _context.Products
            .ToListAsync();

        var productKinds = await MapPartVehicleModels(allProducts, kinds);

        await _context.SaveChangesAsync();

        await MapDisplayNames();


        await _context.SaveChangesAsync();



        return true;
    }
    #region CustomerType

    public void MapCustomerTypes()
    {
        foreach (var state in Enum.GetValues(typeof(CustomerTypeEnum)))
        {
            if (_context.CustomerTypes.All(c => c.CustomerTypeEnum != (CustomerTypeEnum)state))
            {
                _context.CustomerTypes.Add(new CustomerType()
                {
                    CustomerTypeEnum = (CustomerTypeEnum)state,
                    DiscountPercent = 0
                });
            }
        }
    }
    #endregion
    #region VehicleBrand
    public DbVehicleBrand MapVehicleBrand(Brand brand, DbVehicleBrand? existingBrand)
    {
        var brandOnyx = existingBrand ?? new DbVehicleBrand();

        brandOnyx.Related7SoftBrandId = brand.UniqueId;
        brandOnyx.Code = brand.Code;

        return brandOnyx;
    }
    public async Task<List<DbVehicleBrand>> MapVehicleBrands()
    {
        var response = await GetData("Brand/GetAll", RequestType.Brand);
        var brands = new List<Brand>();
        if (response != null)
        {
            var json = await response.Content.ReadAsStringAsync();
            brands = JsonConvert.DeserializeObject<List<Brand>>(json);
            brands = brands?.Where(c => c.BrandTypeId == Guid.Parse("C43B6519-6171-456C-997B-9798334E0801")).ToList();
        }

        var brandsOnyx = new List<DbVehicleBrand>();

        var dbBrands = await _context.VehicleBrands.ToListAsync();
        foreach (var brand in brands!)
        {
            var existingEntity = dbBrands.FirstOrDefault(e => e.Related7SoftBrandId == brand.UniqueId);
            var brandOnyx = MapVehicleBrand(brand, existingEntity);

            var haveDuplicateInputName = brands
                .Where(c => c.UniqueId != brand.UniqueId)
                .Any(c => Compare(c.BrandEnglishName, brand.BrandEnglishName));
            var duplicateNames = dbBrands
                .Where(c => c.Related7SoftBrandId != brand.UniqueId)
                .Where(c => c.Name.ToLower().Contains(brand.BrandEnglishName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateNames.Count > 0 || haveDuplicateInputName)
            {
                brandOnyx.Name = brand.BrandEnglishName + brand.Code;
            }
            else
            {
                brandOnyx.Name = brand.BrandEnglishName;
            }

            var haveDuplicateInputLocalizedName = brands
                .Where(c => c.UniqueId != brand.UniqueId)
                .Any(c => Compare(c.BrandLocalizedName, brand.BrandLocalizedName));
            var duplicateLocalizedNames = dbBrands
                .Where(c => c.Related7SoftBrandId != brand.UniqueId)
                .Where(c => c.LocalizedName.ToLower().Contains(brand.BrandLocalizedName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateLocalizedNames.Count > 0 || haveDuplicateInputLocalizedName)
            {
                brandOnyx.LocalizedName = brand.BrandLocalizedName + brand.Code;
                brandOnyx.Slug = brandOnyx.LocalizedName.ToLower().Replace(' ', '-');
            }
            else
            {
                brandOnyx.LocalizedName = brand.BrandLocalizedName;
                brandOnyx.Slug = brandOnyx.LocalizedName.ToLower().Replace(' ', '-');
            }

            if (existingEntity == null)
            {
                await _context.VehicleBrands.AddAsync(brandOnyx);
            }
            brandsOnyx.Add(brandOnyx);
        }


        //for remove not thick brands
        var brandsToRemove = _context.VehicleBrands
            .Where(p => !brandsOnyx.Select(c => c.Related7SoftBrandId)
                .Contains(p.Related7SoftBrandId)).ToList();
        _context.VehicleBrands.RemoveRange(brandsToRemove);


        return brandsOnyx;
    }
    #endregion
    #region ProductBrand
    public DbProductBrand MapProductBrand(Brand brand, DbProductBrand? existingBrand)
    {
        var brandOnyx = existingBrand ?? new DbProductBrand();

        brandOnyx.Related7SoftBrandId = brand.UniqueId;
        brandOnyx.Code = brand.Code;

        return brandOnyx;
    }
    public async Task<List<DbProductBrand>> MapProductBrands()
    {
        var response = await GetData("Brand/GetAll", RequestType.Brand);
        var brands = new List<Brand>();
        if (response != null)
        {
            var json = await response.Content.ReadAsStringAsync();
            brands = JsonConvert.DeserializeObject<List<Brand>>(json);
            brands = brands?.Where(c => c.BrandTypeId == Guid.Parse("0C9AE8FD-B2CF-42DB-B52E-15A1D4273EB3")).ToList();
        }

        var brandsOnyx = new List<DbProductBrand>();
        var dbBrands = await _context.ProductBrands.ToListAsync();
        foreach (var brand in brands!)
        {
            var existingEntity = dbBrands.FirstOrDefault(e => e.Related7SoftBrandId == brand.UniqueId);
            var brandOnyx = MapProductBrand(brand, existingEntity);

            var haveDuplicateInputName = brands
                .Where(c => c.UniqueId != brand.UniqueId)
                .Any(c => Compare(c.BrandEnglishName, brand.BrandEnglishName));
            var duplicateNames = dbBrands
                .Where(c => c.Related7SoftBrandId != brand.UniqueId)
                .Where(c => c.Name.ToLower().Contains(brand.BrandEnglishName?.ToLower() ?? string.Empty, StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateNames.Count > 0 || haveDuplicateInputName)
            {
                brandOnyx.Name = brand.BrandEnglishName + brand.Code;
            }
            else
            {
                brandOnyx.Name = brand.BrandEnglishName;
            }

            var haveDuplicateInputLocalizedName = brands
                .Where(c => c.UniqueId != brand.UniqueId)
                .Any(c => Compare(c.BrandLocalizedName, brand.BrandLocalizedName));
            var duplicateLocalizedNames = dbBrands
                .Where(c => c.Related7SoftBrandId != brand.UniqueId)
                .Where(c => c.LocalizedName.ToLower().Contains(brand.BrandLocalizedName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateLocalizedNames.Count > 0 || haveDuplicateInputLocalizedName)
            {
                brandOnyx.LocalizedName = brand.BrandLocalizedName + brand.Code;
                brandOnyx.Slug = brandOnyx.LocalizedName.ToLower().Replace(' ', '-');
            }
            else
            {
                brandOnyx.LocalizedName = brand.BrandLocalizedName;
                brandOnyx.Slug = brandOnyx.LocalizedName.ToLower().Replace(' ', '-');
            }

            if (existingEntity == null)
            {
                await _context.ProductBrands.AddAsync(brandOnyx);
            }
            brandsOnyx.Add(brandOnyx);
        }


        //for remove not thick brands
        var brandsToRemove = _context.ProductBrands
            .Where(p => !brandsOnyx.Select(c => c.Related7SoftBrandId)
                .Contains(p.Related7SoftBrandId)).ToList();
        _context.ProductBrands.RemoveRange(brandsToRemove);


        return brandsOnyx;
    }
    #endregion
    #region VehicleType
    public DbFamily MapVehicleType(VehicleType vehicleType, List<DbVehicleBrand> brands, DbFamily? existingFamily)
    {
        var familyOnyx = existingFamily ?? new DbFamily();

        familyOnyx.Related7SoftFamilyId = vehicleType.UniqueId;
        familyOnyx.VehicleBrandId = brands.SingleOrDefault(c => c.Related7SoftBrandId == vehicleType.BrandId)!.Id;

        return familyOnyx;
    }
    public async Task<List<DbFamily>> MapVehicleTypes(List<DbVehicleBrand> brands)
    {
        var response = await GetData("VehicleType/GetAll", RequestType.VehicleType);
        var vehicleTypes = new List<VehicleType>();
        if (response != null)
        {
            var json = await response.Content.ReadAsStringAsync();
            vehicleTypes = JsonConvert.DeserializeObject<List<VehicleType>>(json);
        }

        var familiesOnyx = new List<DbFamily>();
        var dbFamilies = await _context.Families.ToListAsync();
        foreach (var vehicleType in vehicleTypes!)
        {
            var existingEntity = dbFamilies
                .FirstOrDefault(e => e.Related7SoftFamilyId == vehicleType.UniqueId);
            var familyOnyx = MapVehicleType(vehicleType, brands, existingEntity);

            var vehicleTypeEnglishName = vehicleType.VehicleTypeEnglishName ?? vehicleType.VehicleTypeLocalizedName;
            var haveDuplicateInputName = vehicleTypes
                .Where(c => c.UniqueId != vehicleType.UniqueId)
                .Any(c => Compare(c.VehicleTypeEnglishName ?? c.VehicleTypeLocalizedName, vehicleTypeEnglishName));
            var duplicateNames = dbFamilies
                .Where(c => c.Related7SoftFamilyId != vehicleType.UniqueId)
                .Where(c => c.Name.ToLower().Contains(vehicleTypeEnglishName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateNames.Count > 0 || haveDuplicateInputName)
            {
                familyOnyx.Name = (vehicleType.VehicleTypeEnglishName ?? vehicleType.VehicleTypeLocalizedName) + vehicleType.Code;
            }
            else
            {
                familyOnyx.Name = vehicleType.VehicleTypeEnglishName ?? vehicleType.VehicleTypeLocalizedName;
            }

            var haveDuplicateInputLocalizedName = vehicleTypes
                .Where(c => c.UniqueId != vehicleType.UniqueId)
                .Any(c => Compare(c.VehicleTypeLocalizedName, vehicleType.VehicleTypeLocalizedName));
            var duplicateLocalizedNames = dbFamilies
                .Where(c => c.Related7SoftFamilyId != vehicleType.UniqueId)
                .Where(c => c.LocalizedName.ToLower().Contains(vehicleType.VehicleTypeLocalizedName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateLocalizedNames.Count > 0 || haveDuplicateInputLocalizedName)
            {
                familyOnyx.LocalizedName = vehicleType.VehicleTypeLocalizedName + vehicleType.Code;
            }
            else
            {
                familyOnyx.LocalizedName = vehicleType.VehicleTypeLocalizedName;
            }

            if (existingEntity == null)
            {
                await _context.Families.AddAsync(familyOnyx);
            }
            familiesOnyx.Add(familyOnyx);
        }


        //for remove not thick families
        var familiesToRemove = _context.Families
            .Where(p => !familiesOnyx.Select(c => c.Related7SoftFamilyId)
                .Contains(p.Related7SoftFamilyId)).ToList();
        _context.Families.RemoveRange(familiesToRemove);


        return familiesOnyx;
    }
    #endregion
    #region Vehicle
    public DbModel MapVehicle(Vehicle vehicle, List<DbFamily> families, DbModel? existingModel)
    {
        var modelOnyx = existingModel ?? new DbModel();

        modelOnyx.Related7SoftModelId = vehicle.UniqueId;
        modelOnyx.FamilyId = families.SingleOrDefault(c => c.Related7SoftFamilyId == vehicle.VehicleTypeId)!.Id;

        return modelOnyx;
    }
    public async Task<List<DbModel>> MapVehicles(List<DbFamily> families)
    {
        var response = await GetData("Vehicle/GetAll", RequestType.Vehicle);
        var vehicles = new List<Vehicle>();
        if (response != null)
        {
            var json = await response.Content.ReadAsStringAsync();
            vehicles = JsonConvert.DeserializeObject<List<Vehicle>>(json);
        }

        var modelsOnyx = new List<DbModel>();

        var dbModels = await _context.Models.ToListAsync();
        foreach (var vehicle in vehicles!)
        {
            var existingEntity = dbModels
                .FirstOrDefault(e => e.Related7SoftModelId == vehicle.UniqueId);
            var modelOnyx = MapVehicle(vehicle, families, existingEntity);

            //var vehicleEnglishName = vehicle.VehicleEnglishName ?? vehicle.VehicleLocalizedName;
            //var nameCounter = modelsOnyx
            //    .Where(c => c.Related7SoftModelId != vehicle.UniqueId)
            //    .Where(c => c.Name.ToLower().Contains(vehicleEnglishName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList().Count;
            //var duplicateNames = dbModels
            //    .Where(c => c.Related7SoftModelId != vehicle.UniqueId)
            //    .Where(c => c.Name.ToLower().Contains(vehicleEnglishName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            //if (duplicateNames.Count > 0 || nameCounter > 0)
            //{
            //    modelOnyx.Name = (vehicle.VehicleEnglishName ?? vehicle.VehicleLocalizedName) + (duplicateNames.Count + nameCounter);
            //}
            //else
            //{
                modelOnyx.Name = vehicle.VehicleEnglishName ?? vehicle.VehicleLocalizedName;
            //}

            //var localizedNameCounter = modelsOnyx
            //    .Where(c => c.Related7SoftModelId != vehicle.UniqueId)
            //    .Where(c => c.LocalizedName.ToLower().Contains(vehicle.VehicleLocalizedName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList().Count;
            //var duplicateLocalizedNames = dbModels
            //    .Where(c => c.Related7SoftModelId != vehicle.UniqueId)
            //    .Where(c => c.LocalizedName.ToLower().Contains(vehicle.VehicleLocalizedName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            //if (duplicateLocalizedNames.Count > 0 || localizedNameCounter > 0)
            //{
            //    modelOnyx.LocalizedName = vehicle.VehicleLocalizedName + (duplicateLocalizedNames.Count + localizedNameCounter);
            //}
            //else
            //{
                modelOnyx.LocalizedName = vehicle.VehicleLocalizedName;
            //}

            if (existingEntity == null)
            {
                await _context.Models.AddAsync(modelOnyx);
            }
            modelsOnyx.Add(modelOnyx);
        }

        //for remove not thick models
        var modelsToRemove = _context.Models
            .Where(p => !modelsOnyx.Select(c => c.Related7SoftModelId)
                .Contains(p.Related7SoftModelId)).ToList();
        _context.Models.RemoveRange(modelsToRemove);

        return modelsOnyx;
    }
    #endregion
    #region VehicleModel
    public DbKind MapVehicleModel(VehicleModel vehicleModel, List<DbModel> models, DbKind? existingKind)
    {
        var kindOnyx = existingKind ?? new DbKind();

        kindOnyx.Related7SoftKindId = vehicleModel.UniqueId;
        kindOnyx.ModelId = models.SingleOrDefault(c => c.Related7SoftModelId == vehicleModel.VehicleId)!.Id;

        return kindOnyx;
    }
    public async Task<List<DbKind>> MapVehicleModels(List<DbModel> models)
    {
        var response = await GetData("VehicleModel/GetAll", RequestType.VehicleModel);
        var vehicleModels = new List<VehicleModel>();
        if (response != null)
        {
            var json = await response.Content.ReadAsStringAsync();
            vehicleModels = JsonConvert.DeserializeObject<List<VehicleModel>>(json);
        }

        var kindsOnyx = new List<DbKind>();
        var dbKinds = await _context.Kinds.ToListAsync();
        foreach (var vehicleModel in vehicleModels!)
        {
            var existingEntity = dbKinds
                .FirstOrDefault(e => e.Related7SoftKindId == vehicleModel.UniqueId);
            var kindOnyx = MapVehicleModel(vehicleModel, models, existingEntity);

            //var vehicleModelEnglishName = vehicleModel.VehicleModelEnglishName ?? vehicleModel.VehicleModelLocalizeName;
            //var nameCounter = kindsOnyx
            //    .Where(c => c.Related7SoftKindId != vehicleModel.UniqueId)
            //    .Where(c => c.Name.ToLower().Contains(vehicleModelEnglishName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList().Count;
            //var duplicateNames = dbKinds
            //    .Where(c => c.Related7SoftKindId != vehicleModel.UniqueId)
            //    .Where(c => c.Name.ToLower().Contains(vehicleModelEnglishName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            //if (duplicateNames.Count > 0 || nameCounter > 0)
            //{
            //    kindOnyx.Name = vehicleModel.VehicleModelEnglishName + (duplicateNames.Count + nameCounter);
            //}
            //else
            //{
                kindOnyx.Name = vehicleModel.VehicleModelEnglishName ?? vehicleModel.VehicleModelLocalizeName;
            //}

            //var localizedNameCounter = kindsOnyx
            //    .Where(c => c.Related7SoftKindId != vehicleModel.UniqueId)
            //    .Where(c => c.LocalizedName.ToLower().Contains(vehicleModel.VehicleModelLocalizeName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList().Count;
            //var duplicateLocalizedNames = dbKinds
            //    .Where(c => c.Related7SoftKindId != vehicleModel.UniqueId)
            //    .Where(c => c.LocalizedName.ToLower().Contains(vehicleModel.VehicleModelLocalizeName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            //if (duplicateLocalizedNames.Count > 0 || localizedNameCounter > 0)
            //{
            //    kindOnyx.LocalizedName = vehicleModel.VehicleModelLocalizeName + (duplicateLocalizedNames.Count + localizedNameCounter);
            //}
            //else
            //{
                kindOnyx.LocalizedName = vehicleModel.VehicleModelLocalizeName;
            //}
            if (existingEntity == null)
            {
                await _context.Kinds.AddAsync(kindOnyx);
            }
            kindsOnyx.Add(kindOnyx);
        }


        //for remove not thick kinds
        var kindsToRemove = _context.Kinds
            .Where(p => !kindsOnyx.Select(c => c.Related7SoftKindId)
                .Contains(p.Related7SoftKindId)).ToList();
        _context.Kinds.RemoveRange(kindsToRemove);


        return kindsOnyx;
    }
    #endregion
    #region PartGroup
    public DbProductCategory MapParentPartGroup(PartGroup partGroup, DbProductCategory? existingProductCategory)
    {
        var productCategoryOnyx = existingProductCategory ?? new DbProductCategory() { IsActive = false };

        productCategoryOnyx.CategoryType = 0;
        productCategoryOnyx.Related7SoftProductCategoryId = partGroup.UniqueId;
        productCategoryOnyx.Code = partGroup.Code;
        productCategoryOnyx.ProductCategoryNo = partGroup.PartGroupNo;
        productCategoryOnyx.ProductParentCategoryId = null;

        return productCategoryOnyx;
    }
    public DbProductCategory MapChildPartGroup(PartGroup partGroup, List<DbProductCategory> productCategories, DbProductCategory? existingProductCategory)
    {
        var productCategoryOnyx = existingProductCategory ?? new DbProductCategory() { IsActive = false };

        productCategoryOnyx.CategoryType = 0;
        productCategoryOnyx.Related7SoftProductCategoryId = partGroup.UniqueId;
        productCategoryOnyx.Code = partGroup.Code;
        productCategoryOnyx.ProductCategoryNo = partGroup.PartGroupNo;
        productCategoryOnyx.ProductParentCategoryId = productCategories.SingleOrDefault(c => c.Related7SoftProductCategoryId == partGroup.ParentPartGroupId)!.Id;


        return productCategoryOnyx;
    }
    public async Task<List<DbProductCategory>> MapPartGroups()
    {
        var response = await GetData("Part/GetAllPartGroups", RequestType.PartGroup);
        var partGroups = new List<PartGroup>();
        if (response != null)
        {
            var json = await response.Content.ReadAsStringAsync();
            partGroups = JsonConvert.DeserializeObject<List<PartGroup>>(json);
        }

        var productCategoriesOnyx = new List<DbProductCategory>();
        var links = new List<Link>();
        var firstLayerPartGroups = partGroups.Where(g => g.ParentPartGroupId == null).ToList();
        var dbProductCategories = await _context.ProductCategories.ToListAsync();
        foreach (var partGroup in firstLayerPartGroups)
        {
            var existingEntity = dbProductCategories
                .FirstOrDefault(e => e.Related7SoftProductCategoryId == partGroup.UniqueId);
            var productCategoryOnyx = MapParentPartGroup(partGroup, existingEntity);

            var haveDuplicateInputName = firstLayerPartGroups
                .Where(c => c.UniqueId != partGroup.UniqueId)
                .Any(c => Compare(c.PartGroupName, partGroup.PartGroupName));
            var duplicateNames = dbProductCategories
                .Where(c => c.Related7SoftProductCategoryId != partGroup.UniqueId)
                .Where(c => c.Name.ToLower().Contains(partGroup.PartGroupName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateNames.Count > 0 || haveDuplicateInputName)
            {
                productCategoryOnyx.Name = partGroup.PartGroupName + partGroup.Code;
            }
            else
            {
                productCategoryOnyx.Name = partGroup.PartGroupName;
            }

            var haveDuplicateInputLocalizedName = firstLayerPartGroups
                .Where(c => c.UniqueId != partGroup.UniqueId)
                .Any(c => Compare(c.PartGroupLocalizedName, partGroup.PartGroupLocalizedName));
            var duplicateLocalizedNames = dbProductCategories
                .Where(c => c.Related7SoftProductCategoryId != partGroup.UniqueId)
                .Where(c => c.LocalizedName.ToLower().Contains(partGroup.PartGroupLocalizedName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateLocalizedNames.Count > 0 || haveDuplicateInputLocalizedName)
            {
                productCategoryOnyx.LocalizedName = partGroup.PartGroupLocalizedName + partGroup.Code;
                productCategoryOnyx.Slug = productCategoryOnyx.LocalizedName.ToLower().Replace(' ', '-');
            }
            else
            {
                productCategoryOnyx.LocalizedName = partGroup.PartGroupLocalizedName;
                productCategoryOnyx.Slug = productCategoryOnyx.LocalizedName.ToLower().Replace(' ', '-');
            }


            if (existingEntity == null)
            {
                await _context.ProductCategories.AddAsync(productCategoryOnyx);
            }
            productCategoriesOnyx.Add(productCategoryOnyx);

        }
        await _context.SaveChangesAsync();


        var notFirstLayerPartGroups = partGroups.Except(firstLayerPartGroups).ToList();

        while (notFirstLayerPartGroups.Count > 0)
        {
            var currentLayerPartGroups = notFirstLayerPartGroups.Where(c =>
                productCategoriesOnyx.Select(v => v.Related7SoftProductCategoryId).Contains(c.ParentPartGroupId)).ToList();

            foreach (var partGroup in currentLayerPartGroups)
            {
                var existingEntity = dbProductCategories
                    .FirstOrDefault(e => e.Related7SoftProductCategoryId == partGroup.UniqueId);
                var productCategoryOnyx = MapChildPartGroup(partGroup, productCategoriesOnyx, existingEntity);

                var nameCounterInner = productCategoriesOnyx
                    .Where(c => c.Related7SoftProductCategoryId != partGroup.UniqueId)
                    .Where(c => c.Name.ToLower().Contains(partGroup.PartGroupName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList().Count;
                var duplicateNamesInDb = dbProductCategories
                    .Where(c => c.Related7SoftProductCategoryId != partGroup.UniqueId)
                    .Where(c => c.Name.ToLower().Contains(partGroup.PartGroupName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
                if (duplicateNamesInDb.Count > 0 || nameCounterInner > 0)
                {
                    productCategoryOnyx.Name = partGroup.PartGroupName + (duplicateNamesInDb.Count + nameCounterInner);
                }
                else
                {
                    productCategoryOnyx.Name = partGroup.PartGroupName;
                }

                var localizedNameCounterInner = productCategoriesOnyx
                    .Where(c => c.Related7SoftProductCategoryId != partGroup.UniqueId)
                    .Where(c => c.LocalizedName.ToLower().Contains(partGroup.PartGroupLocalizedName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList().Count;
                var duplicateLocalizedNamesInDb = dbProductCategories
                    .Where(c => c.Related7SoftProductCategoryId != partGroup.UniqueId)
                    .Where(c => c.LocalizedName.ToLower().Contains(partGroup.PartGroupLocalizedName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
                if (duplicateLocalizedNamesInDb.Count > 0 || localizedNameCounterInner > 0)
                {
                    productCategoryOnyx.LocalizedName = partGroup.PartGroupLocalizedName + (duplicateLocalizedNamesInDb.Count + localizedNameCounterInner);
                    productCategoryOnyx.Slug = productCategoryOnyx.LocalizedName.ToLower().Replace(' ', '-');
                }
                else
                {
                    productCategoryOnyx.LocalizedName = partGroup.PartGroupLocalizedName;
                    productCategoryOnyx.Slug = productCategoryOnyx.LocalizedName.ToLower().Replace(' ', '-');
                }

                if (existingEntity == null)
                {
                    await _context.ProductCategories.AddAsync(productCategoryOnyx);
                }
                productCategoriesOnyx.Add(productCategoryOnyx);
            }

            notFirstLayerPartGroups = notFirstLayerPartGroups.Except(currentLayerPartGroups).ToList();

            await _context.SaveChangesAsync();
        }




        var dbLinks = await _context.Links.ToListAsync();
        foreach (var productCategory in productCategoriesOnyx.Where(c => c.ProductParentCategory == null))
        {
            var dbLink = dbLinks.FirstOrDefault(c => c.RelatedProductCategoryId == productCategory.Id);
            var link = dbLink ?? new Link() { IsActive = false };

            link.Title = productCategory.LocalizedName;
            link.Url = "/shop/category/" + productCategory.Slug + "/products";
            link.RelatedProductCategoryId = productCategory.Id;

            if (dbLink == null)
            {
                links.Add(link);
                _context.Links.Add(link);
            }

        }
        foreach (var productCategory in productCategoriesOnyx.Where(c => c.ProductParentCategory != null && c.ProductParentCategory.ProductParentCategory == null))
        {
            var dbLink = dbLinks.FirstOrDefault(c => c.RelatedProductCategoryId == productCategory.Id);
            var link = dbLink ?? new Link() { IsActive = false };

            var parentLink = links
                .SingleOrDefault(c => c.RelatedProductCategoryId == productCategory.ProductParentCategory.Id);

            link.Title = productCategory.LocalizedName;
            link.Url = "/shop/category/" + productCategory.Slug + "/products";
            link.RelatedProductCategoryId = productCategory.Id;
            link.ParentLink = parentLink;

            if (dbLink == null)
            {
                links.Add(link);
                _context.Links.Add(link);
            }
        }
        foreach (var productCategory in productCategoriesOnyx.Where(c => c.ProductParentCategory != null && c.ProductParentCategory.ProductParentCategory != null && c.ProductParentCategory.ProductParentCategory.ProductParentCategory == null))
        {
            var dbLink = dbLinks.FirstOrDefault(c => c.RelatedProductCategoryId == productCategory.Id);
            var link = dbLink ?? new Link() { IsActive = false };

            var parentLink = links
                .SingleOrDefault(c => c.RelatedProductCategoryId == productCategory.ProductParentCategory.Id);

            link.Title = productCategory.LocalizedName;
            link.Url = "/shop/category/" + productCategory.Slug + "/products";
            link.RelatedProductCategoryId = productCategory.Id;
            link.ParentLink = parentLink;

            if (dbLink == null)
            {
                links.Add(link);
                _context.Links.Add(link);
            }
        }



        //for remove not thick productCategories
        var productCategoriesToRemove = _context.ProductCategories
            .Where(p => !productCategoriesOnyx.Select(c => c.Related7SoftProductCategoryId)
                .Contains(p.Related7SoftProductCategoryId)).ToList();
        _context.ProductCategories.RemoveRange(productCategoriesToRemove);


        //for remove not thick links
        var linksToRemove = _context.Links
            .Where(p => !links.Select(c => c.RelatedProductCategoryId)
                .Contains(p.RelatedProductCategoryId)).ToList();
        _context.Links.RemoveRange(linksToRemove);



        await _context.SaveChangesAsync();

        return productCategoriesOnyx;
    }

    #endregion
    #region CostType
    public DbCostType MapCostType(CostType costType, DbCostType? existingCostType)
    {
        var costTypeOnyx = existingCostType ?? new DbCostType() { IsActive = false };

        costTypeOnyx.Value = costType.Value;

        switch (costType.Value)
        {
            case "1":
                costTypeOnyx.CostTypeEnum = CostTypeEnum.Part;
                break;
            case "2":
                costTypeOnyx.CostTypeEnum = CostTypeEnum.Delivery;
                break;
        }
        return costTypeOnyx;
    }
    public async Task<List<DbCostType>> MapCostTypes()
    {
        var response = await GetData("Part/GetAllItemTypes", RequestType.CostType);
        var costTypes = new List<CostType>();
        if (response != null)
        {
            var json = await response.Content.ReadAsStringAsync();
            costTypes = JsonConvert.DeserializeObject<List<CostType>>(json);
        }

        var costTypesOnyx = new List<DbCostType>();
        var dbCostTypes = await _context.CostTypes.ToListAsync();
        foreach (var costType in costTypes!)
        {
            var existingEntity = dbCostTypes
                .FirstOrDefault(e => e.Value == costType.Value);
            var costTypeOnyx = MapCostType(costType, existingEntity);

            var haveDuplicateInputName = costTypes
                .Where(c => c.Value != costType.Value)
                .Any(c => Compare(c.Text, costType.Text));
            var duplicateNames = dbCostTypes
                .Where(c => c.Value != costType.Value)
                .Where(c => c.Text.ToLower().Contains(costType.Text.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateNames.Count > 0 || haveDuplicateInputName)
            {
                costTypeOnyx.Text = costType.Text + costType.Value;
            }
            else
            {
                costTypeOnyx.Text = costType.Text;
            }

            if (existingEntity == null)
            {
                await _context.CostTypes.AddAsync(costTypeOnyx);
            }
            costTypesOnyx.Add(costTypeOnyx);
        }

        //for remove not thick costTypes
        var costTypesToRemove = _context.CostTypes
            .Where(p => !costTypesOnyx.Select(c => c.Value)
                .Contains(p.Value)).ToList();
        _context.CostTypes.RemoveRange(costTypesToRemove);


        return costTypesOnyx;
    }
    #endregion
    #region Country
    public DbCountry MapCountry(Country country, DbCountry? existingCountry)
    {
        var countryOnyx = existingCountry ?? new DbCountry() { IsActive = false };

        countryOnyx.Related7SoftCountryId = country.UniqueId;
        countryOnyx.Code = country.Code;
        return countryOnyx;
    }
    public async Task<List<DbCountry>> MapCountries()
    {
        var response = await GetData("Country/GetAll", RequestType.Country);
        var countries = new List<Country>();
        if (response != null)
        {
            var json = await response.Content.ReadAsStringAsync();
            countries = JsonConvert.DeserializeObject<List<Country>>(json);
        }

        var countriesOnyx = new List<DbCountry>();
        var dbCountries = await _context.Countries.ToListAsync();
        foreach (var country in countries!)
        {
            var existingEntity = dbCountries
                .FirstOrDefault(e => e.Related7SoftCountryId == country.UniqueId);
            var countryOnyx = MapCountry(country, existingEntity);

            var haveDuplicateInputName = countries
                .Where(c => c.UniqueId != country.UniqueId)
                .Any(c => Compare(c.CountryEnglishName, country.CountryEnglishName));
            var duplicateNames = dbCountries
            .Where(c => c.Related7SoftCountryId != country.UniqueId)
            .Where(c => c.Name.ToLower().Contains(country.CountryEnglishName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateNames.Count > 0 || haveDuplicateInputName)
            {
                countryOnyx.Name = country.CountryEnglishName + country.Code;
            }
            else
            {
                countryOnyx.Name = country.CountryEnglishName;
            }

            var haveDuplicateInputLocalizedName = countries
                .Where(c => c.UniqueId != country.UniqueId)
                .Any(c => Compare(c.CountryLocalizedName, country.CountryLocalizedName));
            var duplicateLocalizedNames = dbCountries
            .Where(c => c.Related7SoftCountryId != country.UniqueId)
            .Where(c => c.LocalizedName.ToLower().Contains(country.CountryLocalizedName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateLocalizedNames.Count > 0 || haveDuplicateInputLocalizedName)
            {
                countryOnyx.LocalizedName = country.CountryLocalizedName + country.Code;
            }
            else
            {
                countryOnyx.LocalizedName = country.CountryLocalizedName;
            }

            if (existingEntity == null)
            {
                await _context.Countries.AddAsync(countryOnyx);
            }
            countriesOnyx.Add(countryOnyx);
        }


        //for remove not thick countries
        var countriesToRemove = _context.Countries
            .Where(p => !countriesOnyx.Select(c => c.Related7SoftCountryId)
                .Contains(p.Related7SoftCountryId)).ToList();
        _context.Countries.RemoveRange(countriesToRemove);


        return countriesOnyx;
    }
    #endregion
    #region CountingUnitType
    public DbCountingUnitType MapCountingUnitType(CountingUnitType countingUnitType, DbCountingUnitType? existingCountingUnitType)
    {
        var countingUnitTypeOnyx = existingCountingUnitType ?? new DbCountingUnitType() { IsActive = false };

        countingUnitTypeOnyx.Related7SoftCountingUnitTypeId = countingUnitType.UniqueId;
        countingUnitTypeOnyx.Code = countingUnitType.Code;

        return countingUnitTypeOnyx;
    }
    public async Task<List<DbCountingUnitType>> MapCountingUnitTypes()
    {
        var response = await GetData("Part/GetAllCountingUnitType", RequestType.CountingUnitType);
        var countingUnitTypes = new List<CountingUnitType>();
        if (response != null)
        {
            var json = await response.Content.ReadAsStringAsync();
            countingUnitTypes = JsonConvert.DeserializeObject<List<CountingUnitType>>(json);
        }

        var countingUnitTypesOnyx = new List<DbCountingUnitType>();
        var dbCountingUnitTypes = await _context.CountingUnitTypes.ToListAsync();
        foreach (var countingUnitType in countingUnitTypes!)
        {
            var existingEntity = dbCountingUnitTypes
                .FirstOrDefault(e => e.Related7SoftCountingUnitTypeId == countingUnitType.UniqueId);
            var countingUnitTypeOnyx = MapCountingUnitType(countingUnitType, existingEntity);


            var haveDuplicateInputName = countingUnitTypes
                .Where(c => c.UniqueId != countingUnitType.UniqueId)
                .Any(c => Compare(c.CountingUnitTypeName, countingUnitType.CountingUnitTypeName));
            var duplicateNames = dbCountingUnitTypes
                .Where(c => c.Related7SoftCountingUnitTypeId != countingUnitType.UniqueId)
                .Where(c => c.Name.ToLower().Contains(countingUnitType.CountingUnitTypeName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateNames.Count > 0 || haveDuplicateInputName)
            {
                countingUnitTypeOnyx.Name = countingUnitType.CountingUnitTypeName + countingUnitType.Code;
            }
            else
            {
                countingUnitTypeOnyx.Name = countingUnitType.CountingUnitTypeName;
            }


            var haveDuplicateInputLocalizedName = countingUnitTypes
                .Where(c => c.UniqueId != countingUnitType.UniqueId)
                .Any(c => Compare(c.CountingUnitTypeLocalizedName, countingUnitType.CountingUnitTypeLocalizedName));
            var duplicateLocalizedNames = dbCountingUnitTypes
                .Where(c => c.Related7SoftCountingUnitTypeId != countingUnitType.UniqueId)
                .Where(c => c.LocalizedName.ToLower().Contains(countingUnitType.CountingUnitTypeLocalizedName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateLocalizedNames.Count > 0 || haveDuplicateInputLocalizedName)
            {
                countingUnitTypeOnyx.LocalizedName = countingUnitType.CountingUnitTypeLocalizedName + countingUnitType.Code;
            }
            else
            {
                countingUnitTypeOnyx.LocalizedName = countingUnitType.CountingUnitTypeLocalizedName;
            }
            if (existingEntity == null)
            {
                await _context.CountingUnitTypes.AddAsync(countingUnitTypeOnyx);
            }
            countingUnitTypesOnyx.Add(countingUnitTypeOnyx);
        }

        //for remove not thick countingUnitTypes
        var countingUnitTypesToRemove = _context.CountingUnitTypes
            .Where(p => !countingUnitTypesOnyx.Select(c => c.Related7SoftCountingUnitTypeId)
                .Contains(p.Related7SoftCountingUnitTypeId)).ToList();
        _context.CountingUnitTypes.RemoveRange(countingUnitTypesToRemove);




        return countingUnitTypesOnyx;
    }
    #endregion
    #region CountingUnit
    public DbCountingUnit MapCountingUnit(CountingUnit countingUnit, List<DbCountingUnitType> countingUnitTypes, DbCountingUnit? existingCountingUnit)
    {
        var countingUnitOnyx = existingCountingUnit ?? new DbCountingUnit() { IsActive = false };

        countingUnitOnyx.Related7SoftCountingUnitId = countingUnit.UniqueId;
        countingUnitOnyx.Code = countingUnit.Code;
        countingUnitOnyx.CountingUnitTypeId = countingUnitTypes.SingleOrDefault(c => c.Code == countingUnit.CountingUnitTypeId)?.Id ?? 1;

        return countingUnitOnyx;
    }
    public async Task<List<DbCountingUnit>> MapCountingUnits(List<DbCountingUnitType> countingUnitTypes)
    {
        var response = await GetData("Part/GetAllCountingUnit", RequestType.CountingUnit);
        var countingUnits = new List<CountingUnit>();
        if (response != null)
        {
            var json = await response.Content.ReadAsStringAsync();
            countingUnits = JsonConvert.DeserializeObject<List<CountingUnit>>(json);
        }

        var countingUnitsOnyx = new List<DbCountingUnit>();
        var dbCountingUnits = await _context.CountingUnits.ToListAsync();
        foreach (var countingUnit in countingUnits!)
        {
            var existingEntity = dbCountingUnits
                .FirstOrDefault(e => e.Related7SoftCountingUnitId == countingUnit.UniqueId);
            var countingUnitOnyx = MapCountingUnit(countingUnit, countingUnitTypes, existingEntity);

            var haveDuplicateInputName = countingUnits
                .Where(c => c.UniqueId != countingUnit.UniqueId)
                .Any(c => Compare(c.CountingUnitName, countingUnit.CountingUnitName));
            var duplicateNames = dbCountingUnits
                .Where(c => c.Related7SoftCountingUnitId != countingUnit.UniqueId)
                .Where(c => c.Name.ToLower().Contains(countingUnit.CountingUnitName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateNames.Count > 0 || haveDuplicateInputName)
            {
                countingUnitOnyx.Name = countingUnit.CountingUnitName + countingUnit.Code;
            }
            else
            {
                countingUnitOnyx.Name = countingUnit.CountingUnitName;
            }

            var haveDuplicateInputLocalizedName = countingUnits
                .Where(c => c.UniqueId != countingUnit.UniqueId)
                .Any(c => Compare(c.CountingUnitLocalizedName, countingUnit.CountingUnitLocalizedName));
            var duplicateLocalizedNames = dbCountingUnits
                .Where(c => c.Related7SoftCountingUnitId != countingUnit.UniqueId)
                .Where(c => c.LocalizedName.ToLower().Contains(countingUnit.CountingUnitLocalizedName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateLocalizedNames.Count > 0 || haveDuplicateInputLocalizedName)
            {
                countingUnitOnyx.LocalizedName = countingUnit.CountingUnitLocalizedName + countingUnit.Code;
            }
            else
            {
                countingUnitOnyx.LocalizedName = countingUnit.CountingUnitLocalizedName;
            }

            if (existingEntity == null)
            {
                await _context.CountingUnits.AddAsync(countingUnitOnyx);
            }
            countingUnitsOnyx.Add(countingUnitOnyx);
        }


        //for remove not thick countingUnits
        var countingUnitsToRemove = _context.CountingUnits
            .Where(p => !countingUnitsOnyx.Select(c => c.Related7SoftCountingUnitId)
                .Contains(p.Related7SoftCountingUnitId)).ToList();
        _context.CountingUnits.RemoveRange(countingUnitsToRemove);


        return countingUnitsOnyx;
    }
    #endregion
    #region PartStatus
    public DbProductStatus MapPartStatus(PartStatus partStatus, DbProductStatus? existingProductStatus)
    {
        var productStatusOnyx = existingProductStatus ?? new DbProductStatus() { IsActive = false };

        productStatusOnyx.Related7SoftProductStatusId = partStatus.UniqueId;
        productStatusOnyx.Code = partStatus.Code;

        return productStatusOnyx;
    }
    public async Task<List<DbProductStatus>> MapPartStatuses()
    {
        var response = await GetData("Part/GetAllPartStatus", RequestType.PartStatus);
        var partStatuses = new List<PartStatus>();
        if (response != null)
        {
            var json = await response.Content.ReadAsStringAsync();
            partStatuses = JsonConvert.DeserializeObject<List<PartStatus>>(json);
        }

        var productStatusesOnyx = new List<DbProductStatus>();
        var dbProductStatuses = await _context.ProductStatuses.ToListAsync();
        foreach (var partStatus in partStatuses!)
        {
            var existingEntity = dbProductStatuses
                .FirstOrDefault(e => e.Related7SoftProductStatusId == partStatus.UniqueId);
            var productStatusOnyx = MapPartStatus(partStatus, existingEntity);

            var haveDuplicateInputName = partStatuses
                .Where(c => c.UniqueId != partStatus.UniqueId)
                .Any(c => Compare(c.PartStatusName, partStatus.PartStatusName));
            var duplicateNames = dbProductStatuses
                .Where(c => c.Related7SoftProductStatusId != partStatus.UniqueId)
                .Where(c => c.Name.ToLower().Contains(partStatus.PartStatusName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateNames.Count > 0 || haveDuplicateInputName)
            {
                productStatusOnyx.Name = partStatus.PartStatusName + partStatus.Code;
            }
            else
            {
                productStatusOnyx.Name = partStatus.PartStatusName;
            }

            var haveDuplicateInputLocalizedName = partStatuses
                .Where(c => c.UniqueId != partStatus.UniqueId)
                .Any(c => Compare(c.PartStatusLocalizedName, partStatus.PartStatusLocalizedName));
            var duplicateLocalizedNames = dbProductStatuses
                .Where(c => c.Related7SoftProductStatusId != partStatus.UniqueId)
                .Where(c => c.LocalizedName.ToLower().Contains(partStatus.PartStatusLocalizedName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateLocalizedNames.Count > 0 || haveDuplicateInputLocalizedName)
            {
                productStatusOnyx.LocalizedName = partStatus.PartStatusLocalizedName + partStatus.Code;
            }
            else
            {
                productStatusOnyx.LocalizedName = partStatus.PartStatusLocalizedName;
            }

            if (existingEntity == null)
            {
                await _context.ProductStatuses.AddAsync(productStatusOnyx);
            }
            productStatusesOnyx.Add(productStatusOnyx);
        }


        //for remove not thick productStatuses
        var productStatusesToRemove = _context.ProductStatuses
            .Where(p => !productStatusesOnyx.Select(c => c.Related7SoftProductStatusId)
                .Contains(p.Related7SoftProductStatusId)).ToList();
        _context.ProductStatuses.RemoveRange(productStatusesToRemove);


        return productStatusesOnyx;
    }
    #endregion
    #region PartType
    public DbProductType MapPartType(PartType partType, DbProductType? existingProductType)
    {
        var productTypeOnyx = existingProductType ?? new DbProductType() { IsActive = false };

        productTypeOnyx.Related7SoftProductTypeId = partType.UniqueId;
        productTypeOnyx.Code = partType.Code;

        return productTypeOnyx;
    }
    public async Task<List<DbProductType>> MapPartTypes()
    {
        var response = await GetData("Part/GetAllPartType", RequestType.PartType);
        var partTypes = new List<PartType>();
        if (response != null)
        {
            var json = await response.Content.ReadAsStringAsync();
            partTypes = JsonConvert.DeserializeObject<List<PartType>>(json);
        }

        var productTypesOnyx = new List<DbProductType>();
        var dbProductTypes = await _context.ProductTypes.ToListAsync();
        foreach (var partType in partTypes!)
        {
            var existingEntity = dbProductTypes
                .FirstOrDefault(e => e.Related7SoftProductTypeId == partType.UniqueId);
            var productTypeOnyx = MapPartType(partType, existingEntity);

            var duplicateNamesInRequest = partTypes
                .Where(c => c.UniqueId != partType.UniqueId)
                .Any(c => Compare(c.PartTypeName, partType.PartTypeName));
            var duplicateNamesInDb = dbProductTypes
                .Where(c => c.Related7SoftProductTypeId == partType.UniqueId)
                .Where(c => c.Name.ToLower().Contains(partType.PartTypeName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateNamesInDb.Count > 0 || duplicateNamesInRequest)
            {
                productTypeOnyx.Name = partType.PartTypeName + partType.Code;
            }
            else
            {
                productTypeOnyx.Name = partType.PartTypeName;
            }


            var duplicateLocalizedNamesInRequest = partTypes
                .Where(c => c.UniqueId != partType.UniqueId)
                .Any(c => Compare(c.PartTypeLocalizedName, partType.PartTypeLocalizedName));
            var duplicateLocalizedNamesInDb = dbProductTypes
                .Where(c => c.Related7SoftProductTypeId == partType.UniqueId)
                .Where(c => c.LocalizedName.ToLower().Contains(partType.PartTypeLocalizedName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateLocalizedNamesInDb.Count > 0 || duplicateLocalizedNamesInRequest)
            {
                productTypeOnyx.LocalizedName = partType.PartTypeLocalizedName + partType.Code;
            }
            else
            {
                productTypeOnyx.LocalizedName = partType.PartTypeLocalizedName;
            }
            if (existingEntity == null)
            {
                await _context.ProductTypes.AddAsync(productTypeOnyx);
            }
            productTypesOnyx.Add(productTypeOnyx);
        }


        //for remove not thick productTypes
        var productTypesToRemove = _context.ProductTypes
            .Where(p => !productTypesOnyx.Select(c => c.Related7SoftProductTypeId)
                .Contains(p.Related7SoftProductTypeId)).ToList();
        _context.ProductTypes.RemoveRange(productTypesToRemove);


        return productTypesOnyx;
    }
    #endregion
    #region Provider
    public DbProvider MapProvider(Provider provider, DbProvider? existingProvider)
    {
        var countingUnitTypeOnyx = existingProvider ?? new DbProvider() { IsActive = false };

        countingUnitTypeOnyx.Related7SoftProviderId = provider.UniqueId;
        countingUnitTypeOnyx.Code = provider.Code;
        countingUnitTypeOnyx.LocalizedCode = provider.ProviderLocalizedCode;
        countingUnitTypeOnyx.Description = provider.Description;

        return countingUnitTypeOnyx;
    }
    public async Task<List<DbProvider>> MapProviders()
    {
        var response = await GetData("Provider/GetAll", RequestType.Provider);
        var providers = new List<Provider>();
        if (response != null)
        {
            var json = await response.Content.ReadAsStringAsync();
            providers = JsonConvert.DeserializeObject<List<Provider>>(json);
        }

        var providersOnyx = new List<DbProvider>();
        var dbProviders = await _context.Providers.ToListAsync();
        foreach (var provider in providers!)
        {
            var existingEntity = dbProviders
                .FirstOrDefault(e => e.Related7SoftProviderId == provider.UniqueId);
            var providerOnyx = MapProvider(provider, existingEntity);

            var providerEnglishName = provider.ProviderName ?? provider.ProviderLocalizedName;
            var duplicateNamesInRequest = providers
                .Where(c => c.UniqueId != provider.UniqueId)
                .Any(c => Compare(c.ProviderName ?? c.ProviderLocalizedName, providerEnglishName));
            var duplicateNamesInDb = dbProviders
                .Where(c => c.Related7SoftProviderId != provider.UniqueId)
                .Where(c => c.Name.ToLower().Contains(providerEnglishName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateNamesInDb.Count > 0 || duplicateNamesInRequest)
            {
                providerOnyx.Name = provider.ProviderName + provider.Code;
            }
            else
            {
                providerOnyx.Name = provider.ProviderName ?? provider.ProviderLocalizedName;
            }

            var duplicateLocalizedNamesInRequest = providers
                .Where(c => c.UniqueId != provider.UniqueId)
                .Any(c => Compare(c.ProviderLocalizedName, provider.ProviderLocalizedName));
            var duplicateLocalizedNamesInDb = dbProviders
                .Where(c => c.Related7SoftProviderId != provider.UniqueId)
                .Where(c => c.LocalizedName.ToLower().Contains(provider.ProviderLocalizedName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
            if (duplicateLocalizedNamesInDb.Count > 0 || duplicateLocalizedNamesInRequest)
            {
                providerOnyx.LocalizedName = provider.ProviderLocalizedName + provider.Code;
            }
            else
            {
                providerOnyx.LocalizedName = provider.ProviderLocalizedName;
            }

            if (existingEntity == null)
            {
                await _context.Providers.AddAsync(providerOnyx);
            }
            providersOnyx.Add(providerOnyx);
        }


        //for remove not thick providers
        var providersToRemove = _context.Providers
            .Where(p => !providersOnyx.Select(c => c.Related7SoftProviderId)
                .Contains(p.Related7SoftProviderId)).ToList();
        _context.Providers.RemoveRange(providersToRemove);


        return providersOnyx;
    }
    #endregion
    #region Part
    public DbProduct MapPart(
        Part part,
        List<DbProvider> providers,
        List<DbProductCategory> productCategories,
        List<DbCountry> countries,
        List<DbProductBrand> brands,
        List<DbCountingUnit> countingUnits,
        List<DbProductType> productTypes,
        List<DbProductStatus> productStatusList,
        ProductAttributeType productAttributeType,
        DbProduct? existingProduct,
        double count,
        decimal price)
    {
        var productOnyx = existingProduct ?? new DbProduct() { IsActive = false };

        productOnyx.SetProductAttributeType(productAttributeType);

        var productAttributeOption =
            productOnyx.AttributeOptions.SingleOrDefault() ?? new ProductAttributeOption();

        productAttributeOption.SetTotalCount(count);
        productAttributeOption.SafetyStockQty = part.SaftyStockQty;
        productAttributeOption.MinStockQty = part.MinStockQty;
        productAttributeOption.MaxStockQty = part.MaxStockQty;
        productAttributeOption.IsDefault = true;
        productAttributeOption.SetPrice(price);

        productOnyx.AttributeOptions = new List<ProductAttributeOption>() { productAttributeOption };




        var productAttributeOptionRole1 = productOnyx.AttributeOptions?
            .FirstOrDefault()?.ProductAttributeOptionRoles
            .FirstOrDefault(c => c.CustomerTypeEnum == CustomerTypeEnum.Personal);

        if (productAttributeOptionRole1 == null)
        {
            productAttributeOptionRole1 = new ProductAttributeOptionRole();
            productAttributeOptionRole1.ProductAttributeOption = productAttributeOption;
            productAttributeOptionRole1.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(0);
            productAttributeOptionRole1.DiscountPercent = 0;
        }
        else
        {
            productAttributeOptionRole1.ProductAttributeOption = productAttributeOption;
        }
        productAttributeOptionRole1.SetMainMaxOrderQty(part.MaxOrderQty);
        productAttributeOptionRole1.SetMainMinOrderQty(part.MinOrderQty);
        productAttributeOptionRole1.CustomerTypeEnum = CustomerTypeEnum.Personal;
        productAttributeOptionRole1.ProductAttributeOption = productAttributeOption;




        var productAttributeOptionRole2 = productOnyx.AttributeOptions?
            .FirstOrDefault()?.ProductAttributeOptionRoles
            .FirstOrDefault(c => c.CustomerTypeEnum == CustomerTypeEnum.Store);

        if (productAttributeOptionRole2 == null)
        {
            productAttributeOptionRole2 = new ProductAttributeOptionRole();
            productAttributeOptionRole2.ProductAttributeOption = productAttributeOption;
            productAttributeOptionRole2.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(0);
            productAttributeOptionRole2.DiscountPercent = 0;
        }
        else
        {
            productAttributeOptionRole2.ProductAttributeOption = productAttributeOption;
        }
        productAttributeOptionRole2.SetMainMaxOrderQty(part.MaxOrderQty);
        productAttributeOptionRole2.SetMainMinOrderQty(part.MinOrderQty);
        productAttributeOptionRole2.CustomerTypeEnum = CustomerTypeEnum.Store;
        productAttributeOptionRole2.ProductAttributeOption = productAttributeOption;




        var productAttributeOptionRole3 = productOnyx.AttributeOptions?
            .FirstOrDefault()?.ProductAttributeOptionRoles
            .FirstOrDefault(c => c.CustomerTypeEnum == CustomerTypeEnum.Agency);

        if (productAttributeOptionRole3 == null)
        {
            productAttributeOptionRole3 = new ProductAttributeOptionRole();
            productAttributeOptionRole3.ProductAttributeOption = productAttributeOption;
            productAttributeOptionRole3.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(0);
            productAttributeOptionRole3.DiscountPercent = 0;
        }
        else
        {
            productAttributeOptionRole3.ProductAttributeOption = productAttributeOption;
        }
        productAttributeOptionRole3.SetMainMaxOrderQty(part.MaxOrderQty);
        productAttributeOptionRole3.SetMainMinOrderQty(part.MinOrderQty);
        productAttributeOptionRole3.CustomerTypeEnum = CustomerTypeEnum.Agency;
        productAttributeOptionRole3.ProductAttributeOption = productAttributeOption;





        var productAttributeOptionRole4 = productOnyx.AttributeOptions?
            .FirstOrDefault()?.ProductAttributeOptionRoles
            .FirstOrDefault(c => c.CustomerTypeEnum == CustomerTypeEnum.CentralRepairShop);

        if (productAttributeOptionRole4 == null)
        {
            productAttributeOptionRole4 = new ProductAttributeOptionRole();
            productAttributeOptionRole4.ProductAttributeOption = productAttributeOption;
            productAttributeOptionRole4.SetMinimumStockToDisplayProductForThisCustomerTypeEnum(0);
            productAttributeOptionRole4.DiscountPercent = 0;
        }
        else
        {
            productAttributeOptionRole4.ProductAttributeOption = productAttributeOption;
        }
        productAttributeOptionRole4.SetMainMaxOrderQty(part.MaxOrderQty);
        productAttributeOptionRole4.SetMainMinOrderQty(part.MinOrderQty);
        productAttributeOptionRole4.CustomerTypeEnum = CustomerTypeEnum.CentralRepairShop;
        productAttributeOptionRole4.ProductAttributeOption = productAttributeOption;

        productAttributeOption.ProductAttributeOptionRoles.Add(productAttributeOptionRole1);
        productAttributeOption.ProductAttributeOptionRoles.Add(productAttributeOptionRole2);
        productAttributeOption.ProductAttributeOptionRoles.Add(productAttributeOptionRole3);
        productAttributeOption.ProductAttributeOptionRoles.Add(productAttributeOptionRole4);


        var dbAttributes = productOnyx.Attributes;
        var dbAttribute1 = dbAttributes
            .SingleOrDefault(c => c.Name == "طول");
        var productAttribute1 = dbAttribute1 ?? throw new NotFoundException(nameof(ProductAttribute), "طول");

        productAttribute1.Name = "طول";
        productAttribute1.Slug = "طول";
        productAttribute1.Featured = false;
        productAttribute1.ValueName = part.PartLength.ToString() ?? "-";
        productAttribute1.ValueSlug = part.PartLength.ToString()?.ToLower().Replace(' ', '-') ?? "-";

        if (dbAttribute1 == null)
        {
            productOnyx.Attributes.Add(productAttribute1);
        }


        var dbAttribute2 = dbAttributes
            .SingleOrDefault(c => c.Name == "عرض");
        var productAttribute2 = dbAttribute2 ?? throw new NotFoundException(nameof(ProductAttribute), "عرض");

        productAttribute2.Name = "عرض";
        productAttribute2.Slug = "عرض";
        productAttribute2.Featured = false;
        productAttribute2.ValueName = part.PartWidth.ToString() ?? "-";
        productAttribute2.ValueSlug = part.PartWidth.ToString()?.ToLower().Replace(' ', '-') ?? "-";

        if (dbAttribute2 == null)
        {
            productOnyx.Attributes.Add(productAttribute2);
        }


        var dbAttribute3 = dbAttributes
            .SingleOrDefault(c => c.Name == "ارتفاع");
        var productAttribute3 = dbAttribute3 ?? throw new NotFoundException(nameof(ProductAttribute), "ارتفاع");

        productAttribute3.Name = "ارتفاع";
        productAttribute3.Slug = "ارتفاع";
        productAttribute3.Featured = false;
        productAttribute3.ValueName = part.PartWidth.ToString() ?? "-";
        productAttribute3.ValueSlug = part.PartWidth.ToString()?.ToLower().Replace(' ', '-') ?? "-";

        if (dbAttribute3 == null)
        {
            productOnyx.Attributes.Add(productAttribute3);
        }

        var dbAttribute4 = dbAttributes
            .SingleOrDefault(c => c.Name == "وزن خالص کالا");
        var productAttribute4 = dbAttribute4 ?? throw new NotFoundException(nameof(ProductAttribute), "وزن خالص کال");

        productAttribute4.Name = "وزن خالص کالا";
        productAttribute4.Slug = "وزن-خالص-کالا";
        productAttribute4.Featured = false;
        productAttribute4.ValueName = part.PartWidth.ToString() ?? "-";
        productAttribute4.ValueSlug = part.PartWidth.ToString()?.ToLower().Replace(' ', '-') ?? "-";

        if (dbAttribute4 == null)
        {
            productOnyx.Attributes.Add(productAttribute4);
        }


        var dbAttribute5 = dbAttributes
            .SingleOrDefault(c => c.Name == "وزن ناخالص کالا");
        var productAttribute5 = dbAttribute5 ?? throw new NotFoundException(nameof(ProductAttribute), "وزن ناخالص کالا");

        productAttribute5.Name = "وزن ناخالص کالا";
        productAttribute5.Slug = "وزن-ناخالص-کالا";
        productAttribute5.Featured = false;
        productAttribute5.ValueName = part.PartWidth.ToString() ?? "-";
        productAttribute5.ValueSlug = part.PartWidth.ToString()?.ToLower().Replace(' ', '-') ?? "-";

        if (dbAttribute5 == null)
        {
            productOnyx.Attributes.Add(productAttribute5);
        }

        var dbAttribute6 = dbAttributes
            .SingleOrDefault(c => c.Name == "وزن حجمی کالا");
        var productAttribute6 = dbAttribute6 ?? throw new NotFoundException(nameof(ProductAttribute), "وزن حجمی کال");

        productAttribute6.Name = "وزن حجمی کالا";
        productAttribute6.Slug = "وزن-حجمی-کالا";
        productAttribute6.Featured = false;
        productAttribute6.ValueName = part.PartWidth.ToString() ?? "-";
        productAttribute6.ValueSlug = part.PartWidth.ToString()?.ToLower().Replace(' ', '-') ?? "-";

        if (dbAttribute6 == null)
        {
            productOnyx.Attributes.Add(productAttribute6);
        }

        productOnyx.Related7SoftProductId = part.UniqueId;
        productOnyx.Code = part.Code;
        productOnyx.ProductNo = part.PartNo;
        productOnyx.ProviderId = providers.SingleOrDefault(c => c.Related7SoftProviderId == part.PartProviderId)!.Id;
        productOnyx.Name = part.PartName ?? part.PartLocalizedName;
        productOnyx.LocalizedName = part.PartLocalizedName;
        productOnyx.Slug = part.PartLocalizedName.ToLower().Replace(' ', '-');
        productOnyx.OldProductNo = part.OldProductNo;
        productOnyx.ProductCategoryId = productCategories.SingleOrDefault(c => c.Related7SoftProductCategoryId == part.PartGroupId)!.Id;
        productOnyx.CountryId = countries.SingleOrDefault(c => c.Related7SoftCountryId == part.CountryId)!.Id;
        productOnyx.ProductBrandId = brands.SingleOrDefault(c => c.Related7SoftBrandId == part.PartBrandId)!.Id;
        productOnyx.ProductCatalog = part.PartCatalog;
        productOnyx.MainCountingUnitId =
            countingUnits.SingleOrDefault(c => c.Related7SoftCountingUnitId == part.MainCountingUnitId)!.Id;
        productOnyx.CommonCountingUnitId =
            countingUnits.SingleOrDefault(c => c.Related7SoftCountingUnitId == part.CommonCountingUnitId)!.Id;
        productOnyx.OrderRate = part.OrderRate;
        productOnyx.ProductTypeId =
            productTypes.SingleOrDefault(c => c.Related7SoftProductTypeId == part.PartTypeId)!.Id;
        productOnyx.ProductStatusId =
            productStatusList.SingleOrDefault(c => c.Related7SoftProductStatusId == part.PartStatusId)?.Id;


        productOnyx.Mileage = part.Mileage;
        productOnyx.Duration = part.Duration;
        productOnyx.Compatibility = part.AllVehicleModels ? CompatibilityEnum.All : CompatibilityEnum.Compatible;

        if (existingProduct == null)
        {
            productOnyx.Description = "-";
            productOnyx.Excerpt = "-";
        }

        return productOnyx;
    }

    public async Task<List<DbProduct>> MapParts(
        List<DbProvider> providers,
        List<DbProductCategory> productCategories,
        List<DbCountry> countries,
        List<DbProductBrand> productBrands,
        List<DbCountingUnit> countingUnits,
        List<DbProductType> productTypes,
        List<DbProductStatus> productStatusList)
    {
        var requestDetailsCount = await _context.RequestLogs.Where(c => (c.RequestType == RequestType.Part || c.RequestType == RequestType.PartFull) && c.ResponseStatus == HttpStatusCode.OK).CountAsync();
        var apiType = requestDetailsCount % 7 == 0 ? RequestType.PartFull : RequestType.Part;
        var response = await GetData("Part/GetAllParts", apiType);
        var partResponse = new PartResponse();
        if (response != null)
        {
            var json = await response.Content.ReadAsStringAsync();
            partResponse = JsonConvert.DeserializeObject<PartResponse>(json);
        }

        var parts = partResponse?.ReturnModel;
        var productPrices = new List<ProductPrice>();
        var productCounts = new List<ProductCount>();
        if (parts != null)
        {
            var pricesResponse = await GetPrices(parts.Select(c => c.UniqueId).ToList());
            if (pricesResponse != null)
            {
                var json = await pricesResponse.Content.ReadAsStringAsync();
                productPrices = JsonConvert.DeserializeObject<List<ProductPrice>>(json);
            }

            var countsResponse = await GetCounts(parts.Select(c => c.UniqueId).ToList());
            if (countsResponse != null)
            {
                var json = await countsResponse.Content.ReadAsStringAsync();
                productCounts = JsonConvert.DeserializeObject<List<ProductCount>>(json);
            }
        }

        var productsOnyx = new List<DbProduct>();

        var dbProducts = await _context.Products
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.OptionValues)
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .Include(c => c.Attributes)
            .Include(c => c.ProductAttributeType)
            .ThenInclude(c => c.AttributeGroups)
            .ThenInclude(c => c.Attributes)
            .ToListAsync();

        if (parts != null)
        {
            var dbProductAttributeType =
                await _context.ProductAttributeTypes.SingleOrDefaultAsync(c => c.Name == "پیش فرض");
            var productAttributeType = dbProductAttributeType ?? new ProductAttributeType();

            if (dbProductAttributeType == null)
            {
                productAttributeType.Name = "پیش فرض";
                productAttributeType.Slug = "پیش-فرض";
                productAttributeType.AttributeGroups = new List<ProductTypeAttributeGroup>();
            }

            var dbProductTypeAttributeGroups = dbProductAttributeType?.AttributeGroups;
            var dbProductTypeAttributeGroup1 = dbProductTypeAttributeGroups?
                .SingleOrDefault(c => c.Name == "ابعاد");
            var productTypeAttributeGroup1 = dbProductTypeAttributeGroup1 ?? new ProductTypeAttributeGroup();

            if (dbProductTypeAttributeGroup1 == null)
            {
                productTypeAttributeGroup1.Name = "ابعاد";
                productTypeAttributeGroup1.Slug = "ابعاد";
                productTypeAttributeGroup1.Attributes = new List<ProductTypeAttributeGroupAttribute>();
                productAttributeType.AttributeGroups.Add(productTypeAttributeGroup1);
            }


            var dbProductTypeAttributeGroup2 = dbProductTypeAttributeGroups?
                .SingleOrDefault(c => c.Name == "ویژگی های وزنی");
            var productTypeAttributeGroup2 = dbProductTypeAttributeGroup2 ?? new ProductTypeAttributeGroup();

            if (dbProductTypeAttributeGroup2 == null)
            {
                productTypeAttributeGroup2.Name = "ویژگی های وزنی";
                productTypeAttributeGroup2.Slug = "ویژگی-های-وزنی";
                productTypeAttributeGroup2.Attributes = new List<ProductTypeAttributeGroupAttribute>();
                productAttributeType.AttributeGroups.Add(productTypeAttributeGroup2);
            }


            var dbProductTypeAttributeGroupAttributes1 = dbProductTypeAttributeGroup1?.Attributes;
            var dbProductTypeAttributeGroupAttribute1 = dbProductTypeAttributeGroupAttributes1?
                .SingleOrDefault(c => c.Value == "طول");
            var productTypeAttributeGroupAttribute1 = dbProductTypeAttributeGroupAttribute1 ?? new ProductTypeAttributeGroupAttribute("طول");
            if (dbProductTypeAttributeGroupAttribute1 == null)
            {
                productTypeAttributeGroup1.Attributes.Add(productTypeAttributeGroupAttribute1);
            }

            var dbProductTypeAttributeGroupAttribute2 = dbProductTypeAttributeGroupAttributes1?
                .SingleOrDefault(c => c.Value == "عرض");
            var productTypeAttributeGroupAttribute2 = dbProductTypeAttributeGroupAttribute2 ?? new ProductTypeAttributeGroupAttribute("عرض");
            if (dbProductTypeAttributeGroupAttribute2 == null)
            {
                productTypeAttributeGroup1.Attributes.Add(productTypeAttributeGroupAttribute2);
            }

            var dbProductTypeAttributeGroupAttribute3 = dbProductTypeAttributeGroupAttributes1?
                .SingleOrDefault(c => c.Value == "ارتفاع");
            var productTypeAttributeGroupAttribute3 = dbProductTypeAttributeGroupAttribute3 ?? new ProductTypeAttributeGroupAttribute("ارتفاع");
            if (dbProductTypeAttributeGroupAttribute3 == null)
            {
                productTypeAttributeGroup1.Attributes.Add(productTypeAttributeGroupAttribute3);
            }

            var dbProductTypeAttributeGroupAttributes2 = dbProductTypeAttributeGroup2?.Attributes;
            var dbProductTypeAttributeGroupAttribute4 = dbProductTypeAttributeGroupAttributes2?
                .SingleOrDefault(c => c.Value == "وزن خالص کالا");
            var productTypeAttributeGroupAttribute4 = dbProductTypeAttributeGroupAttribute4 ?? new ProductTypeAttributeGroupAttribute("وزن خالص کالا");
            if (dbProductTypeAttributeGroupAttribute4 == null)
            {
                productTypeAttributeGroup2.Attributes.Add(productTypeAttributeGroupAttribute4);
            }

            var dbProductTypeAttributeGroupAttribute5 = dbProductTypeAttributeGroupAttributes2?
                .SingleOrDefault(c => c.Value == "وزن ناخالص کالا");
            var productTypeAttributeGroupAttribute5 = dbProductTypeAttributeGroupAttribute5 ?? new ProductTypeAttributeGroupAttribute("وزن ناخالص کالا");
            if (dbProductTypeAttributeGroupAttribute5 == null)
            {
                productTypeAttributeGroup2.Attributes.Add(productTypeAttributeGroupAttribute5);
            }

            var dbProductTypeAttributeGroupAttribute6 = dbProductTypeAttributeGroupAttributes2?
                .SingleOrDefault(c => c.Value == "وزن حجمی کالا");
            var productTypeAttributeGroupAttribute6 = dbProductTypeAttributeGroupAttribute6 ?? new ProductTypeAttributeGroupAttribute("وزن حجمی کالا");
            if (dbProductTypeAttributeGroupAttribute6 == null)
            {
                productTypeAttributeGroup2.Attributes.Add(productTypeAttributeGroupAttribute6);
            }


            foreach (var part in parts)
            {
                var existingEntity = dbProducts
                    .FirstOrDefault(e => e.Related7SoftProductId == part.UniqueId);


                var price = productPrices?.SingleOrDefault(c => c.PartId == part.UniqueId)?.Price;
                //if (price == null)
                //{
                //    throw new SevenException("price not found for this id:" + part.UniqueId);
                //}
                var count = productCounts?.SingleOrDefault(c => c.PartId == part.UniqueId && c.TransactionTypeId == 1)?.Number;
                //if (count == null)
                //{
                //    throw new SevenException("count not found for this id:" + part.UniqueId);
                //}

                var productOnyx = MapPart(part, providers, productCategories, countries, productBrands, countingUnits,
                    productTypes, productStatusList, productAttributeType, existingEntity, count ?? 0, price ?? 0);



                var partEnglishName = part.PartName ?? part.PartLocalizedName;
                var duplicateNamesInRequest = parts
                    .Where(c => c.UniqueId != part.UniqueId)
                    .Any(c => Compare(c.PartName ?? c.PartLocalizedName, partEnglishName));
                var duplicateNamesInDb = dbProducts
                    .Where(c => c.Related7SoftProductId != part.UniqueId)
                    .Where(c => c.Name.ToLower().Contains(partEnglishName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
                if (duplicateNamesInDb.Count > 0 || duplicateNamesInRequest)
                {
                    productOnyx.Name = (part.PartName ?? part.PartLocalizedName) + " کد " + part.Code;
                }
                else
                {
                    productOnyx.Name = part.PartName ?? part.PartLocalizedName;
                }


                var duplicateLocalizedNamesInRequest = parts
                    .Where(c => c.UniqueId != part.UniqueId)
                    .Any(c => Compare(c.PartLocalizedName, part.PartLocalizedName));
                var duplicateLocalizedNamesInDb = dbProducts
                    .Where(c => c.Related7SoftProductId != part.UniqueId)
                    .Where(c => c.LocalizedName.ToLower().Contains(part.PartLocalizedName.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
                if (duplicateLocalizedNamesInDb.Count > 0 || duplicateLocalizedNamesInRequest)
                {
                    productOnyx.LocalizedName = part.PartLocalizedName + " کد " + part.Code;
                    productOnyx.Slug = productOnyx.LocalizedName.ToLower().Replace(' ', '-');
                }
                else
                {
                    productOnyx.LocalizedName = part.PartLocalizedName;
                    productOnyx.Slug = productOnyx.LocalizedName.ToLower().Replace(' ', '-');
                }

                if (existingEntity == null)
                {
                    await _context.Products.AddAsync(productOnyx);
                }

                productsOnyx.Add(productOnyx);
            }
        }


        //for remove not thick products
        if (apiType == RequestType.PartFull)
        {
            var productsToRemove = _context.Products
                .Where(p => !productsOnyx.Select(c => c.Related7SoftProductId)
                    .Contains(p.Related7SoftProductId)).ToList();
            //_context.Products.RemoveRange(productsToRemove);
            foreach (var product in productsToRemove)
            {
                foreach (var option in product.AttributeOptions)
                {
                    foreach (var optionRole in option.ProductAttributeOptionRoles)
                    {
                        optionRole.SetMainMaxOrderQty(0);
                        optionRole.SetMainMinOrderQty(0);
                    }
                }
            }
        }



        return productsOnyx;
    }
    #endregion
    #region PartVehicleModel
    public DbProductKind MapPartVehicleModel(PartVehicleModel partVehicleModel, List<DbProduct> products, List<DbKind> kinds, DbProductKind? existingDbProductKind)
    {
        var productKindOnyx = existingDbProductKind ?? new DbProductKind();

        productKindOnyx.KindId = kinds.SingleOrDefault(c => c.Related7SoftKindId == partVehicleModel.VehicleModelId)?.Id ?? 0;
        productKindOnyx.ProductId = products.SingleOrDefault(c => c.Related7SoftProductId == partVehicleModel.PartId)?.Id ?? 0;
        productKindOnyx.Related7SoftKindId = partVehicleModel.VehicleModelId;
        productKindOnyx.Related7SoftProductId = partVehicleModel.PartId;
        productKindOnyx.Related7SoftProductKindId = partVehicleModel.UniqueId;

        return productKindOnyx;
    }
    public async Task<List<DbProductKind>> MapPartVehicleModels(List<DbProduct> products, List<DbKind> kinds)
    {
        var requestDetailsCount = await _context.RequestLogs.Where(c => (c.RequestType == RequestType.PartVehicleModel || c.RequestType == RequestType.PartVehicleModelFull) && c.ResponseStatus == HttpStatusCode.OK).CountAsync();
        var apiType = requestDetailsCount % 7 == 0 ? RequestType.PartVehicleModelFull : RequestType.PartVehicleModel;
        var response = await GetData("PartVehicleModel/GetAll", apiType);

        var partVehicleResponse = new PartVehicleResponse();
        if (response != null)
        {
            var json = await response.Content.ReadAsStringAsync();
            partVehicleResponse = JsonConvert.DeserializeObject<PartVehicleResponse>(json);
        }

        var partVehicleModels = partVehicleResponse?.ReturnModel;


        var productKindsOnyx = new List<DbProductKind>();
        var dbProductKinds = await _context.ProductKinds.ToListAsync();

        if (partVehicleModels != null)
        {
            foreach (var partVehicleModel in partVehicleModels)
            {
                var existingEntity = dbProductKinds
                    .FirstOrDefault(e => e.Related7SoftProductKindId == partVehicleModel.UniqueId);
                var productKindOnyx = MapPartVehicleModel(partVehicleModel, products, kinds, existingEntity);

                if (existingEntity == null && productKindOnyx.ProductId != 0 && productKindOnyx.KindId != 0)
                {
                    await _context.ProductKinds.AddAsync(productKindOnyx);
                }

                productKindsOnyx.Add(productKindOnyx);
            }
        }


        //for remove not thick productKinds
        if (apiType == RequestType.PartVehicleModelFull)
        {
            var productKindsToRemove = _context.ProductKinds
                .Where(p => !productKindsOnyx.Select(c => c.Related7SoftProductKindId)
                    .Contains(p.Related7SoftProductKindId)).ToList();
            _context.ProductKinds.RemoveRange(productKindsToRemove);
        }




        return productKindsOnyx;
    }
    #endregion
    #region DisplayNames
    public async Task<bool> MapDisplayNames()
    {
        var products = await _context.Products
            .Include(c => c.Kinds)
            .ThenInclude(c => c.Model)
            .ThenInclude(c => c.Family)
            .Include(c => c.ProductBrand)
            .Include(c => c.ProductDisplayVariants)
            .ToListAsync();

        var productDisplayVariants = await _context.ProductDisplayVariants.ToListAsync();

        foreach (var product in products)
        {
            var displayNames = new List<string>();
            var families = new List<string>();
            foreach (var kind in product.Kinds)
            {
                var displayName = product.LocalizedName + " " + kind.Model.Family.LocalizedName + " " +
                                  product.ProductBrand.LocalizedName;
                var family = kind.Model.Family.LocalizedName;
                if (displayNames.All(c => c != displayName))
                {
                    displayNames.Add(displayName);
                }
                if (families.All(c => c != family))
                {
                    families.Add(family);
                }
            }
            product.Description = "خودروهای سازگار با این قطعه:" + "\n" + string.Join(",  ", families);

            var dbRecordsForRemove = productDisplayVariants.Where(c => c.ProductId == product.Id).ToList();
            foreach (var name in displayNames)
            {
                var dbRecord = productDisplayVariants.SingleOrDefault(c => c.ProductId == product.Id && c.Name == name);
                if (dbRecord == null)
                {
                    _context.ProductDisplayVariants.Add(new ProductDisplayVariant()
                    {
                        Name = name,
                        ProductId = product.Id,
                        IsActive = true
                    });
                }
                else
                {
                    dbRecordsForRemove.Remove(dbRecord);
                }
            }
            _context.ProductDisplayVariants.RemoveRange(dbRecordsForRemove);
        }

        return true;
    }
    #endregion

    private async Task<HttpResponseMessage?> GetData(string url, RequestType requestType)
    {
        using HttpClient client = new HttpClient();

        var baseUri = new Uri(_applicationSettings.SevenSoftBaseUrl);

        var completeUrl = baseUri + url;


        if (requestType == RequestType.Part)
        {
            var lastRequestLog = await _context.RequestLogs.OrderBy(c => c.Created).LastOrDefaultAsync(c => c.RequestType == RequestType.Part);
            if (lastRequestLog != null)
            {
                var filterFromLastChangeDate = lastRequestLog.Created.AddHours(-1).ToString(CultureInfo.InvariantCulture);
                var filterToLastChangeDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                completeUrl +=
                    $"?FilterFromLastChangeDate={filterFromLastChangeDate}&FilterToLastChangeDate={filterToLastChangeDate}";
            }
        }

        if (requestType == RequestType.PartVehicleModel)
        {
            var lastRequestLog = await _context.RequestLogs.OrderBy(c => c.Created).LastOrDefaultAsync(c => c.RequestType == RequestType.PartVehicleModel);
            if (lastRequestLog != null)
            {
                var filterFromLastChangeDate = lastRequestLog.Created.AddHours(-1).ToString(CultureInfo.InvariantCulture);
                var filterToLastChangeDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                completeUrl +=
                    $"?FilterFromLastChangeDate={filterFromLastChangeDate}&FilterToLastChangeDate={filterToLastChangeDate}";
            }
        }

        var accessToken = _accessToken?.Substring(1, _accessToken.Length - 2);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        HttpResponseMessage response = await client.GetAsync(completeUrl);

        var requestLog = new RequestLog
        {
            ApiAddress = completeUrl,
            ResponseStatus = response.StatusCode,
            RequestType = requestType,
            Created = DateTime.Now,
            ApiType = ApiType.Get
        };

        if (requestType == RequestType.Part)
        {
            var json = await response.Content.ReadAsStringAsync();
            var partResponse = JsonConvert.DeserializeObject<PartResponse>(json);
            if (partResponse?.AddStatus != 0)
            {
                requestLog.ResponseStatus = HttpStatusCode.BadRequest;
            }
        }

        if (requestType == RequestType.PartVehicleModel)
        {
            var json = await response.Content.ReadAsStringAsync();
            var partVehicleResponse = JsonConvert.DeserializeObject<PartVehicleResponse>(json);
            if (partVehicleResponse?.AddStatus != 0)
            {
                requestLog.ResponseStatus = HttpStatusCode.BadRequest;
            }
        }

        await _context.RequestLogs.AddAsync(requestLog);
        await _context.SaveChangesAsync();

        return response.IsSuccessStatusCode ? response : null;
    }

    #endregion

    #region SyncProductPriceAndCount

    public async Task<bool> SyncProductPriceAndCount()
    {
        _accessToken = await _sharedService.Authenticate();

        var successfulPriceSync = await SyncPrices();
        var successfulCountSync = await SyncCounts();

        return successfulCountSync && successfulPriceSync;
    }

    #region Price

    public async Task<bool> SyncPrices()
    {
        var dbProducts = await _context.Products
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.Prices)
            .Where(c => c.Related7SoftProductId != null).ToListAsync();
        var dbProductIds = dbProducts.Select(c => c.Related7SoftProductId ?? Guid.Empty).ToList();


        var productPrices = new List<ProductPrice>();
        var pricesResponse = await GetPrices(dbProductIds);
        if (pricesResponse != null)
        {
            var json = await pricesResponse.Content.ReadAsStringAsync();
            productPrices = JsonConvert.DeserializeObject<List<ProductPrice>>(json);
        }

        foreach (var dbProduct in dbProducts)
        {
            var price = productPrices?.SingleOrDefault(c => c.PartId == dbProduct.Related7SoftProductId)?.Price;
            if (price == null)
            {
                continue;
            }

            var productAttributeOption = dbProduct.AttributeOptions?.SingleOrDefault();
            if (productAttributeOption == null)
            {
                return false;
            }

            productAttributeOption.SetPrice(price ?? 0);
        }

        return true;
    }

    #endregion

    #region Count

    public async Task<bool> SyncCounts()
    {
        var dbProducts = await _context.Products
            .Include(c => c.AttributeOptions)
            .ThenInclude(c => c.ProductAttributeOptionRoles)
            .Where(c => c.Related7SoftProductId != null).ToListAsync();
        var dbProductIds = dbProducts.Select(c => c.Related7SoftProductId ?? Guid.Empty).ToList();


        var productCounts = new List<ProductCount>();
        var countsResponse = await GetCounts(dbProductIds);
        if (countsResponse != null)
        {
            var json = await countsResponse.Content.ReadAsStringAsync();
            productCounts = JsonConvert.DeserializeObject<List<ProductCount>>(json);
        }

        foreach (var dbProduct in dbProducts)
        {
            var mainCont = productCounts?.SingleOrDefault(c => c.PartId == dbProduct.Related7SoftProductId && c.TransactionTypeId == 1)?.Number;
            var reservedCount = productCounts?.SingleOrDefault(c => c.PartId == dbProduct.Related7SoftProductId && c.TransactionTypeId == 3)?.Number;
            var convertedCount = mainCont;
            if (mainCont == null)
            {
                continue;
            }
            if (reservedCount != null)
            {
                convertedCount = mainCont - reservedCount;
            }

            var productAttributeOption = dbProduct.AttributeOptions?.SingleOrDefault();
            if (productAttributeOption == null)
            {
                return false;
            }
            productAttributeOption.SetTotalCount(convertedCount ?? 0);



            var productAttributeOptionRole1 = dbProduct.AttributeOptions?
                .FirstOrDefault()?.ProductAttributeOptionRoles
                .FirstOrDefault(c => c.CustomerTypeEnum == CustomerTypeEnum.Personal);
            if (productAttributeOptionRole1 == null)
            {
                return false;
            }


            var productAttributeOptionRole2 = dbProduct.AttributeOptions?
                .FirstOrDefault()?.ProductAttributeOptionRoles
                .FirstOrDefault(c => c.CustomerTypeEnum == CustomerTypeEnum.Store);
            if (productAttributeOptionRole2 == null)
            {
                return false;
            }


            var productAttributeOptionRole3 = dbProduct.AttributeOptions?
                .FirstOrDefault()?.ProductAttributeOptionRoles
                .FirstOrDefault(c => c.CustomerTypeEnum == CustomerTypeEnum.Agency);
            if (productAttributeOptionRole3 == null)
            {
                return false;
            }


            var productAttributeOptionRole4 = dbProduct.AttributeOptions?
                .FirstOrDefault()?.ProductAttributeOptionRoles
                .FirstOrDefault(c => c.CustomerTypeEnum == CustomerTypeEnum.CentralRepairShop);
            if (productAttributeOptionRole4 == null)
            {
                return false;
            }
        }

        return true;
    }

    #endregion

    #endregion

    #region Shared

    private bool Compare(string? string1, string? string2)
    {
        return string1?.ToLower() == string2?.ToLower();
    }

    private async Task<HttpResponseMessage?> GetPrices(List<Guid> partIds)
    {
        using HttpClient client = new HttpClient();

        var completeUrl = new Uri(_applicationSettings.SevenSoftBaseUrl + "part/GetPartsPriceById?salePriceType=2");


        var jsonIds = JsonSerializer.Serialize(partIds, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        var content = new StringContent(jsonIds, System.Text.Encoding.UTF8, "application/json");


        var accessToken = _accessToken?.Substring(1, _accessToken.Length - 2);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        HttpResponseMessage response = await client.PostAsync(completeUrl, content);

        var requestLog = new RequestLog
        {
            ApiAddress = completeUrl.ToString(),
            ResponseStatus = response.StatusCode,
            RequestType = RequestType.PartPrice,
            Created = DateTime.Now,
            ApiType = ApiType.Post,
            RequestBody = jsonIds,
        };

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var productPrices = JsonConvert.DeserializeObject<List<ProductPrice>>(json);
            if (productPrices != null && !productPrices.Any())
            {
                requestLog.ResponseStatus = HttpStatusCode.BadRequest;
            }
        }
        await _context.RequestLogs.AddAsync(requestLog);
        await _context.SaveChangesAsync();

        return response.IsSuccessStatusCode ? response : null;
    }

    private async Task<HttpResponseMessage?> GetCounts(List<Guid> partIds)
    {
        using HttpClient client = new HttpClient();

        var completeUrl = new Uri(_applicationSettings.SevenSoftBaseUrl + "inventory/GetInventoryById");


        var jsonIds = JsonSerializer.Serialize(partIds, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        var content = new StringContent(jsonIds, System.Text.Encoding.UTF8, "application/json");


        var accessToken = _accessToken?.Substring(1, _accessToken.Length - 2);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        HttpResponseMessage response = await client.PostAsync(completeUrl, content);

        var requestLog = new RequestLog
        {
            ApiAddress = completeUrl.ToString(),
            ResponseStatus = response.StatusCode,
            RequestType = RequestType.PartCount,
            Created = DateTime.Now,
            RequestBody = jsonIds,
        };

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var productCounts = JsonConvert.DeserializeObject<List<ProductCount>>(json);
            if (productCounts != null && !productCounts.Any())
            {
                requestLog.ResponseStatus = HttpStatusCode.BadRequest;
            }
        }
        await _context.RequestLogs.AddAsync(requestLog);
        await _context.SaveChangesAsync();

        return response.IsSuccessStatusCode ? response : null;
    }

    #endregion
}
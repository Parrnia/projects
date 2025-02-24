using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Common.Models;
using Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.FrontOffice.GetAllVehicleBrandsForDropDown;
using Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.FrontOffice.GetVehicleBrand.GetVehicleBrandById;
using Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.FrontOffice.GetVehicleBrands.GetAllVehicleBrands;
using Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.FrontOffice.GetVehicleBrands.GetVehicleBrandsForBlock;
using Onyx.Application.Main.BrandsCluster.VehicleBrands.Queries.FrontOffice.GetVehicleBrandsWithPagination.GetVehicleBrandsWithPagination;

namespace Onyx.Web.FrontOffice.Controllers.BrandsCluster;


public class VehicleBrandsController : ApiControllerBase
{
    [HttpGet("allVehicle")]
    public async Task<ActionResult<List<AllVehicleBrandDto>>> GetAllVehicleBrands()
    {
        return await Mediator.Send(new GetAllVehicleBrandsQuery());
    }

    [HttpGet("allVehicleWithPagination")]
    public async Task<ActionResult<PaginatedList<VehicleBrandWithPaginationDto>>> GetVehicleBrandsWithPagination([FromQuery] GetVehicleBrandsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpGet("allForBlock{limit}")]
    public async Task<ActionResult<PaginatedList<VehicleBrandForBlockDto>>> GetVehicleBrandsForBlock(int limit)
    {
        return await Mediator.Send(new GetVehicleBrandsForBlockQuery(limit));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleBrandByIdDto?>> GetVehicleBrandById(int id)
    {
        return await Mediator.Send(new GetVehicleBrandByIdQuery(id));
    }

    [HttpGet("allVehicleForDropDown")]
    public async Task<ActionResult<List<AllVehicleBrandForDropDownDto>>> GetAllVehicleBrandsForDropDown()
    {
        return await Mediator.Send(new GetAllVehicleBrandsForDropDownQuery());
    }
}

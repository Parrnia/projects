import { Injectable } from '@angular/core';
import { Observable, from } from 'rxjs';
import { map, switchMap, toArray } from 'rxjs/operators';
import { AllVehicleBrandDto, VehicleBrandWithPaginationDto, VehicleBrandByIdDto, VehicleBrandForBlockDto, AllVehicleBrandForDropDownDto } from '../../web-api-client';
import { Brand } from '../../interfaces/brand';
import { ImageService } from '../image.service';

@Injectable({
  providedIn: 'root'
})
export class VehicleBrandmapperService {

  constructor(private imageService: ImageService) {}

  //#region VehicleBrandById
  mapVehicleBrandById(brandDto: VehicleBrandByIdDto): Brand {
    let brand = new Brand();
    brand.name = brandDto.localizedName ?? '';
    brand.slug = brandDto.slug ?? '';
    brand.country = brandDto.country?.localizedName ?? '';
    brand.image = this.imageService.makeImageUrl(brandDto.brandLogo ?? '');
    return brand;
  }
  //#endregion

  //#region AllVehicleBrands
  mapAllVehicleBrand(brandDto: AllVehicleBrandDto): Brand {
    let brand = new Brand();
    brand.name = brandDto.localizedName ?? '';
    brand.slug = brandDto.slug ?? '';
    brand.country = brandDto.country?.localizedName ?? '';
    brand.image = this.imageService.makeImageUrl(brandDto.brandLogo ?? '');
    return brand;
  }

  mapAllVehicleBrands(brandDtos: AllVehicleBrandDto[]): Brand[] {
    let brands: Brand[] = [];
    brandDtos.forEach(c => {
      brands.push(this.mapAllVehicleBrand(c));
    })
    return brands;
  }
  //#endregion

  //#region VehicleBrandsForBlock
  mapVehicleBrandForBlock(brandDto: VehicleBrandForBlockDto): Brand {
    let brand = new Brand();
    brand.name = brandDto.name ?? '';
    brand.slug = brandDto.slug ?? '';
    brand.country = brandDto.countryName ?? '';
    brand.image = this.imageService.makeImageUrl(brandDto.brandLogo ?? '');
    return brand;
  }

  mapVehicleBrandsForBlock(brandDtos: VehicleBrandForBlockDto[]): Brand[] {
    let brands: Brand[] = [];
    brandDtos.forEach(c => {
      brands.push(this.mapVehicleBrandForBlock(c));
    })
    return brands;
  }
  //#endregion

  //#region VehicleBrandsWithPagination
  mapVehicleBrandWithPagination(brandDto: VehicleBrandWithPaginationDto): Brand {
    let brand = new Brand();
    brand.name = brandDto.localizedName ?? '';
    brand.slug = brandDto.slug ?? '';
    brand.country = brandDto.country?.localizedName ?? '';
    brand.image = this.imageService.makeImageUrl(brandDto.brandLogo ?? '');
    return brand;
  }

  mapVehicleBrandsWithPaginationDto(brandDtos: VehicleBrandWithPaginationDto[]): Brand[] {
    let brands: Brand[] = [];
    brandDtos.forEach(c => {
      brands.push(this.mapVehicleBrandWithPagination(c));
    })
    return brands;
  }
  //#endregion

  //#region AllVehicleBrandsForDropDown
  mapAllVehicleBrandForDropDown(brandDto: AllVehicleBrandForDropDownDto): Brand {
    let brand = new Brand();
    brand.name = brandDto.localizedName ?? '';
    brand.slug = brandDto.slug ?? '';
    return brand;
  }

  mapAllVehicleBrandsForDropDown(brandDtos: AllVehicleBrandForDropDownDto[]): Brand[] {
    let brands: Brand[] = [];
    brandDtos.forEach(c => {
      brands.push(this.mapAllVehicleBrandForDropDown(c));
    });
    return brands;
  }
  //#endregion
}

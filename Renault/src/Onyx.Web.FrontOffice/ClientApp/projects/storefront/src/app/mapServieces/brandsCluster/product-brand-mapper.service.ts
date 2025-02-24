import { Injectable } from '@angular/core';
import { AllProductBrandDto, ProductBrandWithPaginationDto, ProductBrandByIdDto, ProductBrandForBlockDto } from '../../web-api-client';
import { Brand } from '../../interfaces/brand';
import { ImageService } from '../image.service';
import { from, map, Observable, switchMap, toArray } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductBrandmapperService {

  constructor(private imageService: ImageService) { }


  //#region ProductBrandById
  mapProductBrandById(brandDto: ProductBrandByIdDto): Brand {
    let brand = new Brand();
    brand.name = brandDto.localizedName ?? "";
    brand.slug = brandDto.slug ?? "";
    brand.country = brandDto.country?.localizedName ?? "";
    brand.image = this.imageService.makeImageUrl(brandDto.brandLogo)
    return brand;
  }
  
  //#endregion

  //#region AllProductBrands
  mapAllProductBrand(brandDto: AllProductBrandDto): Brand {
    let brand = new Brand();
    brand.name = brandDto.localizedName ?? '';
    brand.slug = brandDto.slug ?? '';
    brand.country = brandDto.country?.localizedName ?? '';
    brand.image = this.imageService.makeImageUrl(brandDto.brandLogo);
    return brand;
  }

  mapAllProductBrands(brandDtos: AllProductBrandDto[]): Brand[] {
    let brands: Brand[] = [];
    brandDtos.forEach(c => {
      brands.push(this.mapAllProductBrand(c));
    })
    return brands;
  }
  //#endregion

  //#region ProductBrandsForBlock
  mapProductBrandForBlock(brandDto: ProductBrandForBlockDto): Brand {
    let brand = new Brand();
    brand.name = brandDto.name ?? '';
    brand.slug = brandDto.slug ?? '';
    brand.country = brandDto.countryName ?? '';
    brand.image = this.imageService.makeImageUrl(brandDto.brandLogo)
    return brand;
  }

  mapProductBrandsForBlock(brandDtos: ProductBrandForBlockDto[]): Brand[] {
    let brands: Brand[] = [];
    brandDtos.forEach(c => {
      brands.push(this.mapProductBrandForBlock(c));
    })
    return brands;
  }
  //#endregion

  //#region ProductBrandsWithPagination
  mapProductBrandWithPagination(brandDto: ProductBrandWithPaginationDto): Brand {
    let brand = new Brand();
    brand.name = brandDto.localizedName ?? '';
    brand.slug = brandDto.slug ?? '';
    brand.country = brandDto.country?.localizedName ?? '';
    brand.image = this.imageService.makeImageUrl(brandDto.brandLogo)
    return brand;
  }

  mapProductBrandsWithPaginationDto(brandDtos: ProductBrandWithPaginationDto[]): Brand[] {
    let brands: Brand[] = [];
    brandDtos.forEach(c => {
      brands.push(this.mapProductBrandWithPagination(c));
    })
    return brands;
  }
  //#endregion

}

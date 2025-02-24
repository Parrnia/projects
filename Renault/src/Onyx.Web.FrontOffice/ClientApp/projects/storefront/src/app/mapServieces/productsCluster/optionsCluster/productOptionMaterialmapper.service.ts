import { Injectable } from '@angular/core';
import { AllProductOptionMaterialDto, ProductOptionMaterialByIdDto, ProductOptionMaterialDto, ProductOptionValueMaterialDto, ProductOptionValueMaterialDto20 } from '../../../web-api-client';
import { ProductOptionMaterial, ProductOptionValueBase } from '../../../interfaces/product';


@Injectable({
  providedIn: 'root'
})
export class ProductOptionMaterialmapperService {

  constructor() { }

  //#region ProductOptionMaterialById
  
  mapProductOptionMaterialById(productOptionMaterialDto: ProductOptionMaterialByIdDto) {
    let productOptionMaterial = new ProductOptionMaterial();
    productOptionMaterial.name = productOptionMaterialDto.name ?? "";
    productOptionMaterial.slug = productOptionMaterialDto.slug ?? "";
    productOptionMaterial.type = 'material';
    productOptionMaterial.values = this.mapProductOptionValueMaterials(productOptionMaterialDto.values ?? []);
    return productOptionMaterial;
  }

  mapProductOptionMaterialsById(productOptionMaterialDtos: ProductOptionMaterialByIdDto[]) {
    let productOptionMaterials: ProductOptionMaterial[] = [];
    productOptionMaterialDtos.forEach(c => {
      productOptionMaterials.push(this.mapProductOptionMaterialById(c));
    })
    return productOptionMaterials;
  }
  //#endregion

  //#region AllProductOptionMaterial

  mapAllProductOptionMaterial(productOptionMaterialDto: AllProductOptionMaterialDto) {
    let productOptionMaterial = new ProductOptionMaterial();
    productOptionMaterial.name = productOptionMaterialDto.name ?? "";
    productOptionMaterial.slug = productOptionMaterialDto.slug ?? "";
    productOptionMaterial.type = 'material';
    productOptionMaterial.values = this.mapProductOptionValueMaterials(productOptionMaterialDto.values ?? []);
    return productOptionMaterial;
  }

  mapAllProductOptionMaterials(productOptionMaterialDtos: AllProductOptionMaterialDto[]) {
    let productOptionMaterials: ProductOptionMaterial[] = [];
    productOptionMaterialDtos.forEach(c => {
      productOptionMaterials.push(this.mapAllProductOptionMaterial(c));
    })
    return productOptionMaterials;
  }

  //#endregion


  mapProductOptionValueMaterial(productOptionValueMaterialDto: ProductOptionValueMaterialDto20) {
    let productOptionValueMaterial = new ProductOptionValueBase();
    productOptionValueMaterial.name = productOptionValueMaterialDto.name ?? "";
    productOptionValueMaterial.slug = productOptionValueMaterialDto.slug ?? "";
    return productOptionValueMaterial;
  }

  mapProductOptionValueMaterials(productOptionValueMaterialDtos: ProductOptionValueMaterialDto20[]) {
    let productOptionValueMaterials: ProductOptionValueBase[] = [];
    productOptionValueMaterialDtos.forEach(c => {
      productOptionValueMaterials.push(this.mapProductOptionValueMaterial(c));
    })
    return productOptionValueMaterials;
  }

}

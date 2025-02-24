import { Injectable } from '@angular/core';
import { AllProductOptionColorDto, AllProductOptionValueColorDto, ProductOptionColorByIdDto, ProductOptionValueColorByIdDto } from '../../../web-api-client';
import { ProductOptionColor, ProductOptionValueColor } from '../../../interfaces/product';


@Injectable({
  providedIn: 'root'
})
export class ProductOptionColormapperService {

  constructor() { }

  //#region ProductOptionColorById

  mapProductOptionColorById(productOptionColorDto: ProductOptionColorByIdDto) {
    let productOptionColor = new ProductOptionColor();
    productOptionColor.name = productOptionColorDto.name ?? "";
    productOptionColor.slug = productOptionColorDto.slug ?? "";
    productOptionColor.type = 'color';
    productOptionColor.values = this.mapProductOptionValueColorsById(productOptionColorDto.values ?? []);
    return productOptionColor;
  }

  mapProductOptionColorsById(productOptionColorDtos: ProductOptionColorByIdDto[]) {
    let productOptionColors: ProductOptionColor[] = [];
    productOptionColorDtos.forEach(c => {
      productOptionColors.push(this.mapProductOptionColorById(c));
    })
    return productOptionColors;
  }

  mapProductOptionValueColorById(productOptionValueColorDto: ProductOptionValueColorByIdDto) {
    let productOptionValueColor = new ProductOptionValueColor();
    productOptionValueColor.name = productOptionValueColorDto.name ?? "";
    productOptionValueColor.slug = productOptionValueColorDto.slug ?? "";
    productOptionValueColor.color = productOptionValueColorDto.color ?? "";
    return productOptionValueColor;
  }

  mapProductOptionValueColorsById(productOptionValueColorDtos: ProductOptionValueColorByIdDto[]) {
    let productOptionValueColors: ProductOptionValueColor[] = [];
    productOptionValueColorDtos.forEach(c => {
      productOptionValueColors.push(this.mapProductOptionValueColorById(c));
    })
    return productOptionValueColors;
  }
  //#endregion

  //#region AllProductOptionColor

  mapAllProductOptionColor(productOptionColorDto: AllProductOptionColorDto) {
    let productOptionColor = new ProductOptionColor();
    productOptionColor.name = productOptionColorDto.name ?? "";
    productOptionColor.slug = productOptionColorDto.slug ?? "";
    productOptionColor.type = 'color';
    productOptionColor.values = this.mapAllProductOptionValueColors(productOptionColorDto.values ?? []);
    return productOptionColor;
  }

  mapAllProductOptionColors(productOptionColorDtos: AllProductOptionColorDto[]) {
    let productOptionColors: ProductOptionColor[] = [];
    productOptionColorDtos.forEach(c => {
      productOptionColors.push(this.mapAllProductOptionColor(c));
    })
    return productOptionColors;
  }

  mapAllProductOptionValueColor(productOptionValueColorDto: AllProductOptionValueColorDto) {
    let productOptionValueColor = new ProductOptionValueColor();
    productOptionValueColor.name = productOptionValueColorDto.name ?? "";
    productOptionValueColor.slug = productOptionValueColorDto.slug ?? "";
    productOptionValueColor.color = productOptionValueColorDto.color ?? "";
    return productOptionValueColor;
  }

  mapAllProductOptionValueColors(productOptionValueColorDtos: AllProductOptionValueColorDto[]) {
    let productOptionValueColors: ProductOptionValueColor[] = [];
    productOptionValueColorDtos.forEach(c => {
      productOptionValueColors.push(this.mapAllProductOptionValueColor(c));
    })
    return productOptionValueColors;
  }
  //#endregion


  

}

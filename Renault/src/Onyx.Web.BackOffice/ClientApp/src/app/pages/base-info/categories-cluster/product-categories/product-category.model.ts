import { CategoryTypeEnum } from "./../../../../web-api-client";
export interface ProductCategoryModel {
  id: number;
  code: string;
  localizedName: string;
  name: string;
  slug: string;
  productCategoryNo: string;
  image: string;
  imageSrc: string;
  menuImage: string;
  menuImageSrc: string;
  productParentCategoryId: number;
  productParentCategoryName: string;
  isPopular: boolean;
  isFeatured: boolean;
  isActive: boolean;
  state: number;
}

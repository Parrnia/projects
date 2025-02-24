import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductsComponent } from './products/products.component';
import { ProductTypeComponent } from '../base-info/products-cluster/product-types/product-type.component';
import { ProductAttributeTypesComponent } from './product-attribute-types/product-attribute-types.component';
import { ProductAttributeOptionColorComponent } from './product-attribute-option-color/product-option-color.component';
import { ProductAttributeGroupComponent } from './product-attribute-group/product-attribute-group.component';
import { ProductAttributeGroupAttributeComponent } from './product-attribute-group-attribute/product-attribute-group-attribute.component';
import { ProductAttributeOptionMaterialComponent } from './product-attribute-option-material/product-option-material.component';


// Component Pages

const routes: Routes = [
  {
    path: "products",
    component: ProductsComponent
  },
  {
    path: "productTypes",
    component: ProductTypeComponent
  },
  {
    path: "productAttributeTypes",
    component: ProductAttributeTypesComponent
  },
  {
    path: "productAttributeGroups",
    component: ProductAttributeGroupComponent
  },
  {
    path: "productAttributeGroupAttributes",
    component: ProductAttributeGroupAttributeComponent
  },
  {
    path: "productOptionColor",
    component:  ProductAttributeOptionColorComponent 

  },
  {
    path: "productOptionMaterial",
    component:  ProductAttributeOptionMaterialComponent 

  },
  // {
  //   path: "productOptionRole",
  //   component:  ProductAttributeOptionRoleComponent 

  // },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class ProductsFormsRoutingModule { }

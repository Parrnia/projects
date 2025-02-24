import { NgModule } from '@angular/core';

// modules (angular)
import { CommonModule } from '@angular/common';
// modules (third-party)
import { TranslateModule } from '@ngx-translate/core';
// modules
import { BlocksModule } from '../blocks/blocks.module';
import { HomeTwoRoutingModule } from './home-two-routing.module';
import { SharedModule } from '../shared/shared.module';

// pages
import { PageTwoComponent } from './pages/page-two/page-two.component';
import { CategorymapperService } from '../../mapServieces/categoriesCluster/categorymapper.service';
import { ProductmapperService } from '../../mapServieces/productsCluster/productmapper.service';
import { VehicleBrandmapperService } from '../../mapServieces/brandsCluster/vehicle-brand-mapper.service';
import { ProductBrandmapperService } from '../../mapServieces/brandsCluster/product-brand-mapper.service';

@NgModule({
    declarations: [
        // pages
        PageTwoComponent,
    ],
    imports: [
        // modules (angular)
        CommonModule,
        // modules (third-party)
        TranslateModule.forChild(),
        // modules
        BlocksModule,
        HomeTwoRoutingModule,
        SharedModule,
    ],
    providers : [
        ProductmapperService,
        CategorymapperService,
        ProductBrandmapperService,
        VehicleBrandmapperService,
    ]
})
export class HomeTwoModule { }

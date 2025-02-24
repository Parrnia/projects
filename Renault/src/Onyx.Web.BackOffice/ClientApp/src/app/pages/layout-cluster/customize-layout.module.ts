import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomizeLayoutRoutingModule } from './customize-layout.routing';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbAlertModule, NgbDropdownModule, NgbPaginationModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { TablesRoutingModule } from '../tables/tables-routing.module';
import { FlatpickrModule } from 'angularx-flatpickr';
import { defineElement } from 'lord-icon-element';
import lottie from 'lottie-web';
import { CarouselModule } from './carousels/carousel.module';
import { FooterLinkContainerModule } from './footers/footer-link-containers/footer-link-container.module';
import { SocialLinkModule } from './footers/social-links/social-link.module';
import { BlockBannerModule } from './block-banners/block-banner.module';
import { ThemeModule } from './themes/theme.module';


@NgModule({
  declarations: [
    
  ],
  imports: [
    CommonModule,
    CustomizeLayoutRoutingModule,
    CommonModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgbDropdownModule,
    NgbPaginationModule,
    NgbTypeaheadModule,
    NgbAlertModule,
    FlatpickrModule,
    TablesRoutingModule,
    SharedModule,
    BlockBannerModule,
    CarouselModule,
    FooterLinkContainerModule,
    ThemeModule,
    SocialLinkModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})

export class CustomizeLayoutModule { 
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}

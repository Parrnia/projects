import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Component Pages

import { CarouselComponent } from './carousels/carousel.component';
import { FooterLinkContainerComponent } from './footers/footer-link-containers/footer-link-container.component';
import { SocialLinkComponent } from './footers/social-links/social-link.component';
import { BlockBannerComponent } from './block-banners/block-banner.component';
import { ThemeComponent } from './themes/theme.component';

const routes: Routes = [
  {
    path: "blockBanners",
    component: BlockBannerComponent
  },
  {
    path: "carousels",
    component: CarouselComponent
  },
  {
    path: "footerLinkContainers",
    component: FooterLinkContainerComponent
  },
  {
    path: "socialLinks",
    component: SocialLinkComponent
  },
  {
    path: "themes",
    component: ThemeComponent
  },
  // {
  //   path: "header",
  //   component: HeaderComponent
  // },
  // {
  //   path: "topmenu",
  //   component: TopmenuComponent
  // },
  // {
  //   path: "categoriesMenu",
  //   component: CategoriesMenuComponent
  // },
  // {
  //   path: "mainMenu",
  //   component: MainMenuComponent
    
  // }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class CustomizeLayoutRoutingModule { }

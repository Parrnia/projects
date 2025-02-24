import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import {
  NgbNavModule,
  NgbAccordionModule,
  NgbDropdownModule,
  NgbActiveModal,
} from "@ng-bootstrap/ng-bootstrap";

// Swiper Slider
import { NgxUsefulSwiperModule } from "ngx-useful-swiper";

// Counter
import { CountToModule } from "angular-count-to";

import { BreadcrumbsComponent } from "./breadcrumbs/breadcrumbs.component";
import { ClientLogoComponent } from "./landing/index/client-logo/client-logo.component";
import { ServicesComponent } from "./landing/index/services/services.component";
import { CollectionComponent } from "./landing/index/collection/collection.component";
import { CtaComponent } from "./landing/index/cta/cta.component";
import { DesignedComponent } from "./landing/index/designed/designed.component";
import { PlanComponent } from "./landing/index/plan/plan.component";
import { FaqsComponent } from "./landing/index/faqs/faqs.component";
import { ReviewComponent } from "./landing/index/review/review.component";
import { CounterComponent } from "./landing/index/counter/counter.component";
import { WorkProcessComponent } from "./landing/index/work-process/work-process.component";
import { TeamComponent } from "./landing/index/team/team.component";
import { ContactComponent } from "./landing/index/contact/contact.component";
import { FooterComponent } from "./landing/index/footer/footer.component";
import { ScrollspyDirective } from "./scrollspy.directive";

// NFT Landing
import { MarketPlaceComponent } from "./landing/nft/market-place/market-place.component";
import { WalletComponent } from "./landing/nft/wallet/wallet.component";
import { FeaturesComponent } from "./landing/nft/features/features.component";
import { CategoriesComponent } from "./landing/nft/categories/categories.component";
import { DiscoverComponent } from "./landing/nft/discover/discover.component";
import { TopCreatorComponent } from "./landing/nft/top-creator/top-creator.component";

// Job Landing
import { ProcessComponent } from "./landing/job/process/process.component";
import { FindjobsComponent } from "./landing/job/findjobs/findjobs.component";
import { CandidatesComponent } from "./landing/job/candidates/candidates.component";
import { BlogComponent } from "./landing/job/blog/blog.component";
import { JobcategoriesComponent } from "./landing/job/jobcategories/jobcategories.component";
import { JobFooterComponent } from "./landing/job/job-footer/job-footer.component";
import { ImageInputComponent } from "./form/imageInput/image-input.component";
import { FileUploadComponent } from "./form/file-upload/file-upload.component";
// import { ExportExcelFormComponent } from "./components/exportExcelForm/export-excel-form/export-excel-form.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatDatepickerModule } from "@angular/material/datepicker";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatNativeDateModule } from "@angular/material/core";
import { ExportExcelModalComponent } from './components/exportForm/export-excel-modal/export-excel-modal.component';
import { NgPersianDatepickerModule } from "ng-persian-datepicker";
@NgModule({
  declarations: [
    BreadcrumbsComponent,
    ClientLogoComponent,
    ServicesComponent,
    CollectionComponent,
    CtaComponent,
    DesignedComponent,
    PlanComponent,
    FaqsComponent,
    ReviewComponent,
    CounterComponent,
    WorkProcessComponent,
    TeamComponent,
    ContactComponent,
    FooterComponent,
    ScrollspyDirective,
    MarketPlaceComponent,
    WalletComponent,
    FeaturesComponent,
    CategoriesComponent,
    DiscoverComponent,
    TopCreatorComponent,
    ProcessComponent,
    FindjobsComponent,
    CandidatesComponent,
    BlogComponent,
    JobcategoriesComponent,
    JobFooterComponent,
    ImageInputComponent,
    FileUploadComponent,
    ExportExcelModalComponent,
  ],
  imports: [
    CommonModule,
    NgbNavModule,
    NgbAccordionModule,
    NgbDropdownModule,
    NgxUsefulSwiperModule,
    CountToModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatInputModule,
    MatNativeDateModule,
    NgPersianDatepickerModule,
    FormsModule,
  ],
  exports: [
    BreadcrumbsComponent,
    ClientLogoComponent,
    ServicesComponent,
    CollectionComponent,
    CtaComponent,
    DesignedComponent,
    PlanComponent,
    FaqsComponent,
    ReviewComponent,
    CounterComponent,
    WorkProcessComponent,
    TeamComponent,
    ContactComponent,
    FooterComponent,
    ScrollspyDirective,
    WalletComponent,
    MarketPlaceComponent,
    FeaturesComponent,
    CategoriesComponent,
    DiscoverComponent,
    TopCreatorComponent,
    ProcessComponent,
    FindjobsComponent,
    CandidatesComponent,
    BlogComponent,
    JobcategoriesComponent,
    JobFooterComponent,
    ImageInputComponent,
    FileUploadComponent,
    ExportExcelModalComponent,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class SharedModule {}

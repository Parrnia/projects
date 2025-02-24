import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";

// search module
import { Ng2SearchPipeModule } from "ng2-search-filter";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";

import { LayoutsModule } from "./layouts/layouts.module";
import { PagesModule } from "./pages/pages.module";

// Auth
import {
  HttpClientModule,
  HttpClient,
  HTTP_INTERCEPTORS,
} from "@angular/common/http";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { environment } from "../environments/environment";
import { initFirebaseBackend } from "./authUtils";
// Language
import { TranslateHttpLoader } from "@ngx-translate/http-loader";
import {
  TranslateModule,
  TranslateLoader,
  TranslateService,
} from "@ngx-translate/core";

import { ErrorHandlingInterceptor } from "./core/services/interceptors/error-handling.interceptor";
import { AuthInterceptor } from "./core/services/interceptors/auth.interceptor";
import { ToastrModule } from "ngx-toastr";
import { NgbToastModule } from "@ng-bootstrap/ng-bootstrap";
import { ToastsContainer } from "./core/services/toastService/toasts-container.component";

export function createTranslateLoader(http: HttpClient): any {
  return new TranslateHttpLoader(http, "assets/i18n/", ".json");
}

if (environment.defaultauth === "firebase") {
  initFirebaseBackend(environment.firebaseConfig);
} else {
}

@NgModule({
  declarations: [AppComponent, ToastsContainer],
  imports: [
    BrowserAnimationsModule,
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    LayoutsModule,
    PagesModule,
    Ng2SearchPipeModule,
    NgbToastModule,
    ToastrModule.forRoot(),
    TranslateModule.forRoot({
      defaultLanguage: "en",
      loader: {
        provide: TranslateLoader,
        useFactory: createTranslateLoader,
        deps: [HttpClient],
      },
    }),
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorHandlingInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}

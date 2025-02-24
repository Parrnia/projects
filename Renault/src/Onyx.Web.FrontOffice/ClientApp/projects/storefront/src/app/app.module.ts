import { NgModule } from '@angular/core';
import { filter } from 'rxjs/operators';
import { ViewportScroller } from '@angular/common';
import { Router, Scroll, Event, RouterModule } from '@angular/router';

// modules (angular)
import { BrowserModule } from '@angular/platform-browser';
// modules (third-party)
import { ModalModule } from 'ngx-bootstrap/modal';
import { ToastrModule } from 'ngx-toastr';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { TranslateModule } from '@ngx-translate/core';
// modules
import { AppRoutingModule } from './app-routing.module';
import { CurrencyModule } from './modules/currency/currency.module';
import { FakeApiModule } from './api';
import { FooterModule } from './modules/footer/footer.module';
import { HeaderModule } from './modules/header/header.module';
import { LanguageModule } from './modules/language/language.module';
import { MobileModule } from './modules/mobile/mobile.module';
import { SharedModule } from './modules/shared/shared.module';

// components
import { AppComponent } from './app.component';
import { RootComponent } from './components/root/root.component';

// pages
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoadingInterceptor } from './services/interceptors/loading.interceptor';
import { LoadingService } from './services/loader.service';
import { AuthService } from './services/authService/auth.service';
import { AuthInterceptor } from './services/interceptors/auth.interceptor';
import { ErrorHandlingInterceptor } from './services/interceptors/error-handling.interceptor';


@NgModule({
    declarations: [
        // components
        AppComponent,
        RootComponent,
        // pages
        PageNotFoundComponent,
    ],
    imports: [
        // modules (angular)
        BrowserModule.withServerTransition({ appId: 'serverApp' }),
        // modules (third-party)
        ModalModule.forRoot(),
        ToastrModule.forRoot(),
        TooltipModule.forRoot(),
        TranslateModule.forChild(),
        // modules
        AppRoutingModule,
        CurrencyModule.config({
            default: 'تومان',
            currencies: [
                {
                    symbol: '',
                    name: 'تومان',
                    code: 'تومان',
                    rate: 1,
                    formatFn: (value) => { return value.toLocaleString() + 'تومان'; }
                },
                // {
                //     symbol: '$',
                //     name: 'US Dollar',
                //     code: 'USD',
                //     rate: 1,
                // },
                // {
                //     symbol: '£',
                //     name: 'Pound Sterling',
                //     code: 'GBP',
                //     rate: 0.78,
                // },
                // {
                //     symbol: '€',
                //     name: 'Euro',
                //     code: 'EUR',
                //     rate: 0.92,
                // },
                // {
                //     symbol: '₽',
                //     name: 'Russian Ruble',
                //     code: 'RUB',
                //     rate: 64,
                //     formatFn: (value) => {
                //         const digits = [].slice.call(Math.round(value).toFixed()).reverse();
                //         const parts = [];

                //         while (digits.length) {
                //             parts.push(digits.splice(0, 3).reverse().join(''));
                //         }

                //         return parts.reverse().join(' ') + ' руб';
                //     },
                // },
            ],
        }),
        FakeApiModule,
        FooterModule,
        HeaderModule,
        LanguageModule.config({
            default: 'fa',
            languages: [
                {
                    code: 'fa',
                    name: 'Persian',
                    image: 'assets/images/languages/language-1.png',
                    direction: 'rtl',
                }
            ],
        }),
        MobileModule,
        SharedModule
    ],
    providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: LoadingInterceptor,
            multi: true,
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthInterceptor,
            multi: true,
        },
        {
            provide: HTTP_INTERCEPTORS,
            useClass: ErrorHandlingInterceptor,
            multi: true,
        },
        LoadingService,
        AuthService
    ],
})
export class AppModule {
    constructor(router: Router, viewportScroller: ViewportScroller) {
        router.events.pipe(
            filter((e: Event): e is Scroll => e instanceof Scroll),
        ).subscribe(e => {
            if (e.position) {
                viewportScroller.scrollToPosition(e.position);
            } else if (!e.anchor) {
                viewportScroller.scrollToPosition([0, 0]);
            }
        });
    }
}

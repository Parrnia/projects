import { Component, Inject, NgZone, OnDestroy, OnInit, PLATFORM_ID, Renderer2 } from '@angular/core';
import { LanguageService } from './modules/language/services/language.service';
import { ToastrService } from 'ngx-toastr';
import { CartService } from './services/cart.service';
import { CompareService } from './services/compare.service';
import { WishlistService } from './services/wishlist.service';
import { DOCUMENT, isPlatformBrowser } from '@angular/common';
import { filter, first, takeUntil } from 'rxjs/operators';
import { NavigationEnd, Router } from '@angular/router';
import { Observable, Subject } from 'rxjs';
import { VehicleApi } from './api';
import { CurrentVehicleService } from './services/current-vehicle.service';
import { TranslateService } from '@ngx-translate/core';
@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    constructor(
        @Inject(DOCUMENT) private document: Document,
        @Inject(PLATFORM_ID) private platformId: any,
        private renderer: Renderer2,
        private router: Router,
        private zone: NgZone,
        // Services omitted for brevity
    ) {}

    ngOnInit(): void {
        if (isPlatformBrowser(this.platformId)) {
            this.zone.runOutsideAngular(() => {
                this.router.events.pipe(
                    filter(event => event instanceof NavigationEnd),
                    first(),
                ).subscribe(() => {
                    const preloader = this.document.querySelector('.site-preloader') as HTMLElement;

                    if (!preloader) {
                        return;
                    }

                    const preloaderStyle = this.document.querySelector('.site-preloader-style') as HTMLElement;

                    // Add fade class with a delay fallback
                    preloader.classList.add('site-preloader__fade');
                    
                    // Ensure the preloader is removed even if transitionend doesn't fire
                    const fadeDuration = parseFloat(
                        getComputedStyle(preloader).getPropertyValue('transition-duration') || '0'
                    ) * 1000;

                    setTimeout(() => {
                        preloader.remove();
                        preloaderStyle?.remove();
                    }, fadeDuration + 500); // Adding a buffer of 500ms
                });
            });
        }
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
}
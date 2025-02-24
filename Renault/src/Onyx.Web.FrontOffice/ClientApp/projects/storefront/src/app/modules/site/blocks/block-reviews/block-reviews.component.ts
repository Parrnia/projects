import { Component, OnInit, OnDestroy, HostBinding, ChangeDetectorRef } from "@angular/core";
import { OwlCarouselOConfig } from "ngx-owl-carousel-o/lib/carousel/owl-carousel-o-config";
import { ImageService } from "projects/storefront/src/app/mapServieces/image.service";
import { TestimonialsClient } from "projects/storefront/src/app/web-api-client";
import { Subject, switchMap, of, timer, takeUntil } from "rxjs";
import { LanguageService } from "../../../language/services/language.service";
import { Testimonial } from "projects/storefront/src/app/interfaces/testimonial";


@Component({
    selector: 'app-block-reviews',
    templateUrl: './block-reviews.component.html',
    styleUrls: ['./block-reviews.component.scss'],
})
export class BlockReviewsComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    showCarousel = true;

    testimonials: Testimonial[] = [];

    carouselOptions!: Partial<OwlCarouselOConfig>;

    @HostBinding('class.block') classBlock = true;

    @HostBinding('class.block-reviews') classBlockReviews = true;

    constructor(
        private language: LanguageService,
        private cd: ChangeDetectorRef,
        private testimonialsClient: TestimonialsClient,
        private imageService: ImageService
    ) { }

    ngOnInit(): void {
        this.testimonialsClient.getAllTestimonials().subscribe(c => {
            this.testimonials = c;
            this.testimonials.forEach(e => {
                e.avatar = this.imageService.makeImageUrl(e.avatar)
            });
        });

        this.initOptions();

        // Since ngx-owl-carousel-o cannot re-initialize itself, we will do it manually when the direction changes.
        this.language.directionChange$.pipe(
            switchMap(() => timer(250)),
            takeUntil(this.destroy$),
        ).subscribe(() => {
            this.initOptions();

            this.showCarousel = false;
            this.cd.detectChanges();
            this.showCarousel = true;
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    initOptions(): void {
        this.carouselOptions = {
            dots: true,
            margin: 20,
            items: 1,
            loop: true,
            rtl: this.language.isRTL(),
            responsive: {
                0: { items: 1 },
            },
        };
    }
}

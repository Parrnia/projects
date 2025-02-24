import { Component, HostBinding, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { PageProductLayout } from '../../pages/page-product/page-product.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ShopApi } from '../../../../api';
import { Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { ReviewsListComponent } from '../reviews-list/reviews-list.component';
import { ReviewsClient } from 'projects/storefront/src/app/web-api-client';
import { ReviewmapperService } from 'projects/storefront/src/app/mapServieces/productsCluster/reviewmapper.service';
import { Review } from 'projects/storefront/src/app/interfaces/review';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';

@Component({
    selector: 'app-reviews-view',
    templateUrl: './reviews-view.component.html',
    styleUrls: ['./reviews-view.component.scss'],
})
export class ReviewsViewComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    submitInProgress = false;

    form!: FormGroup;

    isAuthenticated: boolean = false;
    @Input() productId!: number;

    @Input() productPageLayout: PageProductLayout = 'full';

    @HostBinding('class.reviews-view') classReviewsView = true;

    @ViewChild(ReviewsListComponent) list!: ReviewsListComponent;

    constructor(
        private fb: FormBuilder,
        private toastr: ToastrService,
        private translate: TranslateService,
        private reviewsClient: ReviewsClient,
        private reviewmapperService: ReviewmapperService,
        private authService: AuthService
    ) {
        this.authService.isLoggedIn().subscribe((isAuthenticated) => {
            this.isAuthenticated = isAuthenticated;
        });
    }

    ngOnInit(): void {
        this.form = this.fb.group({
            rating: ['', [Validators.required]],
            content: ['', [Validators.required]],
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    submit(): void {
        this.form.markAllAsTouched();
         
        if (this.submitInProgress || this.form.invalid) {
            return;
        }

        if (this.isAuthenticated) {
            this.submitInProgress = true;
             
            const formValue = this.form.value;
            let review = new Review();
            review.rating = parseFloat(formValue.rating);
            review.author = localStorage.getItem('name')!;
            review.content = formValue.content;
            this.reviewsClient.create(this.reviewmapperService.mapReview(review, this.productId, localStorage.getItem('userId')!)).pipe(
                finalize(() => this.submitInProgress = false),
                takeUntil(this.destroy$),
            ).subscribe(() => {
                this.form.reset({
                    rating: '',
                    content: '',
                });
                this.list.reload();
                this.toastr.success(this.translate.instant('TEXT_TOAST_REVIEW_ADDED'));
            });
        } else {
            this.toastr.error('لطفا برای ثبت دیدگاه وارد شوید');
        }

    }
}

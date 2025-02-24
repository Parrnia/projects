import { AfterViewInit, Component, OnDestroy, ViewChild } from '@angular/core';
import { Subject, timer } from 'rxjs';
import { Product, ProductAttributeOption, ProductStock } from '../../../../interfaces/product';
import { QuickviewService } from '../../../../services/quickview.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { filter, finalize, switchMap, takeUntil } from 'rxjs/operators';
import { UrlService } from '../../../../services/url.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { CartService } from '../../../../services/cart.service';
import { NavigationStart, Router } from '@angular/router';

@Component({
    selector: 'app-quickview',
    templateUrl: './quickview.component.html',
    styleUrls: ['./quickview.component.scss'],
})
export class QuickviewComponent implements OnDestroy, AfterViewInit {
    private destroy$: Subject<void> = new Subject();

    showGallery = false;

    product: Product|null = null;

    form!: FormGroup;

    addToCartInProgress = false;


    selectedColor : string | undefined = '';
    selectedMaterail : string | undefined = '';
    selectedAttributeOption! : ProductAttributeOption;
    selectedPrice : number = -1;
    selectedCompareAtPrice? : number;
    selectedAvailibility : ProductStock = 'out-of-stock';
    selectedBadges : string[] = [];
    @ViewChild('modal') modal!: ModalDirective;

    constructor(
        private fb: FormBuilder,
        private quickview: QuickviewService,
        private translate: TranslateService,
        private cart: CartService,
        private router: Router,
        public url: UrlService,
    ) { }

    ngAfterViewInit(): void {
        this.quickview.show$.pipe(
            switchMap(product => {
                this.modal.show();
                this.product = product;

                this.form = this.fb.group({
                    options: [{}],
                    quantity: [product.selectedAttributeOption?.minOrderQuantityPerOrder ?? 1, [Validators.required]],
                });

                // We are waiting for when the content will be rendered.
                // 150 = BACKDROP_TRANSITION_DURATION
                return timer(150);
            }),
            takeUntil(this.destroy$),
        ).subscribe(() => {
            // Show gallery only after content is rendered.
            this.showGallery = this.product !== null;
        });

        this.router.events.pipe(
            filter(event => event instanceof NavigationStart),
            takeUntil(this.destroy$),
        ).subscribe(() => {
            if (this.modal && this.modal.isShown) {
                this.modal.hide();
            }
        });

        this.modal.onHidden.pipe(takeUntil(this.destroy$)).subscribe(() => {
             
            this.product = null;
            this.showGallery = false;
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    addToCart(): void {
        const product = this.product;

        if (!product) {
            return;
        }
        if (this.addToCartInProgress) {
            return;
        }
        if (this.form.get('quantity')!.invalid) {
            alert(this.translate.instant('ERROR_ADD_TO_CART_QUANTITY'));
            return;
        }
        if (this.form.get('options')!.invalid) {
            alert(this.translate.instant('ERROR_ADD_TO_CART_OPTIONS'));
            return;
        }

        const options: {name: string; value: string}[] = [];
        const formOptions = this.form.get('options')!.value;

        Object.keys(formOptions).forEach(optionSlug => {
            const option = product.options.find(x => x.slug === optionSlug);

            if (!option) {
                return;
            }

            const value = option.values.find(x => x.slug === formOptions[optionSlug]);

            if (!value) {
                return;
            }

            options.push({ name: option.name, value: value.name });
        });

        this.addToCartInProgress = true;

        this.cart.add(product, this.form.get('quantity')!.value, options).pipe(
            finalize(() => this.addToCartInProgress = false),
        ).subscribe();
    }
    setProductSelectedOptions(values : (string | undefined)[]){
         
        this.product!.selectedAttributeOption = this.product?.attributeOptions.find(e => (
            ((values[0] == undefined) || (e.optionValues.find(r => r.name == 'Color'))?.value.toLowerCase() == values[0]?.toLowerCase()))
        && ((values[1] == undefined) || (e.optionValues.find(r => r.name == 'Material'))?.value.toLowerCase() == values[1]?.toLowerCase())!)!;
        this.setSelectedOptions(values);
         
    }
    setSelectedOptions(values : (string | undefined)[]){
         
        this.selectedColor = values[0];
        this.selectedMaterail = values[1];
        this.selectedPrice = this.product!.selectedAttributeOption?.price;
        this.selectedCompareAtPrice = this.product?.selectedAttributeOption?.compareAtPrice;
        this.selectedBadges = this.product?.selectedAttributeOption?.badges ?? [];
        this.selectedAvailibility = this.product?.selectedAttributeOption?.availability ?? 'out-of-stock';
        if(this.selectedColor){
            this.product!.attributes.find(e => e.name == 'Color')?.values.splice(0);
            this.product!.attributes.find(e => e.name == 'Color')?.values.push({ name : this.selectedColor, slug : this.selectedColor});
        }
        if(this.selectedMaterail){
            this.product!.attributes.find(e => e.name == 'Material')?.values.splice(0);
            this.product!.attributes.find(e => e.name == 'Material')?.values.push({ name : this.selectedMaterail, slug : this.selectedMaterail});
        }
    }
}

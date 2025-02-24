import { Subject, takeUntil } from 'rxjs';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { CartItem, CartService } from '../../../../services/cart.service';
import { FormControl } from '@angular/forms';

import { UrlService } from '../../../../services/url.service';

@Component({
    selector: 'app-page-cart',
    templateUrl: './page-cart.component.html',
    styleUrls: ['./page-cart.component.scss'],
})
export class PageCartComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject();

    items: CartItem[] = [];
    quantityControls: FormControl[] = [];
    showSupportMessageForProductDiscount = false;
    showSupportMessageForCustomerDiscount = false;
    showGeneralSupportMessage = false;
    updating = false;

    constructor(
        public cart: CartService,
        public url: UrlService,
    ) { 
        this.cart.getProductsForNewIdentityState().subscribe({
            next: (res) => this.cart.calc()
        });
    }

    ngOnInit(): void {
        this.cart.items$.pipe(takeUntil(this.destroy$)).subscribe(items => {
            this.items = items;
            this.quantityControls = items.map(item => new FormControl(this.getQuantity(item)));
             this.update();
        });

        // Subscribe to totals$ to check for discounts
        this.cart.totals$.pipe(takeUntil(this.destroy$)).subscribe(totals => {
            this.checkDiscounts(totals);
        });

    } 

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    update(): void {
         
        const entries = [];
         
        for (let i = 0; i < this.items.length; i++) {
            const item = this.items[i];
            const quantityControl = this.quantityControls[i];
             
            if (item.quantity !== quantityControl.value) {
                entries.push({
                    item,
                    quantity: quantityControl.value,
                });
            }
        }

        if (entries.length <= 0) {
            return;
        }

        this.updating = true;
        this.cart.update(entries).pipe(takeUntil(this.destroy$)).subscribe({
            complete: () => this.updating = false,
        });
    }

    needUpdate(): boolean {
        let needUpdate = false;

        for (let i = 0; i < this.items.length; i++) {
            const item = this.items[i];
            const quantityControl = this.quantityControls[i];

            if (!quantityControl.valid) {
                return false;
            }

            if (quantityControl.value !== item.quantity) {
                needUpdate = true;
            }
        }

        return needUpdate;
    }
    parseStringToNumber(value: string | number): number {
        return Number(value);
    }
    checkDiscounts(totals: any[]): void {
        // Find and check discount types
        const discountOnProduct = totals.find(total => total.type === 'discountOnProduct');
        const discountOnCustomerType = totals.find(total => total.type === 'discountOnCustomerType');
    
        // Determine if discounts are zero
        const isProductDiscountZero = discountOnProduct?.price === 0;
        const isCustomerDiscountZero = discountOnCustomerType?.price === 0;
    
        // Set message flags
        this.showSupportMessageForProductDiscount = isProductDiscountZero;
        this.showSupportMessageForCustomerDiscount = isCustomerDiscountZero;
        this.showGeneralSupportMessage = isProductDiscountZero && isCustomerDiscountZero; 
    }
    
    private getQuantity(item : CartItem) {
        if(item.quantity < item.product.selectedAttributeOption?.minOrderQuantityPerOrder){
            return item.product.selectedAttributeOption?.minOrderQuantityPerOrder;
        }
        if(item.quantity > item.product.selectedAttributeOption?.maxOrderQuantityPerOrder){
            return item.product.selectedAttributeOption?.maxOrderQuantityPerOrder;
        }
        return item.quantity;
    }
}

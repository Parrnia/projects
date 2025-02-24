import { Component, NgZone, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { concatMap, filter, finalize, map, pairwise, startWith, switchMap, takeUntil, tap } from 'rxjs/operators';
import { ICreateOrderRequest, IPayPalConfig } from 'ngx-paypal';
import { BsModalService } from 'ngx-bootstrap/modal';
import { TermsModalComponent } from '../../../shared/components/terms-modal/terms-modal.component';
import { CartItem, CartService, CartTotal } from '../../../../services/cart.service';
import { forkJoin, Observable, of, Subject } from 'rxjs';
import { Router } from '@angular/router';
import { AddressFormComponent } from '../../../shared/components/address-form/address-form.component';
import { RegisterFormComponent } from '../../../shared/components/register-form/register-form.component';
import { TranslateService } from '@ngx-translate/core';
import { AccountApi, CheckoutData } from '../../../../api';
import { environment } from '../../../../../environments/environment';
import { Address, AddressData } from '../../../../interfaces/address';
import { UrlService } from '../../../../services/url.service';
import { AddressesClient, CreateAddressCommand, OrdersClient, PaymentType, CustomerTypeEnum, OrderPaymentType, CreateOrderCommand } from 'projects/storefront/src/app/web-api-client';
import { Customer } from 'projects/storefront/src/app/interfaces/user';
import { OrdermapperService } from 'projects/storefront/src/app/mapServieces/ordersCluster/ordermapper.service';
import { AddressMapperService } from 'projects/storefront/src/app/mapServieces/userProfilesCluster/address-mapper.service';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';
import { ToastrService } from 'ngx-toastr';


@Component({
    selector: 'app-page-checkout',
    templateUrl: './page-checkout.component.html',
    styleUrls: ['./page-checkout.component.scss'],
})
export class PageCheckoutComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    private checkout$: Subject<CheckoutData> = new Subject<CheckoutData>();

    items: CartItem[] = [];

    form: FormGroup;

    checkoutInProgress = false;

    payments = [
        {
            name: PaymentType.Credit,
            label: 'TEXT_PAYMENT_CREDIT_LABEL',
            description: 'TEXT_PAYMENT_CREDIT_DESCRIPTION',
        },
        {
            name: PaymentType.Online,
            label: 'TEXT_PAYMENT_ONLINE_LABEL',
            description: 'TEXT_PAYMENT_ONLINE_DESCRIPTION',
        }
    ];

    @ViewChild('billingAddressForm', { read: AddressFormComponent }) billingAddressForm!: AddressFormComponent;

    @ViewChild('shippingAddressForm', { read: AddressFormComponent }) shippingAddressForm!: AddressFormComponent;

    @ViewChild('registerForm', { read: RegisterFormComponent }) registerForm!: RegisterFormComponent;

    enablePaypalButton = () => { };
    disablePaypalButton = () => { };

    constructor(
        private fb: FormBuilder,
        private modalService: BsModalService,
        private router: Router,
        private translate: TranslateService,
        private zone: NgZone,
        public url: UrlService,
        public cart: CartService,
        private ordersClient: OrdersClient,
        private addressesClient: AddressesClient,
        private addressMapperService: AddressMapperService,
        private ordermapperService: OrdermapperService,
        public authService: AuthService,
        private toastr: ToastrService,
    ) {

        this.cart.getProductsForNewIdentityState().subscribe({
            next: (res) => this.cart.calc()
        });

        this.form = this.fb.group({
            billingAddress: [{}],
            comments: [''],
            //paymentMethod: ['', Validators.required],
            agree: [false, [Validators.requiredTrue]],
        });

        this.form.valueChanges.pipe(
            startWith(of(this.form.value)),
            pairwise(),
        ).subscribe(([oldValue, newValue]) => {

        });
    }



    ngOnInit(): void {
        this.cart.quantity$
            .pipe(
                filter(x => x === 0),
                takeUntil(this.destroy$),
            )
            .subscribe(() => this.router.navigateByUrl('/shop/cart').then());


        this.cart.items$.pipe(takeUntil(this.destroy$)).subscribe(items => {
            this.items = items;
            this.update(items);
        });

        this.checkout$
            .pipe(
                tap(() => this.checkoutInProgress = true),
                switchMap(checkoutData => {

                    let subtotal = checkoutData.items.reduce((total, item) => total + item.quantity * item.price, 0);
                    debugger;
                    let createOrderCommand = new CreateOrderCommand({
                        quantity: checkoutData.items.reduce((total, item) => total + item.quantity, 0),
                        subtotal: subtotal,
                        total: subtotal + checkoutData.totals.reduce((total, item) => total + item.price, 0),
                        title: checkoutData.title,
                        company: checkoutData.company,
                        countryId: checkoutData.countryId,
                        addressDetails1: checkoutData.address1,
                        addressDetails2: checkoutData.address2,
                        city: checkoutData.city,
                        state: checkoutData.state,
                        postcode: checkoutData.postcode,
                        customerId: undefined,
                        orderItems: this.ordermapperService.mapOrderItemCommands(checkoutData.items),
                        customerTypeEnum: undefined,
                        personType: undefined,
                        phoneNumber: undefined,
                        firstName: undefined,
                        lastName: undefined,
                        nationalCode: undefined
                    });

                    return this.ordersClient.createOrder(createOrderCommand);
                }),
                takeUntil(this.destroy$),
            )
            .subscribe(orderId => {

                if (orderId == 0) {
                    this.toastr.error("شیوه پرداخت نامعتبر است")
                }
                localStorage.removeItem('cartItems');
                this.cart.items = [];
                this.cart.load();
                this.cart.save();
                this.cart.calc();
                this.router.navigateByUrl(`/shop/checkout/${orderId}`).then();
            });
    }


    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    toggleFormControl(controlName: string, isEnabled: boolean): void {
        const control = this.form.get(controlName);

        if (!control) {
            throw new Error('Control not found');
        }

        if (isEnabled) {
            control.enable({ emitEvent: false });
        } else {
            control.disable({ emitEvent: false });
        }
    }

    showTerms(event: MouseEvent): void {
        event.preventDefault();

        this.modalService.show(TermsModalComponent, { class: 'modal-lg' });
    }

    createOrder(): void {
        if (!this.checkData()) {
            return;
        }

        this.checkout();
    }

    private markAllAsTouched(): void {

        this.form.markAllAsTouched();
        this.billingAddressForm.markAsTouched();

        if (this.form.value.createAccount) {
            this.registerForm.markAsTouched();
        }

    }

    private checkData(): boolean {
        this.markAllAsTouched();

        if (this.form.invalid) {
            this.toastr.error(this.translate.instant('ERROR_CHECKOUT'));
        }

        return this.form.valid;
    }

    private checkout(): void {
        const value = this.form.value;

        let billingAddressForSave!: Address;
        this.authService.isLoggedIn().subscribe({
            next: (res) => {
                if (res) {
                    this.addressesClient.getAddressById(value.billingAddress.addressForm).subscribe(res => {

                        billingAddressForSave = this.addressMapperService.mapAddress(res);
                        debugger;
                        let totals: CartTotal[] = [];
                        this.cart.totals$.subscribe(c => totals = c);
                        const checkoutData: CheckoutData = {
                            //payment: OrderPaymentType.Unspecified,
                            items: this.cart.items.map((item) => ({
                                price: item.product.selectedAttributeOption.compareAtPrice ?? item.product.selectedAttributeOption.price,
                                quantity: item.quantity,
                                productId: item.product.id,
                                productSevenId: item.product.sevenId,
                                options: item.product.selectedAttributeOption.optionValues.map(e => ({ name: e.name, value: e.value, orderItemId: -1 })),
                                selectedProductAttributeOption: item.product.selectedAttributeOption
                            })),
                            title: billingAddressForSave.title,
                            company: billingAddressForSave.company,
                            country: billingAddressForSave.country,
                            countryId: billingAddressForSave.countryId,
                            address1: billingAddressForSave.address1,
                            address2: billingAddressForSave.address2,
                            city: billingAddressForSave.city,
                            state: billingAddressForSave.state,
                            postcode: billingAddressForSave.postcode,
                            comment: value.comment,
                            totals: totals,
                            customerId: localStorage.getItem('userId')!
                        };

                        this.checkout$.next(checkoutData);
                    });
                } else {
                    billingAddressForSave = value.billingAddress;

                    let totals: CartTotal[] = [];
                    this.cart.totals$.subscribe((c) => (totals = c));
                    const checkoutData: CheckoutData = {
                        //payment: value.paymentMethod,
                        items: this.cart.items.map((item) => ({
                            price: item.product.selectedAttributeOption.compareAtPrice ?? item.product.selectedAttributeOption.price,
                            quantity: item.quantity,
                            productId: item.product.id,
                            productSevenId: item.product.sevenId,
                            options: item.product.selectedAttributeOption.optionValues.map(e => ({ name: e.name, value: e.value, orderItemId: -1 })),
                            selectedProductAttributeOption: item.product.selectedAttributeOption,
                            orderId: -1
                        })),
                        title: billingAddressForSave.title,
                        company: billingAddressForSave.company,
                        country: billingAddressForSave.country,
                        countryId: billingAddressForSave.countryId,
                        address1: billingAddressForSave.address1,
                        address2: billingAddressForSave.address2,
                        city: billingAddressForSave.city,
                        state: billingAddressForSave.state,
                        postcode: billingAddressForSave.postcode,
                        comment: value.comment,
                        totals: totals,
                        customerId: undefined
                    };

                    this.checkout$.next(checkoutData);
                }
            }
        });
    }

    login() {
        this.router.navigateByUrl('/account/login').then();
    }
    update(items: CartItem[]): void {

        const entries = [];

        for (let i = 0; i < this.items.length; i++) {
            const item = this.items[i];
            const quantity = this.getQuantity(item);

            if (item.quantity !== quantity) {
                entries.push({
                    item,
                    quantity: quantity,
                });
            }
        }

        if (entries.length <= 0) {
            return;
        }

        this.cart.update(entries).pipe(takeUntil(this.destroy$)).subscribe();
    }
    private getQuantity(item: CartItem) {
        if (item.quantity < item.product.selectedAttributeOption?.minOrderQuantityPerOrder) {
            return item.product.selectedAttributeOption?.minOrderQuantityPerOrder;
        }
        if (item.quantity > item.product.selectedAttributeOption?.maxOrderQuantityPerOrder) {
            return item.product.selectedAttributeOption?.maxOrderQuantityPerOrder;
        }
        return item.quantity;
    }
}


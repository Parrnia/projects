import { switchMap } from 'rxjs/operators';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Product } from '../interfaces/product';
import { isPlatformBrowser } from '@angular/common';
import { BehaviorSubject, Subject, Observable, of, map, forkJoin } from 'rxjs';
import { AuthService } from './authService/auth.service';
import {
    CustomerTypesClient,
    CustomerTypeEnum,
    ProductsClient,
} from '../web-api-client';
import { ProductmapperService } from '../mapServieces/productsCluster/productmapper.service';

export class CartItem {
    product!: Product;
    options!: {
        name: string;
        value: string;
    }[];
    quantity!: number;
}

export class CartTotal {
    title!: string;
    price!: number;
    type!:
        | 'shipping'
        | 'tax'
        | 'other'
        | 'discountOnProduct'
        | 'discountOnCustomerType';
    orderId!: number;
}

interface CartData {
    items: CartItem[];
    quantity: number;
    subtotal: number;
    totals: CartTotal[];
    total: number;
}

@Injectable({
    providedIn: 'root',
})
export class CartService {
    private data: CartData = {
        items: [],
        quantity: 0,
        subtotal: 0,
        totals: [],
        total: 0,
    };

    private itemsSubject$: BehaviorSubject<CartItem[]> = new BehaviorSubject(
        this.data.items
    );
    private quantitySubject$: BehaviorSubject<number> = new BehaviorSubject(
        this.data.quantity
    );
    private subtotalSubject$: BehaviorSubject<number> = new BehaviorSubject(
        this.data.subtotal
    );
    private totalsSubject$: BehaviorSubject<CartTotal[]> = new BehaviorSubject(
        this.data.totals
    );
    private totalSubject$: BehaviorSubject<number> = new BehaviorSubject(
        this.data.total
    );
    private onAddingSubject$: Subject<Product | undefined> = new Subject();

    get items(): Array<CartItem> {
        return this.data.items;
    }
    set items(cartItems: CartItem[]) {
        this.data.items = cartItems;
    }
    get subtotal(): number {
        return this.data.subtotal;
    }

    get totals(): ReadonlyArray<CartTotal> {
        return this.data.totals;
    }

    get quantity(): number {
        return this.data.quantity;
    }

    get total(): number {
        return this.data.total;
    }

    readonly items$: Observable<CartItem[]> = this.itemsSubject$.asObservable();
    readonly quantity$: Observable<number> = this.quantitySubject$.asObservable();
    readonly subtotal$: Observable<number> = this.subtotalSubject$.asObservable();
    readonly totals$: Observable<CartTotal[]> = this.totalsSubject$.asObservable();
    readonly total$: Observable<number> = this.totalSubject$.asObservable();
    readonly onAdding$: Observable<Product | undefined> = this.onAddingSubject$.asObservable();

    private lastSelectedAttributeOptionId = -1;

    constructor(
        @Inject(PLATFORM_ID) private platformId: any,
        private authService: AuthService,
        private customerTypesClient: CustomerTypesClient,
        private productsClient: ProductsClient,
        private productmapperService: ProductmapperService
    ) {
        if (isPlatformBrowser(this.platformId)) {
            this.load();
            this.calc();
        }
    }

    add(
        product: Product,
        quantity: number,
        options: { name: string; value: string }[] = []
    ): Observable<CartItem> {
        debugger;
        let item = this.items.find((eachItem) => {
            debugger;
            if (
                eachItem.product.id !== product.id ||
                eachItem.options.length !== options.length ||
                eachItem.product.selectedAttributeOption.id !==
                product.selectedAttributeOption.id
            ) {
                return false;
            }

            if (
                eachItem.product.selectedAttributeOption.id ===
                product.selectedAttributeOption.id
            ) {
                return true;
            }

            if (eachItem.options.length) {
                for (const option of options) {
                    if (
                        !eachItem.options.find(
                            (itemOption) =>
                                itemOption.name === option.name &&
                                itemOption.value === option.value
                        )
                    ) {
                        return false;
                    }
                }
            }

            return true;
        });

        if (item && (product.selectedAttributeOption.maxOrderQuantityPerOrder <= item.quantity)) {
            this.onAddingSubject$.next(undefined);
        } else if (
            item &&
            product.selectedAttributeOption.maxOrderQuantityPerOrder >= (item.quantity + quantity) &&
            (product.selectedAttributeOption.id == this.lastSelectedAttributeOptionId ||
                product.id == item.product.id)
        ) {
            item.quantity += quantity;
            this.onAddingSubject$.next(product);
        } else {
            let newProductResult = new Product();
            newProductResult = { ...product };
            item = new CartItem();
            item.product = newProductResult;
            item.quantity = quantity;
            item.options = options;
            this.data.items.push(item);
            this.onAddingSubject$.next(product);

        }
        this.lastSelectedAttributeOptionId = product.selectedAttributeOption.id;
        this.save();
        this.calc();

        return of(item);
    }

    update(updates: { item: CartItem; quantity: number }[]): Observable<void> {
        updates.forEach((update) => {
            const item = this.items.find(
                (eachItem) => eachItem === update.item
            );

            if (item) {
                item.quantity = update.quantity;
            }
        });

        this.save();
        this.calc();

        return of(null) as unknown as Observable<void>;
    }

    remove(item: CartItem): Observable<void> {
        this.data.items = this.data.items.filter(
            (eachItem) => eachItem !== item
        );

        this.save();
        this.calc();

        return of(null) as unknown as Observable<void>;
    }

    public getProductsForNewIdentityState(): Observable<void> {
        const observables = this.data.items.map((item) => {
            return this.productsClient
                .getProductById(item.product.id,item.product.name, undefined)
                .pipe(
                    map((res) => {
                        item.product =
                            this.productmapperService.mapProductById(res);
                        return item.product;
                    })
                );
        });
        return forkJoin(observables).pipe(switchMap(() => of(undefined)));
    }

    public calc(): void {
        this.authService.isLoggedIn().subscribe((isLoggedIn) => {
            let discountPercent = 0;
            if (isLoggedIn) {
                this.customerTypesClient
                    .getCustomerTypeByCustomerTypeEnum()
                    .subscribe({
                        next: (res) => {
                            discountPercent = res.discountPercent ?? 0;
                            this.calcImplement(discountPercent);
                        },
                        error: (err) => console.log(err),
                    });
            } else {
                this.calcImplement(discountPercent);
            }
        });
    }
    private calcImplement(discountPercent: number): void {
        let quantity = 0;
        let subtotal = 0;
        let discountOnProduct = 0;
        this.data.items.forEach((item) => {
            quantity += item.quantity;
            if (item.product.selectedAttributeOption.compareAtPrice) {
                subtotal += item.product.selectedAttributeOption.compareAtPrice * item.quantity;
                discountOnProduct += item.product.selectedAttributeOption.compareAtPrice *
                 ((item.product.selectedAttributeOption.discountPercent ?? 0)/100) * item.quantity;
            } else {
                subtotal += item.product.selectedAttributeOption.price * item.quantity;
            }
        });
        const totals: CartTotal[] = [];

        totals.push({
            title: 'DISCOUNTONCUSTOMERTYPE',
            price: Math.floor(subtotal * -(discountPercent / 100)),
            type: 'discountOnCustomerType',
            orderId: 1,
        });
        totals.push({
            title: 'DISCOUNTONPRODUCT',
            price: Math.floor(-discountOnProduct),
            type: 'discountOnProduct',
            orderId: 1,
        });

        let total =
            subtotal +
            totals.reduce((acc, eachTotal) => acc + eachTotal.price, 0);

        let deliveryTotal = this.calculateDeliveryCost(total);
        totals.push(deliveryTotal);
        total = total + deliveryTotal.price;

        this.data.quantity = quantity;
        this.data.subtotal = subtotal;
        this.data.totals = totals;
        this.data.total = total;
        this.itemsSubject$.next(this.data.items);
        this.quantitySubject$.next(this.data.quantity);
        this.subtotalSubject$.next(this.data.subtotal);
        this.totalsSubject$.next(this.data.totals);
        this.totalSubject$.next(this.data.total);
    }
    public save(): void {
        localStorage.setItem('cartItems', JSON.stringify(this.data.items));
    }

    public load(): void {
        const items = localStorage.getItem('cartItems');

        if (items) {
            this.data.items = JSON.parse(items);
        }
    }

    private calculateDeliveryCost(orderCost: number): CartTotal {
        let shippingCartTotal = new CartTotal();
        shippingCartTotal.title = 'SHIPPING';
        shippingCartTotal.type = 'shipping';
        shippingCartTotal.orderId = 1;

        if (orderCost >= 10000000) {
            shippingCartTotal.price = 0;
        } else if (orderCost >= 5000000) {
            shippingCartTotal.price = 60000;
        } else {
            shippingCartTotal.price = 100000;
        }

        return shippingCartTotal;
    }
}

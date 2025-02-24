import {
    AfterViewChecked,
    AfterViewInit,
    Component,
    ComponentFactoryResolver,
    ComponentRef,
    ElementRef,
    HostBinding,
    Inject,
    Input,
    NgZone,
    OnDestroy,
    OnInit,
    PLATFORM_ID,
    TemplateRef,
    ViewChild,
    ViewContainerRef,
} from '@angular/core';
import { MobileMenuService } from '../../../../services/mobile-menu.service';
import { MobileMenuPanelComponent } from '../mobile-menu-panel/mobile-menu-panel.component';
import { fromEvent, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { isPlatformBrowser } from '@angular/common';
import { mobileMenuLinks } from '../../../../../data/mobile-menu';
import { MobileMenuLink } from '../../../../interfaces/mobile-menu-link';
import { CorporationInfo } from '../../../../interfaces/corporation-info';

interface StackItem {
    content: TemplateRef<any>;
    componentRef: ComponentRef<MobileMenuPanelComponent>;
}

@Component({
    selector: 'app-mobile-menu',
    templateUrl: './mobile-menu.component.html',
    styleUrls: ['./mobile-menu.component.scss'],
})
export class MobileMenuComponent
    implements OnInit, OnDestroy, AfterViewInit, AfterViewChecked
{
    private destroy$: Subject<void> = new Subject<void>();
    @Input() corporationInfo: CorporationInfo | undefined;

    links = [
        {
            title: 'صفحه اصلی',
            url: '/',
            // submenu: [
            //     { title: 'Home One', url: '/' },
            //     { title: 'Home Two', url: '/home-two' },
            //     {
            //         title: 'Header Spaceship',
            //         url: '/header-spaceship-variant-one',
            //         submenu: [
            //             { title: 'Variant One', url: '/header-spaceship-variant-one' },
            //             { title: 'Variant Two', url: '/header-spaceship-variant-two' },
            //             { title: 'Variant Three', url: '/header-spaceship-variant-three' },
            //         ],
            //     },
            //     {
            //         title: 'Header Classic',
            //         url: '/header-classic-variant-one',
            //         submenu: [
            //             { title: 'Variant One', url: '/header-classic-variant-one' },
            //             { title: 'Variant Two', url: '/header-classic-variant-two' },
            //             { title: 'Variant Three', url: '/header-classic-variant-three' },
            //             { title: 'Variant Four', url: '/header-classic-variant-four' },
            //             { title: 'Variant Five', url: '/header-classic-variant-five' },
            //         ],
            //     },
            //     {
            //         title: 'Mobile Header',
            //         url: '/mobile-header-variant-one',
            //         submenu: [
            //             { title: 'Variant One', url: '/mobile-header-variant-one' },
            //             { title: 'Variant Two', url: '/mobile-header-variant-two' },
            //         ],
            //     },
            // ],
        },
        {
            title: 'فروشگاه',
            url: '/shop/shop-grid-4-sidebar',
            submenu: [
                {
                    title: 'دسته بندی ها',
                    url: '/shop/category',
                    submenu: [
                        // { title: '3 Columns Sidebar', url: '/shop/category-columns-3-sidebar' },
                        // { title: '4 Columns Sidebar', url: '/shop/category-columns-4-sidebar' },
                        {
                            title: 'محصولات با نوار کناری',
                            url: '/shop/category-columns-5-sidebar',
                        },
                        // { title: '4 Columns Full', url: '/shop/category-columns-4-full' },
                        {
                            title: 'محصولات تمام صفحه',
                            url: '/shop/category-columns-5-full',
                        },
                        // { title: '6 Columns Full', url: '/shop/category-columns-6-full' },
                        // { title: '7 Columns Full', url: '/shop/category-columns-7-full' },
                        // { title: 'Right Sidebar', url: '/shop/category-right-sidebar' },
                    ],
                },
                {
                    title: 'محصولات',
                    url: '/shop/shop-grid-4-sidebar',
                    // submenu: [
                    //     { title: '6 Columns Full', url: '/shop/shop-grid-6-full' },
                    //     { title: '5 Columns Full', url: '/shop/shop-grid-5-full' },
                    //     { title: '4 Columns Full', url: '/shop/shop-grid-4-full' },
                    //     { title: '4 Columns Sidebar', url: '/shop/shop-grid-4-sidebar' },
                    //     { title: '3 Columns Sidebar', url: '/shop/shop-grid-3-sidebar' },
                    // ],
                },
                { title: 'لیست محصولات', url: '/shop/shop-list' },
                { title: 'جدول محصولات', url: '/shop/shop-table' },
                // { title: 'Shop Right Sidebar', url: '/shop/shop-right-sidebar' },
                // {
                //     title: 'Product',
                //     url: '/shop/product-full',
                //     submenu: [
                //         { title: 'Full Width', url: '/shop/product-full' },
                //         { title: 'Left Sidebar', url: '/shop/product-sidebar' },
                //     ],
                // },
                // { title: 'Cart', url: '/shop/cart' },
                // { title: 'Checkout', url: '/shop/checkout' },
                // { title: 'Order Success', url: '/shop/order-success' },
                // { title: 'Wishlist', url: '/shop/wishlist' },
                // { title: 'Compare', url: '/shop/compare' },
                // { title: 'Track Order', url: '/shop/track-order' },
            ],
        },
        // {
        //     title: 'Blog',
        //     url: '/blog',
        //     submenu: [
        //         {
        //             title: 'Blog Classic',
        //             url: '/blog/classic-right-sidebar',
        //             submenu: [
        //                 { title: 'Left Sidebar', url: '/blog/classic-left-sidebar' },
        //                 { title: 'Right Sidebar', url: '/blog/classic-right-sidebar' },
        //             ],
        //         },
        //         {
        //             title: 'Blog List',
        //             url: '/blog/list-right-sidebar',
        //             submenu: [
        //                 { title: 'Left Sidebar', url: '/blog/list-left-sidebar' },
        //                 { title: 'Right Sidebar', url: '/blog/list-right-sidebar' },
        //             ],
        //         },
        //         {
        //             title: 'Blog Grid',
        //             url: '/blog/grid-right-sidebar',
        //             submenu: [
        //                 { title: 'Left Sidebar', url: '/blog/grid-left-sidebar' },
        //                 { title: 'Right Sidebar', url: '/blog/grid-right-sidebar' },
        //             ],
        //         },
        //         {
        //             title: 'Post Page',
        //             url: '/blog/post-full-width',
        //             submenu: [
        //                 { title: 'Full Width', url: '/blog/post-full-width' },
        //                 { title: 'Left Sidebar', url: '/blog/post-left-sidebar' },
        //                 { title: 'Right Sidebar', url: '/blog/post-right-sidebar' },
        //             ],
        //         },
        //         { title: 'Post Without Image', url: '/blog/post-without-image' },
        //     ],
        // },
        {
            title: 'حساب کاربری',
            url: '/account',
            submenu: [
                { title: 'ساختن حساب یا ورود', url: '/account/login' },
                { title: 'ناحیه کاربری', url: '/account/dashboard' },
                { title: 'گاراژ', url: '/account/garage' },
                { title: 'ویرایش حساب', url: '/account/profile' },
                { title: 'تاریخچه سفارشات', url: '/account/orders' },
                //{ title: 'Order Details', url: '/account/order-details' },
                { title: 'آدرس ها', url: '/account/addresses' },
                //{ title: 'Edit Address', url: '/account/edit-address' },
                { title: 'ویرایش رمز عبور', url: '/account/password' },
            ],
        },
        {
            title: 'سوالی دارید؟',
            url: '/site/faq',
            // submenu: [
            //     { title: 'About Us', url: '/site/about-us' },
            //     { title: 'Contact Us v1', url: '/site/contact-us-v1' },
            //     { title: 'Contact Us v2', url: '/site/contact-us-v2' },
            //     { title: '404', url: '/site/not-found' },
            //     { title: 'Terms And Conditions', url: '/site/terms' },
            //     { title: 'FAQ', url: '/site/faq' },
            //     { title: 'Components', url: '/site/components' },
            //     { title: 'Typography', url: '/site/typography' },
            // ],
        },
    ];

    currentLevel = 0;

    panelsStack: StackItem[] = [];
    panelsBin: StackItem[] = [];

    forceConveyorTransition = false;

    @HostBinding('class.mobile-menu') classMobileMenu = true;

    @HostBinding('class.mobile-menu--open') get classMobileMenuOpen() {
        return this.menu.isOpen;
    }

    @ViewChild('body') body!: ElementRef;

    @ViewChild('conveyor') conveyor!: ElementRef;

    @ViewChild('panelsContainer', { read: ViewContainerRef })
    panelsContainer!: ViewContainerRef;

    constructor(
        @Inject(PLATFORM_ID) private platformId: any,
        private cfr: ComponentFactoryResolver,
        private zone: NgZone,
        public menu: MobileMenuService
    ) {}

    ngOnInit(): void {
        this.menu.onOpenPanel
            .pipe(takeUntil(this.destroy$))
            .subscribe(({ content, label }) => {
                if (
                    this.panelsStack.findIndex((x) => x.content === content) !==
                    -1
                ) {
                    return;
                }

                const componentFactory = this.cfr.resolveComponentFactory(
                    MobileMenuPanelComponent
                );
                const componentRef =
                    this.panelsContainer.createComponent(componentFactory);

                componentRef.instance.label = label;
                componentRef.instance.content = content;
                componentRef.instance.level = this.panelsStack.length + 1;

                this.panelsStack.push({ content, componentRef });
                this.currentLevel += 1;

                this.removeUnusedPanels();
            });
        this.menu.onCloseCurrentPanel
            .pipe(takeUntil(this.destroy$))
            .subscribe(() => {
                const panel = this.panelsStack.pop();

                if (!panel) {
                    return;
                }

                this.panelsBin.push(panel);
                this.currentLevel -= 1;

                if (!isPlatformBrowser(this.platformId)) {
                    this.removeUnusedPanels();
                }
            });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    ngAfterViewInit(): void {
        if (isPlatformBrowser(this.platformId)) {
            this.zone.runOutsideAngular(() => {
                fromEvent<TransitionEvent>(
                    this.body.nativeElement,
                    'transitionend'
                )
                    .pipe(takeUntil(this.destroy$))
                    .subscribe((event) => {
                        if (
                            event.target === this.body.nativeElement &&
                            event.propertyName === 'transform' &&
                            !this.menu.isOpen
                        ) {
                            this.zone.run(() => this.onMenuClosed());
                        }
                    });

                fromEvent<TransitionEvent>(
                    this.conveyor.nativeElement,
                    'transitionend'
                )
                    .pipe(takeUntil(this.destroy$))
                    .subscribe((event) => {
                        if (
                            event.target === this.conveyor.nativeElement &&
                            event.propertyName === 'transform'
                        ) {
                            this.zone.run(() => this.onConveyorStopped());
                        }
                    });
            });
        }
    }

    ngAfterViewChecked(): void {
        if (this.forceConveyorTransition) {
            this.forceConveyorTransition = false;

            if (isPlatformBrowser(this.platformId)) {
                this.conveyor.nativeElement.style.transition = 'none';
                this.conveyor.nativeElement.getBoundingClientRect(); // force reflow
                this.conveyor.nativeElement.style.transition = '';
            }
        }
    }

    onMenuClosed(): void {
        let panel: StackItem | undefined;

        while ((panel = this.panelsStack.pop())) {
            this.panelsBin.push(panel);
            this.currentLevel -= 1;
        }

        this.removeUnusedPanels();
        this.forceConveyorTransition = true;
    }

    onConveyorStopped(): void {
        this.removeUnusedPanels();
    }

    removeUnusedPanels(): void {
        let panel: StackItem | undefined;

        while ((panel = this.panelsBin.pop())) {
            panel.componentRef.destroy();
        }
    }

    onLinkClick(item: MobileMenuLink): void {
        if (!item.submenu || item.submenu.length < 1) {
            this.menu.close();
        }
    }
}

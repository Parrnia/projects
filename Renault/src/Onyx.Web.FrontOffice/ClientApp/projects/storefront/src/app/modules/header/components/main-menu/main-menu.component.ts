import { Component, HostBinding, Input, SimpleChanges, OnInit } from '@angular/core';
import { mainMenu } from '../../../../../data/header-main-menu';
import { MainMenuLink } from '../../../../interfaces/main-menu-link';
import { HeaderService } from '../../../../services/header.service';

@Component({
    selector: 'app-main-menu',
    templateUrl: './main-menu.component.html',
    styleUrls: ['./main-menu.component.scss'],
})
export class MainMenuComponent implements OnInit {
    items: MainMenuLink[] = [];
    // @Input() data: MainMenuLink[] = [];
    //     items :any=[];
    hoveredItem: MainMenuLink|null = null;

    @HostBinding('class.main-menu') classMainMenu = true;

    constructor(
        public header: HeaderService
    ) {  }

    ngOnInit(): void {
        this.items = [
            {
                title: 'صفحه اصلی',
                url: '/'
            },
            // {
            //     title: 'Megamenu',
            //     url: '/shop',
            //     submenu: {
            //         type: 'megamenu',
            //         size: 'nl',
            //         columns: [
            //             {
            //                 size: '1of3',
            //                 links: [
            //                     {
            //                         title: 'Headlights & Lighting',
            //                         url: '/shop',
            //                         links: [
            //                             { title: 'Headlights', url: '/shop' },
            //                             { title: 'Tail Lights', url: '/shop' },
            //                             { title: 'Fog Lights', url: '/shop' },
            //                             { title: 'Turn Signals', url: '/shop' },
            //                             { title: 'Switches & Relays', url: '/shop' },
            //                             { title: 'Corner Lights', url: '/shop' },
            //                         ],
            //                     },
            //                     {
            //                         title: 'Brakes & Suspension',
            //                         url: '/shop',
            //                         links: [
            //                             { title: 'Brake Discs', url: '/shop' },
            //                             { title: 'Wheel Hubs', url: '/shop' },
            //                             { title: 'Air Suspension', url: '/shop' },
            //                             { title: 'Ball Joints', url: '/shop' },
            //                         ],
            //                     },
            //                 ],
            //             },
            //             {
            //                 size: 6,
            //                 links: [
            //                     {
            //                         title: 'Interior Parts',
            //                         url: '/shop',
            //                         links: [
            //                             { title: 'Floor Mats', url: '/shop' },
            //                             { title: 'Gauges', url: '/shop' },
            //                             { title: 'Consoles & Organizers', url: '/shop' },
            //                             { title: 'Mobile Electronics', url: '/shop' },
            //                         ],
            //                     },
            //                     {
            //                         title: 'Engine & Drivetrain',
            //                         url: '/shop',
            //                         links: [
            //                             { title: 'Air Filters', url: '/shop' },
            //                             { title: 'Oxygen Sensors', url: '/shop' },
            //                             { title: 'Heating', url: '/shop' },
            //                             { title: 'Exhaust', url: '/shop' },
            //                             { title: 'Cranks & Pistons', url: '/shop' },
            //                             { title: 'Cargo Accessories', url: '/shop' },
            //                         ],
            //                     },
            //                 ],
            //             },
            //         ],
            //     },
            //     customFields: {
            //         ignoreIn: ['spaceship'],
            //     },
            // },
            {
                title: 'فروشگاه',
                url: '/shop/shop-grid-4-sidebar',
                submenu: {
                    type: 'menu',
                    links: [
                        {
                            title: 'دسته بندی ها',
                            url: '/shop/category',
                            links: [
                                //{ title: '3 Columns Sidebar', url: '/shop/category-columns-3-sidebar' },
                                //{ title: '4 Columns Sidebar', url: '/shop/category-columns-4-sidebar' },
                                { title: 'محصولات با نوار کناری', url: '/shop/category-columns-5-sidebar' },
                                //{ title: '4 Columns Full', url: '/shop/category-columns-4-full' },
                                { title: 'محصولات تمام صفحه', url: '/shop/category-columns-5-full' },
                                //{ title: '6 Columns Full', url: '/shop/category-columns-6-full' },
                                //{ title: '7 Columns Full', url: '/shop/category-columns-7-full' },
                                //{ title: 'Right Sidebar', url: '/shop/category-right-sidebar' },
                            ],
                        },
                        {
                            title: 'محصولات',
                            url: '/shop/shop-grid-6-full',
                            // links: [
                            //     { title: 'محصولات تمام صفحه', url: '/shop/shop-grid-6-full' },
                            //     { title: '5 Columns Full', url: '/shop/shop-grid-5-full' },
                            //     { title: '4 Columns Full', url: '/shop/shop-grid-4-full' },
                            //     { title: '4 Columns Sidebar', url: '/shop/shop-grid-4-sidebar' },
                            //     { title: '3 Columns Sidebar', url: '/shop/shop-grid-3-sidebar' },
                            // ],
                        },
                        { title: 'لیست محصولات', url: '/shop/shop-list' },
                        { title: 'جدول محصولات', url: '/shop/shop-table' },
                        //{ title: 'Shop Right Sidebar', url: '/shop/shop-right-sidebar' },
                        // {
                        //     title: 'Product',
                        //     url: '/shop/product-full',
                        //     links: [
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
            },
            // {
            //     title: 'Blog',
            //     url: '/blog',
            //     submenu: {
            //         type: 'menu',
            //         links: [
            //             {
            //                 title: 'Blog Classic',
            //                 url: '/blog/classic-right-sidebar',
            //                 links: [
            //                     { title: 'Left Sidebar', url: '/blog/classic-left-sidebar' },
            //                     { title: 'Right Sidebar', url: '/blog/classic-right-sidebar' },
            //                 ],
            //             },
            //             {
            //                 title: 'Blog List',
            //                 url: '/blog/list-right-sidebar',
            //                 links: [
            //                     { title: 'Left Sidebar', url: '/blog/list-left-sidebar' },
            //                     { title: 'Right Sidebar', url: '/blog/list-right-sidebar' },
            //                 ],
            //             },
            //             {
            //                 title: 'Blog Grid',
            //                 url: '/blog/grid-right-sidebar',
            //                 links: [
            //                     { title: 'Left Sidebar', url: '/blog/grid-left-sidebar' },
            //                     { title: 'Right Sidebar', url: '/blog/grid-right-sidebar' },
            //                 ],
            //             },
            //             {
            //                 title: 'Post Page',
            //                 url: '/blog/post-full-width',
            //                 links: [
            //                     { title: 'Full Width', url: '/blog/post-full-width' },
            //                     { title: 'Left Sidebar', url: '/blog/post-left-sidebar' },
            //                     { title: 'Right Sidebar', url: '/blog/post-right-sidebar' },
            //                 ],
            //             },
            //             { title: 'Post Without Image', url: '/blog/post-without-image' },
            //         ],
            //     },
            // },
            {
                title: 'حساب کاربری',
                url: '/account',
                submenu: {
                    type: 'menu',
                    links: [
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
            },
            {
                title: 'سوالی دارید؟',
                url: '/site/faq',
                // submenu: {
                //     type: 'menu',
                //     links: [
                //         { title: 'About Us', url: '/site/about-us' },
                //         { title: 'Contact Us v1', url: '/site/contact-us-v1' },
                //         { title: 'Contact Us v2', url: '/site/contact-us-v2' },
                //         { title: '404', url: '/site/not-found' },
                //         { title: 'Terms And Conditions', url: '/site/terms' },
                //         { title: 'FAQ', url: '/site/faq' },
                //         { title: 'Components', url: '/site/components' },
                //         { title: 'Typography', url: '/site/typography' },
                //     ],
                // },
            },
            // {
            //     title: 'Buy Theme',
            //     url: 'https://themeforest.net/item/redparts-auto-parts-angular-template/27087440',
            //     external: true,
            //     customFields: {
            //         ignoreIn: ['spaceship'],
            //     },
            // },
        ];
    }

    ngOnChanges(changes: SimpleChanges) {
      
        this.items= changes['data'].currentValue;
     console.log(' main menu this.items' ,  this.items)

    }
    onItemEnter(item: any): void {
        this.hoveredItem = item;
    }

    onItemLeave(item: any): void {
        if ( this.hoveredItem === item ) {
            this.hoveredItem = null;
        }
    }

    onItemClick(): void {
        this.hoveredItem = null;
    }

    isVisible(item: MainMenuLink): boolean {
        return !item.customFields
            || !item.customFields['ignoreIn']
            || !item.customFields['ignoreIn'].includes(this.header.desktopLayout);
    }
}

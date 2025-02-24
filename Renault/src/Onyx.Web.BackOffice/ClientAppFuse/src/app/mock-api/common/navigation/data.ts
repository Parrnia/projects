/* tslint:disable:max-line-length */
import { FuseNavigationItem } from '@fuse/components/navigation';

export const defaultNavigation: FuseNavigationItem[] = [
    {
        id   : 'example',
        title: 'Example',
        type : 'basic',
        icon : 'heroicons_outline:chart-pie',
        link : '/example'
    },
    {
        id      : 'layouts',
        title   : 'سفارشی سازی ',
       
        type    : 'collapsable',
        icon    : 'heroicons_outline:beaker',
        children: [
            {
                id   : 'layouts.header',
                title: 'سفارشی سازی هدر',
                type : 'basic',
                icon : 'heroicons_outline:clipboard-check',
                link : '/layouts/header'
            },
            {
                id   : 'layouts.footer',
                title: 'سفارشی سازی فوتر',
                type : 'basic',
                icon : 'heroicons_outline:chart-pie',
                link : '/layouts/footer'
            },
            {
                id   : 'layouts.brands',
                title: 'سفارشی سازی برندها',
                type : 'basic',
                icon : 'heroicons_outline:cash',
                link : '/layouts/brands'
            },
           
        ]
    },
];
export const compactNavigation: FuseNavigationItem[] = [
    {
        id   : 'example',
        title: 'Example',
        type : 'basic',
        icon : 'heroicons_outline:chart-pie',
        link : '/example'
    }
];
export const futuristicNavigation: FuseNavigationItem[] = [
    {
        id   : 'example',
        title: 'Example',
        type : 'basic',
        icon : 'heroicons_outline:chart-pie',
        link : '/example'
    }
];
export const horizontalNavigation: FuseNavigationItem[] = [
    {
        id   : 'example',
        title: 'Example',
        type : 'basic',
        icon : 'heroicons_outline:chart-pie',
        link : '/example'
    }
];

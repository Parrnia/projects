import { Component, HostBinding, Input } from '@angular/core';
import { Link } from '../../../../interfaces/link';
import { FooterLinkDto } from 'projects/storefront/src/app/web-api-client';

@Component({
    selector: 'app-links',
    templateUrl: './links.component.html',
    styleUrls: ['./links.component.scss'],
})
export class LinksComponent {
    @Input() header: string = '';

    @Input() links: FooterLinkDto[] = [];

    @HostBinding('class.footer-links') classFooterLinks = true;

    constructor() { }
}

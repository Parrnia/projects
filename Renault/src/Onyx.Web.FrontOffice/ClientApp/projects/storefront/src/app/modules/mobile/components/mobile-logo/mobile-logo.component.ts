import { Component, HostBinding, Input } from '@angular/core';
import { CorporationInfo } from 'projects/storefront/src/app/interfaces/corporation-info';
import { CorporationInfoDto } from 'projects/storefront/src/app/web-api-client';

@Component({
    selector: 'app-mobile-logo',
    templateUrl: './mobile-logo.component.html',
    styleUrls: ['./mobile-logo.component.scss'],
})
export class MobileLogoComponent {
    @HostBinding('class.mobile-logo') classMobileLogo = true;
    @Input() corporationInfo : CorporationInfo | undefined ;

    constructor() {
     }
}

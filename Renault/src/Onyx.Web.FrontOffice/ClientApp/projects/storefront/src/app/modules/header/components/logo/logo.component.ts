import { Component, Input, OnInit } from '@angular/core';
import { ImageService } from 'projects/storefront/src/app/mapServieces/image.service';
import { CorporationInfoDto } from 'projects/storefront/src/app/web-api-client';
import { CorporationInfo } from 'projects/storefront/src/app/interfaces/corporation-info';
@Component({
    selector: 'app-logo',
    templateUrl: './logo.component.html',
    styleUrls: ['./logo.component.scss'],
})
export class LogoComponent implements OnInit {
    @Input() corporationInfo: CorporationInfo | undefined;
    desktopLogo: string | undefined;
    constructor() {}
    ngOnInit(): void {}
}

import { CorporationInfoDto, CorporationInfosClient } from 'projects/storefront/src/app/web-api-client';
import { Component, HostBinding } from '@angular/core';
import { theme } from '../../../../../data/theme';
import { CorporationInfo } from 'projects/storefront/src/app/interfaces/corporation-info';
import { map, Observable, switchMap } from 'rxjs';
import { ImageService } from 'projects/storefront/src/app/mapServieces/image.service';

@Component({
    selector: 'app-contacts',
    templateUrl: './contacts.component.html',
    styleUrls: ['./contacts.component.scss'],
})
export class ContactsComponent {
    @HostBinding('class.footer-contacts') classFooterContacts = true;
    public info!: Observable<CorporationInfoDto>; 

    constructor(
        private corporationInfosClient: CorporationInfosClient,
        private imageService: ImageService
    ) {
        this.info = this.corporationInfosClient.getCorporationInfo().pipe(map((res) => {
            if(res != undefined){
                res.desktopLogo = this.imageService.makeImageUrl(res.desktopLogo);
                res.mobileLogo = this.imageService.makeImageUrl(res.mobileLogo);
                res.footerLogo = this.imageService.makeImageUrl(res.footerLogo);
                res.sliderBackGroundImage = this.imageService.makeImageUrl(res.sliderBackGroundImage);
            }
            return res;
        }));
     }
}

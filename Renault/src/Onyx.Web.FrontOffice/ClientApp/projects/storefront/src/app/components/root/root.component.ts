import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DesktopHeaderVariant, HeaderService, MobileHeaderVariant } from '../../services/header.service';
import { takeUntil } from 'rxjs/operators';
import { Observable, Subject } from 'rxjs';
import { CorporationInfosClient, CorporationInfoDto } from '../../web-api-client';
import { ImageService } from '../../mapServieces/image.service';



export interface RootComponentData {
    desktopHeader: DesktopHeaderVariant;
    mobileHeader: MobileHeaderVariant;
}


@Component({
    selector: 'app-root',
    templateUrl: './root.component.html',
    styleUrls: ['./root.component.scss'],
})
export class RootComponent implements OnInit, OnDestroy {
    destroy$: Subject<void> =  new Subject<void>();
    corporationInfo : CorporationInfoDto | undefined;

    constructor(
        private route: ActivatedRoute,
        public header: HeaderService,
        private corporationInfoClient: CorporationInfosClient,
        private imageService: ImageService
    ) { }

    ngOnInit(): void {
        const data$ = this.route.data as Observable<RootComponentData>;
        console.log(this.corporationInfo)

        data$.pipe(
            takeUntil(this.destroy$),
        ).subscribe((data: RootComponentData) => {
            
            this.header.setDesktopVariant(data.desktopHeader || 'spaceship/one');
            this.header.setMobileVariant(data.mobileHeader || 'one');
        });
        
        this.corporationInfoClient.getCorporationInfo().subscribe(result => {
            if(result){
                this.corporationInfo = result;
                this.corporationInfo.desktopLogo = this.imageService.makeImageUrl(this.corporationInfo.desktopLogo);
                this.corporationInfo.footerLogo = this.imageService.makeImageUrl(this.corporationInfo.footerLogo);
                this.corporationInfo.mobileLogo = this.imageService.makeImageUrl(this.corporationInfo.mobileLogo);
                this.corporationInfo.sliderBackGroundImage = this.imageService.makeImageUrl(this.corporationInfo.sliderBackGroundImage);
            }
            
          }, error => console.error(error));
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
}

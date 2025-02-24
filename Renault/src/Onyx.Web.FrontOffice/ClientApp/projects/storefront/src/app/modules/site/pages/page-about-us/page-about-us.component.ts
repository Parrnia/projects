import { Component } from '@angular/core';
import { ImageService } from 'projects/storefront/src/app/mapServieces/image.service';
import { AboutUsClient } from 'projects/storefront/src/app/web-api-client';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-page-about-us',
    templateUrl: './page-about-us.component.html',
    styleUrls: ['./page-about-us.component.scss'],
})
export class PageAboutUsComponent {
    textContent!: string;
    imageContent!: string;

    constructor(
        private aboutUsClient: AboutUsClient,
        private imageService: ImageService
    ) {
        this.aboutUsClient.getAboutUs().subscribe({
            next: (res) => {
                this.textContent = res?.textContent ?? '';
                this.imageContent =
                    this.imageService.makeImageUrl(res?.imageContent) ?? '';
            },
        });
    }
}

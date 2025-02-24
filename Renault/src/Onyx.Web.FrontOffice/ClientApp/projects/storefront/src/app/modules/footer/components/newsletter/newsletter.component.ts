import {
    AllSocialLinkDto,
    SocialLinksClient,
} from './../../../../web-api-client';
import { Component, HostBinding, Pipe } from '@angular/core';
import { ImageService } from 'projects/storefront/src/app/mapServieces/image.service';
import { map, Observable, of } from 'rxjs';

@Component({
    selector: 'app-newsletter',
    templateUrl: './newsletter.component.html',
    styleUrls: ['./newsletter.component.scss'],
})
export class NewsletterComponent {
    @HostBinding('class.footer-newsletter') classFooterNewsletter = true;

    socialLinks: Observable<SocialLink[]> = of([]);
    constructor(
        private socialLinksClient: SocialLinksClient,
        private imageService: ImageService
    ) {
        this.socialLinks = socialLinksClient.getAllSocialLinks().pipe(
            map((e) => {
                e.forEach((c) => {
                    c.icon = this.imageService.makeImageUrl(c.icon);
                });
                return e;
            })
        );
    }
}

export class SocialLink {
    id?: number;
    url?: string;
    icon?: string;
}

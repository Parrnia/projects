import {
    FooterLinkContainersClient,
    PaginatedListOfFooterLinkContainerWithPaginationDto,
} from './../../web-api-client';
import { Component, Input } from '@angular/core';
import { theme } from '../../../data/theme';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss'],
})
export class FooterComponent {
    @Input() logo: string | undefined;
    @Input() poweredBy: string | undefined;
    footerLinkContainers: Observable<PaginatedListOfFooterLinkContainerWithPaginationDto>;
    constructor(footerLinkContainersClient: FooterLinkContainersClient) {
        this.footerLinkContainers =
            footerLinkContainersClient.getFooterLinkContainersWithPagination(
                1,
                2
            );
    }
}

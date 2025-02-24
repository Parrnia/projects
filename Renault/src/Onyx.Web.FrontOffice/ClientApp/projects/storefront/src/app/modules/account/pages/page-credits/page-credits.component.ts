import { CreditsClient } from './../../../../web-api-client';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { merge, Observable, of, Subject } from 'rxjs';
import { distinctUntilChanged, mergeMap, switchMap, takeUntil } from 'rxjs/operators';
import { FormControl } from '@angular/forms';
import { UrlService } from '../../../../services/url.service';
import { GetCreditsListOptions } from 'projects/storefront/src/app/api';
import { creditsList } from 'projects/storefront/src/app/interfaces/list';
import { Credit } from 'projects/storefront/src/app/interfaces/credit';
import { CreditMapperService } from 'projects/storefront/src/app/mapServieces/userProfilesCluster/credit-mapper.service';

@Component({
    selector: 'app-page-credits',
    templateUrl: './page-credits.component.html',
    styleUrls: ['./page-credits.component.scss'],
})
export class PageCreditsComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    currentPage: FormControl = new FormControl(1);
    list!: creditsList;

    constructor(
        public url: UrlService,
        private creditsClient: CreditsClient,
        private creditMapperService: CreditMapperService
    ) { }

    ngOnInit(): void {
        merge(
            of(this.currentPage.value),
            this.currentPage.valueChanges,
        ).pipe(
            distinctUntilChanged(),
            mergeMap(page => this.getcreditsList({
                limit: 5,
                page,
            })),
            takeUntil(this.destroy$),
        ).subscribe(x => this.list = x);
    }


    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
    getcreditsList(options?: GetCreditsListOptions): Observable<creditsList> {
        options = options || {};
    
        return this.creditsClient.getCreditsByCustomerIdWithPagination(options.page, options.limit,undefined)
            .pipe(
                switchMap(res => {
                    const items: Credit[] = this.creditMapperService.mapCredites(res.items!);
                    const page = options?.page || 1;
                    const limit = options?.limit || 5;
                    const sort = options?.sort || 'default';
                    const total = items.length;
                    const pages = Math.ceil(items.length / limit);
                    const from = (page - 1) * limit + 1;
                    const to = page * limit;
    
                    return of({
                        page,
                        limit,
                        sort,
                        total,
                        pages,
                        from,
                        to,
                        items,
                    });
                })
            );
    }
}

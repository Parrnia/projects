import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { Address } from '../../../../interfaces/address';
import { finalize, mergeMap, takeUntil } from 'rxjs/operators';
import { UrlService } from '../../../../services/url.service';
import { AddressesClient } from 'projects/storefront/src/app/web-api-client';
import { AddressMapperService } from 'projects/storefront/src/app/mapServieces/userProfilesCluster/address-mapper.service';

@Component({
    selector: 'app-page-addresses',
    templateUrl: './page-addresses.component.html',
    styleUrls: ['./page-addresses.component.scss'],
})
export class PageAddressesComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    addresses: Address[] = [];

    removeInProgress: number[] = [];

    constructor(
        public url: UrlService,
        private addressesClient : AddressesClient,
        private addressMapperService : AddressMapperService
    ) { }

    ngOnInit(): void {
        this.addressesClient.getAddressesByCustomerId().pipe(takeUntil(this.destroy$)).subscribe(x => this.addresses = this.addressMapperService.mapAddresses(x));
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    remove(address: Address): void {
        if (this.removeInProgress.indexOf(address.id) !== -1) {
            return;
        }

        this.removeInProgress.push(address.id);

        this.addressesClient.selfDelete(address.id).pipe(
            mergeMap(() => this.addressesClient.getAddressesByCustomerId()),
            finalize(() => {
                const index = this.removeInProgress.indexOf(address.id);

                if (index !== -1) {
                    this.removeInProgress.splice(index, 1);
                }
            }),
            takeUntil(this.destroy$),
        ).subscribe(addresses => this.addresses = this.addressMapperService.mapAddresses(addresses));
    }
}

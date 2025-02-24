import { AddressMapperService } from './../../../../mapServieces/userProfilesCluster/address-mapper.service';
import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { AccountApi } from '../../../../api';
import { Subject } from 'rxjs';
import { Address } from '../../../../interfaces/address';
import { finalize, mergeMap, takeUntil } from 'rxjs/operators';
import { UrlService } from '../../../../services/url.service';
import { AddressesClient } from 'projects/storefront/src/app/web-api-client';

@Component({
    selector: 'app-page-addresses-checkout',
    templateUrl: './page-addresses-checkout.component.html',
    styleUrls: ['./page-addresses-checkout.component.scss'],
})
export class PageAddressesCheckoutComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    @Output() getSelectedAddress = new EventEmitter<number>();
    
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

        this.addressesClient.getAddressesByCustomerId().pipe(
            finalize(() => {
                const index = this.removeInProgress.indexOf(address.id);

                if (index !== -1) {
                    this.removeInProgress.splice(index, 1);
                }
            }),
            takeUntil(this.destroy$),
        ).subscribe(addresses => this.addresses = this.addressMapperService.mapAddresses(addresses));
    }

    selectAddressId(addressId : number){
        this.getSelectedAddress.emit(addressId);
    }
}

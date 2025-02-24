import { AddressMapperService } from './../../../../mapServieces/userProfilesCluster/address-mapper.service';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { combineLatest, Observable, of, Subject } from 'rxjs';
import { finalize, map, switchMap, takeUntil } from 'rxjs/operators';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AddressFormComponent } from '../../../shared/components/address-form/address-form.component';
import { AccountApi, EditAddressData } from '../../../../api';
import { Address } from '../../../../interfaces/address';
import { AddressFormNewComponent } from '../../../shared/components/address-form-new/address-form-new.component';
import { AddressesClient } from 'projects/storefront/src/app/web-api-client';

@Component({
    selector: 'app-page-edit-address',
    templateUrl: './page-edit-address.component.html',
    styleUrls: ['./page-edit-address.component.scss'],
})
export class PageEditAddressComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    form!: FormGroup;

    @ViewChild(AddressFormNewComponent) addressForm!: AddressFormNewComponent;

    addressId: number|null = null;
    saveInProgress = false;
    firstOrDefaultAddress: boolean = false;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private fb: FormBuilder,
        private addressesClient : AddressesClient,
        private addressMapperService : AddressMapperService
    ) { }

    ngOnInit(): void {
        this.form = this.fb.group({
            address: [],
            default: [false],
        });

        this.route.params.pipe(
            map(x => x['id'] ? parseFloat(x['id']) : null),
            switchMap(addressId => combineLatest([
                addressId ? this.addressesClient.getAddressById(addressId) : of(null),
                this.addressesClient.getAddressesByCustomerId().pipe(
                    map((addresses) => addresses.find((address) => address.default === true))),
            ])),
            takeUntil(this.destroy$),
        ).subscribe(([addressDto, defaultAddress]) => {
            if (addressDto) {
                this.addressId = addressDto.id ?? null;
                let address = this.addressMapperService.mapAddress(addressDto);
                this.form.get('address')!.setValue({
                    id: address.id,
                    title: address.title,
                    company: address.company,
                    country: address.countryId,
                    address1: address.address1,
                    address2: address.address2,
                    city: address.city,
                    state: address.state,
                    postcode: address.postcode
                });
            }

            this.firstOrDefaultAddress = (!defaultAddress || (addressDto !== null && addressDto.default)) ?? false;
            this.form.get('default')!.setValue(this.firstOrDefaultAddress);

            if (!defaultAddress) {
                this.form.get('default')!.disable();
            } else {
                this.form.get('default')!.enable();
            }
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    save(): void {
         
        this.form.markAllAsTouched();
        this.addressForm.markAsTouched();

        if (this.saveInProgress || this.form.errors != null){
            return;
        }

        const addressData: Address = {
            ...this.form.value.address,
            default: this.form.value.default,
        };

        this.saveInProgress = true;

        let saveMethod: Observable<any>;

        if (this.addressId) {
            saveMethod = this.addressesClient.selfUpdate(this.addressId,this.addressMapperService.mapUpdateAddressCommand(addressData));
        } else {
            saveMethod = this.addressesClient.create(this.addressMapperService.mapAddressCommand(addressData));
        }

        saveMethod.pipe(
            finalize(() => this.saveInProgress = false),
            takeUntil(this.destroy$),
        ).subscribe(() => this.router.navigateByUrl('/account/addresses'));
    }
}

import { Component, forwardRef, Input, OnDestroy, OnInit } from '@angular/core';
import {
    AbstractControl,
    ControlValueAccessor,
    FormBuilder,
    FormGroup,
    NG_VALIDATORS,
    NG_VALUE_ACCESSOR,
    ValidationErrors,
    Validator,
    Validators,
} from '@angular/forms';
import { Country } from '../../../../interfaces/country';
import { Subject } from 'rxjs';
import { switchMap, takeUntil } from 'rxjs/operators';
import { AddressesClient, CountriesClient } from 'projects/storefront/src/app/web-api-client';
import { CountrymapperService } from 'projects/storefront/src/app/mapServieces/infoCluster/countrymapper.service';
import { Customer } from 'projects/storefront/src/app/interfaces/user';
import { AddressValidators } from './address-validators';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';

let uniqueId = 0;

export interface AddressFormValue {
    title: string;
    firstName: string;
    lastName: string;
    company: string;
    country: number;
    address1: string;
    address2: string;
    city: string;
    state: string;
    postcode: string;
    email: string;
    phone: string;
}

@Component({
    selector: 'app-address-form-new',
    templateUrl: './address-form-new.component.html',
    styleUrls: ['./address-form-new.component.scss'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => AddressFormNewComponent),
            multi: true,
        },
        {
            provide: NG_VALIDATORS,
            useExisting: forwardRef(() => AddressFormNewComponent),
            multi: true,
        },
    ],
})
export class AddressFormNewComponent implements OnInit, OnDestroy, ControlValueAccessor, Validator {
    private destroy$: Subject<void> = new Subject<void>();
    private readonly dataId: number = ++uniqueId;    

    form!: FormGroup;
   
    countries: Country[] = [];

    get formId(): string {
        return `app-address-form-id-${this.dataId}`;
    }

    changeFn: (_: AddressFormValue) => void = () => {};

    touchedFn: () => void = () => {};

    constructor(
        private fb: FormBuilder,
        private countriesClient : CountriesClient,
        private countrymapperService : CountrymapperService,
        private addressesClient : AddressesClient,
        private authService : AuthService
    ) { 
    }

    ngOnInit(): void {

         
        this.form = this.fb.group({
            id: [''],
            title: ['', Validators.required],
            company:   [''],
            country:   ['',  Validators.required],
            address1:  ['',  Validators.required],
            address2:  [''],
            city:      ['',  Validators.required],
            state:     ['',  Validators.required],
            postcode:  ['',  Validators.required],
        });
        
        this.form.valueChanges.subscribe(value => {
            this.changeFn(value);
            this.touchedFn();
            this.form.controls['title'].addAsyncValidators(AddressValidators.validAddressTitle(this.addressesClient, this.form.controls['id'].value));
            this.form.controls['postcode'].addAsyncValidators(AddressValidators.validAddressPostcode(this.addressesClient,  this.form.controls['id'].value));
        });
         
        
        this.countriesClient.getAllCountries().pipe(switchMap((res => ([this.countrymapperService.mapAllCountries(res ?? [])])))).pipe(takeUntil(this.destroy$)).subscribe(x => this.countries = x);
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    registerOnChange(fn: any): void {
        this.changeFn = fn;
    }

    registerOnTouched(fn: any): void {
        this.touchedFn = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        if (isDisabled) {
            this.form.disable({ emitEvent: false });
        } else {
            this.form.enable({ emitEvent: false });
        }
    }

    writeValue(value: any): void {
        if (typeof value !== 'object') {
            value = {};
        }
         

        this.form.setValue(
            {
                id:'',
                title: '',
                company: '',
                country: '',
                address1: '',
                address2: '',
                city: '',
                state: '',
                postcode: '',
                ...value,
            },
            { emitEvent: false },
        );
    }

    validate(control: AbstractControl): ValidationErrors | null {
        return this.form.valid ? null : { addressForm: this.form.errors };
    }

    markAsTouched(): void {
        this.form.markAllAsTouched();
    }
    setFormControlId(addressId : number){
         
        this.form.controls['id'].setValue(addressId);
    }

}

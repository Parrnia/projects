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
import { Observable, of, Subject } from 'rxjs';
import { switchMap, takeUntil } from 'rxjs/operators';
import { CountriesClient } from 'projects/storefront/src/app/web-api-client';
import { CountrymapperService } from 'projects/storefront/src/app/mapServieces/infoCluster/countrymapper.service';
import { Customer } from 'projects/storefront/src/app/interfaces/user';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';


let uniqueId = 0;

export interface AddressFormValue {
    id: number;
}

@Component({
    selector: 'app-address-form',
    templateUrl: './address-form.component.html',
    styleUrls: ['./address-form.component.scss'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => AddressFormComponent),
            multi: true,
        },
        {
            provide: NG_VALIDATORS,
            useExisting: forwardRef(() => AddressFormComponent),
            multi: true,
        },
    ],
})
export class AddressFormComponent implements OnInit, OnDestroy, ControlValueAccessor, Validator {
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
        private authService: AuthService
    ) { 
    }

    ngOnInit(): void {

        this.form = this.fb.group({
            addressForm: ['', Validators.required]
        });
        

        this.form.valueChanges.subscribe(value => {
            this.changeFn(value);
            this.touchedFn();
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
                addressForm:''
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
         
        this.form.controls['addressForm'].setValue(addressId);
        this.form.markAllAsTouched;
    }

}

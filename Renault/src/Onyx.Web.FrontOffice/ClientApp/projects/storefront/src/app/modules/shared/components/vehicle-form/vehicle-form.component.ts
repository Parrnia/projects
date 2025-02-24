import { Component, forwardRef, HostBinding, Input, OnDestroy, OnInit } from '@angular/core';
import { ControlValueAccessor, FormBuilder, FormGroup, NG_VALUE_ACCESSOR } from '@angular/forms';
import { merge, of, Subject } from 'rxjs';
import { catchError, filter, finalize, map, mergeMap, takeUntil, tap } from 'rxjs/operators';
import { Vehicle } from '../../../../interfaces/vehicle';
import { HttpErrorResponse } from '@angular/common/http';
import { AllVehicleBrandForDropDownDto, FamiliesClient, FamilyByBrandIdDto, KindDto, KindsClient, ModelDto, ModelsClient, VehicleBrandsClient, VehiclesClient } from 'projects/storefront/src/app/web-api-client';
import { VehiclemapperService } from 'projects/storefront/src/app/mapServieces/brandsCluster/vehiclemapper.service';

@Component({
    selector: 'app-vehicle-form',
    templateUrl: './vehicle-form.component.html',
    styleUrls: ['./vehicle-form.component.scss'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => VehicleFormComponent),
            multi: true,
        },
    ],
})
export class VehicleFormComponent implements OnInit, OnDestroy, ControlValueAccessor {
    private destroy$: Subject<void> = new Subject<void>();

    value: Vehicle|null = null;

    form!: FormGroup;

    brands: AllVehicleBrandForDropDownDto[] = [];
    families: FamilyByBrandIdDto[] = [];
    models: ModelDto[] = [];
    Kinds: KindDto[] = [];
    loading = {
        brands: false,
        families: false,
        models: false,
        Kinds: false,
        vin: false,
    };

    errors = {
        vin: false,
    };

    vehicleByFilters: Vehicle|null = null;
    vehicleByVin: Vehicle|null = null;

    @Input() location: 'search' | 'account' | 'modal' = 'search';

    @HostBinding('class.vehicle-form') classVehicleForm = true;

    @HostBinding('class.vehicle-form--layout--search') get classVehicleFormLocationSearch() {
        return this.location === 'search';
    }

    @HostBinding('class.vehicle-form--layout--account') get classVehicleFormLocationAccount() {
        return this.location === 'account';
    }

    @HostBinding('class.vehicle-form--layout--modal') get classVehicleFormLocationModal() {
        return this.location === 'modal';
    }

    changeFn: (_: Vehicle|null) => void = () => {};

    touchedFn: () => void = () => {};

    constructor(
        private fb: FormBuilder,
        private brandsClient : VehicleBrandsClient,
        private familiesClient : FamiliesClient,
        private modelsClient : ModelsClient,
        private kindsClient : KindsClient,
        private vehiclesClient : VehiclesClient,
        private  vehiclemapperService: VehiclemapperService
    ) { }

    ngOnInit(): void {
        this.form = this.fb.group({
            brand: [],
            family: [],
            model: [],
            kind: [],
            vin: [''],
        });

        this.form.controls['brand'].valueChanges.subscribe(value => {
            this.form.controls['family'].setValue('none', { onlySelf: true });

            if (value !== 'none') {
                this.loadMakes();
            }
        });
        this.form.controls['family'].valueChanges.subscribe(value => {
            this.form.controls['model'].setValue('none', { onlySelf: true });

            if (value !== 'none') {
                this.loadModels();
            }
        });
        this.form.controls['model'].valueChanges.subscribe(value => {
            this.form.controls['kind'].setValue('none', { onlySelf: true });

            if (value !== 'none') {
                this.loadEngines();
            }
        });
        this.form.controls['kind'].valueChanges.subscribe(value => {
            if (value !== 'none') {
                let vehicle = new Vehicle();
                vehicle.year = this.brands.find(x => x.id == this.form.controls['brand'].value)?.localizedName ?? '';
                vehicle.make = this.families.find(x => x.id == this.form.controls['family'].value)?.localizedName ?? '';
                vehicle.model = this.models.find(x => x.id == this.form.controls['model'].value)?.localizedName ?? '';
                vehicle.engine = this.Kinds.find(x => x.id == this.form.controls['kind'].value)?.localizedName ?? '';
                vehicle.kindId = this.form.controls['kind'].value;
                this.vehicleByFilters = vehicle;
            } else {
                this.vehicleByFilters = null;
            }
            this.updateValue();
        });

        this.form.valueChanges.subscribe(value => {
            if (value.brand && value.brand !== 'none') {
                this.form.controls['family'].enable({ emitEvent: false });
            } else {
                this.form.controls['family'].disable({ emitEvent: false });
            }

            if (value.family && value.family !== 'none') {
                this.form.controls['model'].enable({ emitEvent: false });
            } else {
                this.form.controls['model'].disable({ emitEvent: false });
            }

            if (value.model && value.model !== 'none') {
                this.form.controls['kind'].enable({ emitEvent: false });
            } else {
                this.form.controls['kind'].disable({ emitEvent: false });
            }
        });

        this.form.setValue({ brand: 'none', family: 'none', model: 'none', kind: 'none', vin: '' });

        this.loadYears();

        this.form.controls['vin'].valueChanges.pipe(
            map(value => value.trim()),
            tap(value => {
                this.loading.vin = value !== '';

                if (value === '') {
                    this.vehicleByVin = null;
                    this.errors.vin = false;

                    this.updateValue();
                }
            }),
            filter(value => value !== ''),
            mergeMap(value => this.vehiclesClient.getVehicleByVinNumber(value).pipe(
                catchError(error => of(error)),
                // finalize(() => {
                //     if (this.form.controls['vin'].value.trim() === value) {
                //         this.loading.vin = false;
                //     }
                // }),
                // Abort vehicle search when component is destroyed or VIN is changed.
                takeUntil(merge(this.destroy$, this.form.controls['vin'].valueChanges)),
            )),
        ).subscribe(value => {
            this.loading.vin = false;
             
            if (value instanceof HttpErrorResponse || value == null) {
                this.vehicleByVin = null;
                this.errors.vin = true;
            } else {
                this.vehicleByVin = this.vehiclemapperService.mapVehicleByVinNumber(value);
                this.errors.vin = false;
                
            }

            this.updateValue();
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    loadYears(): void {
        this.loading.brands = true;

        this.brandsClient.getAllVehicleBrandsForDropDown().pipe(
            finalize(() => this.loading.brands = false),
            takeUntil(this.destroy$),
        ).subscribe(brands => this.brands = brands);
    }

    loadMakes(): void {
        this.loading.families = true;

        const brand = this.form.controls['brand'].value;

        this.familiesClient.getFamiliesByBrandId(brand).pipe(
            finalize(() => this.loading.families = false),
            takeUntil(merge(this.destroy$, this.form.controls['brand'].valueChanges)),
        ).subscribe(families => this.families = families);
    }

    loadModels(): void {
        this.loading.models = true;

        const family = this.form.controls['family'].value;

        this.modelsClient.getModelsByFamilyId(family).pipe(
            finalize(() => this.loading.models = false),
            takeUntil(merge(this.destroy$, this.form.controls['family'].valueChanges)),
        ).subscribe(models => this.models = models);
    }

    loadEngines(): void {
        this.loading.Kinds = true;

        const model = this.form.controls['model'].value;

        this.kindsClient.getKindsByModelId(model).pipe(
            finalize(() => this.loading.Kinds = false),
            takeUntil(merge(this.destroy$, this.form.controls['model'].valueChanges)),
        ).subscribe(Kinds => this.Kinds = Kinds);
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

    writeValue(value: any): void { }

    updateValue(): void {
        const value = this.vehicleByVin || this.vehicleByFilters;
        if (value !== this.value) {
            this.value = value;
            this.changeFn(value);
            this.touchedFn();
        }
    }
}

import { Component, forwardRef, HostBinding, Input, OnDestroy, OnInit } from '@angular/core';
import { BaseFilterItem, CheckVehicleFilter,  } from '../../../../interfaces/filter';
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Subject } from 'rxjs';
import { filter, takeUntil } from 'rxjs/operators';

@Component({
    selector: 'app-filter-vehicle-brand-check',
    templateUrl: './filter-vehicle-brand-check.component.html',
    styleUrls: ['./filter-vehicle-brand-check.component.scss'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => FilterVehicleCheckComponent),
            multi: true,
        },
    ],
})
export class FilterVehicleCheckComponent implements OnInit, OnDestroy, ControlValueAccessor {
    private destroy$: Subject<void> = new Subject<void>();

    value: any[] = [];

    control: FormControl = new FormControl([]);

    @Input() options!: CheckVehicleFilter;

    @HostBinding('class.filter-list') classFilterList = true;

    changeFn: (_: number) => void = () => {};

    touchedFn: () => void = () => {};

    constructor() { }

    ngOnInit(): void {
         
        this.control.valueChanges.pipe(
            filter(value => value !== this.value),
            takeUntil(this.destroy$),
        ).subscribe(value => this.changeFn(value));
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

    writeValue(obj: any): void {
        this.control.setValue(this.value = obj, { emitEvent: false });
    }

    trackBy(index: number, item: BaseFilterItem): string {
        return item.slug;
    }
}

import { Component, forwardRef, HostBinding, Inject, Input, OnInit, PLATFORM_ID } from '@angular/core';
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { RangeFilter } from '../../../../interfaces/filter';
import { LanguageService } from '../../../language/services/language.service';
import { debounceTime, filter, tap } from 'rxjs/operators';
import { isPlatformBrowser } from '@angular/common';

@Component({
    selector: 'app-filter-range',
    templateUrl: './filter-range.component.html',
    styleUrls: ['./filter-range.component.scss'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => FilterRangeComponent),
            multi: true,
        },
    ],
})
export class FilterRangeComponent implements OnInit, ControlValueAccessor {
    control!: FormControl;

    value!: [number, number];

    debouncedValue: [number, number] | null = null;

    isPlatformBrowser = isPlatformBrowser(this.platformId);

    @Input() options!: RangeFilter;

    @HostBinding('class.filter-range') classFilterPrice = true;

    changeFn: (_: [number, number]) => void = () => {};

    touchedFn: () => void = () => {};

    constructor(
        @Inject(PLATFORM_ID) private platformId: any,
        private language: LanguageService
    ) {}

    ngOnInit(): void {
        debugger;
        this.value = [this.options.min, this.options.max];
        this.control = new FormControl(this.value);
        this.control.valueChanges
            .pipe(
                filter(
                    (value) =>
                        value[0] !== this.value[0] || value[1] !== this.value[1]
                ),
                tap((value) => {
                    this.debouncedValue = value;
                }),
                debounceTime(350)
            )
            .subscribe((value) => {
                debugger;
                this.debouncedValue = null;
                this.changeFn(value);
                this.touchedFn();
            });
    }

    registerOnChange(fn: any): void {
        this.changeFn = fn;
    }

    registerOnTouched(fn: any): void {
        this.touchedFn = fn;
    }

    setDisabledState(isDisabled: boolean): void {
        if (isDisabled) {
            this.control.disable({ emitEvent: false });
        } else {
            this.control.enable({ emitEvent: false });
        }
    }

    writeValue(value: any): void {
        if (this.debouncedValue !== null) {
            return;
        }

        this.value = value;
        this.control.setValue(this.value, { emitEvent: false });
    }

    isRTL(): boolean {
        return this.language.isRTL();
    }

    onManualMinChange(min: any): void {
        if (min === '' || min === null || min === undefined) {
            this.value[0] = this.options.min;
        } else if (min >= this.options.min && min <= this.value[1]) {
            this.value[0] = min;
        }
        this.control.setValue(this.value, { emitEvent: true });
    }
    onManualMaxChange(max: any): void {
        if (max === '' || max === null || max === undefined) {
            this.value[1] = this.options.max;
        } else if (max <= this.options.max && max >= this.value[0]) {
            this.value[1] = max;
        }
        this.control.setValue(this.value, { emitEvent: true });
    }
}

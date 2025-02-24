import { Component, HostBinding, Input, forwardRef } from "@angular/core";
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from "@angular/forms";
import { BaseFilterItem, MaterialFilter } from "projects/storefront/src/app/interfaces/filter";

@Component({
    selector: 'app-filter-material',
    templateUrl: './filter-material.component.html',
    styleUrls: ['./filter-material.component.scss'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => FilterMaterialComponent),
            multi: true,
        },
    ],
})
export class FilterMaterialComponent implements ControlValueAccessor {
    value: string[] = [];

    @Input() options!: MaterialFilter;

    @HostBinding('class.filter-material') classFilterMaterial = true;

    changeFn: (_: string[]) => void = () => {};

    touchedFn: () => void = () => {};

    constructor() { }

    registerOnChange(fn: any): void {
        this.changeFn = fn;
    }

    registerOnTouched(fn: any): void {
        this.touchedFn = fn;
    }

    writeValue(value: string[]): void {
        this.value = value;
    }


    onItemChange(item: BaseFilterItem, event: Event): void {
        const checked = (event.target as HTMLInputElement).checked;

        if (checked && !this.isItemChecked(item)) {
            this.value = [...this.value, item.slug];
            this.changeFn(this.value);
        }
        if (!checked && this.isItemChecked(item)) {
            this.value = this.value.filter(x => x !== item.slug);
            this.changeFn(this.value);
        }
    }

    isItemChecked(item: BaseFilterItem): boolean {
        return this.value.includes(item.slug);
    }

    trackBySlug(index: number, item: BaseFilterItem): string {
        return item.slug;
    }
}

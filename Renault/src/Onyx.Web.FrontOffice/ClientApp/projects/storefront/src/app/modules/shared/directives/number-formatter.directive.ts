import { Directive, ElementRef, HostListener } from '@angular/core';
import { NgControl } from '@angular/forms';

@Directive({
    selector: '[appNumberFormatter]',
})
export class NumberFormatterDirective {
    constructor(private el: ElementRef, private control: NgControl) {}

    @HostListener('input', ['$event']) onInputChange(event: any): void {
        const input = this.el.nativeElement;
        const value = input.value.replace(/,/g, '');
        if (!isNaN(value)) {
            input.value = this.formatNumber(value);
            this.control.control?.setValue(value);
        }
    }

    private formatNumber(value: string | number): string {
        return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ',');
    }
}

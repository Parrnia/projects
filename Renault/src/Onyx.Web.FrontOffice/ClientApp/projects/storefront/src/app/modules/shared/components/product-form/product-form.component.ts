import { Component, EventEmitter, forwardRef, HostBinding, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { ProductAttributeOption, ProductOption } from '../../../../interfaces/product';
import {
    AbstractControl,
    ControlValueAccessor,
    FormBuilder,
    FormGroup,
    NG_VALIDATORS,
    NG_VALUE_ACCESSOR,
    ValidationErrors,
    Validator, ValidatorFn,
    Validators,
} from '@angular/forms';
import { colorType } from '../../../../functions/color';

@Component({
    selector: 'app-product-form',
    templateUrl: './product-form.component.html',
    styleUrls: ['./product-form.component.scss'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => ProductFormComponent),
            multi: true,
        },
        {
            provide: NG_VALIDATORS,
            useExisting: forwardRef(() => ProductFormComponent),
            multi: true,
        },
    ],
})
export class ProductFormComponent implements OnChanges, ControlValueAccessor, Validator, OnInit {
    form: FormGroup = this.fb.group({});

    @Input() options: ProductOption[] = [];
    @Input() productAttributeOptions : ProductAttributeOption[] = [];
    @HostBinding('class.product-form') classProductForm = true;

    @Output() getOptionsValuesChanges = new EventEmitter<(string | undefined)[]>();

    selectedColor : string = '';
    selectedMaterail : string = '';

    changeFn: (_: any) => void = () => {};

    touchedFn: () => void = () => {};

    constructor(
        private fb: FormBuilder,
    ) {
        
     }
    ngOnInit(): void {
         
        const controls: {[key: string]: [null]} = {};

            this.options.forEach(option => {
                controls[option.slug] = [null];
            });
            this.form = this.fb.group(controls);
            this.selectedColor = this.productAttributeOptions.find(t => t.isDefault == true)?.optionValues.find(e => e.name == 'Color')?.value ?? '';
            this.selectedMaterail = this.productAttributeOptions.find(t => t.isDefault == true)?.optionValues.find(e => e.name == 'Material')?.value ?? '';
            this.setColorOption(this.selectedColor);
            this.setMaterialOption(this.selectedMaterail);
            this.form.updateValueAndValidity();
             
    }
    ngOnChanges(changes: SimpleChanges): void {
        if (changes['options']) {
            this.form.valueChanges.subscribe(value => {
                this.changeFn(value);
                this.touchedFn();
            });
        }
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

        const baseValue: {[key: string]: null} = {};

        this.options.forEach(option => baseValue[option.slug] = null);
        this.form.setValue({ ...baseValue, ...value }, { emitEvent: false });
    }

    validate(control: AbstractControl): ValidationErrors {
        return this.form.valid ? {} : { options: this.form.errors };
    }

    isWhite(color: string): boolean {
        return colorType(color) === 'white';
    }
    setColorOption(value : string){
         
        this.form?.controls['color']?.setValue(value.toLowerCase());
        this.getOptionsValuesChanges.emit([this.form.controls['color']?.value,this.form.controls['material']?.value]);
    }
    setMaterialOption(value : string){
         
        this.form?.controls['material']?.setValue(value.toLowerCase());
        this.getOptionsValuesChanges.emit([this.form.controls['color']?.value,this.form.controls['material']?.value]);
    }
}

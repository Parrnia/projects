import { Component, ElementRef, HostBinding, Input, ViewChild, forwardRef } from '@angular/core';
import { SearchFilter } from '../../../../interfaces/filter';
import { UrlService } from '../../../../services/url.service';
import { FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Subject, distinctUntilChanged, filter, map, of, switchMap, takeUntil } from 'rxjs';

@Component({
    selector: 'app-filter-search',
    templateUrl: './filter-search.component.html',
    styleUrls: ['./filter-search.component.scss'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => FilterSearchComponent),
            multi: true,
        },
    ]
})
export class FilterSearchComponent {
    @ViewChild('input')
    inputElement!: ElementRef<HTMLInputElement>;
    private destroy$: Subject<void> = new Subject<void>();
    control: FormControl = new FormControl(false);
    value: string = '';
    placeHolder: string = '';
    defualtInput: string = '';


    @Input() options!: SearchFilter;

    @HostBinding('class.filter-search') classFilterSearch = true;


    changeFn: (_: string | null) => void = () => { };

    touchedFn: () => void = () => { };

    constructor(
        public url: UrlService,
    ) { }

    ngAfterViewInit() {
        this.inputElement?.nativeElement.addEventListener('input', this.onInputChange.bind(this));
      }
    onInputChange() {
        const inputValue = this.inputElement?.nativeElement.value;
        if (inputValue === '') {
            this.setToForm('');
        }
      }
    ngOnInit(): void {
         
        if(this.options.value == ''){
            this.placeHolder = 'جستجو...';
        }else{
            this.defualtInput = this.options.value;
            this.value = this.options.value;
        }
        this.control.valueChanges.pipe(
            switchMap(() => of(this.value).pipe(
                map(value => this.control.value && value ? value : null),
                distinctUntilChanged(),
            )),
            takeUntil(this.destroy$),
        ).subscribe(value => {
            this.changeFn(value);
        });

        of(this.value).pipe(
            takeUntil(this.destroy$),
        ).subscribe(x => this.control.setValue(x));
    }
    setToForm(value: string) {
        this.value = value;
        this.control.setValue(value);
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
    }
}

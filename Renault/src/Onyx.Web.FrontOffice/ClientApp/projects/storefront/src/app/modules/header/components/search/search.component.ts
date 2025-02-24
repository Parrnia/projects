import {
    AfterViewInit,
    Component,
    ElementRef,
    HostBinding,
    Inject,
    NgZone,
    OnChanges,
    OnDestroy,
    OnInit,
    PLATFORM_ID,
    ViewChild,
} from '@angular/core';
import { FormControl } from '@angular/forms';
import {
    BehaviorSubject,
    fromEvent,
    observable,
    Observable,
    Subject,
    of,
    forkJoin,
} from 'rxjs';
import { Vehicle } from '../../../../interfaces/vehicle';
import {
    debounceTime,
    distinctUntilChanged,
    filter,
    map,
    switchMap,
    takeUntil,
} from 'rxjs/operators';
import { Product } from '../../../../interfaces/product';
import { ShopCategory } from '../../../../interfaces/category';
import { UrlService } from '../../../../services/url.service';
import { isPlatformBrowser } from '@angular/common';
import { fromOutsideClick } from '../../../../functions/rxjs/from-outside-click';
import { TranslateService } from '@ngx-translate/core';
import {
    SearchClient,
    VehicleDto,
    VehiclesClient,
} from 'projects/storefront/src/app/web-api-client';
import { CategorymapperService } from 'projects/storefront/src/app/mapServieces/categoriesCluster/categorymapper.service';
import { ProductmapperService } from 'projects/storefront/src/app/mapServieces/productsCluster/productmapper.service';
import { VehiclemapperService } from 'projects/storefront/src/app/mapServieces/brandsCluster/vehiclemapper.service';
import { CurrentVehicleService } from 'projects/storefront/src/app/services/current-vehicle.service';
import { UserVehicleService } from 'projects/storefront/src/app/services/user-vehicle.service';
import { UserVehicleAccountService } from 'projects/storefront/src/app/services/user-vehicle-account.service';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';

@Component({
    selector: 'app-search',
    templateUrl: './search.component.html',
    styleUrls: ['./search.component.scss'],
})
export class SearchComponent implements OnInit, OnDestroy, AfterViewInit {
    private destroy$: Subject<void> = new Subject<void>();

    query$: Subject<string> = new Subject<string>();

    suggestionsIsOpen = false;

    hasSuggestions = false;

    searchPlaceholder$!: Observable<string>;

    vehiclePickerIsOpen = false;

    vehiclePanel: 'list' | 'form' = 'form';

    vehicles$!: Observable<Vehicle[]>;

    currentVehicle!: Vehicle | null;

    currentVehicleControl: FormControl = new FormControl(
        JSON.parse(localStorage.getItem('currentVehicle') ?? '{}')
    );

    addVehicleControl: FormControl = new FormControl(null);

    products: Product[] = [];

    categories: ShopCategory[] = [];

    @HostBinding('class.search') classSearch = true;

    @ViewChild('selectVehicleButton')
    selectVehicleButton!: ElementRef<HTMLElement>;

    @ViewChild('vehiclePickerDropdown')
    vehiclePickerDropdown!: ElementRef<HTMLElement>;

    get element(): HTMLElement {
        return this.elementRef.nativeElement;
    }

    constructor(
        @Inject(PLATFORM_ID) private platformId: any,
        private zone: NgZone,
        private translate: TranslateService,
        private elementRef: ElementRef,
        public url: UrlService,
        private searchClient: SearchClient,
        private productmapperService: ProductmapperService,
        private categorymapperService: CategorymapperService,
        public currentVehicleService: CurrentVehicleService,
        private userVehicleService: UserVehicleService,
        private userVehicleAccountService: UserVehicleAccountService,
        private authService: AuthService
    ) {}

    ngOnInit(): void {
        this.loadVehicles();

        this.currentVehicle = this.currentVehicleControl.value;

        this.authService.isLoggedIn().subscribe((isLoggedIn) => {
            if (isLoggedIn) {
                this.currentVehicleControl.valueChanges
                    .pipe(
                        switchMap((vehicle) =>
                            this.userVehicleAccountService.vehicles$.pipe(
                                map((vehicles) => {
                                    return (
                                        vehicles.find(
                                            (x) => x.kindId === vehicle?.kindId
                                        ) || null
                                    );
                                })
                            )
                        )
                    )
                    .subscribe((vehicle) => {
                        let vehicleForset = vehicle;
                        this.currentVehicleService.value =
                            vehicleForset?.id != 0 ? vehicleForset : null;
                        this.currentVehicle = this.currentVehicleService.value;
                        this.loadPlaceHolder();
                    });
            } else {
                this.currentVehicleControl.valueChanges.subscribe(
                    (vehicleId) => {
                        let vehicleForset =
                            this.userVehicleService.vehicles.find(
                                (vehicle) =>
                                    +vehicle.kindId == vehicleId?.kindId
                            ) ?? null;
                        this.currentVehicle =
                            vehicleForset?.id != 0 ? vehicleForset : null;
                        this.currentVehicleService.value = this.currentVehicle;
                        this.loadPlaceHolder();
                    }
                );
            }
        });

        this.query$
            .pipe(
                distinctUntilChanged(),
                switchMap((query) => {
                    return this.searchClient.getSearchSuggestions(
                        4,
                        5,
                        query != '' || null ? query : 'NothingInserted',
                        this.currentVehicle?.kindId ?? -1
                    );
                }),
                takeUntil(this.destroy$)
            )
            .subscribe((result) => {
                if (
                    result.products?.items?.length === 0 &&
                    result.productCategories?.items?.length === 0
                ) {
                    this.hasSuggestions = false;
                    return;
                }

                this.hasSuggestions = true;

                this.products =
                    this.productmapperService.mapProductsForSearchSuggestion(
                        result.products?.items ?? []
                    );
                this.products = this.products.slice(0,4);
                this.categories =
                    this.categorymapperService.mapProductCategoriesForSearchSuggestion(
                        result.productCategories?.items ?? []
                    );

            });
        this.loadPlaceHolder();
    }
    loadPlaceHolder() {
        console.log('loadPlaceHolder called');
        this.searchPlaceholder$ = this.currentVehicleService.value$.pipe(
            switchMap((vehicle) => {
                console.log('Current vehicle:', vehicle);

                if (vehicle) {
                    return this.translate.stream(
                        'INPUT_SEARCH_PLACEHOLDER_VEHICLE',
                        vehicle
                    );
                } else {
                    return this.translate.stream('INPUT_SEARCH_PLACEHOLDER');
                }
            })
        );
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    ngAfterViewInit(): void {
        if (!isPlatformBrowser(this.platformId)) {
            return;
        }

        this.zone.runOutsideAngular(() => {
            fromOutsideClick([
                this.selectVehicleButton.nativeElement,
                this.vehiclePickerDropdown.nativeElement,
            ])
                .pipe(
                    filter(() => this.vehiclePickerIsOpen),
                    takeUntil(this.destroy$)
                )
                .subscribe(() => {
                    this.zone.run(() => (this.vehiclePickerIsOpen = false));
                });

            fromOutsideClick(this.element)
                .pipe(
                    filter(() => this.suggestionsIsOpen),
                    takeUntil(this.destroy$)
                )
                .subscribe(() => {
                    this.zone.run(() => this.toggleSuggestions(false));
                });

            fromEvent(this.element, 'focusout')
                .pipe(debounceTime(10), takeUntil(this.destroy$))
                .subscribe(() => {
                    if (document.activeElement === document.body) {
                        return;
                    }
                    // Close suggestions if the focus received an external element.
                    if (
                        document.activeElement &&
                        document.activeElement.closest('.search') !==
                            this.element
                    ) {
                        this.zone.run(() => this.toggleSuggestions(false));
                    }
                });
        });
    }

    search(query: string): void {
        this.query$.next(query);
    }

    toggleSuggestions(force?: boolean): void {
        this.suggestionsIsOpen =
            force !== undefined ? force : !this.suggestionsIsOpen;

        if (this.suggestionsIsOpen) {
            this.toggleVehiclePicker(false);
        }
    }

    toggleVehiclePicker(force?: boolean): void {
        this.vehiclePickerIsOpen =
            force !== undefined ? force : !this.vehiclePickerIsOpen;

        if (this.vehiclePickerIsOpen) {
            this.toggleSuggestions(false);
        }
    }

    onInput(event: Event): void {
        const input = event.target as HTMLInputElement;

        this.search(input.value);
    }

    onInputFocus(event: FocusEvent): void {
        const input = event.target as HTMLInputElement;

        this.toggleSuggestions(true);
        this.search(input.value);
    }
    onVehicleAdded(vehicle: Vehicle): void {
        this.vehiclePanel = 'list';
        this.currentVehicle = vehicle;
        this.currentVehicleControl.setValue(this.currentVehicle, {
            emitEvent: false,
        });
        this.currentVehicleService.value = this.currentVehicle;
        this.loadVehicles();
    }
    loadVehicles() {
        this.authService.isLoggedIn().subscribe({
            next: (res) => {
                if (res) {
                    this.vehicles$ = this.userVehicleAccountService.vehicles$;
                } else {
                    this.vehicles$ = this.userVehicleService.vehicles$;
                }
            },
        });
    }
}

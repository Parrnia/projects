import { Component, Input, OnInit } from '@angular/core';
import { WishlistService } from '../../services/wishlist.service';
import { Observable, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { CartService } from '../../services/cart.service';
import { HeaderService } from '../../services/header.service';
import { TranslateService } from '@ngx-translate/core';
import { UrlService } from '../../services/url.service';
import { CorporationInfoDto } from '../../web-api-client';
import { AuthService } from '../../services/authService/auth.service';
import { CorporationInfo } from '../../interfaces/corporation-info';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit {
    public email$: Observable<string | null> = of(null);

    departmentsLabel$!: Observable<string>;
    departmentsItems!: any;
    mainMenuItems!: any;
    topRightMenu: any = [];
    @Input() corporationInfo: CorporationInfo | undefined;
    isAuth$: Observable<boolean>;

    constructor(
        private translate: TranslateService,
        public wishlist: WishlistService,
        public cart: CartService,
        public header: HeaderService,
        public url: UrlService,
        private authService: AuthService
    ) {
        this.isAuth$ = this.authService.isLoggedIn();
    }

    ngOnInit(): void {
        this.departmentsLabel$ = this.header.desktopLayout$.pipe(
            switchMap((layout) =>
                this.translate.stream(
                    layout === 'spaceship'
                        ? 'BUTTON_DEPARTMENTS'
                        : 'BUTTON_DEPARTMENTS_LONG'
                )
            )
        );

        this.getAllDepartmentsItems();
        this.authService.isLoggedIn().subscribe({
            next: (res) => {
                if (res) {
                    this.authService.getCustomer().subscribe({
                        next: () => {
                            this.email$ = this.authService.getEmail();
                        },
                    });
                }
            },
            error: (err) => console.log(err),
        });
    }

    public getAllDepartmentsItems() {
        // this.client.getAllHeaderMenu(undefined).subscribe(result => {
        //     this.departmentsItems = result.departmentMenu;
        //     this.mainMenuItems = result.mainMenu;
        //     this.topRightMenu = result.topRightMenu;
        // }, error => console.error(error));
    }
}

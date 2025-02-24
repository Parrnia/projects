import { blogCategoriesList } from './../../../../../fake-server/database/categories';
import { Component, EventEmitter, HostBinding, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { Observable, Subject, of } from 'rxjs';
import { finalize, map, takeUntil } from 'rxjs/operators';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';
import { CustomersClient } from 'projects/storefront/src/app/web-api-client';
import { CartService } from 'projects/storefront/src/app/services/cart.service';
import { CaptchaValidation } from 'projects/storefront/src/app/services/authService/models/entities/CaptchaValidation';
import { PageCaptchaComponent } from '../../../account/pages/captcha/captcha.component';
import { ImageService } from 'projects/storefront/src/app/mapServieces/image.service';

@Component({
    selector: 'app-account-menu',
    templateUrl: './account-menu.component.html',
    styleUrls: ['./account-menu.component.scss'],
})
export class AccountMenuComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    isAuth$: Observable<boolean>;
    firstName$!: Observable<string | null>;
    lastName$!: Observable<string | null>;
    email$!: Observable<string | null>;
    avatar$!: Observable<string | undefined>;
    isLoaded: boolean;
    captchaId!: string;

    form!: FormGroup;

    loginInProgress = false;

    @Output() closeMenu: EventEmitter<void> = new EventEmitter<void>();

    @HostBinding('class.account-menu') classAccountMenu = true;
    @ViewChild(PageCaptchaComponent) captchaComponent!: any;
    constructor(
        private fb: FormBuilder,
        public router: Router,
        public authService: AuthService,
        private customersClient: CustomersClient,
        private cartService: CartService,
        private imageService: ImageService
    ) {
        this.isAuth$ = this.authService.isLoggedIn();
        this.isLoaded = false;
        this.authService.isLoggedIn().subscribe({
            next: res => {
                if (res) {
                    this.authService.getCustomer().subscribe({
                        next: () => {
                            this.firstName$ = this.authService.getFirstName();
                            this.lastName$ = this.authService.getLastName();
                            this.email$ = this.authService.getEmail();
                            this.isLoaded = true;
                        }
                    });
                    let user = this.customersClient.getCustomerById();
                    this.avatar$ = user.pipe(map(c => this.imageService.makeImageUrl(c?.avatar)));
                }
            },
            error: err => console.log(err)
        });
    }

    ngOnInit(): void {
        this.form = this.fb.group({
            userName: [, [Validators.required]],
            password: [, [Validators.required]],
            captchaCode: [, [Validators.required]]
        });
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    setCaptcha(captchaValidation: CaptchaValidation){
        this.captchaId = captchaValidation.captchaId;
        this.form.controls['captchaCode'].setValue(captchaValidation.captchaCode);
    }
    
    login(): void {
        
        this.form.markAllAsTouched();

        if (this.loginInProgress || this.form.invalid) {
            return;
        }

        this.loginInProgress = true;
        this.authService.passwordSignIn({ username: this.form.value.userName, password: this.form.value.password,
            captchaCode : this.form.value.captchaCode, captchaId : this.captchaId },'/account/dashboard')
            .subscribe({
                next: () => {
                    
                    this.loginInProgress = false;
                    this.cartService.getProductsForNewIdentityState().subscribe({
                        next: (res) => this.cartService.calc()
                    });
                    this.closeMenu.emit();
                    this.captchaComponent.reLoad();
                },
                error: () => {
                    
                    this.loginInProgress = false;
                    this.captchaComponent.reLoad();
                }
            });
    }

    logout(): void {
        this.authService.logout();
        this.closeMenu.emit();
        this.router.navigateByUrl('/account/login').then();
    }
}

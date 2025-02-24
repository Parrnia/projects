import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { mustMatchValidator } from '../../../../functions/validators/must-match';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';
import { CaptchaValidation } from 'projects/storefront/src/app/services/authService/models/entities/CaptchaValidation';
import { CartService } from 'projects/storefront/src/app/services/cart.service';
import { PageCaptchaComponent } from '../captcha/captcha.component';

@Component({
    selector: 'app-page-login',
    templateUrl: './page-login.component.html',
    styleUrls: ['./page-login.component.scss'],
})
export class PageLoginComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    loginForm!: FormGroup;
    loginInProgress = false;
    showPassword: boolean = false;
    captchaId!: string;
    returnUrl!: string;
    @ViewChild(PageCaptchaComponent) captchaComponent!: any;
    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private cartService: CartService,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        this.route.queryParams.subscribe(params => {
            
            this.returnUrl = params['returnUrl'] || '/account/dashboard';
          });

        this.loginForm = this.fb.group({
            userName: [, [Validators.required]],
            password: [, [Validators.required]],
            captchaCode: [, [Validators.required]],
            remember: [false],
        });
    }

    togglePasswordVisibility() {
        this.showPassword = !this.showPassword;
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
    setCaptcha(captchaValidation: CaptchaValidation) {
        this.captchaId = captchaValidation.captchaId;
        this.loginForm.controls['captchaCode'].setValue(captchaValidation.captchaCode);
    }

    login(): void {
        this.loginForm.markAllAsTouched();

        if (this.loginInProgress || this.loginForm.invalid) {
            return;
        }

        this.loginInProgress = true;

        this.authService.passwordSignIn({
            username: this.loginForm.value.userName, password: this.loginForm.value.password,
            captchaCode: this.loginForm.value.captchaCode, captchaId: this.captchaId
        }, this.returnUrl)
            .subscribe({
                next: () => {
                    this.loginInProgress = false;
                    this.cartService.getProductsForNewIdentityState().subscribe({
                        next: (res) => this.cartService.calc()
                    });
                },
                error: () => {
                    this.loginInProgress = false;
                    this.captchaComponent.reLoad();
                }
            });
    }
}

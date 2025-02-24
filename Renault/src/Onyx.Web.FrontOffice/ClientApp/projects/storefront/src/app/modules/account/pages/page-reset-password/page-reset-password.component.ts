import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { mustMatchValidator } from '../../../../functions/validators/must-match';
import { Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';
import { AccountApi } from '../../../../api';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';
import { CaptchaValidation } from 'projects/storefront/src/app/services/authService/models/entities/CaptchaValidation';
import { PageCaptchaComponent } from '../captcha/captcha.component';

@Component({
    selector: 'app-page-reset-password',
    templateUrl: './page-reset-password.component.html',
    styleUrls: ['./page-reset-password.component.scss'],
})
export class PageResetPasswordComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    form!: FormGroup;
    captchaId!: string;

    saveInProgress = false;
    @ViewChild(PageCaptchaComponent) captchaComponent!: any;
    showPassword: boolean = false;
    showConfirmPassword: boolean = false;
    phoneNumber = this.maskMiddleCharacters(localStorage.getItem('phoneNumber') ?? '');
    constructor(
        private toastr: ToastrService,
        private translate: TranslateService,
        private fb: FormBuilder,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        this.form = this.fb.group({
            code: ['', [Validators.required]],
            newPassword: [, [Validators.required ,
                Validators.minLength(8),
                (control: AbstractControl) => {
                    const password = control.value;
                    if (!password) {
                        return null;
                    }
            
                    if (!/\d/.test(password)) {
                        return { missingDigit: true };
                    }
            
                    if (!/[a-z]/.test(password)) {
                        return { missingLowercase: true };
                    }
            
                    if (!/[A-Z]/.test(password)) {
                        return { missingUppercase: true };
                    } 
            
                    if (new Set(password).size < 3) {
                        return { notEnoughUniqueChars: true };
                    }
            
                    return null;
                }]
            ],
            confirmPassword: ['', [Validators.required]],
            captchaCode: [, [Validators.required]],
        }, { validators: [mustMatchValidator('newPassword', 'confirmPassword')] });
    }
    togglePasswordVisibility() {
        this.showPassword = !this.showPassword;
    }
    toggleConfirmPasswordVisibility() {
        this.showConfirmPassword = !this.showConfirmPassword;
    }

    setCaptcha(captchaValidation: CaptchaValidation){
        this.captchaId = captchaValidation.captchaId;
        this.form.controls['captchaCode'].setValue(captchaValidation.captchaCode);
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    save(): void {
        this.form.markAllAsTouched();

        if (this.saveInProgress || this.form.invalid) {
            return;
        }

        this.saveInProgress = true;
        let userId = localStorage.getItem('userId')!;
        this.authService.resetPassword({
            userId: userId,
            code: this.form.value.code,
            confirmPassword: this.form.value.confirmPassword,
            newPassword: this.form.value.newPassword,
            captchaId: this.captchaId,
            captchaCode: this.form.value.captchaCode
        }
        ).pipe(
            finalize(() => this.saveInProgress = false),
            takeUntil(this.destroy$),
        ).subscribe(
            () => {
                this.toastr.success(this.translate.instant('TEXT_TOAST_PASSWORD_CHANGED'));

            },
            error => {
                this.captchaComponent.reLoad();
                if (error instanceof HttpErrorResponse) {
                    this.form.setErrors({
                        server: `ERROR_API_${error.error.message}`,
                    });
                } else {
                    alert(error);
                }
            },
        );
    }
    private maskMiddleCharacters(inputString: string): string {

        const start = inputString.slice(0, 2);
        const end = inputString.slice(-2);
        const middle = "*******"; // 7 asterisks

        return ' کد تایید به شماره' + end + middle + start + 'ارسال شد';
    }
}

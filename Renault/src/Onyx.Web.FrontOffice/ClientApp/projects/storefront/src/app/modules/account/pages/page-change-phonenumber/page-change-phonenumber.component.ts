import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { mustMatchValidator } from '../../../../functions/validators/must-match';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';
import { CaptchaValidation } from 'projects/storefront/src/app/services/authService/models/entities/CaptchaValidation';
import { PageCaptchaComponent } from '../captcha/captcha.component';
import { VerifyActivationCodeCommand } from 'projects/storefront/src/app/services/authService/models/commands/VerifyActivationCodeCommand';
import { ChangePhoneNumberCommand } from 'projects/storefront/src/app/services/authService/models/commands/ChangePhoneNumberCommand';

@Component({
    selector: 'app-page-change-phonenumber',
    templateUrl: './page-change-phonenumber.component.html',
    styleUrls: ['./page-change-phonenumber.component.scss'],
})
export class PageChangePhonenumberComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    changeForm!: FormGroup;
    changeProgress = false;
    resendProgress = false;
    captchaId!: string;
    @ViewChild(PageCaptchaComponent) captchaComponent!: any;
    resendDisabled: boolean = false;
    countdown: number = 60;
    intervalId: any;
    showPassword: boolean = false;
    phoneNumber = this.maskMiddleCharacters(localStorage.getItem('phoneNumber') ?? '');
    constructor(
        private fb: FormBuilder,
        private router: Router,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        this.changeForm = this.fb.group({
            password: [, [Validators.required]],
            phoneNumber: [, [Validators.required, Validators.maxLength(11), Validators.minLength(11), Validators.pattern(/^(?:\+98|0)?9\d{9}$/)]],
            captchaCode: [, [Validators.required]]
        });
    }
    togglePasswordVisibility() {
        this.showPassword = !this.showPassword;
    }
    setCaptcha(captchaValidation: CaptchaValidation) {
        this.captchaId = captchaValidation.captchaId;
        this.changeForm.controls['captchaCode'].setValue(captchaValidation.captchaCode);
    }
    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    change(): void {
        this.changeForm.markAllAsTouched();
        
        if (this.changeProgress || this.changeForm.invalid) {
            return;
        }

        this.changeProgress = true;
        let cmd = new ChangePhoneNumberCommand();
        
        cmd.userId = localStorage.getItem('userId')!;
        cmd.password = this.changeForm.value.password;
        cmd.phoneNumber = this.changeForm.value.phoneNumber;
        cmd.captchaCode = this.changeForm.value.captchaCode;
        cmd.captchaId = this.captchaId;
        this.authService.changePhoneNumber(cmd)
            .subscribe({
                next: () => {
                    this.changeProgress = false;
                    this.router.navigateByUrl('/account/confirmPhonenumber');
                },
                error: () => {
                    this.changeProgress = false;
                    this.captchaComponent.reLoad();
                }
            });
    }

    private maskMiddleCharacters(inputString: string): string {
        return ' کد تایید به شماره' + inputString + 'ارسال شد';
    }
}

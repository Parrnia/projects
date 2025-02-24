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
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-page-confirm-phonenumber',
    templateUrl: './page-confirm-phonenumber.component.html',
    styleUrls: ['./page-confirm-phonenumber.component.scss'],
})
export class PageConfirmPhonenumberComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    confirmForm!: FormGroup;
    confirmProgress = false;
    resendProgress = false;
    captchaId!: string;
    @ViewChild(PageCaptchaComponent) captchaComponent!: any;
    resendDisabled: boolean = false;
    countdown: number = 60;
    intervalId: any;
    phoneNumber = this.maskMiddleCharacters(localStorage.getItem('phoneNumber') ?? '');
    constructor(
        private fb: FormBuilder,
        private router: Router,
        private toastr: ToastrService,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        this.confirmForm = this.fb.group({
            code: [, [Validators.required]],
            captchaCode: [, [Validators.required]]
        });
    }
    setCaptcha(captchaValidation: CaptchaValidation) {
        this.captchaId = captchaValidation.captchaId;
        this.confirmForm.controls['captchaCode'].setValue(captchaValidation.captchaCode);
    }
    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    confirm(): void {
        this.confirmForm.markAllAsTouched();

        if (this.confirmProgress || this.confirmForm.invalid) {
            return;
        }

        this.confirmProgress = true;
        let cmd = new VerifyActivationCodeCommand();
        
        cmd.userId = localStorage.getItem('userId')!;
        cmd.code = this.confirmForm.value.code;
        cmd.captchaCode = this.confirmForm.value.captchaCode;
        cmd.captchaId = this.captchaId;
        this.authService.verifyActivationCode(cmd)
            .subscribe({
                next: () => this.confirmProgress = false,
                error: () => {
                    this.confirmProgress = false;
                    this.captchaComponent.reLoad();
                }
            });
    }
    resendActivationCode(): void {

        if (this.resendProgress || this.confirmForm.controls['captchaCode'].invalid) {
            this.toastr.error('کد کپچا به درستی وارد نشده است');
            return;
        }

        this.resendDisabled = true;

        this.intervalId = setInterval(() => {
            if (this.countdown > 0) {
                this.countdown--;
            } else {
                this.resendDisabled = false;
                clearInterval(this.intervalId);
                this.countdown = 60;
            }
        }, 1000);

        this.confirmForm.controls['captchaCode'].markAsTouched();



        this.resendProgress = true;
        this.authService.resendActivationCode({
            userId: localStorage.getItem('userId')!,
            captchaCode: this.confirmForm.value.captchaCode, captchaId: this.captchaId
        })
            .subscribe({
                next: () => {
                    this.resendProgress = false;
                    this.captchaComponent.reLoad();
                },
                error: () => {
                    this.resendProgress = false;
                    this.captchaComponent.reLoad();
                }
            });
    }

    private maskMiddleCharacters(inputString: string): string {
        return ' کد تایید به شماره' + inputString + 'ارسال شد';
    }
}

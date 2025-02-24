import { ForgetPasswordCommand } from '../../../../services/authService/models/commands/ForgetPasswordCommand';
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

@Component({
    selector: 'app-page-forgot-password',
    templateUrl: './page-forgot-password.component.html',
    styleUrls: ['./page-forgot-password.component.scss'],
})
export class PageForgotPasswordComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    form!: FormGroup;
    loginInProgress = false;
    captchaId!: string;

    @ViewChild(PageCaptchaComponent) captchaComponent!: any;
    constructor(
        private fb: FormBuilder,
        private router: Router,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        this.form = this.fb.group({
            userName: [, [Validators.required]],
            captchaCode: [, [Validators.required]],
        });
    }

    setCaptcha(captchaValidation: CaptchaValidation){
        this.captchaId = captchaValidation.captchaId;
        this.form.controls['captchaCode'].setValue(captchaValidation.captchaCode);
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    confirm(): void {
        this.form.markAllAsTouched();

        if (this.loginInProgress || this.form.invalid) {
            return;
        }

        this.loginInProgress = true;

        this.authService.forgotPassword({ userNameOrNationalCode: this.form.value.userName,
            captchaCode : this.form.value.captchaCode, captchaId : this.captchaId })
            .subscribe({
                next: () => this.loginInProgress = false,
                error: () => {
                    this.loginInProgress = false;
                    this.captchaComponent.reLoad();
                }
            });
    }
}

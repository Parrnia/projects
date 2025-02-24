import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { mustMatchValidator } from '../../../../functions/validators/must-match';
import { Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';
import { AccountApi } from '../../../../api';
import { HttpErrorResponse } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';

@Component({
    selector: 'app-page-password',
    templateUrl: './page-password.component.html',
    styleUrls: ['./page-password.component.scss'],
})
export class PagePasswordComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    form!: FormGroup;
    showPassword: boolean = false;
    showConfirmPassword: boolean = false;
    showCurrentPassword:boolean = false;
    saveInProgress = false;

    constructor(
        private toastr: ToastrService,
        private translate: TranslateService,
        private fb: FormBuilder,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        this.form = this.fb.group({
            currentPassword: ['', [Validators.required]],
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
        }, { validators: [mustMatchValidator('newPassword', 'confirmPassword')] });
    }
    togglePasswordVisibility() {
        this.showPassword = !this.showPassword;
    }
    toggleConfirmPasswordVisibility() {
        this.showConfirmPassword = !this.showConfirmPassword;
    }
    toggleCurrentPasswordVisibility(){
        this.showCurrentPassword = !this.showCurrentPassword;
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
        this.authService.changePassword({
            userId: userId,
            currentPassword: this.form.value.currentPassword,
            confirmPassword: this.form.value.confirmPassword,
            newPassword: this.form.value.newPassword
        }
        ).pipe(
            finalize(() => this.saveInProgress = false),
            takeUntil(this.destroy$),
        ).subscribe(
            () => {
                this.toastr.success(this.translate.instant('TEXT_TOAST_PASSWORD_CHANGED'));
            },
            error => {
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
}

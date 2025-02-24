import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { mustMatchValidator } from '../../../../functions/validators/must-match';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';
import { CustomerTypeEnum } from 'projects/storefront/src/app/web-api-client';
import { CaptchaValidation } from 'projects/storefront/src/app/services/authService/models/entities/CaptchaValidation';
import { PageCaptchaComponent } from '../captcha/captcha.component';
import { PersonType } from 'projects/storefront/src/app/services/authService/models/entities/PersonType';
import { AuthValidators } from 'projects/storefront/src/app/functions/validators/auth-validators';

@Component({
    selector: 'app-page-register',
    templateUrl: './page-register.component.html',
    styleUrls: ['./page-register.component.scss'],
})
export class PageRegisterComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();


    registerForm!: FormGroup;
    registerInProgress = false;
    captchaId!: string;
    showPassword: boolean = false;
    showConfirmPassword: boolean = false;
    @ViewChild(PageCaptchaComponent) captchaComponent!: any;
    selectedFormPart: string = 'natural';
    constructor(
        private fb: FormBuilder,
        private router: Router,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        this.registerForm = this.fb.group({
            firstName: [, [Validators.required]],
            lastName: [, [Validators.required]],
            userName: [, [Validators.required, Validators.pattern(/^[A-Za-z0-9]+$/)] ],
            nationalCode: [, [Validators.required]],
            phoneNumber: [, [Validators.required,Validators.maxLength(11),Validators.minLength(11),Validators.pattern(/^(?:\+98|0)?9\d{9}$/)]],
            email: [, [Validators.required, Validators.email]],
            avatar: [],
            password: [, [Validators.required ,
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
            confirmPassword: [, [Validators.required]],
            captchaCode: [,[Validators.required]]
        }, { validators: [mustMatchValidator('password', 'confirmPassword')] })
          this.registerForm.get('userName')?.addAsyncValidators(
            AuthValidators.validUniqueUserName(this.authService,'')
          );
          this.registerForm.get('nationalCode')?.addAsyncValidators(
            AuthValidators.validUniqueNationalCode(this.authService,'')
          );
      
          this.registerForm.get('id')?.valueChanges.subscribe((id) => {
            this.registerForm.get('userName')?.setAsyncValidators(
                AuthValidators.validUniqueUserName(
                this.authService,
                id != null ? id : 0
              )
            );
            this.registerForm.get('nationalCode')?.setAsyncValidators(
                AuthValidators.validUniqueNationalCode(
                this.authService,
                id != null ? id : 0
              )
            );
          });
    }
    togglePasswordVisibility() {
        this.showPassword = !this.showPassword;
    }
    toggleConfirmPasswordVisibility() {
        this.showConfirmPassword = !this.showConfirmPassword;
    }
    setCaptcha(captchaValidation: CaptchaValidation){
        this.captchaId = captchaValidation.captchaId;
        this.registerForm.controls['captchaCode'].setValue(captchaValidation.captchaCode);
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    register(): void {
        this.registerForm.markAllAsTouched();

        if (this.registerInProgress || this.registerForm.invalid) {
            return;
        }
         
        this.registerInProgress = true;

        this.authService.register({
            firstName: this.registerForm.value.firstName,
            lastName: this.registerForm.value.lastName,
            userName: this.registerForm.value.userName,
            nationalCode: this.registerForm.value.nationalCode,
            phoneNumber: this.registerForm.value.phoneNumber,
            email: this.registerForm.value.email,
            customerType: CustomerTypeEnum.Personal,
            personType: this.selectedFormPart === 'legal' ? PersonType.Legal : PersonType.Natural,
            password: this.registerForm.value.password,
            confirmPassword: this.registerForm.value.confirmPassword,
            captchaCode: this.registerForm.value.captchaCode,
            captchaId: this.captchaId
        },
        this.registerForm.value.avatar)
            .subscribe({
                next: () => {this.registerInProgress = false},
                error: () => {
                    this.registerInProgress = false;
                    this.captchaComponent.reLoad();
                }
            });
    }
}

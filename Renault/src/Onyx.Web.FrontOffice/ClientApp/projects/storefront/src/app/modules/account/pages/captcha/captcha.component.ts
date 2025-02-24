import { Component, OnDestroy, OnInit, Output, OnChanges, EventEmitter, SimpleChanges, Input } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { finalize, takeUntil } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { mustMatchValidator } from '../../../../functions/validators/must-match';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';
import { CaptchaValidation } from 'projects/storefront/src/app/services/authService/models/entities/CaptchaValidation';

@Component({
    selector: 'app-captcha',
    templateUrl: './captcha.component.html',
    styleUrls: ['./captcha.component.scss'],
})
export class PageCaptchaComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    captchaForm!: FormGroup;
    @Output('changeValue') change = new EventEmitter<CaptchaValidation>();
    @Output('formConfirm') loginForm = new EventEmitter<void>();

    @Input('form') form! : FormGroup;
    capchaImage!: string;
    capchaId!: string;
    reloadInProgress = false;
    constructor(
        private fb: FormBuilder,
        private router: Router,
        private authService: AuthService
    ) { }
    onCaptchaChange(captchaValidation: CaptchaValidation) {
        this.change.emit(captchaValidation);
      }

    ngOnInit(): void {
        this.load();
        this.createForm();
        this.subscribeToFormChanges();
    }
    createForm(): void {
        this.captchaForm = this.fb.group({
            captchaCode: [, [Validators.required]],
        });
    }

    subscribeToFormChanges(): void {
        this.captchaForm.valueChanges.pipe(takeUntil(this.destroy$)).subscribe(() => {
            this.emitCaptchaValidation();
        });
    }
    emitCaptchaValidation(): void {
        const captchaValidation = new CaptchaValidation();
        captchaValidation.captchaCode = this.captchaForm.value.captchaCode;
        captchaValidation.captchaId = this.capchaId;
        this.change.emit(captchaValidation);
      }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    load(): void {

        this.authService.getCaptcha()
            .subscribe({
                next: (res) => {
                    this.capchaId = res.content.captchaId;
                    this.capchaImage = res.content.captchaImage;
                }
            });
    }
    reLoad(): void {
        this.reloadInProgress = true;
        this.authService.getCaptchaAgain(this.capchaId)
            .subscribe({
                next: (res) => {
                    this.capchaId = res.content.captchaId;
                    this.capchaImage = res.content.captchaImage;
                    this.reloadInProgress = false;
                }
            });
    }
    getDynamicImageSource(): string {
        
        if(this.capchaImage != undefined){
            return `data:image/png;base64,${this.capchaImage}`;
        }
        return '';
    }
    onKeyUp(): void {
        
          this.loginForm.emit();
        
      }
}

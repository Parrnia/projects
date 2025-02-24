import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CaptchaValidation } from 'projects/storefront/src/app/services/authService/models/entities/CaptchaValidation';
import { PageCaptchaComponent } from '../../../account/pages/captcha/captcha.component';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';
import { OrdersClient } from 'projects/storefront/src/app/web-api-client';
import { Router } from '@angular/router';
import { UrlService } from 'projects/storefront/src/app/services/url.service';
import { OrdermapperService } from 'projects/storefront/src/app/mapServieces/ordersCluster/ordermapper.service';

@Component({
    selector: 'app-page-track-order',
    templateUrl: './page-track-order.component.html',
    styleUrls: ['./page-track-order.component.scss'],
})
export class PageTrackOrderComponent implements OnInit {
    form!: FormGroup;
    sendProgress = false;
    captchaId!: string;
    @ViewChild(PageCaptchaComponent) captchaComponent!: any;

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        public url: UrlService,
        private ordersClient: OrdersClient,
        private router: Router,
        private ordermapperService: OrdermapperService
    ) {}
    
    ngOnInit(): void {
        this.form = this.fb.group({
            orderNumber: [,  Validators.required],
            phoneNumber: [,  Validators.required],
            captchaCode: [, [Validators.required]]
        });
    }

    setCaptcha(captchaValidation: CaptchaValidation) {
        this.captchaId = captchaValidation.captchaId;
        this.form.controls['captchaCode'].setValue(captchaValidation.captchaCode);
    }
    markAllControlsAsTouched(formGroup: FormGroup): void {
        Object.values(formGroup.controls).forEach(control => {
          if (control instanceof FormGroup) {
            this.markAllControlsAsTouched(control);
          } else {
            control.markAsTouched();
          }
        });
      }

    track(): void {
        this.markAllControlsAsTouched(this.form);

        if (this.sendProgress || this.form.invalid) {
            return;
        }
        
        let captchaValidation = new CaptchaValidation();
        captchaValidation.captchaCode = this.form.value.captchaCode;
        captchaValidation.captchaId = this.captchaId;
        this.sendProgress = true;
        debugger;
        this.authService.checkCaptcha(captchaValidation).subscribe({
            next: () => {
                debugger;
                this.ordersClient.getOrderByNumber(this.form.value.orderNumber, this.form.value.phoneNumber)
                    .subscribe({
                        next: (res) => {
                            this.sendProgress = false;
                            let url = this.url.orderAnonymous();
                            this.router.navigate([url, res.number, res.phoneNumber]);
                        },
                        error: () => {
                            this.sendProgress = false;
                        }
                    });
            },
            error:() => {
                this.sendProgress = false;
                this.captchaComponent.reLoad();
            }
        });
    }
}

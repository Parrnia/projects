import { Component, OnInit } from '@angular/core';
import { theme } from '../../../../../data/theme';
import { CorporationInfo } from 'projects/storefront/src/app/interfaces/corporation-info';
import { CorporationInfoDto, CorporationInfosClient, CreateCustomerTicketCommand, CustomerTicketsClient } from 'projects/storefront/src/app/web-api-client';
import { Observable, finalize, map } from 'rxjs';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { ImageService } from 'projects/storefront/src/app/mapServieces/image.service';

@Component({
    selector: 'app-page-contact-us-one',
    templateUrl: './page-contact-us-one.component.html',
    styleUrls: ['./page-contact-us-one.component.scss'],
})
export class PageContactUsOneComponent implements OnInit {
    public info!: Observable<CorporationInfoDto>;
    form!: FormGroup;
    isAuthenticated: boolean = false;
    submitInProgress = false;

    constructor(
        private corporationInfosClient: CorporationInfosClient,
        private fb: FormBuilder,
        private customerTicketsClient: CustomerTicketsClient,
        private authService: AuthService,
        private toastr: ToastrService,
        private translate: TranslateService,
        private imageService: ImageService
    ) {
        this.info = this.corporationInfosClient.getCorporationInfo().pipe(map((res) => {
            res.desktopLogo = this.imageService.makeImageUrl(res.desktopLogo);
            res.mobileLogo = this.imageService.makeImageUrl(res.mobileLogo);
            res.footerLogo = this.imageService.makeImageUrl(res.footerLogo);
            res.sliderBackGroundImage = this.imageService.makeImageUrl(res.sliderBackGroundImage);
            return res;
        }));
        this.authService.isLoggedIn().subscribe((isAuthenticated) => {
            this.isAuthenticated = isAuthenticated;
        });
    }

    ngOnInit(): void {
        this.form = this.fb.group({
            subject: ['', [Validators.required, Validators.maxLength(50)]],
            message: ['', [Validators.required, Validators.maxLength(1000)]]
        });
    }

    get f() {
        return this.form.controls;
    }

    onSubmit() {
        this.form.markAllAsTouched();

        if (this.submitInProgress || this.form.invalid) {
            return;
        }


        if (this.isAuthenticated) {
            this.submitInProgress = true;

            let cmd = new CreateCustomerTicketCommand();
            cmd.subject = this.form.value.subject;
            cmd.message = this.form.value.message;
            cmd.customerId = localStorage.getItem('userId')!;
            cmd.customerPhoneNumber = localStorage.getItem('phoneNumber')!;
            cmd.customerName = localStorage.getItem('name')!;

            this.customerTicketsClient.create(cmd).pipe(
                finalize(() => this.submitInProgress = false),
            ).subscribe(() => {
                this.form.reset({
                    subject: '',
                    message: '',
                });
                this.toastr.success(this.translate.instant('TEXT_TOAST_TICKET_ADDED'));
            });
        } else {
            this.toastr.error('لطفا برای ثبت تیکت وارد شوید');
        }
    }

}


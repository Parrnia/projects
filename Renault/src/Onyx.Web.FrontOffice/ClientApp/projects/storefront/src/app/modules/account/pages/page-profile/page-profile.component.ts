import {
    FileParameter,
    FileUploadMetadataDto,
    UpdateAddressCommand,
} from './../../../../web-api-client';
import { UpdateCustomerCommandAuth } from '../../../../services/authService/models/commands/UpdateCustomerCommandAuth';
import { Customer } from 'projects/storefront/src/app/interfaces/user';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountApi } from '../../../../api';
import { finalize, takeUntil } from 'rxjs/operators';
import { Observable, Subject, forkJoin, of } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';
import { UserDto } from 'projects/storefront/src/app/services/authService/models/entities/UserDto';
import {
    CustomerDto,
    CustomersClient,
} from 'projects/storefront/src/app/web-api-client';
import { PersonType } from 'projects/storefront/src/app/services/authService/models/entities/PersonType';
import { ImageService } from 'projects/storefront/src/app/mapServieces/image.service';
import { AuthValidators } from 'projects/storefront/src/app/functions/validators/auth-validators';

@Component({
    selector: 'app-page-profile',
    templateUrl: './page-profile.component.html',
    styleUrls: ['./page-profile.component.scss'],
})
export class PageProfileComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();
    avatarUrl: string | undefined = undefined;
    form!: FormGroup;
    saveInProgress = false;
    customer: CustomerDto = new CustomerDto();
    selectedFormPart: string =
        localStorage.getItem('personType') == '2' ? 'legal' : 'natural';
    profileIsLoaded: boolean = false;
    constructor(
        private fb: FormBuilder,
        private toastr: ToastrService,
        private translate: TranslateService,
        private authService: AuthService,
        private customersClient: CustomersClient,
        private imageService: ImageService
    ) {}

    ngOnInit(): void {
        if (this.authService.isTokenExpired()) {
            throw new Error('User is unauthorized');
        }
        this.form = this.fb.group({
            firstName: ['', [Validators.required]],
            lastName: ['', [Validators.required]],
            email: ['', [Validators.required, Validators.email]],
            nationalCode: ['', [Validators.required]],
            avatar: '',
        });
        this.form.get('nationalCode')?.addAsyncValidators(
            AuthValidators.validUniqueNationalCode(this.authService,localStorage.getItem('userId') ?? '')

          );
          
        forkJoin(
            this.customersClient.getCustomerById(),
            this.authService.getCustomer()
        ).subscribe((res) => {
            debugger;
            this.customer = res[0];
            this.form.controls['firstName'].setValue(
                localStorage.getItem('firstName')
            );
            this.form.controls['lastName'].setValue(
                localStorage.getItem('lastName')
            );
            this.form.controls['email'].setValue(localStorage.getItem('email'));
            this.form.controls['nationalCode'].setValue(
                localStorage.getItem('nationalCode')
            );
            debugger;
            this.form.controls['avatar'].setValue(res[0].avatar);
            this.avatarUrl = this.imageService.makeImageUrl(res[0].avatar);
            this.profileIsLoaded = true;
        });

        
    }
    updateNationalCodeValidators() {
        const nationalCodeControl:any = this.form.get('nationalCode');
    
        if (this.selectedFormPart === 'natural') {
          // Apply minlength validator for natural customers
          nationalCodeControl.setValidators([Validators.required, Validators.minLength(10)]);
        } else {
          // Apply only required validator for legal customers
          nationalCodeControl.setValidators([Validators.required]);
        }
    
        nationalCodeControl.updateValueAndValidity(); // Update control with new validators
      }
      
    onFileSelected(event: FileUploadMetadataDto | null): void {
        if (event) {
          this.form.controls["avatar"].setValue(event.fileId);
        } else {
          this.form.controls["avatar"].setValue(null);
        }
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
        let cmdAuth = new UpdateCustomerCommandAuth();
        cmdAuth.userId = localStorage.getItem('userId')!;
        cmdAuth.firstName = this.form.value.firstName;
        cmdAuth.lastName = this.form.value.lastName;
        cmdAuth.nationalCode = this.form.value.nationalCode;
        cmdAuth.email = this.form.value.email;
        cmdAuth.personType =
            this.selectedFormPart === 'legal'
                ? PersonType.Legal
                : PersonType.Natural;

        this.authService
            .updateCustomer(
                cmdAuth,
                undefined,
                undefined,
                undefined,
                undefined,
                undefined,
                undefined,
                this.form.value.avatar
            )
            .pipe(
                finalize(() => (this.saveInProgress = false)),
                takeUntil(this.destroy$)
            )
            .subscribe(() => {
                this.toastr.success(
                    this.translate.instant('TEXT_TOAST_PROFILE_SAVED')
                );
            });
    }
}

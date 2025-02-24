import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EMPTY, Observable, Observer, of, throwError, observable, BehaviorSubject, finalize, forkJoin, takeUntil } from 'rxjs';
import { PasswordSignInCommand } from './models/commands/PasswordSignInCommand';
import { RegisterCustomerCommand } from './models/commands/RegisterCustomerCommand';
import { GetUserByNameQuery } from './models/queries/GetUserByNameQuery';
import { UpdateCustomerCommandAuth } from './models/commands/UpdateCustomerCommandAuth';
import { ResendActivationCodeCommand } from './models/commands/ResendActivationCodeCommand';
import { VerifyActivationCodeCommand } from './models/commands/VerifyActivationCodeCommand';
import { ForgetPasswordCommand } from './models/commands/ForgetPasswordCommand';
import { ResetPasswordCommand } from './models/commands/ResetPasswordCommand';
import { ChangePasswordCommand } from './models/commands/ChangePasswordCommand';
import { ToastrService } from 'ngx-toastr';
import * as jwtDecode from 'jwt-decode';
import { Router } from '@angular/router';
import { CreateCustomerCommand, CustomersClient, FileParameter, UpdateCustomerCommand } from '../../web-api-client';
import { TranslateService } from '@ngx-translate/core';
import { ChangePhoneNumberCommand } from './models/commands/ChangePhoneNumberCommand';
import { AppConfig } from 'config';
import { CaptchaValidation } from './models/entities/CaptchaValidation';
import { UniqueUserNameValidator } from './models/validators/UniqueUserNameValidator';
import { UniqueNationalCodeValidator } from './models/validators/UniqueNationalCodeValidator';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = AppConfig.authUrl;

  private userName: string | undefined;
  private password: string | undefined;
  private isAuthSubject = new BehaviorSubject<boolean>(false);
  constructor(
    private http: HttpClient,
    private toastr: ToastrService,
    private router: Router,
    private customersClient: CustomersClient,
    private translate: TranslateService
  ) { }
  //#region accountsController
  //#region uniqueNationalCode
  isUserUniqueNationalCode(query:UniqueNationalCodeValidator ): Observable<any> {
    const url = `${this.baseUrl}/Accounts/uniqueNationalCode`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, { params: query as any }).subscribe({
        next: (res) => {
          let isUniqueNationalCode : boolean = JSON.parse((res as any).content);
          observer.next(isUniqueNationalCode);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        },
      });
    });
  }
  //#endregion

  //#region UniqueUserName
  IsUserUniqueUserName(query: UniqueUserNameValidator): Observable<any> {
    const url = `${this.baseUrl}/Accounts/uniqueUserName`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, { params: query as any }).subscribe({
        next: (res) => {
          let isUniqueUserName : boolean = JSON.parse((res as any).content);
          observer.next(isUniqueUserName);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        },
      });
    });
  }

  //#endregion
  //#endregion

  //#region register
  register(command: RegisterCustomerCommand, avatar: string): Observable<any> {
    const url = `${this.baseUrl}/Customers/register`;
    this.userName = command.userName;
    this.password = command.password;
    localStorage.setItem('phoneNumber', command.phoneNumber);
    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, command).subscribe({
        next: (res) => {
          this.handleRegisterResponse(res);
          let cmd = new UpdateCustomerCommand();
          cmd.id = res.content;
          cmd.avatar = avatar;
          this.customersClient.create(cmd);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        }
      });
    });
  }

  private handleRegisterResponse = (response: any): void => {

    let message = response.message;
    this.toastr.success(message);
    const userId = response.content;
    localStorage.setItem('userId', userId);
    this.router.navigateByUrl('/account/confirmPhonenumber');
  }
  //#endregion

  //#region passwordSignIn
  passwordSignIn(command: PasswordSignInCommand, returnUrl: string): Observable<any> {
    const url = `${this.baseUrl}/signin/password`;

    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, command).subscribe({
        next: (res) => {
          this.handlePasswordSignInResponse(res, observer, returnUrl);

          this.userName = command.username;
          this.password = command.password;
          observer.next(res);
        },
        error: (err) => {
          observer.error(err);
        }
      });
    });
  }

  private handlePasswordSignInResponse = (response: any, observer: Observer<any>, returnUrl: string): void => {

    if (!response.message) {
      this.toastr.error('مشکلی پیش آمده است لطفا کمی بعد دوباره تلاش کنید');
    }
    let message = response.message;
    this.toastr.success(message);
    const userId = response.content.userId;
    const phoneNumber = response.content.userInfo.phoneNumber;

    switch (response.content.result) {
      case "LoginSuccessful":
        const token = response.content.accessToken;
        const name = response.content.userInfo.firstName + ' ' + response.content.userInfo.lastName;
        const userName = response.content.userInfo.userName;
        const personType = this.mapPersonType(response.content.userInfo.personType);
        const customerType = this.mapCustomerType(response.content.userInfo.customerType);

        if (token) {
          localStorage.setItem('token', token);
          localStorage.setItem('userId', userId);
          localStorage.setItem('name', name);
          localStorage.setItem('firstName', response.content.userInfo.firstName);
          localStorage.setItem('lastName', response.content.userInfo.lastName);
          localStorage.setItem('nationalCode', response.content.userInfo.nationalCode);
          localStorage.setItem('userName', userName);
          localStorage.setItem('personType', personType);
          localStorage.setItem('customerType', customerType);
          localStorage.setItem('phoneNumber', phoneNumber);
        }
        let cmd = new CreateCustomerCommand();
        cmd.id = userId;
        cmd.avatar = undefined;
        this.customersClient.create(cmd).subscribe({
          next: () => {
            this.router.navigateByUrl(returnUrl);
          },
          error: (err) => {
            this.logout();
            this.toastr.error("خطایی پیش آمده لطفا مدتی بعد دوباره وارد شوید");
            observer.error(err);
          }
        }
        );
        break;
      case "ActivationNeeded":

        localStorage.setItem('userId', userId);
        localStorage.setItem('phoneNumber', phoneNumber);
        this.router.navigateByUrl('/account/confirmPhonenumber');
        break;
    }

  }
  //#endregion

  //#region refreshAccessToken
  refreshAccessToken(): Observable<any> {
    const url = `${this.baseUrl}/signin/refresh`;

    return new Observable((observer: Observer<any>) => {
      this.http.post(url, {}, {}).subscribe({
        next: (res) => {
          this.handlePasswordSignInResponse(res, observer, '/account/dashboard');
          observer.next(res);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        }
      });
    });
  }
  //#endregion

  //#region logout
  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('name');
    localStorage.removeItem('userName');
    localStorage.removeItem('personType');
    localStorage.removeItem('phoneNumber');
    localStorage.removeItem('firstName');
    localStorage.removeItem('lastName');
    localStorage.removeItem('email');
    localStorage.removeItem('isActive');
    localStorage.removeItem('nationalCode');
    localStorage.removeItem('customerInfo');
    this.userName = undefined;
    this.password = undefined;
    this.router.navigateByUrl('/');
  }
  //#endregion

  //#region getCustomer
  getCustomer(): Observable<any> {
    const url = `${this.baseUrl}/Accounts/self`;
    return new Observable((observer: Observer<any>) => {

      // let result = localStorage.getItem('customerInfo');
      // if (!result) {
      this.http.get(url).subscribe({
        next: (res) => {
          localStorage.setItem('customerInfo', JSON.stringify(res));
          this.handleGetCustomerResponse(res);
          observer.next(res);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        }
      });
      // } else {
      //   this.handleGetCustomerResponse(JSON.parse(result));
      //   observer.next(JSON.parse(result));
      // }

    });
  }

  private handleGetCustomerResponse = (response: any): void => {

    let message = "عملیات با موفقیت انجام شد";
    if (response.content == undefined) {
      message = "کاربر یافت نشد";
      this.toastr.error(message);
      return undefined;
    }
    localStorage.setItem('firstName', response.content.firstName);
    localStorage.setItem('lastName', response.content.lastName);
    localStorage.setItem('email', response.content.email);
    localStorage.setItem('isActive', response.content.isActive);
    localStorage.setItem('phoneNumber', response.content.phoneNumber);
    localStorage.setItem('nationalCode', response.content.nationalCode);
    localStorage.setItem('personType', this.mapPersonType(response.content.personType));

    //this.toastr.success(message);
  }
  //#endregion

  //#region isUserExists
  isUserExists(query: GetUserByNameQuery): Observable<any> {
    const url = `${this.baseUrl}/Accounts/exists`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, { params: query as any }).subscribe({
        next: (res) => {
          this.handleIsUserExistsResponse(res);
          observer.next(res);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        }
      });
    });
  }
  private handleIsUserExistsResponse = (response: any): void => { }
  //#endregion

  //#region isLoggedIn
  isLoggedIn() {
    this.checkTokenExpireTime();
    return this.getAuthStatus();
  }
  //#endregion

  //#region UpdateCustomer
  updateCustomer(commandAuth: UpdateCustomerCommandAuth, commentIds: number[] | null | undefined, widgetCommentIds: number[] | null | undefined, addressIds: number[] | null | undefined, orderIds: number[] | null | undefined, reviewIds: number[] | null | undefined, vehicleIds: number[] | null | undefined, avatar: string | undefined): Observable<any> {
    const url = `${this.baseUrl}/customers/self`;
    return new Observable((observer: Observer<any>) => {
      let cmd = new UpdateCustomerCommand();
      cmd.id = localStorage.getItem('userId')!;
      cmd.avatar = avatar;
      const update = this.customersClient.update(
        localStorage.getItem('userId')!,
        cmd);

      const UpdateAuth = this.http.put<any>(url, commandAuth);
      forkJoin([update, UpdateAuth]).subscribe({
        next: (res) => {
          this.handleUpdateCustomerResponse(res[1]);
          observer.next(res);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        }
      });
    });
  }
  private handleUpdateCustomerResponse = (response: any): void => {
    let message = response.message;
    this.toastr.success(message);
    this.getCustomer();
  }
  //#endregion

  //#region ResendActivationCode
  resendActivationCode(command: ResendActivationCodeCommand): Observable<any> {
    const url = `${this.baseUrl}/customers/${command.userId}/activation/resend`;
    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, command).subscribe({
        next: (res) => {
          this.handleResendActivationCodeResponse(res);
          observer.next(res);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        }
      });
    });
  }
  private handleResendActivationCodeResponse = (response: any) => {
    let message = response.message;
    this.toastr.success(message);

  }
  //#endregion

  //#region VerifyActivationCode
  verifyActivationCode(command: VerifyActivationCodeCommand): Observable<any> {
    if (this.userName && this.password) {
      command.username = this.userName;
      command.password = this.password;
    } else {
      this.router.navigateByUrl('/account/login');
    }
    const url = `${this.baseUrl}/customers/${command.userId}/activation/verify`;
    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, command).subscribe({
        next: (res) => {
          this.handleVerifyActivationCodeResponse(res, observer);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        }
      });
    });
  }
  private handleVerifyActivationCodeResponse = (response: any, observer: Observer<any>) => {

    const userId = response.content.userId;
    const token = response.content.accessToken;
    const name = response.content.userInfo.firstName + ' ' + response.content.userInfo.lastName;
    const userName = response.content.userInfo.userName;
    const personType = this.mapPersonType(response.content.userInfo.personType);
    const customerType = this.mapCustomerType(response.content.userInfo.customerType);
    const phoneNumber = response.content.userInfo.phoneNumber;

    if (token) {
      localStorage.setItem('token', token);
      localStorage.setItem('userId', userId);
      localStorage.setItem('name', name);
      localStorage.setItem('firstName', response.content.userInfo.firstName);
      localStorage.setItem('lastName', response.content.userInfo.lastName);
      localStorage.setItem('nationalCode', response.content.userInfo.nationalCode);
      localStorage.setItem('userName', userName);
      localStorage.setItem('personType', personType);
      localStorage.setItem('customerType', customerType);
      localStorage.setItem('phoneNumber', phoneNumber);
    }
    let cmd = new CreateCustomerCommand();
    cmd.id = userId;
    cmd.avatar = undefined;
    this.customersClient.create(cmd).subscribe({
      next: () => {
        this.router.navigateByUrl('/account/dashboard');
      },
      error: (err) => {
        this.logout();
        this.toastr.error("خطایی پیش آمده لطفا مدتی بعد دوباره وارد شوید");
        observer.error(err);
      }
    }
    );

  }
  //#endregion

  //#region ForgotPassword
  forgotPassword(command: ForgetPasswordCommand): Observable<any> {
    const url = `${this.baseUrl}/password/forget`;

    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, command).subscribe({
        next: (res) => {
          this.handleForgotPasswordResponse(res);
          observer.next(res);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        }
      });
    });
  }
  private handleForgotPasswordResponse(response: any) {
    //let message = response.message;

    let message = "کد تایید برای شما با موفقیت ارسال شد";
    this.toastr.success(message);
    const userId = response.content.userId;
    const phoneNumber = response.content.phoneNumber;
    localStorage.setItem('userId', userId);
    localStorage.setItem('phoneNumber', phoneNumber);
    this.router.navigateByUrl('/account/reset-password');

  }
  //#endregion

  //#region ResetPassword
  resetPassword(command: ResetPasswordCommand): Observable<any> {
    const url = `${this.baseUrl}/password/reset/${command.userId}`;

    localStorage;
    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, command).subscribe({
        next: (res) => {
          this.handleResetPasswordResponse(res);
          observer.next(res);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        }
      });
    });
  }
  private handleResetPasswordResponse(response: any) {
    let message = response.message;
    this.toastr.success(message);
    const userId = response.content.userId;
    localStorage.setItem('userId', userId);
    this.router.navigateByUrl('/account/login');

  }
  //#endregion

  //#region ChangePassword
  changePassword(command: ChangePasswordCommand): Observable<any> {
    const url = `${this.baseUrl}/password/change`;
    command.userId = localStorage.getItem('userId')!;
    return new Observable((observer: Observer<any>) => {
      this.http.put<any>(url, command).subscribe({
        next: (res) => {
          this.handleChangePasswordResponse(res);
          observer.next(res);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        }
      });
    });
  }
  private handleChangePasswordResponse(response: any) {
    let message = response.message;
    this.toastr.success(message);
    this.toastr.success(this.translate.instant('TEXT_TOAST_PASSWORD_CHANGED'));
  }
  //#endregion

  //#region ChangePhoneNumber
  changePhoneNumber(command: ChangePhoneNumberCommand): Observable<any> {
    const url = `${this.baseUrl}/password/changePhoneNumber`;
    command.userId = localStorage.getItem('userId')!;
    return new Observable((observer: Observer<any>) => {
      this.http.put<any>(url, command).subscribe({
        next: (res) => {
          this.handleChangePhoneNumberResponse(res);
          observer.next(res);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        }
      });
    });
  }
  private handleChangePhoneNumberResponse(response: any) {
    let message = response.message;
    this.toastr.success(message);
    this.toastr.success(this.translate.instant('TEXT_TOAST_PHONENUMBER_CHANGED'));
  }
  //#endregion

  //#region getCaptcha
  getCaptcha(): Observable<any> {
    const url = `${this.baseUrl}/Captcha/`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url).subscribe({
        next: (res) => {
          observer.next(res);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        }
      });
    });
  }

  //#endregion

  //#region getCaptchaAgain
  getCaptchaAgain(captchaId: string): Observable<any> {
    const url = `${this.baseUrl}/Captcha/${captchaId}`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url).subscribe({
        next: (res) => {
          observer.next(res);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        }
      });
    });
  }

  //#endregion

  //#region checkCaptcha
  checkCaptcha(captchaValidation: CaptchaValidation): Observable<any> {
    const url = `${this.baseUrl}/Captcha`;

    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, captchaValidation).subscribe({
        next: (res) => {
          observer.next(res);
        },
        error: (err) => {
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        }
      });
    });
  }

  //#endregion

  //#region methods
  private mapPersonType(personTypeString: string) {

    switch (personTypeString) {
      case "Natural":
        return '1';
      case "Legal":
        return '2';
      default:
        return '1';
    }
  }
  private mapCustomerType(customerTypeString: string) {
    switch (customerTypeString) {
      case "Personal":
        return '1';
      case "Store":
        return '2';
      case "Agency":
        return '3';
      case "CentralRepairShop":
        return '4';
      default:
        return '1';
    }
  }
  checkTokenExpireTime() {
    const token = localStorage.getItem('token');
    if (token) {
      const decodedToken: any = jwtDecode.jwtDecode(token);
      const currentTime = Date.now() / 1000;
      this.setAuthStatus(decodedToken.exp > currentTime);
    } else {
      this.setAuthStatus(false);
    }
  }
  isTokenExpired() {
    const token = localStorage.getItem('token');
    if (token) {
      const decodedToken: any = jwtDecode.jwtDecode(token);
      const currentTime = Date.now() / 1000;
      return decodedToken.exp < currentTime;
    }
    return true;
  }
  setAuthStatus(isAuth: boolean): void {
    this.isAuthSubject.next(isAuth);
  }

  getAuthStatus(): Observable<boolean> {
    return this.isAuthSubject.asObservable();
  }
  getToken(): Observable<string | null> {
    return of(localStorage.getItem('token'));
  }
  getUserId(): Observable<string | null> {
    return of(localStorage.getItem('userId'));
  }
  getName(): Observable<string | null> {
    return of(localStorage.getItem('name'));
  }
  getUserName(): Observable<string | null> {
    return of(localStorage.getItem('userName'));
  }
  getPersonType(): Observable<string | null> {
    return of(localStorage.getItem('personType'));
  }
  getCustomerType(): Observable<string | null> {
    return of(localStorage.getItem('customerType'));
  }
  getPhoneNumber(): Observable<string | null> {
    return of(localStorage.getItem('phoneNumber'));
  }
  getFirstName(): Observable<string | null> {
    return of(localStorage.getItem('firstName'));
  }
  getLastName(): Observable<string | null> {
    return of(localStorage.getItem('lastName'));
  }
  getEmail(): Observable<string | null> {
    return of(localStorage.getItem('email'));
  }
  getIsActive(): Observable<string | null> {
    return of(localStorage.getItem('isActive'));
  }
  getNationalCode(): Observable<string | null> {
    return of(localStorage.getItem('nationalCode'));
  }
  //#endregion

}
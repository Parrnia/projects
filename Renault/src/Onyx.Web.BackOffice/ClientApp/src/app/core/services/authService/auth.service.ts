import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable, Observer, forkJoin, of } from "rxjs";
import { GlobalComponent } from "src/app/global-component";
import { CustomersClient, UsersClient, CreateCustomerCommand, UpdateCustomerCommand, CreateUserCommand, UpdateUserCommand } from "src/app/web-api-client";
import { ToastService } from "../toastService/toast-service";
import { AddGroupCommand } from "./models/commands/AddGroupCommand";
import { AddGroupPermissionsCommand } from "./models/commands/AddGroupPermissionsCommand";
import { AddUserGroupsCommand } from "./models/commands/AddUserGroupsCommand";
import { AdminRegisterCustomerCommand } from "./models/commands/AdminRegisterCustomerCommand";
import { ChangePasswordCommand } from "./models/commands/ChangePasswordCommand";
import { DeleteGroupCommand } from "./models/commands/DeleteGroupCommand";
import { ForgetPasswordCommand } from "./models/commands/ForgetPasswordCommand";
import { PasswordSignInCommand } from "./models/commands/PasswordSignInCommand";
import { RegisterCustomerCommand } from "./models/commands/RegisterCustomerCommand";
import { RegisterEmployeeCommand } from "./models/commands/RegisterEmployeeCommand";
import { RemoveGroupPermissionsCommand } from "./models/commands/RemoveGroupPermissionsCommand";
import { RemoveUserGroupsCommand } from "./models/commands/RemoveUserGroupsCommand";
import { ResetPasswordCommand } from "./models/commands/ResetPasswordCommand";
import { UpdateCustomerCommandAuth } from "./models/commands/UpdateCustomerCommandAuth";
import { UpdateGroupCommand } from "./models/commands/UpdateGroupCommand";
import { UpdateEmployeeCommand } from "./models/commands/UpdateUserCommandAuth";
import { FilteredUser } from "./models/entities/FilteredUser";
import { Group } from "./models/entities/Group";
import { Permission } from "./models/entities/Permission";
import { Role } from "./models/entities/Role";
import { User } from "./models/entities/User";
import { UserGroupPermission } from "./models/entities/UserGroupPermission";
import { GetCustomersQuery } from "./models/queries/GetCustomersQuery";
import { GetEmployeesQuery } from "./models/queries/GetEmployeesQuery";
import { GetUserByNameQuery } from "./models/queries/GetUserByNameQuery";
import { UniqueNationalCodeValidator } from "./models/validators/UniqueNationalCodeValidator";
import { UniqueUserNameValidator } from "./models/validators/UniqueUserNameValidator";
import * as jwtDecode from "jwt-decode";



const AUTH_API = GlobalComponent.AUTH_API;

@Injectable({
  providedIn: "root",
})
export class AuthenticationService {
  private userName: string | undefined;
  private password: string | undefined;
  private isAuthSubject = new BehaviorSubject<boolean>(false);

  constructor(
    private http: HttpClient,
    private toast: ToastService,
    private router: Router,
    private customersClient: CustomersClient,
    private usersClient: UsersClient
  ) {}

  //#region accountsController

  //#region selfGetEmployee
  selfGetEmployee(): Observable<any> {
    const url = `${AUTH_API}/Accounts/self`;
    return new Observable((observer: Observer<any>) => {
      let result = localStorage.getItem("userInfo");
      if (!result) {
        this.http.get(url).subscribe({
          next: (res) => {
            localStorage.setItem("userInfo", JSON.stringify(res));
            this.handleselfGetEmployeeResponse(res);
            observer.next(res);
          },
          error: (err) => {
            observer.error(err);
          },
          complete: () => {
            observer.complete();
          },
        });
      } else {
        this.handleselfGetEmployeeResponse(JSON.parse(result));
        observer.next(JSON.parse(result));
      }
    });
  }

  private handleselfGetEmployeeResponse = (response: any): void => {
    let message = "عملیات با موفقیت انجام شد";
    if (response.content == undefined) {
      message = "کاربر یافت نشد";
      this.toast.showError(message);
      return undefined;
    }
    localStorage.setItem("firstName", response.content.firstName);
    localStorage.setItem("lastName", response.content.lastName);
    localStorage.setItem("email", response.content.email);
    localStorage.setItem("isActive", response.content.isActive);
    localStorage.setItem("phoneNumber", response.content.phoneNumber);
    localStorage.setItem("nationalCode", response.content.nationalCode);

    //this.toast.showSuccess(message);
  };
  //#endregion

  //#region getUser
  getUser(query: GetUserByNameQuery): Observable<any> {
    const url = `${AUTH_API}/Accounts/by-name`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, { params: query as any }).subscribe({
        next: (res) => {
          this.handleGetUserResponse(res);
          let user: User = JSON.parse((res as any).content);
          observer.next(user);
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

  private handleGetUserResponse = (response: any): void => {
    let message = "عملیات با موفقیت انجام شد";
    if (response.content == undefined) {
      message = "کاربر یافت نشد";
      this.toast.showError(message);
      return undefined;
    }
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region isUserExists
  isUserExists(query: GetUserByNameQuery): Observable<any> {
    const url = `${AUTH_API}/Accounts/exists`;
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
        },
      });
    });
  }
  private handleIsUserExistsResponse = (response: any): void => {};
  //#endregion

  //#region uniqueNationalCode
  isUserUniqueNationalCode(query:UniqueNationalCodeValidator ): Observable<any> {
    const url = `${AUTH_API}/Accounts/uniqueNationalCode`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, { params: query as any }).subscribe({
        next: (res) => {
          debugger
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
    const url = `${AUTH_API}/Accounts/uniqueUserName`;
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
  
  //#region customersController

  //#region getCustomers
  getCustomers(): Observable<User[]> {
    const url = `${AUTH_API}/Customers`;
    let query = new GetCustomersQuery();

    return new Observable((observer: Observer<any>) => {
      this.http.get(url, { params: query as any }).subscribe({
        next: (res) => {
          this.handleGetCustomersResponse(res);
          let users: User[] = (res as any).content;
          observer.next(users);
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

  private handleGetCustomersResponse = (response: any): void => {
    let message = "عملیات با موفقیت انجام شد";
    if (response.content == undefined) {
      message = "کاربری یافت نشد";
      this.toast.showError(message);
      return undefined;
    }

    this.toast.showSuccess(message);
  };
  //#endregion

  //#region getCustomersWithPagination
  getCustomersWithPagination(
    pageNumber: number,
    pageSize: number,
    sortColumn?: string,
    sortDirection?: string,
    searchTerm?: string
  ): Observable<FilteredUser> {
    const url = `${AUTH_API}/Customers/withPagination`;
    const params = {
      pageNumber: pageNumber.toString(),
      pageSize: pageSize.toString(),
      sortColumn: sortColumn || "",
      sortDirection: sortDirection || "",
      searchTerm: searchTerm || "",
    };

    return new Observable((observer: Observer<any>) => {
      return this.http.get<any>(url, { params }).subscribe({
        next: (res) => {
          this.handleGetCustomersWithPaginationResponse(res);
          let users: User[] = (res as any).content;
          observer.next(users);
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

  private handleGetCustomersWithPaginationResponse = (response: any): void => {
    let message = "عملیات با موفقیت انجام شد";
    if (response.content == undefined) {
      message = "کاربری یافت نشد";
      this.toast.showError(message);
      return undefined;
    }

    this.toast.showSuccess(message);
  };
  //#endregion

  //#region getStoreCustomersWithPagination
  getStoreCustomersWithPagination(
    pageNumber: number,
    pageSize: number,
    sortColumn?: string,
    sortDirection?: string,
    searchTerm?: string
  ): Observable<FilteredUser> {
    const url = `${AUTH_API}/Customers/storeWithPagination`;
    const params = {
      pageNumber: pageNumber.toString(),
      pageSize: pageSize.toString(),
      sortColumn: sortColumn || "",
      sortDirection: sortDirection || "",
      searchTerm: searchTerm || "",
    };

    return new Observable((observer: Observer<any>) => {
      return this.http.get<any>(url, { params }).subscribe({
        next: (res) => {
          this.handleGetStoreCustomersWithPaginationResponse(res);
          let users: User[] = (res as any).content;
          observer.next(users);
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

  private handleGetStoreCustomersWithPaginationResponse = (
    response: any
  ): void => {
    let message = "عملیات با موفقیت انجام شد";
    if (response.content == undefined) {
      message = "کاربری یافت نشد";
      this.toast.showError(message);
      return undefined;
    }

    this.toast.showSuccess(message);
  };
  //#endregion

  //#region registerCustomer
  registerCustomer(
    command: RegisterCustomerCommand,
    avatar: string
  ): Observable<any> {
    const url = `${AUTH_API}/Customers/register`;
    let userName = command.userName;
    let password = command.password;
    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, command).subscribe({
        next: (res) => {
          this.handleRegisterCustomerResponse(res);
          let cmd = new CreateCustomerCommand();
          cmd.id = res.content;
          cmd.avatar = avatar;
          this.customersClient.create(cmd).subscribe();
          let passwordSignInRequest = new PasswordSignInCommand();
          passwordSignInRequest.username = userName;
          passwordSignInRequest.password = password;

          this.passwordSignIn(passwordSignInRequest, "/").subscribe({
            next: (res) => {
              observer.next(res);
            },
            error: (err) => {
              observer.error(err);
            },
            complete: () => {
              observer.complete();
            },
          });
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

  private handleRegisterCustomerResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region adminRegisterCustomer
  adminRegisterCustomer(
    command: AdminRegisterCustomerCommand,
    avatar: string | undefined
  ): Observable<any> {
    const url = `${AUTH_API}/Customers/adminRegister`;
    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, command).subscribe({
        next: (res) => {
          this.handleAdminRegisterCustomerResponse(res);
          let cmd = new CreateCustomerCommand();
          cmd.id = res.content;
          cmd.avatar = avatar ?? undefined;
          debugger;
          this.customersClient.create(cmd).subscribe();
          observer.next(res);
        },
        error: (err) => {
          debugger;
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        },
      });
    });
  }

  private handleAdminRegisterCustomerResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region UpdateCustomer
  updateCustomer(
    commandAuth: UpdateCustomerCommandAuth,
    avatar: string | undefined
  ): Observable<any> {
    debugger;
    const url = `${AUTH_API}/customers/${commandAuth.id}`;
    return new Observable((observer: Observer<any>) => {
      let cmd = new UpdateCustomerCommand();
      cmd.id = commandAuth.id!;
      cmd.avatar = avatar;

      const update = this.customersClient.update(commandAuth.id!, cmd);
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
        },
      });
    });
  }
  private handleUpdateCustomerResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#endregion

  //#region employeesController

  //#region getEmployees
  getEmployees(): Observable<any> {
    const url = `${AUTH_API}/Employees`;
    let query = new GetEmployeesQuery();

    return new Observable((observer: Observer<any>) => {
      this.http.get(url, { params: query as any }).subscribe({
        next: (res) => {
          this.handleGetEmployeesResponse(res);
          let users: User[] = JSON.parse((res as any).content);
          observer.next(users);
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

  private handleGetEmployeesResponse = (response: any): void => {
    let message = "عملیات با موفقیت انجام شد";
    if (response.content == undefined) {
      message = "کاربری یافت نشد";
      this.toast.showError(message);
      return undefined;
    }

    this.toast.showSuccess(message);
  };
  //#endregion

  //#region getEmployeesWithPagination
  getEmployeesWithPagination(
    pageNumber: number,
    pageSize: number,
    sortColumn?: string,
    sortDirection?: string,
    searchTerm?: string
  ): Observable<FilteredUser> {
    const url = `${AUTH_API}/Employees/withPagination`;
    const params = {
      pageNumber: pageNumber.toString(),
      pageSize: pageSize.toString(),
      sortColumn: sortColumn || "",
      sortDirection: sortDirection || "",
      searchTerm: searchTerm || "",
    };

    return new Observable((observer: Observer<any>) => {
      return this.http.get<any>(url, { params }).subscribe({
        next: (res) => {
          this.handleGetEmployeesWithPaginationResponse(res);
          let users: User[] = (res as any).content;
          observer.next(users);
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

  private handleGetEmployeesWithPaginationResponse = (response: any): void => {
    let message = "عملیات با موفقیت انجام شد";
    if (response.content == undefined) {
      message = "کاربری یافت نشد";
      this.toast.showError(message);
      return undefined;
    }

    this.toast.showSuccess(message);
  };
  //#endregion

  //#region registerEmployee
  registerEmployee(
    command: RegisterEmployeeCommand,
    avatar: string | undefined
  ): Observable<any> {
    const url = `${AUTH_API}/Employees/register`;
    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, command).subscribe({
        next: (res) => {
          this.handleRegisterEmployeeResponse(res);
          debugger;
          let cmd = new CreateUserCommand();
          cmd.id = res.content;
          cmd.avatar = avatar ?? undefined;
          this.usersClient.create(cmd).subscribe();
          observer.next(res);
        },
        error: (err) => {
          debugger;
          observer.error(err);
        },
        complete: () => {
          observer.complete();
        },
      });
    });
  }

  private handleRegisterEmployeeResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region updateEmployee
  updateEmployee(
    commandAuth: UpdateEmployeeCommand,
    avatar: string | undefined
  ): Observable<any> {
    const url = `${AUTH_API}/Employees/${commandAuth.userId}`;
    debugger;
    return new Observable((observer: Observer<any>) => {
      let cmd = new UpdateUserCommand();
      cmd.id = commandAuth.userId!;
      cmd.avatar = avatar;
      const update = this.usersClient.update(commandAuth.userId!, cmd);
      const UpdateAuth = this.http.put<any>(url, commandAuth);
      forkJoin([update, UpdateAuth]).subscribe({
        next: (res) => {
          this.handleUpdateEmployeeResponse(res[1]);
          observer.next(res);
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
  private handleUpdateEmployeeResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region selfUpdateEmployee
  selfUpdateEmployee(
    commandAuth: UpdateEmployeeCommand,
    avatar: string | undefined
  ): Observable<any> {
    const url = `${AUTH_API}/SystemUsers/self`;
    return new Observable((observer: Observer<any>) => {
      let cmd = new UpdateUserCommand();
      cmd.id = localStorage.getItem("userId")!;
      cmd.avatar = avatar;
      const update = this.usersClient.update(
        localStorage.getItem("userId")!,
        cmd
      );
      const UpdateAuth = this.http.put<any>(url, commandAuth);
      forkJoin([update, UpdateAuth]).subscribe({
        next: (res) => {
          this.handleSelfUpdateEmployeeResponse(res[1]);
          observer.next(res);
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
  private handleSelfUpdateEmployeeResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
    this.selfGetEmployee();
  };
  //#endregion

  //#region activateEmployee
  activateEmployee(userId: string): Observable<any> {
    const url = `${AUTH_API}/${userId}/activate`;
    return new Observable((observer: Observer<any>) => {
      this.http.put<any>(url, {}).subscribe({
        next: (res) => {
          this.handleActivateEmployeeResponse(res);
          observer.next(res);
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
  private handleActivateEmployeeResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region deactivateEmployee
  deactivateEmployee(userId: string): Observable<any> {
    const url = `${AUTH_API}/${userId}/deactivate`;
    return new Observable((observer: Observer<any>) => {
      this.http.put<any>(url, {}).subscribe({
        next: (res) => {
          this.handleDeactivateEmployeeResponse(res);
          observer.next(res);
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
  private handleDeactivateEmployeeResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region activateUsers
  activateUsers(userIds: string[]): Observable<any> {
    const url = `${AUTH_API}/Employees/activateUsers`;
    debugger;
    return new Observable((observer: Observer<any>) => {
      this.http.put(url, userIds).subscribe({
        next: (res) => {
          this.handleActivateUsersResponse(res);
          observer.next(res);
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
  private handleActivateUsersResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region deactivateUsers
  deactivateUsers(userIds: string[]): Observable<any> {
    const url = `${AUTH_API}/Employees/deactivateUsers`;
    debugger;
    return new Observable((observer: Observer<any>) => {
      this.http.put(url, userIds).subscribe({
        next: (res) => {
          this.handleDeactivateUsersResponse(res);
          observer.next(res);
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
  private handleDeactivateUsersResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#endregion

  //#region passwordController

  //#region forgotPassword
  forgotPassword(command: ForgetPasswordCommand): Observable<any> {
    const url = `${AUTH_API}/password/forget`;

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
        },
      });
    });
  }
  private handleForgotPasswordResponse(response: any) {
    //let message = response.message;
    let message = "کد تایید برای شما با موفقیت ارسال شد";
    this.toast.showSuccess(message);
    const userId = response.content;
    localStorage.setItem("userId", userId);
    this.router.navigateByUrl("/account/reset-password");
  }
  //#endregion

  //#region resetPassword
  resetPassword(command: ResetPasswordCommand): Observable<any> {
    const url = `${AUTH_API}/password/reset/${command.userId}`;

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
        },
      });
    });
  }
  private handleResetPasswordResponse(response: any) {
    let message = response.message;
    this.toast.showSuccess(message);
    const userId = response.content.userId;
    localStorage.setItem("userId", userId);

    this.router.navigateByUrl("/account/login");
  }
  //#endregion

  //#region changePassword
  changePassword(command: ChangePasswordCommand): Observable<any> {
    const url = `${AUTH_API}/password/change`;
    command.userId = localStorage.getItem("userId")!;
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
        },
      });
    });
  }
  private handleChangePasswordResponse(response: any) {
    this.toast.showSuccess("");
  }
  //#endregion

  //#region resetPasswordByAdmin
  resetPasswordByAdmin(userId: string): Observable<any> {
    const url = `${AUTH_API}/reset/${userId}/by-admin`;
    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, {}).subscribe({
        next: (res) => {
          this.handleResetPasswordByAdminResponse(res);
          observer.next(res);
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
  private handleResetPasswordByAdminResponse(response: any) {
    let message = response.message;
    this.toast.showSuccess(message);
  }
  //#endregion

  //#endregion

  //#region rolesController

  //#region getRoles
  getRoles(): Observable<any> {
    const url = `${AUTH_API}/Roles`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, {}).subscribe({
        next: (res) => {
          this.handleGetRolesResponse(res);
          let roles: Role[] = JSON.parse((res as any).content);
          observer.next(roles);
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

  private handleGetRolesResponse = (response: any): void => {
    let message = response.message;
    if (response.content == undefined) {
      message = "نقشی یافت نشد";
      this.toast.showError(message);
      return undefined;
    }

    this.toast.showSuccess(message);
  };
  //#endregion

  //#endregion

  //#region checkController

  //#endregion

  //#region signInController

  //#region passwordSignIn
  passwordSignIn(
    command: PasswordSignInCommand,
    returnUrl: string
  ): Observable<any> {
    const url = `${AUTH_API}/signin/password`;
    debugger;
    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, command).subscribe({
        next: (res) => {
          debugger;
          this.handlePasswordSignInResponse(res, observer, returnUrl);
          let cmd = new CreateUserCommand();
          cmd.id = res.content.userId;
          cmd.avatar = undefined;
          this.usersClient.create(cmd).subscribe({
            error: () => {
              this.logout();
              this.toast.showError(
                "خطایی پیش آمده لطفا مدتی بعد دوباره وارد شوید"
              );
              return;
            },
          });
          this.userName = command.username;
          this.password = command.password;
        },
        error: (err) => {
          debugger;
          observer.error(err);
        },
      });
    });
  }

  private handlePasswordSignInResponse = (
    response: any,
    observer: Observer<any>,
    returnUrl: string
  ): void => {
    debugger;
    if (!response.message) {
      this.toast.showError("مشکلی پیش آمده است لطفا کمی بعد دوباره تلاش کنید");
    }
    let message = response.message;
    this.toast.showSuccess(message);

    const userId = response.content.userId;
    const token = response.content.accessToken;
    const name =
      response.content.userInfo.firstName +
      " " +
      response.content.userInfo.lastName;
    const userName = response.content.userInfo.userName;
    const phoneNumber = response.content.userInfo.phoneNumber;

    if (token) {
      localStorage.setItem("token", token);
      localStorage.setItem("userId", userId);
      localStorage.setItem("name", name);
      localStorage.setItem("firstName", response.content.userInfo.firstName);
      localStorage.setItem("lastName", response.content.userInfo.lastName);
      localStorage.setItem(
        "nationalCode",
        response.content.userInfo.nationalCode
      );
      localStorage.setItem("userName", userName);
      localStorage.setItem("phoneNumber", phoneNumber);
    }

    this.selfGetEmployee().subscribe({
      next: (res) => {
        observer.next(res);
      },
      error: (err) => {
        observer.error(err);
      },
      complete: () => {
        observer.complete();
      },
    });
    this.router.navigate([returnUrl]);
  };
  //#endregion

  //#region refreshAccessToken
  refreshAccessToken(): Observable<any> {
    const url = `${AUTH_API}/signin/refresh`;

    return new Observable((observer: Observer<any>) => {
      this.http.post(url, {}, {}).subscribe({
        next: (res) => {
          this.handlePasswordSignInResponse(res, observer, "/");
          observer.next(res);
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

  //#region groupsController

  //#region getGroups
  getGroups(): Observable<any> {
    const url = `${AUTH_API}/Groups`;

    return new Observable((observer: Observer<any>) => {
      this.http.get(url, {}).subscribe({
        next: (res) => {
          debugger;
          this.handleGetGroupsResponse(res);
          let groups: Group[] = (res as any).content;
          observer.next(groups);
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

  private handleGetGroupsResponse = (response: any): void => {
    let message = response.message;
    if (response.content == undefined) {
      message = "گروهی یافت نشد";
      this.toast.showError(message);
      return undefined;
    }
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region addGroup
  addGroup(command: AddGroupCommand): Observable<any> {
    const url = `${AUTH_API}/Groups`;
    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, command).subscribe({
        next: (res) => {
          this.handleAddGroupResponse(res);
          observer.next(res);
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

  private handleAddGroupResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region deleteGroup
  deleteGroup(command: DeleteGroupCommand): Observable<any> {
    const url = `${AUTH_API}/Groups`;
    return new Observable((observer: Observer<any>) => {
      this.http.delete<any>(url, { body: command }).subscribe({
        next: (res) => {
          this.handleDeleteGroupResponse(res);
          observer.next(res);
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

  private handleDeleteGroupResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region updateGroup
  updateGroup(command: UpdateGroupCommand): Observable<any> {
    const url = `${AUTH_API}/Groups/${command.id}`;
    return new Observable((observer: Observer<any>) => {
      this.http.put<any>(url, command).subscribe({
        next: (res) => {
          this.handleUpdateGroupResponse(res);
          observer.next(res);
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
  private handleUpdateGroupResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region getPermissionsByGroupId
  getPermissionsByGroupId(groupId: string): Observable<any> {
    const url = `${AUTH_API}/Groups/${groupId}/permissions`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, {}).subscribe({
        next: (res) => {
          debugger;
          this.handleGetPermissionsByGroupIdResponse(res);
          let permissions: Permission[] = (res as any).content;
          observer.next(permissions);
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

  private handleGetPermissionsByGroupIdResponse = (response: any): void => {
    let message = response.message;
    if (response.content == undefined) {
      message = "مجوزی یافت نشد";
      this.toast.showError(message);
      return undefined;
    }
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region assignPermission
  assignPermission(command: AddGroupPermissionsCommand): Observable<any> {
    debugger;
    const url = `${AUTH_API}/Groups/${command.groupId}/addPermissions`;
    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, command).subscribe({
        next: (res) => {
          this.handleAssignPermissionResponse(res);
          observer.next(res);
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

  private handleAssignPermissionResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region removePermission
  removePermission(command: RemoveGroupPermissionsCommand): Observable<any> {
    debugger;
    const url = `${AUTH_API}/Groups/${command.groupId}/removePermissions`;
    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, command).subscribe({
        next: (res) => {
          this.handleRemovePermissionResponse(res);
          observer.next(res);
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

  private handleRemovePermissionResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#endregion

  //#region permissionsController

  //#region getPermissions
  getPermissions(): Observable<any> {
    const url = `${AUTH_API}/Permissions`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, {}).subscribe({
        next: (res) => {
          this.handleGetPermissionsResponse(res);
          let permissions: Permission[] = (res as any).content;
          observer.next(permissions);
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

  private handleGetPermissionsResponse = (response: any): void => {
    let message = response.message;
    if (response.content == undefined) {
      message = "مجوزی یافت نشد";
      this.toast.showError(message);
      return undefined;
    }

    this.toast.showSuccess(message);
  };
  //#endregion

  //#endregion

  //#region userGroupsController

  //#region getUserGroups
  getUserGroups(userId: string): Observable<any> {
    const url = `${AUTH_API}/UserGroups/users/${userId}/groups`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, {}).subscribe({
        next: (res) => {
          this.handleGetUserGroupsResponse(res);
          let groups: Group[] = (res as any).content;
          observer.next(groups);
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

  private handleGetUserGroupsResponse = (response: any): void => {
    let message = response.message;
    if (response.content == undefined) {
      message = "گروهی یافت نشد";
      this.toast.showError(message);
      return undefined;
    }
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region selfGetUserGroups
  selfGetUserGroups(): Observable<any> {
    const url = `${AUTH_API}/UserGroups/users/self/groups`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, {}).subscribe({
        next: (res) => {
          this.handleSelfGetUserGroupsResponse(res);
          let groups: Group[] = JSON.parse((res as any).content);
          observer.next(groups);
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

  private handleSelfGetUserGroupsResponse = (response: any): void => {
    let message = response.message;
    if (response.content == undefined) {
      message = "گروهی یافت نشد";
      this.toast.showError(message);
      return undefined;
    }
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region assignGroupsToUser
  assignGroupsToUser(command: AddUserGroupsCommand): Observable<any> {
    const url = `${AUTH_API}/UserGroups/users/${command.userId}/addGroups`;
    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, command).subscribe({
        next: (res) => {
          this.handleAssignGroupsToUserResponse(res);
          observer.next(res);
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

  private handleAssignGroupsToUserResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region removeGroupsToUser
  removeGroupsToUser(command: RemoveUserGroupsCommand): Observable<any> {
    const url = `${AUTH_API}/UserGroups/users/${command.userId}/removeGroups`;
    return new Observable((observer: Observer<any>) => {
      this.http.post<any>(url, command).subscribe({
        next: (res) => {
          this.handleRemoveGroupsToUserResponse(res);
          observer.next(res);
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

  private handleRemoveGroupsToUserResponse = (response: any): void => {
    let message = response.message;
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region getAllUserGroupPermissions
  getAllUserGroupPermissions(userId: string): Observable<any> {
    const url = `${AUTH_API}/UserGroups/users/${userId}/groups/all/permissions`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, {}).subscribe({
        next: (res) => {
          this.handleGetAllUserGroupPermissionsResponse(res);
          debugger;
          let userGroupPermissions: UserGroupPermission[] = (res as any)
            .content;
          observer.next(userGroupPermissions);
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

  private handleGetAllUserGroupPermissionsResponse = (response: any): void => {
    let message = response.message;
    if (response.content == undefined) {
      message = "گروه مجوزی یافت نشد";
      this.toast.showError(message);
      return undefined;
    }
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region selfGetAllUserGroupPermissions
  selfGetAllUserGroupPermissions(): Observable<any> {
    const url = `${AUTH_API}/UserGroups/users/self/groups/all/permissions`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, {}).subscribe({
        next: (res) => {
          this.handleSelfGetAllUserGroupPermissionsResponse(res);
          let userGroupPermissions: UserGroupPermission[] = JSON.parse(
            (res as any).content
          );
          observer.next(userGroupPermissions);
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

  private handleSelfGetAllUserGroupPermissionsResponse = (
    response: any
  ): void => {
    let message = response.message;
    if (response.content == undefined) {
      message = "گروه مجوزی یافت نشد";
      this.toast.showError(message);
      return undefined;
    }
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region getUserGroupPermissions
  getUserGroupPermissions(userId: string, groupId: string): Observable<any> {
    const url = `${AUTH_API}/UserGroups/users/${userId}/groups/${groupId}/permissions`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, {}).subscribe({
        next: (res) => {
          this.handleGetUserGroupPermissionsResponse(res);
          let userGroupPermissions: UserGroupPermission[] = JSON.parse(
            (res as any).content
          );
          observer.next(userGroupPermissions);
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

  private handleGetUserGroupPermissionsResponse = (response: any): void => {
    let message = response.message;
    if (response.content == undefined) {
      message = "گروه مجوزی یافت نشد";
      this.toast.showError(message);
      return undefined;
    }
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region selfGetUserGroupPermissions
  selfGetUserGroupPermissions(groupId: string): Observable<any> {
    const url = `${AUTH_API}/UserGroups/users/self/groups/${groupId}/permissions`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, {}).subscribe({
        next: (res) => {
          this.handleSelfGetUserGroupPermissionsResponse(res);
          let userGroupPermissions: UserGroupPermission[] = JSON.parse(
            (res as any).content
          );
          observer.next(userGroupPermissions);
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

  private handleSelfGetUserGroupPermissionsResponse = (response: any): void => {
    let message = response.message;
    if (response.content == undefined) {
      message = "گروه مجوزی یافت نشد";
      this.toast.showError(message);
      return undefined;
    }
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region getUserPermissions
  getUserPermissions(userId: string): Observable<any> {
    const url = `${AUTH_API}/UserGroups/users/${userId}/permissions`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, {}).subscribe({
        next: (res) => {
          this.handleGetUserPermissionsResponse(res);
          let permissions: Permission[] = JSON.parse((res as any).content);
          observer.next(permissions);
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

  private handleGetUserPermissionsResponse = (response: any): void => {
    let message = response.message;
    if (response.content == undefined) {
      message = "مجوزی یافت نشد";
      this.toast.showError(message);
      return undefined;
    }
    this.toast.showSuccess(message);
  };
  //#endregion

  //#region selfGetUserPermissions
  selfGetUserPermissions(userId: string): Observable<any> {
    const url = `${AUTH_API}/UserGroups/users/self/permissions`;
    return new Observable((observer: Observer<any>) => {
      this.http.get(url, {}).subscribe({
        next: (res) => {
          this.handleSelfGetUserPermissionsResponse(res);
          let permissions: Permission[] = JSON.parse((res as any).content);
          observer.next(permissions);
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

  private handleSelfGetUserPermissionsResponse = (response: any): void => {
    let message = response.message;
    if (response.content == undefined) {
      message = "مجوزی یافت نشد";
      this.toast.showError(message);
      return undefined;
    }
    this.toast.showSuccess(message);
  };
  //#endregion

  //#endregion

  //#region captchaController

  //#region getCaptcha
  getCaptcha(): Observable<any> {
    const url = `${AUTH_API}/Captcha`;
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
        },
      });
    });
  }

  //#endregion

  //#region getCaptchaAgain
  getCaptchaAgain(captchaId: string): Observable<any> {
    const url = `${AUTH_API}/Captcha/${captchaId}`;
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
        },
      });
    });
  }



  //#endregion

  //#endregion

  //#region frontHandled

  //#region logout
  logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("userId");
    localStorage.removeItem("name");
    localStorage.removeItem("userName");
    localStorage.removeItem("phoneNumber");
    localStorage.removeItem("firstName");
    localStorage.removeItem("lastName");
    localStorage.removeItem("email");
    localStorage.removeItem("isActive");
    localStorage.removeItem("nationalCode");
    localStorage.removeItem("userInfo");
    this.userName = undefined;
    this.password = undefined;
    this.router.navigateByUrl("/");
  }
  //#endregion

  //#region isLoggedIn
  isLoggedIn() {
    debugger;
    this.checkTokenExpireTime();
    return this.getAuthStatus();
  }
  //#endregion

  //#endregion

  //#region methods
  checkTokenExpireTime() {
    const token = localStorage.getItem("token");
    if (token) {
      const decodedToken: any = jwtDecode.jwtDecode(token);
      const currentTime = Date.now() / 1000;
      this.setAuthStatus(decodedToken.exp > currentTime);
    } else {
      this.setAuthStatus(false);
    }
  }
  isTokenExpired() {
    const token = localStorage.getItem("token");
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
    return of(localStorage.getItem("token"));
  }
  getUserId(): Observable<string | null> {
    return of(localStorage.getItem("userId"));
  }
  getName(): Observable<string | null> {
    return of(localStorage.getItem("name"));
  }
  getUserName(): Observable<string | null> {
    return of(localStorage.getItem("userName"));
  }
  getPhoneNumber(): Observable<string | null> {
    return of(localStorage.getItem("phoneNumber"));
  }
  getFirstName(): Observable<string | null> {
    return of(localStorage.getItem("firstName"));
  }
  getLastName(): Observable<string | null> {
    return of(localStorage.getItem("lastName"));
  }
  getEmail(): Observable<string | null> {
    return of(localStorage.getItem("email"));
  }
  getIsActive(): Observable<string | null> {
    return of(localStorage.getItem("isActive"));
  }
  getNationalCode(): Observable<string | null> {
    return of(localStorage.getItem("nationalCode"));
  }
  //#endregion
}

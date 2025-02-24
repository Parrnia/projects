
import { FileParameter } from 'src/app/web-api-client';
import { RegisterEmployeeCommand } from './../../core/services/authService/models/commands/RegisterEmployeeCommand';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';

// Register Auth
import { environment } from '../../../environments/environment';
import { AuthenticationService } from '../../core/services/authService/auth.service';
import { Router } from '@angular/router';
import { first } from 'rxjs/operators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})

/**
 * Register Component
 */
export class RegisterComponent implements OnInit {

  // Login Form
  signupForm!: UntypedFormGroup;
  submitted = false;
  successmsg = false;
  error = '';
  // set the current year
  year: number = new Date().getFullYear();
  passwordsNotMatch = false;
  constructor(private formBuilder: FormBuilder, private router: Router,
    private authenticationService: AuthenticationService) { }

  ngOnInit(): void {
    /**
     * Form Validatyion
     */
     this.signupForm = this.formBuilder.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      userName: ['', [Validators.required] ],
      nationalCode: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      confirmPassword: ['', [Validators.required]]
    })
  }


  // convenience getter for easy access to form fields
  get form() { return this.signupForm.controls; }
  

  /**
   * Register submit form
   */
   onSubmit() {
    this.submitted = true;
    let registerEmployeeCommand = new RegisterEmployeeCommand();
    registerEmployeeCommand.firstName = this.signupForm.value.firstName;
    registerEmployeeCommand.lastName = this.signupForm.value.lastName;
    registerEmployeeCommand.userName = this.signupForm.value.userName;
    registerEmployeeCommand.nationalCode = this.signupForm.value.nationalCode;
    registerEmployeeCommand.phoneNumber = this.signupForm.value.phoneNumber;
    registerEmployeeCommand.email = this.signupForm.value.email;
    registerEmployeeCommand.password = this.signupForm.value.password;
    //Register Api
    this.authenticationService.registerEmployee(registerEmployeeCommand,undefined).pipe(first()).subscribe(
      (data: any) => {
      this.successmsg = true;
      if (this.successmsg) {
        this.router.navigate(['/auth/login']);
      }
    },
    (error: any) => {
      this.error = error ? error : '';
    });

    // stop here if form is invalid
    // if (this.signupForm.invalid) {
    //   return;
    // } else {
    //   if (environment.defaultauth === 'firebase') {
    //     this.authenticationService.register(this.f['email'].value, this.f['password'].value).then((res: any) => {
    //       this.successmsg = true;
    //       if (this.successmsg) {
    //         this.router.navigate(['']);
    //       }
    //     })
    //       .catch((error: string) => {
    //         this.error = error ? error : '';
    //       });
    //   } else {
    //     this.userService.register(this.signupForm.value)
    //       .pipe(first())
    //       .subscribe(
    //         (data: any) => {
    //           this.successmsg = true;
    //           if (this.successmsg) {
    //             this.router.navigate(['/auth/login']);
    //           }
    //         },
    //         (error: any) => {
    //           this.error = error ? error : '';
    //         });
    //   }
    // }
  }

}

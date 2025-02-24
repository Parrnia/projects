import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

// Login Auth
import { environment } from '../../../environments/environment';
import { AuthenticationService } from '../../core/services/authService/auth.service';
import { first } from 'rxjs/operators';
import { PasswordSignInCommand } from 'src/app/core/services/authService/models/commands/PasswordSignInCommand';
import { CaptchaValidation } from 'src/app/core/services/authService/models/entities/CaptchaValidation';
import { PageCaptchaComponent } from '../auth/captcha/captcha.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

/**
 * Login Component
 */
export class LoginComponent implements OnInit {

  // Login Form
  loginForm!: UntypedFormGroup;
  submitted = false;
  fieldTextType!: boolean;
  error = '';
  returnUrl!: string;
  loginInProgress = false;

  toast!: false;
  captchaId!: string;

  // set the current year
  year: number = new Date().getFullYear();
  @ViewChild(PageCaptchaComponent) captchaComponent!: any;
  constructor(
    private formBuilder: UntypedFormBuilder,
    private authenticationService: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute) {
      // redirect to home if already logged in
      this.authenticationService.isLoggedIn().subscribe({
        next: (res) => {
          if(res){
            this.router.navigate(['/']);
          }
        }
      })
     }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      debugger;
      this.returnUrl = params['returnUrl'] || '/';
    });
    /**
     * Form Validatyion
     */
     this.loginForm = this.formBuilder.group({
      userName: ['', [Validators.required]],
      password: ['', [Validators.required]],
      captchaCode: ['',[Validators.required]]
    });
    // get return url from route parameters or default to '/'
    // this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }
  setCaptcha(captchaValidation: CaptchaValidation){
    this.captchaId = captchaValidation.captchaId;
    this.loginForm.controls['captchaCode'].setValue(captchaValidation.captchaCode);
}
  // convenience getter for easy access to form fields
  get form() { return this.loginForm.controls; }

  /**
   * Form submit
   */
   onSubmit() {
    this.loginInProgress = true;
    this.submitted = true;
    this.markAllControlsAsTouched(this.loginForm);
    debugger
    // Login Api
    if(this.loginForm.valid){
      let signinCommand = new PasswordSignInCommand();
      signinCommand.username = this.form.userName.value;
      signinCommand.password = this.form.password.value;
      signinCommand.captchaId = this.captchaId;
      signinCommand.captchaCode = this.form.captchaCode.value;
      this.authenticationService.passwordSignIn(signinCommand, this.returnUrl).subscribe({
        next: () => {
          this.loginInProgress = false;
        },
        error: (err) => {
          debugger;
          this.captchaComponent.reLoad();
          this.loginInProgress = false;
        }
      });
    }else{
      this.loginInProgress = false;
    }

  }
  
  /**
   * Password Hide/Show
   */
   toggleFieldTextType() {
    this.fieldTextType = !this.fieldTextType;
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
}

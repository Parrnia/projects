import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

// Auth Services
import { AuthenticationService } from '../services/authService/auth.service';
import { map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        debugger;
        let isLoggedIn = !this.authenticationService.isTokenExpired();
            if (!isLoggedIn) {
                this.router.navigate(['/auth/login'], { queryParams: { returnUrl: state.url } });
            }
        
        return isLoggedIn;
    }
}

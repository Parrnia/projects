import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable, OperatorFunction } from 'rxjs';
import { AccountApi } from '../../../api';
import { map } from 'rxjs/operators';
import { AuthService } from '../../../services/authService/auth.service';

type Pipe = ((source$: Observable<boolean>) => Observable<boolean>)
    | OperatorFunction<boolean, boolean | UrlTree>;

@Injectable({
    providedIn: 'root',
})
export class AuthGuard implements CanActivate {
    constructor(
        private router: Router,
        private authService : AuthService
    ) { }

    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        state;
        
        const authGuardMode = next.data['authGuardMode'] || 'redirectToLogin';
        let pipe: Pipe = (source$: Observable<boolean>) => source$;
         
        if (authGuardMode === 'redirectToLogin')  {
            pipe = map((isAuth: boolean) => isAuth || this.router.createUrlTree(['account/login'], { queryParams: { returnUrl: state.url } }));
        } else if (authGuardMode === 'redirectToDashboard') {
            pipe = map((isAuth: boolean) => !isAuth || this.router.createUrlTree(['account/dashboard'], { queryParams: { returnUrl: state.url } }));
        }
         
        return this.authService.isLoggedIn().pipe(map(res => res), pipe);
    }
}

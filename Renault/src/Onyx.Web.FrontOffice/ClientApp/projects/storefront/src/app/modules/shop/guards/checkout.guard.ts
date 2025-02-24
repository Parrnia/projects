import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable, combineLatest } from 'rxjs';
import { CartService } from '../../../services/cart.service';
import { map } from 'rxjs/operators';
import { AuthService } from '../../../services/authService/auth.service';


@Injectable({
    providedIn: 'root',
})
export class CheckoutGuard implements CanActivate {
    constructor(
        private cart: CartService,
        private router: Router,
        private authService: AuthService
    ) { }

    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
        return combineLatest([
            this.cart.quantity$,
            this.authService.isLoggedIn()
        ]).pipe(
            map(([quantity, isAuthenticated]) => {

                if(!isAuthenticated){
                    
                    let returnUrl = state.url;
                    this.router.navigateByUrl('/account/login?returnUrl='+ returnUrl).then();
                    return false;
                }

                if (quantity) {
                    return true;
                }

                this.router.navigateByUrl('/shop/cart').then();

                return false;
            })
        );
    }
}

import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { LayoutComponent } from './components/layout/layout.component';
// pages
import { PageAddressesComponent } from './pages/page-addresses/page-addresses.component';
import { PageDashboardComponent } from './pages/page-dashboard/page-dashboard.component';
import { PageEditAddressComponent } from './pages/page-edit-address/page-edit-address.component';
import { PageGarageComponent } from './pages/page-garage/page-garage.component';
import { PageLoginComponent } from './pages/page-login/page-login.component';
import { PageOrderDetailsComponent } from './pages/page-order-details/page-order-details.component';
import { PageOrdersComponent } from './pages/page-orders/page-orders.component';
import { PagePasswordComponent } from './pages/page-password/page-password.component';
import { PageProfileComponent } from './pages/page-profile/page-profile.component';
import { PageRegisterComponent } from './pages/page-register/page-register.component';
import { PageConfirmPhonenumberComponent } from './pages/page-confirm-phonenumber/page-confirm-phonenumber.component';
import { PageForgotPasswordComponent } from './pages/page-forgot-password/page-forgot-password.component';
import { PageResetPasswordComponent } from './pages/page-reset-password/page-reset-password.component';
import { PageChangePhonenumberComponent } from './pages/page-change-phonenumber/page-change-phonenumber.component';
import { PageCreditsComponent } from './pages/page-credits/page-credits.component';
import { PageOrderDetailsAnonymousComponent } from './pages/page-order-details-anonymous/page-order-details-anonymous.component';
import { PageOrderPaymentComponent } from './pages/page-order-payment/page-order-payment.component';
import { PageInvoiceComponent } from './pages/page-invoice/page-invoice.component';
import { PageReturnOrderComponent } from './pages/page-return-order/page-return-order.component';

const routes: Routes = [
    {
        path: '',
        pathMatch: 'full',
        redirectTo: 'login',
    },
    {
        path: 'register',
        component: PageRegisterComponent,
    },
    {
        path: 'changePhonenumber',
        component: PageChangePhonenumberComponent,
    },
    {
        path: 'confirmPhonenumber',
        component: PageConfirmPhonenumberComponent,
    },
    {
        path: 'forgot-password',
        component: PageForgotPasswordComponent,
    },
    {
        path: 'reset-password',
        component: PageResetPasswordComponent,
    },
    {
        path: 'orders-anonymous/:number/:phoneNumber',
        component: PageOrderDetailsAnonymousComponent,
    },
    {
        path: 'orderpayment/:orderId',
        component: PageOrderPaymentComponent,
    },
    {
        path: '',
        component: LayoutComponent,
        children: [
            {
                path: '',
                pathMatch: 'full',
                redirectTo: 'dashboard',
            },
            {
                path: 'dashboard',
                component: PageDashboardComponent,
            },
            {
                path: 'garage',
                component: PageGarageComponent,
            },
            {
                path: 'profile',
                component: PageProfileComponent,
            },
            {
                path: 'orders',
                component: PageOrdersComponent,
            },
            {
                path: 'invoice/:id',
                component: PageInvoiceComponent,
            },
            {
                path: 'credits',
                component: PageCreditsComponent,
            },
            {
                path: 'orders/:id',
                component: PageOrderDetailsComponent,
            },
            {
                path: 'returnOrder/:id',
                component: PageReturnOrderComponent,
            },
            {
                path: 'addresses',
                component: PageAddressesComponent,
            },
            {
                path: 'addresses/new',
                component: PageEditAddressComponent,
            },
            {
                path: 'addresses/:id',
                component: PageEditAddressComponent,
            },
            {
                path: 'password',
                component: PagePasswordComponent,
            },
            // --- START ---
            // The following routes are only needed to demonstrate possible layouts of some pages. You can delete them.
            {
                path: 'order-details',
                component: PageOrderDetailsComponent,
                data: {
                    orderId: 1,
                },
            },
            {
                path: 'edit-address',
                component: PageEditAddressComponent,
            },
            // --- END ---
        ],
        canActivate: [AuthGuard],
    },
    {
        path: 'login',
        component: PageLoginComponent,
        canActivate: [AuthGuard],
        data: {
            authGuardMode: 'redirectToDashboard',
        },
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class AccountRoutingModule { }

import { NgModule } from '@angular/core';

// modules (angular)
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
// modules (third-party)
import { TranslateModule } from '@ngx-translate/core';
// modules
import { AccountRoutingModule } from './account-routing.module';
import { SharedModule } from '../shared/shared.module';

// components
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
import { PageCaptchaComponent } from './pages/captcha/captcha.component';
import { PageChangePhonenumberComponent } from './pages/page-change-phonenumber/page-change-phonenumber.component';
import { PageCreditsComponent } from './pages/page-credits/page-credits.component';
import { PageOrderDetailsAnonymousComponent } from './pages/page-order-details-anonymous/page-order-details-anonymous.component';
import { OrderInvoiceItemComponent } from './components/OrderInvoiceItem/order-invoice-item.component';
import { PageInvoiceComponent } from './pages/page-invoice/page-invoice.component';
import { OrderInvoiceTotalComponent } from './components/OrderInvoiceTotal/order-invoice-total.component';
import { PageReturnOrderComponent } from './pages/page-return-order/page-return-order.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PageOrderPaymentComponent } from './pages/page-order-payment/page-order-payment.component';
@NgModule({
    declarations: [
        // components
        LayoutComponent,
        OrderInvoiceItemComponent,
        OrderInvoiceTotalComponent,
        // pages
        PageAddressesComponent,
        PageDashboardComponent,
        PageEditAddressComponent,
        PageGarageComponent,
        PageLoginComponent,
        PageRegisterComponent,
        PageOrderDetailsComponent,
        PageReturnOrderComponent,
        PageOrdersComponent,
        PageOrderDetailsAnonymousComponent,
        PageCreditsComponent,
        PagePasswordComponent,
        PageProfileComponent,
        PageConfirmPhonenumberComponent,
        PageChangePhonenumberComponent,
        PageForgotPasswordComponent,
        PageResetPasswordComponent,
        PageCaptchaComponent,
        PageInvoiceComponent,
        PageOrderPaymentComponent,
        PageOrderPaymentComponent,
    ],
    imports: [
        // modules (angular)
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        // modules (third-party)
        TranslateModule.forChild(),
        // modules
        AccountRoutingModule,
        ModalModule.forChild(),
        SharedModule,
    ],
    exports: [PageCaptchaComponent],
})
export class AccountModule {}

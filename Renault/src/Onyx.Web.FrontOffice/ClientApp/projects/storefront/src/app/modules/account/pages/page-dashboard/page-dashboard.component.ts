import { Component, OnDestroy, OnInit } from '@angular/core';
import { AccountApi } from '../../../../api';
import { Observable, Subject, of } from 'rxjs';
import { map, switchMap, takeUntil } from 'rxjs/operators';
import { Address } from '../../../../interfaces/address';
import { UrlService } from '../../../../services/url.service';
import { Order } from '../../../../interfaces/order';
import {
    AddressesClient,
    CreateOnlineOrderPayCommand,
    CustomersClient,
    OrdersClient,
} from 'projects/storefront/src/app/web-api-client';
import { AddressMapperService } from 'projects/storefront/src/app/mapServieces/userProfilesCluster/address-mapper.service';
import { OrdermapperService } from 'projects/storefront/src/app/mapServieces/ordersCluster/ordermapper.service';
import { AuthService } from 'projects/storefront/src/app/services/authService/auth.service';
import { ImageService } from 'projects/storefront/src/app/mapServieces/image.service';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-page-dashboard',
    templateUrl: './page-dashboard.component.html',
    styleUrls: ['./page-dashboard.component.scss'],
})
export class PageDashboardComponent implements OnInit, OnDestroy {
    private destroy$: Subject<void> = new Subject<void>();

    address!: Address;

    isAuth$: Observable<boolean>;
    firstName$!: Observable<string | null>;
    lastName$!: Observable<string | null>;
    email$!: Observable<string | null>;
    avatar$!: Observable<string | undefined>;
    isLoaded = false;
    orders: Order[] = [];

    constructor(
        public url: UrlService,
        private addressesClient: AddressesClient,
        private addressMapperService: AddressMapperService,
        private ordersClient: OrdersClient,
        private ordermapperService: OrdermapperService,
        public authService: AuthService,
        private customersClient: CustomersClient,
        private imageService: ImageService,
    ) {
        this.authService.getCustomer();
        this.isAuth$ = this.authService.isLoggedIn();
        this.authService.isLoggedIn().subscribe({
            next: (res) => {
                if (res) {
                    this.authService.getCustomer().subscribe({
                        next: () => {
                            this.firstName$ = this.authService.getFirstName();
                            this.lastName$ = this.authService.getLastName();
                            this.email$ = this.authService.getEmail();
                            this.isLoaded = true;
                        },
                    });
                    let user = this.customersClient.getCustomerById();
                    this.avatar$ = user.pipe(
                        map((c) => this.imageService.makeImageUrl(c?.avatar))
                    );
                }
            },
            error: (err) => console.log(err),
        });
        this.addressesClient
            .getDefaultAddress()
            .pipe(takeUntil(this.destroy$))
            .subscribe((x) => {
                if (x) {
                    this.address = this.addressMapperService.mapAddress(x);
                }
            });
        this.ordersClient
            .getOrdersByCustomerIdWithPagination(1, 3, '')
            .pipe(takeUntil(this.destroy$))
            .subscribe(
                (x) =>
                    (this.orders =
                        this.ordermapperService.mapOrdersByCustomerIdWithPagination(
                            x.items!
                        ))
            );
        this.isLoaded = true;
    }

    ngOnInit(): void {}

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }

    payOrder(){
        if(true){
          let cmd = new CreateOnlineOrderPayCommand();
          cmd.orderId = 45;
          cmd.customerId = localStorage.getItem('userId') ?? "";
          this.ordersClient.createOnlineOrderPay(cmd).subscribe({
              next: (res) => {
                  if(res.isSuccess && res.paymentUrl){
                      window.location.href = res.paymentUrl;
                  } else {
                      //this.toastr.error(res.errorMessage);
                  }
              }
          })
        }
        
    }
}

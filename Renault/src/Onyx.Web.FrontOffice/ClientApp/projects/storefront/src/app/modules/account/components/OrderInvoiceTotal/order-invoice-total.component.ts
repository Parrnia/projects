// invoice-totals.component.ts
import { Component, Input } from '@angular/core';
import { OrderInvoiceTotalDto  } from 'projects/storefront/src/app/web-api-client';

@Component({
  selector: 'app-invoice-total',
  templateUrl: './order-invoice-total.component.html',
  styleUrls: ['./order-invoice-total.component.scss'],
})
export class OrderInvoiceTotalComponent {
  @Input() totals: OrderInvoiceTotalDto[] = [];
}

// invoice-items.component.ts
import { Component, Input } from '@angular/core';
import {  OrderInvoiceItemDto } from 'projects/storefront/src/app/web-api-client';

@Component({
  selector: 'app-invoice-item',
  templateUrl: './order-invoice-item.component.html',
  styleUrls: ['./order-invoice-item.component.scss'],

})
export class OrderInvoiceItemComponent {
  @Input() items: OrderInvoiceItemDto[] = [];
}

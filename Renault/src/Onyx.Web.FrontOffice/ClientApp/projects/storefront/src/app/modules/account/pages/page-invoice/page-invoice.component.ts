// invoice.component.ts
import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import {ActivatedRoute } from "@angular/router"
import html2canvas from 'html2canvas';
import jsPDF from 'jspdf';
import { OrderInvoiceDto , OrderInvoiceTotalDto, OrdersClient, PersonType } from 'projects/storefront/src/app/web-api-client';

@Component({
    selector: 'app-page-invoice',
    templateUrl: './page-invoice.component.html',
    styleUrls: ['./page-invoice.component.scss'],
})
export class PageInvoiceComponent implements OnInit {
    @ViewChild('invoiceContent') invoiceContent!: ElementRef;
    changeProgress = false;
    invoice: OrderInvoiceDto | null = null;
    shippingCost: OrderInvoiceTotalDto | undefined;
    taxCost: OrderInvoiceTotalDto | undefined;
    discountOnCustomerType: OrderInvoiceTotalDto | undefined;    
    discountOnProduct: OrderInvoiceTotalDto | undefined; 
    name: string = '';
    economicCode: string = '';   
    constructor(
        private route : ActivatedRoute,
        private ordersClient: OrdersClient
    ) { }

    ngOnInit(): void {
        this.route.params.subscribe(params => {
            debugger;
            const orderId = +params['id']; 
            this.fetchInvoice(orderId);
        });
    }

    fetchInvoice(orderId: number): void {
        this.ordersClient.getInvoice(orderId).subscribe((data) => {
            this.invoice = data ?? [];
        debugger;
        console.log('invoice' + this.invoice);
        this.shippingCost = this.invoice.totals?.find(c => c.type == 0);
        this.taxCost = this.invoice.totals?.find(c => c.type == 1);
        this.discountOnCustomerType = this.invoice.totals?.find(c => c.type == 2);
        this.discountOnProduct = this.invoice.totals?.find(c => c.type == 3);
        if(this.invoice.personType == PersonType.Legal){
            this.name = this.invoice.customerFirstName ?? '';
            this.economicCode = this.invoice.customerLastName ?? '';
        } else {
            this.name = this.invoice.customerFirstName + ' ' + this.invoice.customerLastName;
            this.economicCode = ' - '; 
        }
        });
    }

    downloadPDF() {
        this.changeProgress = true;
        const invoiceElement = this.invoiceContent.nativeElement;
    
        const scaleFactor = 3;
    
        html2canvas(invoiceElement, { scale: scaleFactor, useCORS: true }).then(canvas => {
          const imgData = canvas.toDataURL('image/png');
          
          const pdf = new jsPDF('p', 'mm', 'a4');
          const imgWidth = 210;
          const pageHeight = 297;
          const imgHeight = (canvas.height * imgWidth) / canvas.width;
          let heightLeft = imgHeight;
          let position = 0;
    
          pdf.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight, 'SLOW');
          
          heightLeft -= pageHeight;
    
          while (heightLeft >= 0) {
            position = heightLeft - imgHeight;
            pdf.addPage();
            pdf.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight, 'SLOW');
            heightLeft -= pageHeight;
          }
    
          pdf.save('invoice.pdf');
          this.changeProgress = false;
        }).catch(error => {
            console.error('Error generating PDF: ', error);
            this.changeProgress = false;
          });
      }

}

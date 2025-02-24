
export interface ITab {
  id: number;
  title: string;
}

import { Component, Input, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';
import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CreditsClient } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { MaxCreditGridService } from './max-credit-grid.service';
import { MaxCreditModel } from './max-credit.model';

@Component({
  selector: 'app-max-credit',
  templateUrl: './max-credit.component.html',
  styleUrls: ['./max-credit.component.scss'],
  providers: [MaxCreditGridService, DecimalPipe, NgbAlertConfig]
})
export class MaxCreditComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  cancelForm!: FormGroup;
  processForm!: FormGroup;
  unConfirmForm!: FormGroup;
  refundCostForm!: FormGroup;
  unSuccessfulPaymentForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: MaxCreditModel;
  gridjsList$!: Observable<MaxCreditModel[]>;
  total$: Observable<number>;
  selectedMaxCredit?: any;
  @ViewChild('productTabModel') productTabModel: any = [];

  activeTab = 1;
  @Input() customer?: any;
  
  checkedItems: Set<number> = new Set<number>();
  constructor(
    public client: CreditsClient,
    public service: MaxCreditGridService) {
    this.gridjsList$ = service.maxCredits$;
    this.total$ = service.total$;
  }

  ngOnInit(): void {

    this.breadCrumbItems = [
      { label: ' اعتبار' },
      { label: 'لیست تغییرات اعتبار ', active: true }
    ];
    this.service.refreshMaxCredits(this.customer.id);
  }

  refreshGrid(){
    this.service.refreshMaxCredits(this.customer.id);
    this.gridjsList$ = this.service.maxCredits$;
  }

}


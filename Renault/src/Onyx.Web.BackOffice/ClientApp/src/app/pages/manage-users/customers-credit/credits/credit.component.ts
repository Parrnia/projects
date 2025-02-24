
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
import { CreditGridService } from './credit-grid.service';
import { CreditModel } from './credit.model';

@Component({
  selector: 'app-credit',
  templateUrl: './credit.component.html',
  styleUrls: ['./credit.component.scss'],
  providers: [CreditGridService, DecimalPipe, NgbAlertConfig]
})
export class CreditComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  cancelForm!: FormGroup;
  processForm!: FormGroup;
  unConfirmForm!: FormGroup;
  refundCostForm!: FormGroup;
  unSuccessfulPaymentForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: CreditModel;
  gridjsList$!: Observable<CreditModel[]>;
  total$: Observable<number>;
  selectedCredit?: any;
  @ViewChild('productTabModel') productTabModel: any = [];

  activeTab = 1;
  @Input() customer?: any;
  
  checkedItems: Set<number> = new Set<number>();
  constructor(
    public client: CreditsClient,
    public service: CreditGridService) {
    this.gridjsList$ = service.credits$;
    this.total$ = service.total$;
  }

  ngOnInit(): void {

    this.breadCrumbItems = [
      { label: ' اعتبار' },
      { label: 'لیست تغییرات اعتبار ', active: true }
    ];
    this.service.refreshCredits(this.customer.id);
  }

  refreshGrid(){
    this.service.refreshCredits(this.customer.id);
    this.gridjsList$ = this.service.credits$;
  }

}


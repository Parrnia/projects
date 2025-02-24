import { Component, ElementRef, Input, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';

import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ReturnOrdersClient, CountryDto, FileParameter, ModelDto, ModelsClient } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { ReturnOrderTotalDocumentGridService } from './return-order-total-document-grid.service';
import { ReturnOrderTotalDocumentModel } from './return-order-total-document.model';
import { NgbdReturnOrderTotalDocumentSortableHeader, SortEvent } from './return-order-total-document-sortable.directive';
import { ImageService } from "src/app/core/services/image.service";
import { ReturnOrderTotalModel } from '../return-order-total.model';

@Component({
  selector: 'app-return-order-total-document',
  templateUrl: './return-order-total-document.component.html',
  styleUrls: ['./return-order-total-document.component.scss'],
  providers: [ReturnOrderTotalDocumentGridService, DecimalPipe, NgbAlertConfig]
})
export class ReturnOrderTotalDocumentComponent {
  entityState: number = 0;
  breadCrumbTotals!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedTotal?: ReturnOrderTotalDocumentModel;
  gridjsList$!: Observable<ReturnOrderTotalDocumentModel[]>;
  total$: Observable<number>;
  modelsList?: ModelDto[];
  inProgress = false;
  @ViewChildren(NgbdReturnOrderTotalDocumentSortableHeader) returnOrderTotalDocuments!: QueryList<NgbdReturnOrderTotalDocumentSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  modelId?: any;
  formData = new FormData();
  checkedTotals: Set<number> = new Set<number>();
  @Input('returnOrderTotal') returnOrderTotal?: ReturnOrderTotalModel;
  constructor(alertConfig: NgbAlertConfig,
    public client: ReturnOrdersClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private imageService: ImageService,
    public service: ReturnOrderTotalDocumentGridService) {
    this.gridjsList$ = service.returnOrderTotalDocuments$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      description: ['', [Validators.required]],
      image :  ['', [Validators.required]],
    });

    alertConfig.type = 'success';


  }

  ngOnInit(): void {
this.service.getAllReturnOrderTotalDocumentsByReturnOrderTotalId(
  this.returnOrderTotal?.id ?? 0
);
  }


  toggleCheckbox(totalId: number) {
    if (this.checkedTotals.has(totalId)) {
      this.checkedTotals.delete(totalId);
    } else {
      this.checkedTotals.add(totalId);
    }
  }

  isChecked(totalId: number): boolean {
    return this.checkedTotals.has(totalId);
  }


  get form() {
    return this.myForm.controls;
  }

}


import { Component, ElementRef, Input, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';

import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ReturnOrdersClient, CountryDto, FileParameter, ModelDto, ModelsClient } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { ReturnOrderItemDocumentGridService } from './return-order-item-document-grid.service';
import { ReturnOrderItemDocumentModel } from './return-order-item-document.model';
import { NgbdReturnOrderItemDocumentSortableHeader, SortEvent } from './return-order-item-document-sortable.directive';
import { ImageService } from "src/app/core/services/image.service";
import { ReturnOrderItemModel } from '../return-order-item.model';

@Component({
  selector: 'app-return-order-item-document',
  templateUrl: './return-order-item-document.component.html',
  styleUrls: ['./return-order-item-document.component.scss'],
  providers: [ReturnOrderItemDocumentGridService, DecimalPipe, NgbAlertConfig]
})
export class ReturnOrderItemDocumentComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ReturnOrderItemDocumentModel;
  gridjsList$!: Observable<ReturnOrderItemDocumentModel[]>;
  total$: Observable<number>;
  modelsList?: ModelDto[];
  inProgress = false;
  @ViewChildren(NgbdReturnOrderItemDocumentSortableHeader) returnOrderItemDocuments!: QueryList<NgbdReturnOrderItemDocumentSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  modelId?: any;
  formData = new FormData();
  checkedItems: Set<number> = new Set<number>();
  @Input('returnOrderItem') returnOrderItem?: ReturnOrderItemModel;
  constructor(alertConfig: NgbAlertConfig,
    public client: ReturnOrdersClient,
    private fb: FormBuilder,
    private modalService: NgbModal,
    private imageService: ImageService,
    public service: ReturnOrderItemDocumentGridService) {
    this.gridjsList$ = service.returnOrderItemDocuments$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      description: ['', [Validators.required]],
      image :  ['', [Validators.required]],
    });

    alertConfig.type = 'success';


  }

  ngOnInit(): void {
     this.service.getAllReturnOrderItemDocumentsByReturnOrderItemId(
       this.returnOrderItem?.id ?? 0
     );
  }


  toggleCheckbox(itemId: number) {
    if (this.checkedItems.has(itemId)) {
      this.checkedItems.delete(itemId);
    } else {
      this.checkedItems.add(itemId);
    }
  }

  isChecked(itemId: number): boolean {
    return this.checkedItems.has(itemId);
  }


  get form() {
    return this.myForm.controls;
  }

}


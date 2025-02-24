



import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { FileParameter, BadgeDto, BadgesClient } from "src/app/web-api-client";
import { ProductBadgesGridService } from "./product-badges-grid.service";
import { ProductBadgesModel } from "./product-badges.model";

@Component({
  selector: 'app-product-badges',
  templateUrl: './product-badges.component.html',
  styleUrls: ['./product-badges.component.scss'],
  providers: [ProductBadgesGridService, DecimalPipe, NgbAlertConfig],
})
export class ProductBadgesComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: ProductBadgesModel;
  gridjsList$!: Observable<ProductBadgesModel[]>;
  total$: Observable<number>;
  inProgress = false;
  badgeList?: BadgeDto[];
  badgeId?: any;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;

  @ViewChild('formModal') formModal: any = [];
  formModalRef: NgbModalRef | undefined;

  @Input() productOption?: any;

  checkedItems: Set<number> = new Set<number>();
  constructor(alertConfig: NgbAlertConfig, public badgesClient: BadgesClient, private fb: FormBuilder, private modalService: NgbModal, public service: ProductBadgesGridService, private formBuilder: UntypedFormBuilder) {
    this.gridjsList$ = service.productBadges$;
    this.total$ = service.total$;
  }

  ngOnInit(): void {

    this.service.getAllProductBadges(this.productOption.id);
    this.getAllBadgess();
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

  deleteMultiple(content: any) {
    if ([...this.checkedItems].length > 0) {
      this.confirmationModalRef = this.modalService.open(content, { centered: true });
    }
    else {
      Swal.fire({
        title: 'حداقل یک مورد را انتخاب کنید!',
        icon: 'question',
        iconHtml: '!',
        confirmButtonText: 'تایید'
      })

    }
  }


  openInsertModal() {
    this.formModalRef = this.modalService.open(this.formModal);
  }


  openDeleteConfirmationModal(id: any) {

    this.selectedId = id;
    this.modalService.open(this.confirmationModal);
  }


  deleteItems() {
    this.inProgress = true;
    this.badgesClient.deleteRangeBadgeFromAttributeOption(this.productOption?.id ,[...this.checkedItems]).subscribe(result => {
      this.inProgress = false;
      if (result == null) {

        Swal.fire({
          title: 'حذف نشان با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.confirmationModalRef?.close();
        this.service.getAllProductBadges(this.productOption.id);

      } else {

        Swal.fire({
          title: 'حذف نشان با خطا مواجه شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })

        this.confirmationModalRef?.close();


      }
    }, error => {
      this.inProgress = false;
     console.error(error)
    });

    this.selectedId = 0;

  }

  public getAllBadgess() {
    this.badgesClient.getAllBadges().subscribe(result => {
      this.badgeList = result;
    }, error => console.error(error));

  }
}




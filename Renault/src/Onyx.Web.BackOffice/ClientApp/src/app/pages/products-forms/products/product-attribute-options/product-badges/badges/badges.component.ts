



import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import {  FileParameter, BadgeDto, BadgesClient, AddRangeProductAttributeOptionBadgesCommand } from "src/app/web-api-client";
import { BadgesGridService } from "./badges-grid.service";
import { BadgesModel } from "./badges.model";

@Component({
  selector: 'app-badges',
  templateUrl: './badges.component.html',
  styleUrls: ['./badges.component.scss'],
  providers: [BadgesGridService, DecimalPipe, NgbAlertConfig],
})
export class BadgesComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  selectedId: any = 0;
  submit!: boolean;
  selectedItem?: BadgeDto;
  gridjsList$!: Observable<BadgesModel[]>;
  total$: Observable<number>;
  inProgress = false;
  badgeList?: BadgeDto[];
  badgeId?: any;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild('formModal') formModal: any = [];
  formModalRef: NgbModalRef | undefined;
  @ViewChild('addModel') addModel: any = [];
  addModelRef: NgbModalRef | undefined;
  @Input() productAttributeOption?: any;
  checkedItems: Set<number> = new Set<number>();
  constructor(
    public badgesClient: BadgesClient,
    private fb: FormBuilder, 
    private modalService: NgbModal, 
    public service: BadgesGridService, 
    private formBuilder: UntypedFormBuilder) {

    this.gridjsList$ = service.badges$;
    this.total$ = service.total$;
  }

  ngOnInit(): void {
    this.service.getAllBadges();
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

  addMultiple(content: any) {
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



  addItems() {
    this.inProgress = true;
    let command = new AddRangeProductAttributeOptionBadgesCommand();
    command.badgeIds = [...this.checkedItems];
    command.productAttributeOptionId = this.productAttributeOption?.id;
    this.badgesClient.addRange(command).subscribe(result => {
      this.inProgress = false;
      if (result == null) {

        Swal.fire({
          title: 'اضافه کردن نشان با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        debugger;
        this.confirmationModalRef?.close();
        this.checkedItems = new Set<number>();
      } else {

        Swal.fire({
          title: 'اضافه کردن نشان با خطا مواجه شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })

        this.confirmationModalRef?.close();
        this.checkedItems = new Set<number>();
      }
    }, error => {
      this.inProgress = false;
     console.error(error)
    });

  }

  resetForm(){
    var checkboxes: any = document.getElementsByName('checkAll');
    for (var i = 0; i < checkboxes.length; i++) {
        checkboxes[i].checked = false;
    }
  }
}




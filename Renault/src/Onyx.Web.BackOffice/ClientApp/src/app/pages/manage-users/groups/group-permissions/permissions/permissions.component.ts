



import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { PermissionsGridService } from "./permissions-grid.service";
import { PermissionsModel } from "./permissions.model";
import { AuthenticationService } from "src/app/core/services/authService/auth.service";
import { Permission } from "src/app/core/services/authService/models/entities/Permission";
import { AddGroupPermissionsCommand } from "src/app/core/services/authService/models/commands/AddGroupPermissionsCommand";

@Component({
  selector: 'app-permissions',
  templateUrl: './permissions.component.html',
  styleUrls: ['./permissions.component.scss'],
  providers: [PermissionsGridService, DecimalPipe, NgbAlertConfig],
})
export class PermissionsComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  selectedId: any = null;
  submit!: boolean;
  selectedItem?: Permission;
  gridjsList$!: Observable<PermissionsModel[]>;
  total$: Observable<number>;
  inProgress = false;
  permissionList?: Permission[];
  permissionId?: any;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild('formModal') formModal: any = [];
  formModalRef: NgbModalRef | undefined;
  @ViewChild('addModel') addModel: any = [];
  addModelRef: NgbModalRef | undefined;
  @Input() group?: any;
  checkedItems: Set<string> = new Set<string>();
  constructor(
    alertConfig: NgbAlertConfig, 
    public client: AuthenticationService, 
    private fb: FormBuilder, 
    private modalService: NgbModal, 
    public service: PermissionsGridService, 
    private formBuilder: UntypedFormBuilder) {
    this.gridjsList$ = service.permissions$;
    this.total$ = service.total$;
  }

  ngOnInit(): void {

    this.service.getAllPermissions();
  }



  toggleCheckbox(itemId: string) {
    if (this.checkedItems.has(itemId)) {
      this.checkedItems.delete(itemId);
    } else {
      this.checkedItems.add(itemId);
    }
  }

  isChecked(itemId: string): boolean {
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
    let command = new AddGroupPermissionsCommand();
    command.permissionIds = [...this.checkedItems];
    command.groupId = this.group?.id;
    this.client.assignPermission(command).subscribe(result => {
      this.inProgress = false;
        Swal.fire({
          title: 'اضافه کردن مجوزها با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        debugger;
        this.confirmationModalRef?.close();
        this.checkedItems = new Set<string>();
      
    }, error => {
      this.inProgress = false;
      Swal.fire({
        title: 'اضافه کردن مجوزها با خطا مواجه شد',
        icon: 'error',
        iconHtml: '!',
        confirmButtonText: 'تایید'
      })

      this.confirmationModalRef?.close();
      this.checkedItems = new Set<string>();
      console.error(error);
    });

  }

  resetForm(){
    var checkboxes: any = document.getElementsByName('checkAll');
    for (var i = 0; i < checkboxes.length; i++) {
        checkboxes[i].checked = false;
    }
  }
}




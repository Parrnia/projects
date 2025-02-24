



import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { PermissionGroupsGridService } from "./permission-groups-grid.service";
import { PermissionGroupsModel } from "./permission-groups.model";
import { Permission } from "src/app/core/services/authService/models/entities/Permission";
import { AuthenticationService } from "src/app/core/services/authService/auth.service";
import { RemoveGroupPermissionsCommand } from "src/app/core/services/authService/models/commands/RemoveGroupPermissionsCommand";

@Component({
  selector: 'app-permission-groups',
  templateUrl: './permission-groups.component.html',
  styleUrls: ['./permission-groups.component.scss'],
  providers: [PermissionGroupsGridService, DecimalPipe, NgbAlertConfig],
})
export class PermissionGroupsComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  selectedId: any = null;
  submit!: boolean;
  selectedItem?: PermissionGroupsModel;
  gridjsList$!: Observable<PermissionGroupsModel[]>;
  total$: Observable<number>;
  inProgress = false;
  permissionList?: Permission[];
  kindId?: any;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild('formModal') formModal: any = [];
  @Input() group?: any;

  checkedItems: Set<string> = new Set<string>();
  constructor(alertConfig: NgbAlertConfig,
    public client: AuthenticationService, 
    private fb: FormBuilder, 
    private modalService: NgbModal, 
    public service: PermissionGroupsGridService, 
    private formBuilder: UntypedFormBuilder) {
    this.gridjsList$ = service.permissionGroups$;
    this.total$ = service.total$;


  }

  ngOnInit(): void {
    this.service.getAllPermissionGroups(this.group.id);
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
    this.modalService.open(this.formModal);
  }


  openDeleteConfirmationModal(id: any) {

    this.selectedId = id;
    this.modalService.open(this.confirmationModal);
  }


  removeItems() {
    this.inProgress = true;
    let cmd = new RemoveGroupPermissionsCommand();
    cmd.groupId = this.group.id;
    cmd.permissionIds = [...this.checkedItems];
    this.client.removePermission(cmd).subscribe(result => {
      this.inProgress = false;
        Swal.fire({
          title: 'حذف مجوز با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.confirmationModalRef?.close();
        this.service.refresh(this.group.id);

    }, error => {
      this.confirmationModalRef?.close();
      this.inProgress = false;
      Swal.fire({
        title: 'حذف مجوز با خطا مواجه شد',
        icon: 'error',
        iconHtml: '!',
        confirmButtonText: 'تایید'
      })

      this.modalService.dismissAll();
      console.error(error);
    });

    this.selectedId = 0;

  }

  
}
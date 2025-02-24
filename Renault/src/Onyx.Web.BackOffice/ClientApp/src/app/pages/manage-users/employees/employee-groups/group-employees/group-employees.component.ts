



import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { GroupEmployeesGridService } from "./group-employees-grid.service";
import { GroupEmployeesModel } from "./group-employees.model";
import { Permission } from "src/app/core/services/authService/models/entities/Permission";
import { AuthenticationService } from "src/app/core/services/authService/auth.service";
import { RemoveGroupPermissionsCommand } from "src/app/core/services/authService/models/commands/RemoveGroupPermissionsCommand";
import { RemoveUserGroupsCommand } from "src/app/core/services/authService/models/commands/RemoveUserGroupsCommand";

@Component({
  selector: 'app-group-employees',
  templateUrl: './group-employees.component.html',
  styleUrls: ['./group-employees.component.scss'],
  providers: [GroupEmployeesGridService, DecimalPipe, NgbAlertConfig],
})
export class GroupEmployeesComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  selectedId: any = null;
  submit!: boolean;
  selectedItem?: GroupEmployeesModel;
  gridjsList$!: Observable<GroupEmployeesModel[]>;
  total$: Observable<number>;
  inProgress = false;
  permissionList?: Permission[];
  kindId?: any;
  // @ViewChildren(NgbdProductImagesSortableHeader) productImages!: QueryList<NgbdProductImagesSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild('formModal') formModal: any = [];
  @Input() user?: any;

  checkedItems: Set<string> = new Set<string>();
  constructor(alertConfig: NgbAlertConfig,
    public client: AuthenticationService, 
    private fb: FormBuilder, 
    private modalService: NgbModal, 
    public service: GroupEmployeesGridService, 
    private formBuilder: UntypedFormBuilder) {
    this.gridjsList$ = service.groupEmployees$;
    this.total$ = service.total$;


  }

  ngOnInit(): void {
    this.service.getAllGroupEmployees(this.user.id);
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
      this.modalService.open(content, { centered: true });
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
    let cmd = new RemoveUserGroupsCommand();
    cmd.userId = this.user.id;
    cmd.groupIds = [...this.checkedItems];
    this.client.removeGroupsToUser(cmd).subscribe(result => {
      this.inProgress = false;
      if (result == null) {
        Swal.fire({
          title: 'حذف گروه مجوز با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.confirmationModalRef?.close();
        this.service.refresh(this.user.id);

      } else {
        this.confirmationModalRef?.close();
        Swal.fire({
          title: 'حذف گروه مجوز با خطا مواجه شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })

        this.modalService.dismissAll();


      }
    }, error => {
      this.inProgress = false;
     console.error(error)
  });

    this.selectedId = 0;

  }

  
}




import { Component, Input, QueryList, ViewChild, ViewChildren } from "@angular/core";
import { DecimalPipe } from "@angular/common";
import { NgbAlertConfig, NgbModal, NgbModalRef } from "@ng-bootstrap/ng-bootstrap";
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import Swal from "sweetalert2";
import { GroupsGridService } from "./groups-grid.service";
import { GroupsModel } from "./groups.model";
import { AuthenticationService } from "src/app/core/services/authService/auth.service";
import { Group } from "src/app/core/services/authService/models/entities/Group";
import { AddUserGroupsCommand } from "src/app/core/services/authService/models/commands/AddUserGroupsCommand";

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.scss'],
  providers: [GroupsGridService, DecimalPipe, NgbAlertConfig],
})
export class GroupsComponent {
  // bread crumb items
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  selectedId: any = null;
  submit!: boolean;
  selectedItem?: Group;
  gridjsList$!: Observable<GroupsModel[]>;
  total$: Observable<number>;
  inProgress = false;
  groupList?: Group[];
  groupId?: any;
  @ViewChild('confirmationModal') confirmationModal: any;
  confirmationModalRef: NgbModalRef | undefined;
  @ViewChild('formModal') formModal: any = [];
  formModalRef: NgbModalRef | undefined;
  @ViewChild('addModel') addModel: any = [];
  addModelRef: NgbModalRef | undefined;
  @Input() user?: any;
  checkedItems: Set<string> = new Set<string>();
  constructor(
    alertConfig: NgbAlertConfig, 
    public client: AuthenticationService, 
    private fb: FormBuilder, 
    private modalService: NgbModal, 
    public service: GroupsGridService, 
    private formBuilder: UntypedFormBuilder) {
    this.gridjsList$ = service.groups$;
    this.total$ = service.total$;
  }

  ngOnInit(): void {

    this.service.getAllGroups();
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
    let command = new AddUserGroupsCommand();
    command.groupIds = [...this.checkedItems];
    command.userId = this.user?.id;
    this.client.assignGroupsToUser(command).subscribe(result => {
      this.inProgress = false;
      if (result == null) {

        Swal.fire({
          title: 'اضافه کردن گروه مجوزها با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        debugger;
        this.confirmationModalRef?.close();
        this.checkedItems = new Set<string>();
      } else {

        Swal.fire({
          title: 'اضافه کردن گروه مجوزها با خطا مواجه شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })

        this.confirmationModalRef?.close();
        this.checkedItems = new Set<string>();
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




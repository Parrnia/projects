import { Component, ElementRef, QueryList, Renderer2, ViewChild, ViewChildren } from '@angular/core';

import { Observable, of } from 'rxjs';
import { FormBuilder, FormGroup, UntypedFormBuilder, Validators } from '@angular/forms';

import { NgbAlertConfig, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CountriesClient, CountryDto, FileParameter } from 'src/app/web-api-client';
import { DecimalPipe } from '@angular/common';
import Swal from 'sweetalert2';
import { GroupGridService } from './group-grid.service';
import { GroupModel } from './group.model';
import { NgbdGroupSortableHeader, SortEvent } from './group-sortable.directive';
import { ImageService } from 'src/app/core/services/image.service';
import { NonNullAssert } from '@angular/compiler';
import { AuthenticationService } from 'src/app/core/services/authService/auth.service';
import { DeleteGroupCommand } from 'src/app/core/services/authService/models/commands/DeleteGroupCommand';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss'],
  providers: [GroupGridService, DecimalPipe, NgbAlertConfig]
})
export class GroupComponent {
  entityState: number = 0;
  breadCrumbItems!: Array<{}>;
  myForm!: FormGroup;
  selectedId: any = null;
  submit!: boolean;
  selectedItem?: GroupModel;
  gridjsList$!: Observable<GroupModel[]>;
  total$: Observable<number>;
  inProgress = false;
  @ViewChildren(NgbdGroupSortableHeader) groups!: QueryList<NgbdGroupSortableHeader>;
  @ViewChild('confirmationModal') confirmationModal: any;
  @ViewChild('formModal') formModal: any = [];
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;
  selectedGroup?: any;
  @ViewChild('groupTabModel') groupTabModel: any = [];
  formData = new FormData();
  checkedItems: Set<string> = new Set<string>();
  constructor(alertConfig: NgbAlertConfig,
    private fb: FormBuilder,
    private modalService: NgbModal, 
    public service: GroupGridService,
    private imageService: ImageService,
    private client: AuthenticationService) {
    this.gridjsList$ = service.groups$;
    this.total$ = service.total$;

    this.myForm = this.fb.group({
      id: 0,
      name: ['', [Validators.required]]
    });

    alertConfig.type = 'success';


  }

  ngOnInit(): void {
    this.breadCrumbItems = [
      { label: 'خوشه مدیریت کاربران' },
      { label: 'گروه مجوز ', active: true }
    ];
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



  /**
  * Sort table data
  * @param param0 sort the column
  *
  */
  onSort({ column, direction }: SortEvent) {
    // resetting other groups
    this.groups.forEach(group => {
      if (group.sortable !== column) {
        group.direction = '';
      }
    });

    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }
  get form() {
    return this.myForm.controls;
  }


  onSubmit(): void {
    this.inProgress = true;
    this.markAllControlsAsTouched(this.myForm);
    this.submit = true;
    if (this.myForm.valid) {
       debugger;
      if (this.selectedId != null) {
        this.myForm.controls['id'].setValue(this.selectedId);

        this.client.updateGroup(
           this.myForm.value
          ).subscribe(result => {
            this.inProgress = false;
            this.service.getAllGroup();
            Swal.fire({
              title: 'ذخیره گروه مجوز با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });
            this.modalService.dismissAll();
            this.handleCloseFormModal();

        }, error => {
          Swal.fire({
            title: 'ذخیره گروه مجوز با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
          this.inProgress = false;
          this.modalService.dismissAll();
          console.error(error);
        });

      } else {

        this.client.addGroup(this.myForm.value).subscribe(result => {
          this.inProgress = false;
            this.service.getAllGroup();
            Swal.fire({
              title: 'ذخیره گروه مجوز با موفقیت انجام شد',
              icon: 'success',
              iconHtml: '!',
              confirmButtonText: 'تایید'
            });

            this.modalService.dismissAll();
            this.handleCloseFormModal();

        }, error => {
          this.inProgress = false;
          Swal.fire({
            title: 'ذخیره گروه مجوز با خطا مواجه شد',
            icon: 'error',
            iconHtml: '!',
            confirmButtonText: 'تایید'
          });
          this.modalService.dismissAll();
          console.error(error);
        });

      }
    }else{
      this.inProgress = false;
    }
  }

  resetForm(): void {
    Object.keys(this.myForm.controls).forEach(controlName => {
      const control = this.myForm.controls[controlName];
      if (control.enabled) {
        control.markAsPristine();
        control.markAsUntouched();
        control.reset();
      }
    });
  }

  edit(item: GroupModel) {

    debugger;
    this.selectedId = item.id;

    this.myForm.controls['id'].setValue(this.selectedId ?? null);
    this.myForm.controls['name'].setValue(item.name ?? null);
    this.modalService.open(this.formModal, { size: 'lg', backdrop: 'static' });

  }

  openInsertModal() {
    this.modalService.open(this.formModal, { size: 'lg', backdrop: 'static' });
  }

  openMoreDetailGroup(item: any) {
    debugger;
    this.selectedGroup = item;
    this.modalService.open(this.groupTabModel, { size: 'lg', backdrop: 'static' });
  }

  openDeleteConfirmationModal(id: any) {

    this.selectedId = id;
    console.log('    this.selectedId = id;', this.selectedId);
    this.modalService.open(this.confirmationModal);
  }


  deleteItems() {
    this.inProgress = true;
    let cmd = new DeleteGroupCommand();
    cmd.ids = [...this.checkedItems];
    this.client.deleteGroup(cmd).subscribe(result => {
      this.inProgress = false;
        Swal.fire({
          title: 'حذف گروه مجوز با موفقیت انجام شد',
          icon: 'success',
          iconHtml: '!',
          confirmButtonText: 'تایید'
        })
        this.service.getAllGroup();
        this.modalService.dismissAll();
      

    }, error => {
      this.inProgress = false;
      console.error(error);
      Swal.fire({
        title: 'حذف گروه مجوز با خطا مواجه شد',
        icon: 'error',
        iconHtml: '!',
        confirmButtonText: 'تایید'
      })

      this.modalService.dismissAll();
    });

    this.selectedId = null;

  }


  public handleCloseFormModal(){
    this.myForm.reset();
    this.myForm.markAsUntouched();
    this.myForm.setErrors(null);
    this.myForm.markAsPristine();
    this.selectedId = null;
  }
  markAllControlsAsTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      if (control instanceof FormGroup) {
        this.markAllControlsAsTouched(control);
      } else {
        control.markAsTouched();
      }
    });
  }
}


import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import lottie from 'lottie-web';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbAlertModule, NgbDropdownModule, NgbNavModule, NgbPaginationModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FlatpickrModule } from 'angularx-flatpickr';
import { SharedModule } from 'src/app/shared/shared.module';
import { defineElement } from 'lord-icon-element';;
import { NgbdGroupSortableHeader } from './group-sortable.directive';
import { GroupComponent } from './group.component';
import { TablesRoutingModule } from '../../tables/tables-routing.module';
import { PermissionsComponent } from './group-permissions/permissions/permissions.component';
import { PermissionGroupsComponent } from './group-permissions/permission-groups/permission-groups.component';



@NgModule({
  declarations: [
    NgbdGroupSortableHeader,
    GroupComponent,
    PermissionGroupsComponent,
    PermissionsComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgbDropdownModule,
    NgbPaginationModule,
    NgbTypeaheadModule,
    NgbAlertModule,
    NgbNavModule,
    FlatpickrModule,
    TablesRoutingModule,
    SharedModule,
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})


export class GroupModule { 
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}




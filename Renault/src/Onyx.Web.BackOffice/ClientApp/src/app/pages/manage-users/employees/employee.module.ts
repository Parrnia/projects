import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import lottie from 'lottie-web';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbAlertModule, NgbDropdownModule, NgbNavModule, NgbPaginationModule, NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { FlatpickrModule } from 'angularx-flatpickr';
import { SharedModule } from 'src/app/shared/shared.module';
import { defineElement } from 'lord-icon-element';;
import { NgbdEmployeeSortableHeader } from './employee-sortable.directive';
import { EmployeeComponent } from './employee.component';
import { TablesRoutingModule } from '../../tables/tables-routing.module';
import { GroupsComponent } from './employee-groups/groups/groups.component';
import { GroupEmployeesComponent } from './employee-groups/group-employees/group-employees.component';



@NgModule({
  declarations: [
    NgbdEmployeeSortableHeader,
    EmployeeComponent,
    GroupsComponent,
    GroupEmployeesComponent
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


export class EmployeeModule { 
  constructor() {
    defineElement(lottie.loadAnimation);
  }
}




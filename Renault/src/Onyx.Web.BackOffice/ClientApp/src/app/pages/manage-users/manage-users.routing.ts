import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomerComponent } from './customers/customer.component';
import { CustomerCreditComponent } from './customers-credit/customer-credit.component';
import { EmployeeComponent } from './employees/employee.component';
import { GroupComponent } from './groups/group.component';
import { CustomerTypeComponent } from './customer-types/customer-type.component';



// Component Pages

const routes: Routes = [
  {
    path: "customers",
    component: CustomerComponent
  },
  {
    path: "customersCredit",
    component: CustomerCreditComponent
  },
  {
    path: "employees",
    component: EmployeeComponent
  },
  {
    path: "groups",
    component: GroupComponent
  },
  {
    path: "customerTypes",
    component: CustomerTypeComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class ManageUsersRoutingModule { }

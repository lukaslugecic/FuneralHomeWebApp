import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AllEmployeesComponent } from './all-employees/all-employees.component';
import { AllClientsComponent } from './all-clients/all-clients.component';
import { AllUsersComponent } from './all-users/all-users.component';



@NgModule({
  declarations: [
    AllEmployeesComponent,
    AllClientsComponent,
    AllUsersComponent
  ],
  imports: [
    CommonModule
  ]
})
export class AdminModule { }

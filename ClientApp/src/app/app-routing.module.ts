import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGuard } from './guards/admin/admin.guard';
import { AnonGuard } from './guards/anon/anon.guard';
import { HomeComponent } from './pages/home/home.component';
import { HomeModule } from './pages/home/home.module';
import { LoginComponent } from './pages/login/login.component';
import { LoginModule } from './pages/login/login.module';
import { RegisterComponent } from './pages/register/register.component';
import { RegisterModule } from './pages/register/register.module';
import { ServicesComponent } from './pages/services/services.component';
import { ServicesModule } from './pages/services/services.module';
import { EquipmentComponent } from './pages/equipment/equipment.component';
import { EquipmentModule } from './pages/equipment/equipment.module';
import { FuneralsComponent } from './pages/funerals/funerals.component';
import { FuneralsModule } from './pages/funerals/funerals.module';
import { DeathsComponent } from './pages/deaths/deaths.component';
import { DeathsModule } from './pages/deaths/deaths.module';
import { AllUsersComponent } from './pages/admin/all-users/all-users.component';
import { AllUsersModule } from './pages/admin/all-users/all-users.module';
import { FuneralDetailComponent } from './pages/funerals/funeral-detail/funeral-detail.component';
import { InsuranceComponent } from './pages/insurance/insurance.component';
import { InsuranceModule } from './pages/insurance/insurance.module';
import { EquipmentCatalogModule } from './pages/anon/equipment-catalog/equipment-catalog.module';
import { EquipmentCatalogComponent } from './pages/anon/equipment-catalog/equipment-catalog.component';
import { ReportDeathComponent } from './pages/anon/report-death/report-death.component';
import { ReportDeathModule } from './pages/anon/report-death/report-death.module';
import { DeathCustomerFormComponent } from './pages/customer/death-customer-form/death-customer-form.component';
import { DeathCustomerFormModule } from './pages/customer/death-customer-form/death-customer-form.module';
import { OrganizeFuneralComponent } from './pages/anon/organize-funeral/organize-funeral.component';
import { OrganizeFuneralModule } from './pages/anon/organize-funeral/organize-funeral.module';
import { FuneralCustomerFormComponent } from './pages/customer/funeral-customer-form/funeral-customer-form.component';
import { FuneralCustomerFormModule } from './pages/customer/funeral-customer-form/funeral-customer-form.module';
import { AddInsuranceComponent } from './pages/anon/add-insurance/add-insurance.component';
import { AddInsuranceModule } from './pages/anon/add-insurance/add-insurance.module';
import { ProfileComponent } from './pages/customer/profile/profile.component';
import { ProfileModule } from './pages/customer/profile/profile.module';
import { CustomerGuard } from './guards/customer/customer.guard';
import { EmployeeGuard } from './guards/employee/employee.guard';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '', component: HomeComponent },
      { path: 'report-death/info', component: ReportDeathComponent},
      { path: 'organize-funeral', component: OrganizeFuneralComponent},
      { path: 'equipment-catalog', component: EquipmentCatalogComponent},
      { path: 'add-insurance', component: AddInsuranceComponent},
    ],
  },
  {
    path: '',
    children: [
      {
        path: 'users', component: AllUsersComponent,
        canActivate: [AdminGuard],
      },
      { path: 'report-death/form', component: DeathCustomerFormComponent},
      { path: 'organize-funeral/form', component: FuneralCustomerFormComponent},
      { path: 'profile', component: ProfileComponent}
    ],
    canActivate: [CustomerGuard],
  },
  {
    path: '',
    children: [
      { path: 'services', component: ServicesComponent},
      { path: 'equipment', component: EquipmentComponent},
      { path: 'funerals', component: FuneralsComponent},
      { path: 'funerals/:id', component: FuneralDetailComponent},
      { path: 'deaths', component: DeathsComponent},
      { path: 'insurances', component: InsuranceComponent},
    ],
    canActivate: [EmployeeGuard],
  },
  {
    path: '',
    children: [
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
    ],
    canActivate: [AnonGuard],
  },
  { path: '**', redirectTo: '' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    HomeModule,
    RegisterModule,
    LoginModule,
    ServicesModule,
    EquipmentModule,
    FuneralsModule,
    DeathsModule,
    AllUsersModule,
    InsuranceModule,
    EquipmentCatalogModule,
    ReportDeathModule,
    DeathCustomerFormModule,
    OrganizeFuneralModule,
    FuneralCustomerFormModule,
    AddInsuranceModule,
    ProfileModule
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }

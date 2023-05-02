import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGuard } from './guards/admin/admin.guard';
import { AnonGuard } from './guards/anon/anon.guard';
import { AuthGuard } from './guards/auth/auth.guard';
import { HomeComponent } from './pages/home/home.component';
import { HomeModule } from './pages/home/home.module';
import { LoginComponent } from './pages/login/login.component';
import { LoginModule } from './pages/login/login.module';
import { RegisterComponent } from './pages/register/register.component';
import { RegisterModule } from './pages/register/register.module';
import { AllServicesComponent } from './pages/equipment-and-services/all-services/all-services/all-services.component';
import { AllServicesModule } from './pages/equipment-and-services/all-services/all-services/all-services.module';
import { AllEquipmentComponent } from './pages/equipment-and-services/all-equipment/all-equipment/all-equipment.component';
import { AllEquipmentModule } from './pages/equipment-and-services/all-equipment/all-equipment/all-equipment.module';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    children: [
      {
        path: 'admin',
        children: [
          //todo
        ],
        canActivate: [AdminGuard],
      },
      { path: 'services', component: AllServicesComponent},
      { path: 'equipment', component: AllEquipmentComponent}
    ],
    canActivate: [AuthGuard],
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
    AllServicesModule,
    AllEquipmentModule
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }

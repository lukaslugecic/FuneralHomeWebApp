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
import { FuneralsComponent } from './pages/funerals/funerals.component';
import { FuneralsModule } from './pages/funerals/funerals.module';
import { DeathsComponent } from './pages/deaths/deaths.component';
import { DeathsModule } from './pages/deaths/deaths.module';
import { AllUsersComponent } from './pages/admin/all-users/all-users.component';
import { AllUsersModule } from './pages/admin/all-users/all-users.module';
import { FuneralDetailComponent } from './pages/funerals/funeral-detail/funeral-detail.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    children: [
      {
        path: 'users', component: AllUsersComponent,
        canActivate: [AdminGuard],
      },
      { path: 'services', component: AllServicesComponent},
      { path: 'equipment', component: AllEquipmentComponent},
      { path: 'funerals', component: FuneralsComponent},
      { path: 'funerals/:id', component: FuneralDetailComponent},
      { path: 'deaths', component: DeathsComponent}
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
    AllEquipmentModule,
    FuneralsModule,
    DeathsModule,
    AllUsersModule
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }

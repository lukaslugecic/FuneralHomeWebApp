import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddInsuranceComponent } from './add-insurance.component';
import { MainNavigationModule } from 'src/app/components/main-navigation/main-navigation.module';
import { MainFooterModule } from 'src/app/components/main-footer/main-footer.module';
import { MatButtonModule } from '@angular/material/button';
import { AddInsuranceDialogModule } from 'src/app/components/dialogs/add-insurance-dialog/add-insurance-dialog.module';
import { AuthNavigationModule } from 'src/app/components/auth-navigation/auth-navigation.module';



@NgModule({
  declarations: [AddInsuranceComponent],
  imports: [
    CommonModule,
    MainNavigationModule,
    MainFooterModule,
    MatButtonModule,
    AddInsuranceDialogModule,
    AuthNavigationModule
  ]
})
export class AddInsuranceModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrganizeFuneralComponent } from './organize-funeral.component';
import { MainFooterModule } from 'src/app/components/main-footer/main-footer.module';
import { MainNavigationModule } from 'src/app/components/main-navigation/main-navigation.module';
import { MatButtonModule } from '@angular/material/button';
import { AuthNavigationModule } from 'src/app/components/auth-navigation/auth-navigation.module';


@NgModule({
  declarations: [OrganizeFuneralComponent],
  imports: [
    CommonModule,
    MainFooterModule,
    MainNavigationModule,
    MatButtonModule,
    AuthNavigationModule
  ]
})
export class OrganizeFuneralModule { }

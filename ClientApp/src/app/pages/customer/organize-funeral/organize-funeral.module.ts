import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrganizeFuneralComponent } from './organize-funeral.component';
import { MainFooterModule } from 'src/app/components/main-footer/main-footer.module';
import { MainNavigationModule } from 'src/app/components/main-navigation/main-navigation.module';
import { MatButtonModule } from '@angular/material/button';


@NgModule({
  declarations: [OrganizeFuneralComponent],
  imports: [
    CommonModule,
    MainFooterModule,
    MainNavigationModule,
    MatButtonModule
  ]
})
export class OrganizeFuneralModule { }

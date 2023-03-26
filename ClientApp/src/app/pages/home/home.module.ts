import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { MainNavigationModule } from 'src/app/components/main-navigation/main-navigation.module';
import { MainFooterModule } from 'src/app/components/main-footer/main-footer.module';



@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    MainNavigationModule,
    MainFooterModule
  ],
  exports: [HomeComponent]
})
export class HomeModule { }

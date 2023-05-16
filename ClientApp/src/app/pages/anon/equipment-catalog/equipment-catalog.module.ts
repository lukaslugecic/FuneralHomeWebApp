import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EquipmentCatalogComponent, EquipmentFilterPipe } from './equipment-catalog.component';
import { MainNavigationModule } from 'src/app/components/main-navigation/main-navigation.module';
import { MainFooterModule } from 'src/app/components/main-footer/main-footer.module';
import { AuthNavigationModule } from 'src/app/components/auth-navigation/auth-navigation.module';



@NgModule({
  declarations: [EquipmentCatalogComponent, EquipmentFilterPipe],
  imports: [
    CommonModule,
    MainFooterModule,
    MainNavigationModule,
    AuthNavigationModule
  ]
})
export class EquipmentCatalogModule { }

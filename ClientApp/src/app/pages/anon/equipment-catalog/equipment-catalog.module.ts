import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EquipmentCatalogComponent, EquipmentFilterPipe } from './equipment-catalog.component';
import { MainNavigationModule } from 'src/app/components/main-navigation/main-navigation.module';
import { MainFooterModule } from 'src/app/components/main-footer/main-footer.module';
import { AuthNavigationModule } from 'src/app/components/auth-navigation/auth-navigation.module';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import {MatBadgeModule} from '@angular/material/badge';
import { CartDialogModule } from 'src/app/components/dialogs/cart-dialog/cart-dialog.module';



@NgModule({
  declarations: [EquipmentCatalogComponent, EquipmentFilterPipe],
  imports: [
    CommonModule,
    MainFooterModule,
    MainNavigationModule,
    AuthNavigationModule,
    MatButtonModule,
    MatIconModule,
    MatBadgeModule,
    CartDialogModule,
  ]
})
export class EquipmentCatalogModule { }

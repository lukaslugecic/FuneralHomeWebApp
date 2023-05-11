import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeathCustomerFormComponent } from './death-customer-form.component';

import { MatButtonModule } from '@angular/material/button';
import { MatRadioModule } from '@angular/material/radio';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MainNavigationModule } from 'src/app/components/main-navigation/main-navigation.module';


@NgModule({
  declarations: [
    DeathCustomerFormComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatRadioModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    ReactiveFormsModule,
    MainNavigationModule
  ]
})
export class DeathCustomerFormModule { }

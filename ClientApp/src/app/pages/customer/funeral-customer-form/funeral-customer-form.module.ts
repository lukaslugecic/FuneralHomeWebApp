import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FuneralCustomerFormComponent } from './funeral-customer-form.component';

import { MatButtonModule } from '@angular/material/button';
import { MatRadioModule } from '@angular/material/radio';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatStepperModule } from '@angular/material/stepper';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MainNavigationModule } from 'src/app/components/main-navigation/main-navigation.module';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatCheckboxModule } from '@angular/material/checkbox';

@NgModule({
  declarations: [
    FuneralCustomerFormComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatRadioModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    FormsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    ReactiveFormsModule,
    MainNavigationModule,
    MatStepperModule,
    MatListModule,
    MatExpansionModule,
    MatCheckboxModule
  ]
})
export class FuneralCustomerFormModule { }

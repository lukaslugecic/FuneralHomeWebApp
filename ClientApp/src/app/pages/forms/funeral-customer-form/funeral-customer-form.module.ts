import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FuneralCustomerFormComponent } from './funeral-customer-form.component';



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
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
    ReactiveFormsModule,
    MainNavigationModule
  ]
})
export class FuneralCustomerFormModule { }

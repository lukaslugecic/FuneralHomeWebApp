import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthNavigationComponent } from './auth-navigation.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [AuthNavigationComponent],
  imports: [
    CommonModule,
    MatToolbarModule,
    RouterModule,
    MatButtonModule
  ],
  exports: [AuthNavigationComponent],
})
export class AuthNavigationModule {}

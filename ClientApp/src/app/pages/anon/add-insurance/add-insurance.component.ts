import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AddEquipmentDialogComponent } from 'src/app/components/dialogs/add-equipment-dialog/add-equipment-dialog.component';
import { AddInsuranceDialogComponent } from 'src/app/components/dialogs/add-insurance-dialog/add-insurance-dialog.component';
import { AuthService } from 'src/app/services/auth/auth.service';
import { InsuranceService } from 'src/app/services/insurance/insurance.service';

@Component({
  selector: 'app-add-insurance',
  templateUrl: './add-insurance.component.html',
  styleUrls: ['./add-insurance.component.scss']
})
export class AddInsuranceComponent implements OnInit {
  anon = true;
  hasInsurance = false;
  constructor(
    private _router: Router,
    private _authService: AuthService,
    private _insuranceService: InsuranceService,
    private _dialog: MatDialog
    ) { }

  ngOnInit(): void {
    if(this._authService.isLoggedIn()){
      this.anon = false;
    }
    if(!this.anon){
      this._insuranceService.getInsurancesByUserId(this._authService.userValue?.id as number).subscribe({
        next: (res) => {
          if(res.length > 0){
            this.hasInsurance = true;
          }
        }
      })
    }

  }

  openAddForm() {
    if(!this.anon){
      if(this.hasInsurance){
        this._router.navigate(['/profile']);
      } else {
        const dialogRef = this._dialog.open(AddInsuranceDialogComponent);
      }
    } else {
      this._router.navigate(['/register']);
    }
  }
}

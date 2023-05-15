import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AddEquipmentDialogComponent } from 'src/app/components/dialogs/add-equipment-dialog/add-equipment-dialog.component';
import { AddInsuranceDialogComponent } from 'src/app/components/dialogs/add-insurance-dialog/add-insurance-dialog.component';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-add-insurance',
  templateUrl: './add-insurance.component.html',
  styleUrls: ['./add-insurance.component.scss']
})
export class AddInsuranceComponent implements OnInit {
  anon = true;
  constructor(
    private _router: Router,
    private _authService: AuthService,
    private _dialog: MatDialog
    ) { }

  ngOnInit(): void {
    if(this._authService.isLoggedIn()){
      this.anon = false;
    }
  }

  openAddForm() {
    if(!this.anon){
      const dialogRef = this._dialog.open(AddInsuranceDialogComponent);
    } else {
      this._router.navigate(['/register']);
    }
  }
}

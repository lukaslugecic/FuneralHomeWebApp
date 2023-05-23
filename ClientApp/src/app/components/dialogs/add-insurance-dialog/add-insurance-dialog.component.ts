import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DateAdapter } from '@angular/material/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { IInsuranceData } from 'src/app/interfaces/insurance-data';
import { ISmrtniSlucajData } from 'src/app/interfaces/smrtnislucaj-data';
import { AuthService } from 'src/app/services/auth/auth.service';
import { InsuranceService } from 'src/app/services/insurance/insurance.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-add-insurance-dialog',
  templateUrl: './add-insurance-dialog.component.html',
  styleUrls: ['./add-insurance-dialog.component.scss']
})
export class AddInsuranceDialogComponent implements OnInit {
  insuranceForm: FormGroup = new FormGroup({
    datumUgovaranja: new FormControl('', [Validators.required]),
    placanjeNaRate: new FormControl('', [Validators.required]),
    paketOsiguranjaId: new FormControl('', [Validators.required]),
    brojRata: new FormControl('',[
      Validators.required,
      Validators.min(1),
      Validators.max(36)]),
  });
  
  toAdd: IInsuranceData = {} as IInsuranceData;
  packages: any[] = [];

  types = [
    { value: true, naziv: 'Na rate' },
    { value: false, naziv: 'Jednokratno' },
  ];

  constructor(
    private _fb: FormBuilder,
    private _insuranceService: InsuranceService,
    private _authService: AuthService,
    private _dialogRef: MatDialogRef<AddInsuranceDialogComponent>,
    private readonly snackBar: MatSnackBar,
    private _router: Router
  ) {}

  ngOnInit() {
    this._insuranceService.getInsurancePackages().subscribe((res) => {
      this.packages = res;
    });
  }

  onFormSubmit() {
    this.insuranceForm.patchValue({
      datumUgovaranja: new Date(),
      korisnikId: this._authService.userValue?.id,
    });
    if (this.insuranceForm.valid) {
        this.toAdd = {
          Id: 0,
          Ime: this._authService.userValue?.ime as string,
          Prezime: this._authService.userValue?.prezime as string,
          DatumUgovaranja: new Date(),
          KorisnikId: this._authService.userValue?.id as number,
          PlacanjeNaRate: this.insuranceForm.value.brojRata !== 1,
          PaketOsiguranjaId: this.insuranceForm.value.paketOsiguranjaId,
          NazivPaketa: this.packages.find(x => x.id == this.insuranceForm.value.paketOsiguranjaId)?.naziv ?? "",
          BrojRata: this.insuranceForm.value.brojRata
        }
        this._insuranceService.addInsurance(this.toAdd).subscribe({
          next: (val: any) => {
            this.snackBar.open('Osiguranje uspješno ugovoreno!', 'U redu', {
              duration: 3000,
            });
            this._dialogRef.close(true);
            this._router.navigate(['/profile']);
          },
          error: (err: any) => {
            console.error(err);
            this.snackBar.open('Greška prilikom ugovaranja osiguranja!', 'Zatvori', {
              duration: 3000,
            });
          },
        });
    } else {
      if(this.insuranceForm.value.brojRata < 1 || this.insuranceForm.value.brojRata > 36){
        this.snackBar.open('Broj rata mora biti između 1 i 36!', 'U redu', {
          duration: 3000,
        });
      } else {
        this.snackBar.open('Popunite sva polja!', 'U redu', {
          duration: 3000,
        });
    }
    }
  }
}

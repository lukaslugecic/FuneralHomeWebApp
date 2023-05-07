import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { IInsuranceData } from 'src/app/interfaces/insurance-data';
import { ISmrtniSlucajData } from 'src/app/interfaces/smrtnislucaj-data';
import { InsuranceService } from 'src/app/services/insurance/insurance.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-insurance-dialog',
  templateUrl: './insurance-dialog.component.html',
  styleUrls: ['./insurance-dialog.component.scss']
})
export class InsuranceDialogComponent implements OnInit {
  insuranceForm: FormGroup = new FormGroup({
    datumUgovaranja: new FormControl('', [Validators.required]),
    korisnikId: new FormControl('', [Validators.required]),
    placanjeNaRate: new FormControl('', [Validators.required]),
  });
  
  toUpdate: IInsuranceData = {} as IInsuranceData;

  korisnici : Korisnik[] = [];

  types = [
    { value: true, naziv: 'Na rate' },
    { value: false, naziv: 'Jednokratno' },
  ];

  constructor(
    private _fb: FormBuilder,
    private _insuranceService: InsuranceService,
    private _userService: UserService,
    private _dialogRef: MatDialogRef<InsuranceDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    // dohvati sve korisnike i spremi njihov id i ime u polje
    this._userService.getAllUsersWithoutInsurance().subscribe((res) => {
      // iz svakog korisnika izvuci id i ime
      res.forEach((k) => {
          this.korisnici.push({
            korisnikId : k.id,
            imeIprezime : k.ime +  " " + k.prezime
          });
      });
    });
    if (this.data) {
      this.korisnici.push({
        korisnikId : this.data.korisnikId,
        imeIprezime : this.data.ime +  " " + this.data.prezime
      });
      this.insuranceForm.patchValue({
        ime: this.data.ime,
        prezime: this.data.prezime,
        datumUgovaranja: this.data.datumUgovaranja,
        korisnikId: this.data.korisnikId,
        placanjeNaRate: this.data.placanjeNaRate,
      });
    }
  }

  onFormSubmit() {
    if (this.insuranceForm.valid) {
      if (this.data) {
        this.toUpdate = {
          Id: this.data.id,
          Ime: this.korisnici.find(k => k.korisnikId == this.insuranceForm.value.korisnikId)?.imeIprezime.split(" ")[0] as string,
          Prezime: this.korisnici.find(k => k.korisnikId == this.insuranceForm.value.korisnikId)?.imeIprezime.split(" ")[1] as string,
          DatumUgovaranja: new Date(new Date(this.insuranceForm.value.datumUgovaranja).getTime() 
            - new Date(this.insuranceForm.value.datumUgovaranja).getTimezoneOffset() * 60000),
          KorisnikId: this.insuranceForm.value.korisnikId,
          PlacanjeNaRate: this.insuranceForm.value.placanjeNaRate,
        }
        console.log(this.toUpdate);
        this._insuranceService
          .updateInsurance(this.data.id, this.toUpdate)
          .subscribe({
            next: (val: any) => {
              this.snackBar.open('Osiguranje uspješno uređeno!', 'U redu', {
                duration: 3000,
              });
              this._dialogRef.close(true);
            },
            error: (err: any) => {
              console.error(err);
              this.snackBar.open('Greška prilikom uređivanja osiguranja!', 'Zatvori', {
                duration: 3000,
              });
            },
          });
      } else {
        this.toUpdate = {
          Id: 0,
          Ime: this.korisnici.find(k => k.korisnikId == this.insuranceForm.value.korisnikId)?.imeIprezime.split(" ")[0] as string,
          Prezime: this.korisnici.find(k => k.korisnikId == this.insuranceForm.value.korisnikId)?.imeIprezime.split(" ")[1] as string,
          DatumUgovaranja: new Date(new Date(this.insuranceForm.value.datumUgovaranja).getTime() 
            - new Date(this.insuranceForm.value.datumUgovaranja).getTimezoneOffset() * 60000),
          KorisnikId: this.insuranceForm.value.korisnikId,
          PlacanjeNaRate: this.insuranceForm.value.placanjeNaRate,
        }
        this._insuranceService.addInsurance(this.toUpdate).subscribe({
          next: (val: any) => {
            this.snackBar.open('Osiguranje uspješno dodano!', 'U redu', {
              duration: 3000,
            });
            this._dialogRef.close(true);
          },
          error: (err: any) => {
            console.error(err);
            this.snackBar.open('Greška prilikom dodavanja osiguranja!', 'Zatvori', {
              duration: 3000,
            });
          },
        });
        console.log(this.toUpdate);
      }
    } else {
      this.snackBar.open('Popunite sva polja!', 'U redu', {
        duration: 3000,
      });
    }
  }
}

type Korisnik = {
  korisnikId: number;
  imeIprezime: string;
}
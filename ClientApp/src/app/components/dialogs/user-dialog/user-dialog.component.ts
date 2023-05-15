import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { IRegisterData } from 'src/app/interfaces/register-data';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-user-dialog',
  templateUrl: './user-dialog.component.html',
  styleUrls: ['./user-dialog.component.scss']
})
export class UserDialogComponent implements OnInit {
  hide = true;
  admin = false;
  userForm: FormGroup = new FormGroup({
    ime: new FormControl('', [Validators.required]),
    prezime: new FormControl('', [Validators.required]),
    oib: new FormControl('', [
      Validators.required,
      Validators.pattern('^[0-9]{11}$'),
    ]),
    datumRodenja: new FormControl('', [Validators.required]),
    adresa: new FormControl('', [Validators.required]),
    mail: new FormControl('', [Validators.required]),
    lozinka: new FormControl('', [Validators.required]),
    ponovljenaLozinka: new FormControl('', [Validators.required]),
    vrstaKorisnika: new FormControl('', [Validators.required])
  });
  
  toUpdate: IRegisterData = {} as IRegisterData;

  types = [
    { value: 'A', viewValue: 'Administrator' },
    { value: 'Z', viewValue: 'Zaposlenik' },
    { value: 'K', viewValue: 'Klijent' },
  ];

  constructor(
    private _fb: FormBuilder,
    private _userService: UserService,
    private _dialogRef: MatDialogRef<UserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private readonly snackBar: MatSnackBar
  ) {
    if(this.data.admin){
      this.admin = true;
    }
  }

  ngOnInit() {
      if (this.data.id) {
        if(this.data.admin){
          this.userForm.patchValue({
            ime: this.data.ime,
            prezime: this.data.prezime,
            oib: this.data.oib,
            datumRodenja: this.data.datumRodenja,
            adresa: this.data.adresa,
            mail: this.data.mail,
            lozinka: "",
            vrstaKorisnika: this.data.vrstaKorisnika,
          });
        } else {
          this.userForm.patchValue({
            ime: this.data.ime,
            prezime: this.data.prezime,
            oib: this.data.oib,
            datumRodenja: this.data.datumRodenja,
            adresa: this.data.adresa,
            mail: this.data.mail,
            lozinka: "",
            vrstaKorisnika: 'K',
          });
        }
        
      } 
  }

  onFormSubmit() {
    if (this.userForm.valid) {
      if (
        this.userForm.value.lozinka !==
        this.userForm.value.ponovljenaLozinka
      ) {
        this.snackBar.open('Lozinke se moraju podudarati', 'Zatvori', {
          duration: 3000,
        });
        return;
      }
      if (this.data.id) {
        this.toUpdate = {
          Id: this.data.id,
          Ime: this.userForm.value.ime,
          Prezime: this.userForm.value.prezime,
          Oib: this.userForm.value.oib,
          DatumRodenja: this.userForm.value.datumRodenja,
          Adresa: this.userForm.value.adresa,
          Mail: this.userForm.value.mail,
          Lozinka : this.userForm.value.lozinka,
          VrstaKorisnika: this.data.admin ? this.userForm.value.vrstaKorisnika : 'K',
        }
        this._userService
          .updateUser(this.data.id, this.toUpdate)
          .subscribe({
            next: (val: any) => {
              this.snackBar.open('Korisnik uspješno uređen!', 'U redu', {
                duration: 3000,
              });
              this._dialogRef.close(true);
            },
            error: (err: any) => {
              console.error(err);
              this.snackBar.open('Greška prilikom uređivanja korisnika!', 'Zatvori', {
                duration: 3000,
              });
            },
          });
      } else {
        this.toUpdate = {
          Id: 0,
          Ime: this.userForm.value.ime,
          Prezime: this.userForm.value.prezime,
          Oib: this.userForm.value.oib,
          DatumRodenja: this.userForm.value.datumRodenja,
          Adresa: this.userForm.value.adresa,
          Mail: this.userForm.value.mail,
          Lozinka : this.userForm.value.lozinka,
          VrstaKorisnika: this.data.admin ? this.userForm.value.vrstaKorisnika : 'K',
        }
        this._userService.addUser(this.toUpdate).subscribe({
          next: (val: any) => {
            this.snackBar.open('Korisnik uspješno dodan!', 'U redu', {
              duration: 3000,
            });
            this._dialogRef.close(true);
          },
          error: (err: any) => {
            console.error(err);
            this.snackBar.open('Greška prilikom dodavanja korisnika!', 'Zatvori', {
              duration: 3000,
            });
          },
        });
      }
    } else {
      this.snackBar.open('Popunite sva polja!', 'U redu', {
        duration: 3000,
      });
    }
  }
}

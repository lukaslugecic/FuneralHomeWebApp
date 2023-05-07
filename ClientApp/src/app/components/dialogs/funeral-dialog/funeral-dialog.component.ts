import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ISmrtniSlucajData } from 'src/app/interfaces/smrtnislucaj-data';
import { DeathService } from 'src/app/services/death/death.service';
import { UserService } from 'src/app/services/user/user.service';
import { DeathDialogComponent } from '../death-dialog/death-dialog.component';
import { IPogrebData } from 'src/app/interfaces/pogreb-data';
import { FuneralService } from 'src/app/services/funeral/funeral.service';
import { IPogrebSmrtniSlucajData } from 'src/app/interfaces/pogreb-smrtnislucaj-data';
import { DateAdapter } from '@angular/material/core';

@Component({
  selector: 'app-funeral-dialog',
  templateUrl: './funeral-dialog.component.html',
  styleUrls: ['./funeral-dialog.component.scss']
})
export class FuneralDialogComponent implements OnInit {
  show = false;

  types = [
    { value: true, naziv: 'Kremiranje' },
    { value: false, naziv: 'Ukop' },
  ];

  funeralForm: FormGroup = new FormGroup({
    datumPogreba: new FormControl('', [Validators.required]),
    kremacija: new FormControl('', [Validators.required]),
    smrtniSlucajId: new FormControl('', [Validators.required]),
    korisnikId: new FormControl(''),
    ukupnaCijena: new FormControl('', [Validators.required])
  });
  
  toUpdate: IPogrebSmrtniSlucajData = {} as IPogrebSmrtniSlucajData;

  korisnici : Korisnik[] = [];
  slucajevi : SmrtniSlucaj[] = [];

  constructor(
    private _funeralService: FuneralService,
    private _deathService: DeathService,
    private _userService: UserService,
    private _dialog: MatDialog,
    private _dialogRef: MatDialogRef<FuneralDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private readonly snackBar: MatSnackBar,
    private dateAdapter: DateAdapter<Date>
  ) {
    this.dateAdapter.setLocale('hr');
  }

  ngOnInit() {
    // dohvati sve korisnike i spremi njihov id i ime u polje
    this._userService.getAllUsers().subscribe((res) => {
      // iz svakog korisnika izvuci id i ime
      res.forEach((k) => {
        if(k.vrstaKorisnika === 'K'){
          this.korisnici.push( {
            korisnikId : k.id,
            imeIprezime : k.ime +  " " + k.prezime
          });
        }
      });
    });

    this._deathService.getAllDeathsWithoutFuneral().subscribe((res) => {
      // iz svakog korisnika izvuci id i ime
      res.forEach((d) => {
        // ako id vec postoji u polju, ne dodavaj ga
        if(!this.slucajevi.some(s => s.smrtniSlucajId === d.id)){
          this.slucajevi.push( {
            smrtniSlucajId : d.id,
            imeIprezime : d.imePok +  " " + d.prezimePok,
            datumSmrti: d.datumSmrtiPok
          });
        }
      });
    });

    

    if (this.data) {
      this.funeralForm.patchValue({
        datumPogreba: this.data.datumPogreba,
        kremacija: this.data.kremacija,
        smrtniSlucajId: this.data.smrtniSlucajId,
        korisnikId: this.data.korisnikId ?? this.data.korisnik.id,
        ukupnaCijena: this.data.ukupnaCijena
      });
      this.slucajevi.push( {
        smrtniSlucajId : this.data.smrtniSlucajId,
        // dohvatiti iz this.data ili ako ne postoji iz this.data.smrtniSlucaj
        imeIprezime : this.data.ime
          ? this.data.ime + " " + this.data.prezime
          : this.data.smrtniSlucaj.imePok + " " + this.data.smrtniSlucaj.prezimePok, 
        datumSmrti : this.data.datumSmrti ?? this.data.smrtniSlucaj.datumSmrtiPok
      });
    }

    console.log(this.slucajevi);

  }

  onFormSubmit() {
    if (this.funeralForm.valid) {
      // provjeri je li datum pogreba nakon datuma smrti
      if(new Date(this.funeralForm.value.datumPogreba) < new Date(this.slucajevi.find(s => s.smrtniSlucajId === this.funeralForm.value.smrtniSlucajId)!.datumSmrti)){
        this.snackBar.open('Datum pogreba ne može biti prije datuma smrti!', 'U redu', {
          duration: 3000,
        });
        return;
      }
      if (this.data) {
        this.toUpdate = {
          Id: this.data.id,
          DatumPogreba: new Date(new Date(this.funeralForm.value.datumPogreba).getTime() 
            - new Date(this.funeralForm.value.datumPogreba).getTimezoneOffset() * 60000),
          Kremacija: this.funeralForm.value.kremacija,
          SmrtniSlucajId: this.funeralForm.value.smrtniSlucajId,
          ImePok: this.slucajevi.find(s => s.smrtniSlucajId === this.funeralForm.value.smrtniSlucajId)?.imeIprezime?.split(" ")[0] || '',
          PrezimePok: this.slucajevi.find(s => s.smrtniSlucajId === this.funeralForm.value.smrtniSlucajId)?.imeIprezime?.split(" ")[1] || '',
          KorisnikId: this.funeralForm.value.korisnikId,
          Ime: this.korisnici.find(k => k.korisnikId === this.funeralForm.value.korisnikId)?.imeIprezime?.split(" ")[0] || '',
          Prezime: this.korisnici.find(k => k.korisnikId === this.funeralForm.value.korisnikId)?.imeIprezime?.split(" ")[1] || '',
          UkupnaCijena: this.funeralForm.value.ukupnaCijena
        }
        this._funeralService
          .upadateFuneralDeath(this.data.id, this.toUpdate)
          .subscribe({
            next: (val: any) => {
              this.snackBar.open('Pogreb uspješno uređen!', 'U redu', {
                duration: 3000,
              });
              this._dialogRef.close(true);
            },
            error: (err: any) => {
              console.error(err);
              this.snackBar.open('Greška prilikom uređivanja pogreba!', 'Zatvori', {
                duration: 3000,
              });
            },
          });
      } else {
        this.toUpdate = {
          Id: 0,
          DatumPogreba: new Date(new Date(this.funeralForm.value.datumPogreba).getTime() 
            - new Date(this.funeralForm.value.datumPogreba).getTimezoneOffset() * 60000),
          Kremacija: this.funeralForm.value.kremacija,
          SmrtniSlucajId: this.funeralForm.value.smrtniSlucajId,
          ImePok: this.slucajevi.find(s => s.smrtniSlucajId === this.funeralForm.value.smrtniSlucajId)?.imeIprezime?.split(" ")[0] || '',
          PrezimePok: this.slucajevi.find(s => s.smrtniSlucajId === this.funeralForm.value.smrtniSlucajId)?.imeIprezime?.split(" ")[1] || '',
          KorisnikId: 0,
          Ime: '',	
          Prezime: '',
          UkupnaCijena: this.funeralForm.value.ukupnaCijena
        }
        this._funeralService.addFuneral(this.toUpdate).subscribe({
          next: (val: any) => {
            this.snackBar.open('Pogreb uspješno dodan!', 'U redu', {
              duration: 3000,
            });
            this._dialogRef.close(true);
          },
          error: (err: any) => {
            console.error(err);
            this.snackBar.open('Greška prilikom dodavanja pogreba!', 'Zatvori', {
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

  openAddDeathForm() {
    // nakon zatvranja vrati se na ovu komponentu
    const dialogRef = this._dialog.open(DeathDialogComponent);
    dialogRef.afterClosed().subscribe((res) => {
      if (res) {
        this._deathService.getAllDeathsWithoutFuneral().subscribe((res) => {
          // iz svakog korisnika izvuci id i ime
          res.forEach((d) => {
            // ako id vec postoji u polju, ne dodavaj ga
            if(!this.slucajevi.some(s => s.smrtniSlucajId === d.id)){
              this.slucajevi.push( {
                smrtniSlucajId : d.id,
                imeIprezime : d.imePok +  " " + d.prezimePok,
                datumSmrti: d.datumSmrti});
            }
          });
        });
      }
    });
  }

}

type Korisnik = {
  korisnikId: number;
  imeIprezime: string;
}

type SmrtniSlucaj = {
  smrtniSlucajId: number;
  imeIprezime: string;
  datumSmrti: Date;
}
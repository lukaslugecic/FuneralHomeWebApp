import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { IOpremaUslugaData } from 'src/app/interfaces/oprema-usluga-data';
import { IVrstaOpremeUslugeData } from 'src/app/interfaces/vrsta-opreme-usluge-data';
import { EquipmentService } from 'src/app/services/equipment/equipment.service';

@Component({
  selector: 'app-service-dialog',
  templateUrl: './service-dialog.component.html',
  styleUrls: ['./service-dialog.component.scss']
})
export class ServiceDialogComponent implements OnInit {
  
  serviceForm: FormGroup = new FormGroup({
    naziv: new FormControl('', [Validators.required]),
    opis: new FormControl('', [Validators.required]),
    cijena: new FormControl('', [Validators.required]),
    vrstaOpremeUsluge: new FormControl('', [Validators.required])
  });
  
  toUpdate: IOpremaUslugaData = {} as IOpremaUslugaData;

  types: IVrstaOpremeUslugeData[] = [];

  constructor(
    private readonly _equipmentService: EquipmentService,
    private _dialogRef: MatDialogRef<ServiceDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    // dohvatiti vrste usluga i spremiti u this.types zatim popuniti formu sa podacima ako je data != null
    this._equipmentService.getTypesOfServices().subscribe((data: any) => {
      this.types = data;
      if (this.data) {
        this.serviceForm.patchValue({
          naziv: this.data.naziv,
          opis: this.data.opis,
          cijena: this.data.cijena,
          vrstaOpremeUsluge: this.data.vrstaOpremeUslugeId,
        });
      }
    });
  }

  onFormSubmit() {
    if (this.serviceForm.valid) {
      if (this.data) {
        this.toUpdate = {
          Id: this.data.id,
          Naziv: this.serviceForm.value.naziv,
          Opis: this.serviceForm.value.opis,
          Cijena: this.serviceForm.value.cijena,
          VrstaOpremeUslugeId: this.serviceForm.value.vrstaOpremeUsluge,
          VrstaOpremeUslugeNaziv: this.types.find(x => x.id == this.serviceForm.value.vrstaOpremeUsluge)?.naziv ?? "",
          JedinicaMjereNaziv: this.types.find(x => x.id == this.serviceForm.value.vrstaOpremeUsluge)?.jedinicaMjereNaziv ?? "",
          Slika: null,
          Zaliha: null
        }
        console.log(this.toUpdate);
        this._equipmentService
          .updateEquipment(this.data.id, this.toUpdate)
          .subscribe({
            next: (val: any) => {
              this.snackBar.open('Usluga uspješno uređena!', 'U redu', {
                duration: 3000,
              });
              this._dialogRef.close(true);
            },
            error: (err: any) => {
              console.error(err);
              this.snackBar.open('Greška prilikom uređivanja usluge!', 'Zatvori', {
                duration: 3000,
              });
            },
          });
      } else {
        this.toUpdate = {
          Id: 0,
          Naziv: this.serviceForm.value.naziv,
          Opis: this.serviceForm.value.opis,
          Cijena: this.serviceForm.value.cijena,
          VrstaOpremeUslugeId: this.serviceForm.value.vrstaOpremeUsluge,
          VrstaOpremeUslugeNaziv: this.types.find(x => x.id == this.serviceForm.value.vrstaOpremeUsluge)?.naziv ?? "",
          JedinicaMjereNaziv: this.types.find(x => x.id == this.serviceForm.value.vrstaOpremeUsluge)?.jedinicaMjereNaziv ?? "",
          Slika: null,
          Zaliha: null
        }
        this._equipmentService.addEquipment(this.toUpdate).subscribe({
          next: (val: any) => {
            this.snackBar.open('Usluga uspješno dodana!', 'U redu', {
              duration: 3000,
            });
            this._dialogRef.close(true);
          },
          error: (err: any) => {
            console.error(err);
            this.snackBar.open('Greška prilikom dodavanja usluge!', 'Zatvori', {
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

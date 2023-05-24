import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { IJedinicaMjereData } from 'src/app/interfaces/unit-data';
import { IVrstaOpremeUslugeData } from 'src/app/interfaces/vrsta-opreme-usluge-data';
import { EquipmentService } from 'src/app/services/equipment/equipment.service';

@Component({
  selector: 'app-type-dialog',
  templateUrl: './type-dialog.component.html',
  styleUrls: ['./type-dialog.component.scss']
})
export class TypeDialogComponent implements OnInit {

  typeForm: FormGroup = new FormGroup({
    naziv: new FormControl('', [Validators.required]),
    jedinicaMjereId: new FormControl('', [Validators.required]),
  });
  
  jeOprema = false;

  toUpdate: IVrstaOpremeUslugeData = {} as IVrstaOpremeUslugeData;

  types: IJedinicaMjereData[] = [];

  constructor(
    private readonly _equipmentService: EquipmentService,
    private _dialogRef: MatDialogRef<TypeDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    // dohvatiti vrste usluga i spremiti u this.types zatim popuniti formu sa podacima ako je data != null
    this._equipmentService.getUnits().subscribe((data: any) => {
      this.types = data;
      this.types.push({ id: 0, naziv: 'Bez mjere' })
      if (this.data) {
        this.typeForm.patchValue({
          naziv: this.data.naziv,
          jedinicaMjereId: this.data.jedinicaMjereId,
        });
      }
    });

    this.jeOprema = this.data.jeOprema;
  }

  onFormSubmit() {
    if(this.jeOprema){
      this.typeForm.patchValue({
        jedinicaMjereId: 1 // količina
      });
    }
    if (this.typeForm.valid) {
      if (this.data.id) {
        this.toUpdate = {
          id: this.data.id,
          naziv: this.typeForm.value.naziv,
          jedinicaMjereId: this.typeForm.value.jedinicaMjereId === 0 ? null : this.typeForm.value.jedinicaMjereId,
          jedinicaMjereNaziv: this.types.find(x => x.id == this.typeForm.value.jedinicaMjereId)?.naziv ?? "",
          jeOprema: this.data.jeOprema
        }
        console.log(this.toUpdate);
        this._equipmentService
          .updateTypeOfEquipment(this.data.id, this.toUpdate)
          .subscribe({
            next: (val: any) => {
              this.snackBar.open('Vrsta usluge uspješno uređena!', 'U redu', {
                duration: 3000,
              });
              this._dialogRef.close(true);
            },
            error: (err: any) => {
              console.error(err);
              this.snackBar.open('Greška prilikom uređivanja vrste usluge!', 'Zatvori', {
                duration: 3000,
              });
            },
          });
      } else {
        this.toUpdate = {
          id: 0,
          naziv: this.typeForm.value.naziv,
          jedinicaMjereId: this.typeForm.value.jedinicaMjereId,
          jedinicaMjereNaziv: this.types.find(x => x.id == this.typeForm.value.jedinicaMjereId)?.naziv ?? "",
          jeOprema: this.data.jeOprema
        }
        this._equipmentService.addTypeOfEquipment(this.toUpdate).subscribe({
          next: (val: any) => {
            this.snackBar.open('Vrsta usluge uspješno dodana!', 'U redu', {
              duration: 3000,
            });
            this._dialogRef.close(true);
          },
          error: (err: any) => {
            console.error(err);
            this.snackBar.open('Greška prilikom dodavanja vrste usluge!', 'Zatvori', {
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

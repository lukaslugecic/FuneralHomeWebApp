import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { IOpremaUslugaData } from 'src/app/interfaces/oprema-usluga-data';
import { IVrstaOpremeUslugeData } from 'src/app/interfaces/vrsta-opreme-usluge-data';
import { EquipmentService } from 'src/app/services/equipment/equipment.service';

@Component({
  selector: 'app-equipment-dialog',
  templateUrl: './equipment-dialog.component.html',
  styleUrls: ['./equipment-dialog.component.scss']
})
export class EquipmentDialogComponent implements OnInit {
  
  fileName = '';

  equipmentForm: FormGroup = new FormGroup({
    naziv: new FormControl('', [Validators.required]),
    zaliha: new FormControl('', [Validators.required, Validators.min(0)]),
    cijena: new FormControl('', [Validators.required, Validators.min(0)]),
    vrstaOpremeUsluge: new FormControl('', [Validators.required]),
    slika: new FormControl('')
  });
  
  toUpdate: IOpremaUslugaData = {} as IOpremaUslugaData;

  types: IVrstaOpremeUslugeData[] = [];

  constructor(
    private readonly _equipmentService: EquipmentService,
    private _dialogRef: MatDialogRef<EquipmentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    // dohvatiti vrste usluga i spremiti u this.types zatim popuniti formu sa podacima ako je data != null
    this._equipmentService.getTypesOfEquipment().subscribe((data: any) => {
      this.types = data;
      if (this.data) {
        this.equipmentForm.patchValue({
          naziv: this.data.naziv,
          zaliha: this.data.zaliha,
          cijena: this.data.cijena,
          vrstaOpremeUsluge: this.data.vrstaOpremeUslugeId,
          slika: this.data.slika
        });
      }
    });
  }

  onFormSubmit() {
    if (this.equipmentForm.valid) {
      if (this.data) {
        this.toUpdate = {
          Id: this.data.id,
          Naziv: this.equipmentForm.value.naziv,
          Cijena: this.equipmentForm.value.cijena,
          Zaliha: this.equipmentForm.value.zaliha,
          Slika: this.equipmentForm.value.slika,
          VrstaOpremeUslugeId: this.equipmentForm.value.vrstaOpremeUsluge,
          VrstaOpremeUslugeNaziv: this.types.find(x => x.id == this.equipmentForm.value.vrstaOpremeUsluge)?.naziv ?? "",
          Opis: null
        }
        this._equipmentService
          .updateEquipment(this.data.id, this.toUpdate)
          .subscribe({
            next: (val: any) => {
              this.snackBar.open('Oprema uspješno uređena!', 'U redu', {
                duration: 3000,
              });
              this._dialogRef.close(true);
            },
            error: (err: any) => {
              console.error(err);
              this.snackBar.open('Greška prilikom uređivanja opreme!', 'Zatvori', {
                duration: 3000,
              });
            },
          });
      } else {
        this.toUpdate = {
          Id: 0,
          Naziv: this.equipmentForm.value.naziv,
          Cijena: this.equipmentForm.value.cijena,
          Zaliha: this.equipmentForm.value.zaliha,
          Slika: this.equipmentForm.value.slika,
          VrstaOpremeUslugeId: this.equipmentForm.value.vrstaOpremeUsluge,
          VrstaOpremeUslugeNaziv: this.types.find(x => x.id == this.equipmentForm.value.vrstaOpremeUsluge)?.naziv ?? "",
          Opis: null
        }
        this._equipmentService.addEquipment(this.toUpdate).subscribe({
          next: (val: any) => {
            this.snackBar.open('Oprema uspješno dodana!', 'U redu', {
              duration: 3000,
            });
            this._dialogRef.close(true);
          },
          error: (err: any) => {
            console.error(err);
            this.snackBar.open('Greška prilikom dodavanja opreme!', 'Zatvori', {
              duration: 3000,
            });
          },
        });
      }
    } else {
      if(this.equipmentForm.value.cijena < 0){
        this.snackBar.open('Cijena ne može biti manja od 0!', 'U redu', {
          duration: 3000,
        });
      } else if(this.equipmentForm.value.zalihaOpreme < 0){
        this.snackBar.open('Zaliha ne može biti manja od 0!', 'U redu', {
          duration: 3000,
        });
      } else {
        this.snackBar.open('Popunite sva polja!', 'U redu', {
          duration: 3000,
        });
    }
    }
  }


  onFileSelected(event : any) {
    const file = event.target.files[0];
    if (file) {
      this.fileName = file.name;
    }
    // pretvori file u BLOB u hex formatu
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => {
      this.equipmentForm.patchValue({
        slika: reader.result?.toString().split(',')[1]
      });
    }
    
}


}

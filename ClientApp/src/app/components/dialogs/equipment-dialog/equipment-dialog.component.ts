import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { IOpremaData } from 'src/app/interfaces/oprema-data';
import { IUslugaData } from 'src/app/interfaces/usluga-data';
import { IVrstaOpremeData } from 'src/app/interfaces/vrsta-opreme-data';
import { IVrstaUslugeData } from 'src/app/interfaces/vrsta-usluge-data';
import { EmployeeService } from 'src/app/services/employee/employee.service';

@Component({
  selector: 'app-equipment-dialog',
  templateUrl: './equipment-dialog.component.html',
  styleUrls: ['./equipment-dialog.component.scss']
})
export class EquipmentDialogComponent implements OnInit {
  
  fileName = '';

  equipmentForm: FormGroup = new FormGroup({
    naziv: new FormControl('', [Validators.required]),
    zalihaOpreme: new FormControl('', [Validators.required]),
    cijena: new FormControl('', [Validators.required]),
    vrstaOpreme: new FormControl('', [Validators.required]),
    slika: new FormControl('')
  });
  
  toUpdate: IOpremaData = {} as IOpremaData;

  types: IVrstaOpremeData[] = [];

  constructor(
    private _employeeService: EmployeeService,
    private _dialogRef: MatDialogRef<EquipmentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    // dohvatiti vrste usluga i spremiti u this.types zatim popuniti formu sa podacima ako je data != null
    this._employeeService.getTypesOfEquipment().subscribe((data: any) => {
      this.types = data;
      if (this.data) {
        this.equipmentForm.patchValue({
          naziv: this.data.naziv,
          zalihaOpreme: this.data.zalihaOpreme,
          cijena: this.data.cijena,
          vrstaOpreme: this.data.vrstaOpremeId,
          slika: this.data.slika
        });
      }
    });
  }

  onFormSubmit() {
    console.log(this.equipmentForm.value);
    if (this.equipmentForm.valid) {
      if (this.data) {
        this.toUpdate = {
          Id: this.data.id,
          Naziv: this.equipmentForm.value.naziv,
          Cijena: this.equipmentForm.value.cijena,
          ZalihaOpreme: this.equipmentForm.value.zalihaOpreme,
          Slika: this.equipmentForm.value.slika,
          VrstaOpremeId: this.equipmentForm.value.vrstaOpreme,
          VrstaOpremeNaziv: this.types.find(x => x.id == this.equipmentForm.value.vrstaOpreme)?.naziv ?? "",
        }
        console.log(this.toUpdate);
        this._employeeService
          .updateEquipment(this.data.id, this.toUpdate)
          .subscribe({
            next: (val: any) => {
              this.snackBar.open('Oprema uspješno ažurirana!', 'U redu', {
                duration: 3000,
              });
              this._dialogRef.close(true);
            },
            error: (err: any) => {
              console.error(err);
              this.snackBar.open('Greška prilikom ažuriranja opreme!', 'Zatvori', {
                duration: 3000,
              });
            },
          });
      } else {
        this.toUpdate = {
          Id: 0,
          Naziv: this.equipmentForm.value.naziv,
          Cijena: this.equipmentForm.value.cijena,
          ZalihaOpreme: this.equipmentForm.value.zalihaOpreme,
          Slika: this.equipmentForm.value.slika,
          VrstaOpremeId: this.equipmentForm.value.vrstaOpreme,
          VrstaOpremeNaziv: this.types.find(x => x.id == this.equipmentForm.value.vrstaOpreme)?.naziv ?? "",
        }
        this._employeeService.addEquipment(this.toUpdate).subscribe({
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
        console.log(this.toUpdate);
      }
    } else {
      this.snackBar.open('Popunite sva polja!', 'U redu', {
        duration: 3000,
      });
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
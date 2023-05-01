import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject, Observable, switchMap, tap } from 'rxjs';
import { IUslugaData } from 'src/app/interfaces/usluga-data';
import { IVrstaUslugeData } from 'src/app/interfaces/vrsta-usluge-data';
import { EmployeeService } from 'src/app/services/employee/employee.service';

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
    vrstaUsluge: new FormControl('', [Validators.required])
  });
  
  toUpdate: IUslugaData = {} as IUslugaData;

  types: IVrstaUslugeData[] = [];

  constructor(
    private _fb: FormBuilder,
    private _employeeService: EmployeeService,
    private _dialogRef: MatDialogRef<ServiceDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    // dohvatiti vrste usluga i spremiti u this.types zatim popuniti formu sa podacima ako je data != null
    this._employeeService.getTypesOfServices().subscribe((data: any) => {
      this.types = data;
      if (this.data) {
        this.serviceForm.patchValue({
          naziv: this.data.naziv,
          opis: this.data.opis,
          cijena: this.data.cijena,
          vrstaUsluge: this.data.vrstaUslugeId,
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
          VrstaUslugeId: this.serviceForm.value.vrstaUsluge,
          VrstaUslugeNaziv: this.types.find(x => x.id == this.serviceForm.value.vrstaUsluge)?.naziv ?? "",
        }
        console.log(this.toUpdate);
        this._employeeService
          .updateService(this.data.id, this.toUpdate)
          .subscribe({
            next: (val: any) => {
              this.snackBar.open('Usluga uspješno ažurirana!', 'U redu', {
                duration: 3000,
              });
              this._dialogRef.close(true);
            },
            error: (err: any) => {
              console.error(err);
              this.snackBar.open('Greška prilikom ažuriranja usluge!', 'Zatvori', {
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
          VrstaUslugeId: this.serviceForm.value.vrstaUsluge,
          VrstaUslugeNaziv: this.types.find(x => x.id == this.serviceForm.value.vrstaUsluge)?.naziv ?? "",
        }
        this._employeeService.addService(this.toUpdate).subscribe({
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

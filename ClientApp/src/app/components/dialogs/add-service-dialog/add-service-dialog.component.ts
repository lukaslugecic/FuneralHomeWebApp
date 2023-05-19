import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { forkJoin } from 'rxjs';
import { IOpremaData } from 'src/app/interfaces/oprema-usluga-data';
import { IPogrebOpremaData } from 'src/app/interfaces/pogreb-oprema-data';
import { IUslugaData } from 'src/app/interfaces/usluga-data';
import { IVrstaOpremeUslugeData } from 'src/app/interfaces/vrsta-opreme-usluge-data';
import { EquipmentService } from 'src/app/services/equipment/equipment.service';
import { FuneralService } from 'src/app/services/funeral/funeral.service';

@Component({
  selector: 'app-add-service-dialog',
  templateUrl: './add-service-dialog.component.html',
  styleUrls: ['./add-service-dialog.component.scss']
})
export class AddServiceDialogComponent implements OnInit {
  
  serviceForm: FormGroup = new FormGroup({
    uslugaId: new FormControl('', [Validators.required]),
    vrstaUsluge: new FormControl('', [Validators.required])
  });
  
  toAdd: IUslugaData = {} as IUslugaData;

  types: IVrstaOpremeUslugeData[] = [];
  services: any[] = [];
  servicesToShow: Usluga[] = [];
  filteredServicesToShow: Usluga[] = [];


  constructor(
    private readonly _equipmentService: EquipmentService,
    private readonly _funeralService: FuneralService,
    private _dialogRef: MatDialogRef<AddServiceDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    forkJoin([
      this._equipmentService.getTypesOfServices(),
      this._equipmentService.getAllServices(),
    ]).subscribe(([types, services]) => {
      this.types = types;
      this.services = services;
      services.forEach((s: any) => {
        this.servicesToShow.push({
          id: s.id,
          vrstaUsluge: s.vrstaUslugeId,
          naziv: "Naziv: " + s.naziv + ", Cijena: " + s.cijena + "€",
        });
      });
    });
    this.serviceForm.get('vrstaUsluge')?.valueChanges.subscribe(value => {
      this.filteredServicesToShow = this.servicesToShow.filter(e => e.vrstaUsluge === value);
    });


  }

  onFormSubmit() {
    if (this.serviceForm.valid) {
        this.toAdd = {
            Id: this.serviceForm.value.uslugaId,
            Naziv: this.services.find((e: any) => e.id === this.serviceForm.value.uslugaId)?.naziv,
            VrstaUslugeId: this.serviceForm.value.vrstaUsluge,
            VrstaUslugeNaziv: this.types.find((t: any) => t.id === this.serviceForm.value.vrstaUsluge)?.naziv as string,
            Opis: this.services.find((e: any) => e.id === this.serviceForm.value.uslugaId)?.opis,
            Cijena: this.services.find((e: any) => e.id === this.serviceForm.value.uslugaId)?.cijena,
        }
        this._funeralService.addService(this.data, this.toAdd).subscribe({
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
    } else {
      this.snackBar.open('Popunite sva polja!', 'U redu', {
        duration: 3000,
      });
    }
  }
}

type Usluga = {
  id: number;
  naziv: string;
  vrstaUsluge: string;
};
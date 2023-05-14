import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { forkJoin } from 'rxjs';
import { IOpremaData } from 'src/app/interfaces/oprema-data';
import { IPogrebOpremaData } from 'src/app/interfaces/pogreb-oprema-data';
import { IVrstaOpremeData } from 'src/app/interfaces/vrsta-opreme-data';
import { EquipmentService } from 'src/app/services/equipment/equipment.service';
import { FuneralService } from 'src/app/services/funeral/funeral.service';

@Component({
  selector: 'app-add-equipment-dialog',
  templateUrl: './add-equipment-dialog.component.html',
  styleUrls: ['./add-equipment-dialog.component.scss']
})
export class AddEquipmentDialogComponent implements OnInit {
  
  equipmentForm: FormGroup = new FormGroup({
    opremaId: new FormControl('', [Validators.required]),
    vrstaOpreme: new FormControl('', [Validators.required]),
    kolicina : new FormControl('', [Validators.required, Validators.min(1)]),
  });
  
  toAdd: IPogrebOpremaData = {} as IPogrebOpremaData;

  types: IVrstaOpremeData[] = [];
  equipment: any[] = [];
  equipmentToShow: Oprema[] = [];
  filteredEquipmentToShow: Oprema[] = [];


  constructor(
    private readonly _equipmentService: EquipmentService,
    private readonly _funeralService: FuneralService,
    private _dialogRef: MatDialogRef<AddEquipmentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private readonly snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    forkJoin([
      this._equipmentService.getTypesOfEquipment(),
      this._equipmentService.getAllEquipment(),
    ]).subscribe(([types, equipment]) => {
      this.types = types;
      this.equipment = equipment;
      equipment.forEach((e: any) => {
        this.equipmentToShow.push({
          id: e.id,
          vrstaOpreme: e.vrstaOpremeId,
          naziv: "Naziv: " + e.naziv + ", Cijena: " + e.cijena + "€",
        });
      });
    });
    this.equipmentForm.get('vrstaOpreme')?.valueChanges.subscribe(value => {
      // Filter the equipment options based on the selected type
      this.filteredEquipmentToShow = this.equipmentToShow.filter(e => e.vrstaOpreme === value);
    });
  }

  onFormSubmit() {
    if (this.equipmentForm.valid) {
        this.toAdd = {
          oprema: {
            id: this.equipmentForm.value.opremaId,
            naziv: this.equipment.find((e: any) => e.id === this.equipmentForm.value.opremaId)?.naziv,
            vrstaOpremeId: this.equipmentForm.value.vrstaOpreme,
            vrstaOpremeNaziv: this.types.find((t: any) => t.id === this.equipmentForm.get('vrstaOpreme')?.value)?.naziv as string, // this.equipment.find((e: any) => e.id === this.equipmentForm.get('opremaId')?.value)?.VrstaOpremeNaziv as string,
            slika: this.equipment.find((e: any) => e.id === this.equipmentForm.value.opremaId)?.slika,
            zalihaOpreme: this.equipment.find((e: any) => e.id === this.equipmentForm.value.opremaId)?.zalihaOpreme,
            cijena: this.equipment.find((e: any) => e.id === this.equipmentForm.value.opremaId)?.cijena,
          },
          kolicina: this.equipmentForm.get('kolicina')?.value,
        }
        this._funeralService.addEquipment(this.data, this.toAdd).subscribe({
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
    } else {
      if(this.equipmentForm.get('kolicina')?.value < 1){
        this.snackBar.open('Količina mora opreme mora biti veća od 0!', 'U redu', {
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

type Oprema = {
  id: number;
  naziv: string;
  vrstaOpreme: string;
};
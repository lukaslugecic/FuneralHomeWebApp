import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { forkJoin } from 'rxjs';
import { IPogrebOpremaData } from 'src/app/interfaces/pogreb-oprema-data';
import { IVrstaOpremeUslugeData } from 'src/app/interfaces/vrsta-opreme-usluge-data';
import { EquipmentService } from 'src/app/services/equipment/equipment.service';
import { FuneralService } from 'src/app/services/funeral/funeral.service';

@Component({
  selector: 'app-add-equipment-dialog',
  templateUrl: './add-equipment-dialog.component.html',
  styleUrls: ['./add-equipment-dialog.component.scss']
})
export class AddEquipmentDialogComponent implements OnInit {
  
  equipmentForm: FormGroup = new FormGroup({
    opremaUslugaId: new FormControl('', [Validators.required]),
    vrstaOpremeUsluge: new FormControl('', [Validators.required]),
    kolicina : new FormControl('', [Validators.required, Validators.min(1)]),
  });
  
  toAdd: IPogrebOpremaData = {} as IPogrebOpremaData;

  types: IVrstaOpremeUslugeData[] = [];
  equipmentOrServices: any[] = [];
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
    const typesToCall = this.data.isEquipment ?
                        this._equipmentService.getTypesOfEquipment() :
                        this._equipmentService.getTypesOfServices();
    const otherToCall = this.data.isEquipment ?
                        this._equipmentService.getAllEquipment() :
                        this._equipmentService.getAllServices();
    forkJoin([
      typesToCall, otherToCall
    ]).subscribe(([types, equipment]) => {
      this.types = types;
      if(this.data.isEquipment){
        this.equipmentOrServices = equipment.filter(e => e.zaliha > 0);
      } else {
        this.equipmentOrServices = equipment;
      }
        
      this.equipmentOrServices.forEach((e: any) => {
        this.equipmentToShow.push({
          id: e.id,
          vrstaOpremeUsluge: e.vrstaOpremeUslugeId,
          naziv: "Naziv: " + e.naziv + ", Cijena: " + e.cijena + "€",
          zaliha: e.zaliha
        });
      });
    });
    this.equipmentForm.get('vrstaOpremeUsluge')?.valueChanges.subscribe(value => {
      // Filter the equipment options based on the selected type
      this.filteredEquipmentToShow = this.equipmentToShow.filter(e => e.vrstaOpremeUsluge === value);
    });
  }

  onFormSubmit() {
    // provjeri je li kolicina veca od zalihe opreme
    const kolicina = this.equipmentForm.get('kolicina')?.value;
    const zaliha = this.equipmentOrServices.find((e: any) => e.id === this.equipmentForm.value.opremaUslugaId)?.zaliha;
    if(this.data.isEquipment && kolicina > zaliha){
          this.snackBar.open(`Količina odabrane opreme (${kolicina}) veća je od zalihe (${zaliha})!`, 'U redu', {
          duration: 3000,
      });
    } else
    if (this.equipmentForm.valid) {
        this.toAdd = {
          opremaUsluga: {
            id: this.equipmentForm.value.opremaUslugaId,
            vrstaOpremeUslugeId: this.equipmentForm.value.vrstaOpremeUsluge,
            vrstaOpremeUslugeNaziv: this.types.find((t: any) => t.id === this.equipmentForm.get('vrstaOpremeUsluge')?.value)?.naziv as string,
            jeOprema: this.data.isEquipment,
            naziv: this.equipmentOrServices.find((e: any) => e.id === this.equipmentForm.value.opremaUslugaId)?.naziv,
            slika: this.equipmentOrServices.find((e: any) => e.id === this.equipmentForm.value.opremaUslugaId)?.slika,
            zaliha: this.equipmentOrServices.find((e: any) => e.id === this.equipmentForm.value.opremaUslugaId)?.zaliha,
            opis: this.equipmentOrServices.find((e: any) => e.id === this.equipmentForm.value.opremaUslugaId)?.opis,
            jedinicaMjereNaziv: this.equipmentOrServices.find((e: any) => e.id === this.equipmentForm.value.opremaUslugaId)?.jedinicaMjereNaziv,
            cijena: this.equipmentOrServices.find((e: any) => e.id === this.equipmentForm.value.opremaUslugaId)?.cijena,
          },
          kolicina: this.equipmentForm.get('kolicina')?.value,
        }
        console.log(this.toAdd)
        this._funeralService.addEquipment(this.data.funeralId, this.toAdd).subscribe({
          next: (val: any) => {
            if(this.data.isEquipment){
              this.snackBar.open('Oprema uspješno dodana!', 'U redu', {
                duration: 3000,
              });
            } else {
              this.snackBar.open('Usluga uspješno dodana!', 'U redu', {
                duration: 3000,
              });
            }
            this._dialogRef.close(true);
          },
          error: (err: any) => {
            console.error(err);
            if(this.data.isEquipment){
              this.snackBar.open('Greška prilikom dodavanja opreme!', 'U redu', {
                duration: 3000,
              });
            } else {
              this.snackBar.open('Greška prilikom dodavanja usluge!', 'U redu', {
                duration: 3000,
              });
            }
          },
        });
    } else {
      if(this.equipmentForm.get('kolicina')?.value < 1){
        this.snackBar.open('Količina mora biti veća od 0!', 'U redu', {
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
  vrstaOpremeUsluge: string;
  zaliha: number;
};
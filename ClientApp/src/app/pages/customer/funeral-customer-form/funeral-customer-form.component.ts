import { Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { DateAdapter } from '@angular/material/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NavigationExtras, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { DeathDialogComponent } from 'src/app/components/dialogs/death-dialog/death-dialog.component';
import { IKorisnik } from 'src/app/interfaces/korisnik-data';
import { IPogrebAggretageData } from 'src/app/interfaces/pogreb-aggretage-data';
import { IPogrebOpremaData } from 'src/app/interfaces/pogreb-oprema-data';
import { IPogrebUslugeData } from 'src/app/interfaces/pogreb-usluge-data';
import { AuthService } from 'src/app/services/auth/auth.service';
import { DeathService } from 'src/app/services/death/death.service';
import { EquipmentService } from 'src/app/services/equipment/equipment.service';
import { FuneralService } from 'src/app/services/funeral/funeral.service';
import { ServiceService } from 'src/app/services/service/service.service';

@Component({
  selector: 'app-funeral-customer-form',
  templateUrl: './funeral-customer-form.component.html',
  styleUrls: ['./funeral-customer-form.component.scss']
})
export class FuneralCustomerFormComponent implements OnInit {
 
  userDeaths: any[] = [];
  typesOfFuneral = [
    { value: true, naziv: 'Kremiranje' },
    { value: false, naziv: 'Ukop' },
  ];
  typesOfService: any[] = [];
  services: any[] = [];
  typesOfEquipment: any[] = [];
  equipment: any[] = [];
  equipmentQuantity: any[] = [];

  constructor(
    private _deathService: DeathService,
    private _serviceService: ServiceService,
    private _equipmentService: EquipmentService,
    private _funeralService: FuneralService,
    private _authService: AuthService,
    private readonly _snackBar: MatSnackBar,
    private _dateAdapter: DateAdapter<Date>,
    private _router: Router,
    private _builder: FormBuilder,
    private _dialog: MatDialog,
  ) {
    this._dateAdapter.setLocale('hr');
  }
  isLinear: boolean = true;

  ngOnInit() {
    this._deathService.getAllDeathsWithoutFuneralByUserId(this._authService.userValue?.id as number).subscribe({
      next: (res) => {
        this.userDeaths = res;
      }
    });

    this._serviceService.getTypesOfServices().subscribe({
      next: (res) => {
        this.typesOfService = res;
        this.typesOfService.forEach((typeOfService: any) => {
          this.uslugeForm.addControl(typeOfService.naziv, this._builder.control(''));
        });
      }
    });

    this._serviceService.getAllServices().subscribe({
      next: (res) => {
        this.services = res;
      }
    });

    this._equipmentService.getTypesOfEquipment().subscribe({
      next: (res) => {
        this.typesOfEquipment = res;
        this.typesOfEquipment.forEach((typeOfEquipment: any) => {
          this.opremaForm.addControl(typeOfEquipment.naziv, this._builder.control(''));
        });
      }
    });

    this._equipmentService.getAllEquipment().subscribe({
      next: (res) => {
        // u this.equipment spremi sve opreme kojima je zaliha > 0
        this.equipment = res.filter((equipment: any) => equipment.zalihaOpreme > 0);
        this.equipment.forEach((equipment: any) => {
          this.equipmentQuantity.push({id: equipment.id, zaliha: equipment.zalihaOpreme ,kolicina: 0});
        });
      }
    });
  }

  getServices(typeOfServiceId: number) {
    return this.services.filter((service: any) => service.vrstaUslugeId === typeOfServiceId);
  }

  getEquipment(typeOfEquipmentId: number) {
    return this.equipment.filter((equipment: any) => equipment.vrstaOpremeId === typeOfEquipmentId);
  }

  deathForm = this._builder.group({
    smrtniSlucaj: this._builder.group({
      smrtniSlucajId: this._builder.control('',Validators.required),
      datumPogreba: this._builder.control('', [Validators.required, dateNotInPastValidator()]),
      kremacija: this._builder.control('',Validators.required),
    }),
    usluge: this._builder.group({}),
    oprema: this._builder.group({})
  });
  

  get smrtniSlucajForm() {
    return this.deathForm.get('smrtniSlucaj') as FormGroup;
  }

  get uslugeForm() {
    return this.deathForm.get('usluge') as FormGroup;
  }

  get opremaForm() {
    return this.deathForm.get('oprema') as FormGroup;
  }

  openAddForm() {
    const dialogRef = this._dialog.open(DeathDialogComponent, {
      data: { userId : this._authService.userValue?.id as number },
      });
    dialogRef.afterClosed().subscribe(() => {
      this._deathService.getAllDeathsWithoutFuneralByUserId(this._authService.userValue?.id as number).subscribe({
        next: (res) => {
          this.userDeaths = res;
        }
      });
    });
  }
  

  onFormSubmit() {
    if (this.deathForm.valid) {
      // provjeriti je li datum pogreba u budućnosti
      if(new Date(this.smrtniSlucajForm.value.datumPogreba) < new Date()){
        this._snackBar.open('Datum pogreba ne može biti u prošlosti!', 'U redu', {
          duration: 3000,
        });
        return;
      }
      // provjeriti je li datum pogreba prije datuma smrti
      if(new Date(this.smrtniSlucajForm.value.datumPogreba)
          < new Date(this.userDeaths.find((d: any) => d.id === this.smrtniSlucajForm.value.smrtniSlucajId).datumSmrtiPok)){
        this._snackBar.open('Datum pogreba ne može biti prije datuma smrti!', 'U redu', {
          duration: 3000,
        });
        return;
      }
      const pogrebOprema : IPogrebOpremaData[] = [];
      this.equipmentQuantity.forEach((eq: any) => {
        if(eq.kolicina > 0){
          pogrebOprema.push({
            oprema: this.equipment.find((e: any) => e.id === eq.id),
            kolicina: eq.kolicina
          });
        }
      });

      const pogrebUsluge : IPogrebUslugeData[] = [];
      for (const type of this.typesOfService) {
        const formControl = this.deathForm.controls.usluge.get(type.naziv);
        const serviceId = formControl?.value;
        if (serviceId) {
          const service = this.services.find((s: any) => s.id === serviceId);
          const pu : IPogrebUslugeData = {
            id : service.id,
            naziv : service.naziv,
            vrstaUslugeId : service.vrstaUslugeId,
            vrstaUslugeNaziv : service.vrstaUslugeNaziv,
            opis : service.opis,
            cijena : service.cijena
          }
          pogrebUsluge.push(pu);
        }
      }

      const pogreb = {
        id: 0,
        smrtniSlucajId: this.smrtniSlucajForm.value.smrtniSlucajId,
        datumPogreba: new Date(new Date(this.smrtniSlucajForm.value.datumPogreba).getTime() 
        - new Date(this.smrtniSlucajForm.value.datumPogreba).getTimezoneOffset() * 60000),
        kremacija: this.smrtniSlucajForm.value.kremacija,
        ukupnaCijena: 0,
      };
      
      const toInsert = {
        pogreb: pogreb,
        oprema: pogrebOprema,
        usluga: pogrebUsluge
      }

      this._funeralService.addFuneralWithEquipmentAndServices(toInsert).subscribe({
        next: (res) => {
          this._snackBar.open('Pogreb uspješno dodan!', 'U redu', {
            duration: 3000,
          });
          this._router.navigate(['/']);
        },
        error: (err) => {
          this._snackBar.open('Greška prilikom dodavanja pogreba!', 'U redu', {
            duration: 3000,
          });
        }
      });
    } else {
      this._snackBar.open('Popunite sva polja!', 'U redu', {
        duration: 3000,
      });
    }
  }

  addEquipment(id: number) {
    this.equipmentQuantity.find((eq: any) => eq.id === id).kolicina++;
    // provjeri da li je količina veća od zalihe
    if(this.equipmentQuantity.find((eq: any) => eq.id === id).kolicina > this.equipment.find((e: any) => e.id === id).zalihaOpreme){
      this.equipmentQuantity.find((eq: any) => eq.id === id).kolicina--;
      this._snackBar.open('Nema dovoljno opreme na skladištu!', 'U redu', {
        duration: 3000,
      });
    }
  }

  removeEquipment(id: number) {
    if(this.equipmentQuantity.find((eq: any) => eq.id === id).kolicina > 0){
      this.equipmentQuantity.find((eq: any) => eq.id === id).kolicina--;
    }
  }

  getEquipmentQuantity(id: number) {
    return this.equipmentQuantity.find((eq: any) => eq.id === id).kolicina;
  }

  getTotalPrice() {
    let totalPrice = 0;
    this.equipmentQuantity.forEach((eq: any) => {
      totalPrice += this.equipment.find((e: any) => e.id === eq.id).cijena * eq.kolicina;
    });
    for (const type of this.typesOfService) {
      const formControl = this.deathForm.controls.usluge.get(type.naziv);
      const serviceId = formControl?.value;
      if (serviceId) {
        const service = this.services.find((s: any) => s.id === serviceId);
        totalPrice += service.cijena;
      }
    }
    return totalPrice;
  }
}

export function dateNotInPastValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    return new Date(control.value) < new Date() ? { dateNotInPast: { value: control.value } } : null;
  };
}
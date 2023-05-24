import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DateAdapter } from '@angular/material/core';
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth/auth.service';
import { DeathService } from 'src/app/services/death/death.service';
import { EquipmentService } from 'src/app/services/equipment/equipment.service';
import { FuneralService } from 'src/app/services/funeral/funeral.service';
import { PurchaseService } from 'src/app/services/purchase/purchase.service';


@Component({
  selector: 'app-cart-dialog',
  templateUrl: './cart-dialog.component.html',
  styleUrls: ['./cart-dialog.component.scss']
})
export class CartDialogComponent implements OnInit {
  typesOfEquipment: any[] = [];
  equipmentQuantity: any[] = [];

  constructor(
    private _deathService: DeathService,
    private _equipmentService: EquipmentService,
    private _funeralService: FuneralService,
    private _authService: AuthService,
    private _purchaseService: PurchaseService,
    private readonly _snackBar: MatSnackBar,
    private _dateAdapter: DateAdapter<Date>,
    private _router: Router,
    private _builder: FormBuilder,
    private _dialog: MatDialog,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) { }

  cartForm: FormGroup = new FormGroup({
  });

  ngOnInit(): void {
    this.equipmentQuantity = this.data;
  }

  onFormSubmit() {
    // provjeri da li je količina veća od zalihe i ispisi koja je oprema u pitanju
    const tooMuch = this.equipmentQuantity.filter((eq: any) => eq.kolicina > eq.oprema.zaliha);
    if(tooMuch.length > 0) {
      let equipmentNames = '';
      tooMuch.forEach((eq: any) => {
        equipmentNames += eq.oprema.naziv + ', ';
      });
      equipmentNames = equipmentNames.substring(0, equipmentNames.length - 2);
      this._snackBar.open('Količina opreme ' + equipmentNames + ' je veća od zalihe!', 'U redu', {
        duration: 3000
      });
      return;
    }
    if(this.equipmentQuantity.length === 0) {
      this._snackBar.open('Košarica je prazna!', 'U redu', {
        duration: 3000
      });
      return;
    }
    const kupnja = {
      id: 0,
      korisnikId : this._authService.userValue?.id,
      datumKupovine: new Date(new Date().getTime() - (new Date().getTimezoneOffset() * 60000)).toISOString().split('T')[0],
      ukupnaCijena: 0
    }

    const opremaKupnja: any[] = []
    this.equipmentQuantity.forEach((eq: any) => {
      opremaKupnja.push({
        opremaUsluga: eq.oprema,
        kolicina: eq.kolicina,
        cijena: eq.oprema.cijena
      })
    });

    this._purchaseService.addPurchaseWithItems({Kupnja: kupnja, Oprema: opremaKupnja}).subscribe((res: any) => {
      this._snackBar.open('Kupnja je uspješno izvršena!', 'U redu', {
        duration: 3000,
      });
      this._dialog.closeAll();
      this.equipmentQuantity = [];
      this._router.navigate(['/profile']);
      }
    );

    
  }

  
  addEquipment(id: number) {
    this.equipmentQuantity.find((eq: any) => eq.oprema.id === id).kolicina++;
    // provjeri da li je količina veća od zalihe
    if(this.equipmentQuantity.find((eq: any) => eq.kolicina > eq.oprema.zaliha)){
      this.equipmentQuantity.find((eq: any) => eq.oprema.id === id).kolicina--;
      this._snackBar.open('Nema dovoljno opreme na skladištu!', 'U redu', {
        duration: 3000,
      });
    }
  }

  removeEquipment(id: number) {
    if(this.equipmentQuantity.find((eq: any) => eq.oprema.id === id).kolicina > 0){
      this.equipmentQuantity.find((eq: any) => eq.oprema.id === id).kolicina--;
    }
    if(this.equipmentQuantity.find((eq: any) => eq.oprema.id === id).kolicina === 0){
      this.equipmentQuantity = this.equipmentQuantity.filter((eq: any) => eq.oprema.id !== id);
    }
  }

  
  getTotalPrice() {
    let totalPrice = 0;
    this.equipmentQuantity.forEach((eq: any) => {
      totalPrice += eq.oprema.cijena * eq.kolicina;
    });
    return totalPrice;
  }

}

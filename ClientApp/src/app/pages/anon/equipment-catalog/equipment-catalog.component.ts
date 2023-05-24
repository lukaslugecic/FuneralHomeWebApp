import { Component, OnInit, Pipe , Inject} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { BehaviorSubject, forkJoin, map } from 'rxjs';
import { CartDialogComponent } from 'src/app/components/dialogs/cart-dialog/cart-dialog.component';
import { AuthService } from 'src/app/services/auth/auth.service';
import { EquipmentService } from 'src/app/services/equipment/equipment.service';

@Component({
  selector: 'app-equipment-catalog',
  templateUrl: './equipment-catalog.component.html',
  styleUrls: ['./equipment-catalog.component.scss']
})
export class EquipmentCatalogComponent implements OnInit {
  public user$ = this._authService.user$;
  constructor(
    private readonly _equipmentService: EquipmentService,
    private readonly _authService: AuthService,
    private readonly _snackBar: MatSnackBar,
    private readonly _router: Router,
    private readonly _dialog: MatDialog
  ) { }

  equipment: any[] = [];
  types: any[] = [];
  equipmentInCart: any[] = [];
  badge: number = this.equipmentInCart.length;
  badge$: BehaviorSubject<number> = new BehaviorSubject<number>(this.badge);

  ngOnInit(): void {
    this.loadData();
  }
  
  loadData() {
    forkJoin([
      this._equipmentService.getTypesOfEquipment(),
      this._equipmentService.getAllEquipment().pipe(
        map((res: any[]) => res.filter((e: any) => e.slika != null && e.slika != ''))
      )
    ]).subscribe({
      next: ([types, equipment]) => {
        this.types = types;
        this.equipment = equipment;
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
  

  addToCart(equipment: any) {
    if(equipment.zaliha === 0) {
      this._snackBar.open('Nažalost, odabrane opreme nema na skladištu.', 'Zatvori', { duration: 2000 });
      return;
    }
    if(this.equipmentInCart.find((e: any) => e.oprema.id === equipment.id)){
      this.equipmentInCart.find((e: any) => e.oprema.id === equipment.id).kolicina++;
    } else {
      this.equipmentInCart.push({oprema: equipment, kolicina: 1});
    }
    // badge je broj opreme u kosarici
    this.badge = this.equipmentInCart.reduce((acc: number, curr: any) => acc + curr.kolicina, 0);
    this.badge$.next(this.badge);
    this._snackBar.open('Oprema je dodana u košaricu', 'Zatvori', { duration: 2000 });
  }

  openCart() {
    // maknuti sve iz košarice sto je kolicina 0
    this.equipmentInCart = this.equipmentInCart.filter((e: any) => e.kolicina > 0);
    const dialogRef = this._dialog.open(CartDialogComponent, {
      data: this.equipmentInCart
    });
    dialogRef.afterClosed().subscribe({
      next: (res) => {
        this.badge = this.equipmentInCart.reduce((acc: number, curr: any) => acc + curr.kolicina, 0);
        this.badge$.next(this.badge); 
      }
    });
    
  }


}

@Pipe({
  name: 'equipmentFilter'
})
export class EquipmentFilterPipe {
  transform(items: any[], filter: any): any {
    if (!items || !filter) {
      return items;
    }
    return items.filter((item: any) => item.vrstaOpremeUslugeId === filter);
  }
}

import { Component, OnInit, Pipe } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { forkJoin, map } from 'rxjs';
import { EquipmentService } from 'src/app/services/equipment/equipment.service';

@Component({
  selector: 'app-equipment-catalog',
  templateUrl: './equipment-catalog.component.html',
  styleUrls: ['./equipment-catalog.component.scss']
})
export class EquipmentCatalogComponent implements OnInit {
  
  constructor(
    private readonly _equipmentService: EquipmentService,
    private readonly snackBar: MatSnackBar
  ) { }

  equipment: any[] = [];
  types: any[] = [];
  
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
  
}


@Pipe({
  name: 'equipmentFilter'
})
export class EquipmentFilterPipe {
  transform(items: any[], filter: any): any {
    if (!items || !filter) {
      return items;
    }
    return items.filter((item: any) => item.vrstaOpremeId === filter);
  }
}
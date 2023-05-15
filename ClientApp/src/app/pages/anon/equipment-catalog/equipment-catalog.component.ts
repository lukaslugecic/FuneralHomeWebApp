import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EquipmentDialogComponent } from 'src/app/components/dialogs/equipment-dialog/equipment-dialog.component';
import { IVrstaOpremeData } from 'src/app/interfaces/vrsta-opreme-data';
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

  ngOnInit(): void {
    this.getAllEquipment();
  }


  getAllEquipment() {
    // najprije dohvatimo sve opreme i vrste opreme zatim filtriramo opremu po vrsti opreme
    this._equipmentService.getAllEquipment().subscribe({
      next: (res) => {
        this.equipment = res;
      }
    });
  }
  
}



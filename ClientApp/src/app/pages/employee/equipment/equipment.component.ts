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
  selector: 'app-equipment',
  templateUrl: './equipment.component.html',
  styleUrls: ['./equipment.component.scss']
})
export class EquipmentComponent implements OnInit {
  displayedColumns: string[] = [
    'id',
    'vrstaOpremeNaziv',
    'naziv',
    'slika',
    'zalihaOpreme',
    'cijena',
    'action'
    ];

  selectedType: any =  0;
  types: IVrstaOpremeData[] = [
    { id: 0, naziv: 'Sva oprema' }
  ];

  dataSource!: MatTableDataSource<any>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private _dialog: MatDialog,
    private readonly _equipmentService: EquipmentService,
    private readonly snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.getAllEquipment();
  }

  

  getAllEquipment() {
    // najprije dohvatimo sve opreme i vrste opreme zatim filtriramo opremu po vrsti opreme
    this._equipmentService.getAllEquipment().subscribe({
      next: (res) => {
        this._equipmentService.getTypesOfEquipment().subscribe({
          next: (res2) => {
            //dodaj u types sve vrste opreme koje već nisu u types
            res2.forEach((type: any) => {
              if(!this.types.find((t: any) => t.id === type.id)){
                this.types.push(type);
              }
            });
            // ako je odabrana vrsta opreme, filtriramo opremu po vrsti opreme
            if(this.selectedType !== 0){
              this.dataSource = new MatTableDataSource(res.filter((equipment: any) => equipment.vrstaOpremeId === this.selectedType));
            } else {
              this.dataSource = new MatTableDataSource(res);
            }
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
          },
          error: (err) => {
            this.snackBar.open('Greška prilikom dohvaćanja vrsta opreme!', 'Zatvori', {
              duration: 3000,
            });
          },
        });
      }
    });
    
  }

  
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  deleteEquipment(id: number) {
    if (confirm('Jeste li sigurni da želite obrisati opremu?')) {
      this._equipmentService.deleteEquipment(id).subscribe({
        next: (res) => {
          this.snackBar.open('Oprema je uspješno obrisana!', 'U redu', {
            duration: 3000,
          });
          this.getAllEquipment();
        },
        error: (err) => {
          this.snackBar.open('Greška prilikom brisanja opreme!', 'Zatvori', {
            duration: 3000,
          });
        },
      });
    }
  }

  openEditForm(data: any) {
    const dialogRef = this._dialog.open(EquipmentDialogComponent, {
      data,
    });

    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllEquipment();
        }
      },
    });
  }

  openAddForm() {
    const dialogRef = this._dialog.open(EquipmentDialogComponent);
    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllEquipment();
        }
      },
    });
  }
  
}



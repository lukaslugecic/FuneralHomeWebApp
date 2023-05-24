import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EquipmentDialogComponent } from 'src/app/components/dialogs/equipment-dialog/equipment-dialog.component';
import { IVrstaOpremeUslugeData } from 'src/app/interfaces/vrsta-opreme-usluge-data';
import { EquipmentService } from 'src/app/services/equipment/equipment.service';
import { TypeDialogComponent } from 'src/app/components/dialogs/type-dialog/type-dialog.component';
import { Router } from '@angular/router';


@Component({
  selector: 'app-equipment-types',
  templateUrl: './equipment-types.component.html',
  styleUrls: ['./equipment-types.component.scss']
})
export class EquipmentTypesComponent implements OnInit {
  displayedColumns: string[] = [
    'id',
    'naziv',
    'action'
    ];

 

  dataSource!: MatTableDataSource<any>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private _dialog: MatDialog,
    private readonly _equipmentService: EquipmentService,
    private readonly _snackBar: MatSnackBar,
    private readonly _router: Router
  ) { }

  ngOnInit(): void {
    this.getAllTypes();
  }

  

  getAllTypes() {
    // najprije dohvatimo sve opreme i vrste opreme zatim filtriramo opremu po vrsti opreme
    this._equipmentService.getTypesOfEquipment().subscribe({
      next: (res) => {
            this.dataSource = new MatTableDataSource(res);
            this.dataSource.paginator = this.paginator;
            this.dataSource.sort = this.sort;
          },
      error: (err) => {
            this._snackBar.open('Greška prilikom dohvaćanja vrsta opreme!', 'Zatvori', {
              duration: 3000,
            });
        },
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
    if (confirm('Jeste li sigurni da želite obrisati vrstu opreme?')) {
      this._equipmentService.deleteTypeOfEquipment(id).subscribe({
        next: (res) => {
          this._snackBar.open('Vrsta opreme je uspješno obrisana!', 'U redu', {
            duration: 3000,
          });
          this.getAllTypes();
        },
        error: (err) => {
          this._snackBar.open('Greška prilikom brisanja vrste opreme!', 'Zatvori', {
            duration: 3000,
          });
        },
      });
    }
  }

  openEditForm(data: any) {
    const dialogRef = this._dialog.open(TypeDialogComponent, {
      data,
    });

    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllTypes();
        }
      },
    });
  }

  openAddTypeForm() {
    const dialogRef = this._dialog.open(TypeDialogComponent, {
      data: { jeOprema: true}
    });
    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllTypes();
        }
      },
    });
  }

  openEquipmentPage() {
    this._router.navigate(['/equipment']);
  }
}


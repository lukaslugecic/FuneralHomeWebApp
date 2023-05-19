import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { ServiceDialogComponent } from 'src/app/components/dialogs/service-dialog/service-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { IVrstaOpremeUslugeData } from 'src/app/interfaces/vrsta-opreme-usluge-data';
import { EquipmentService } from 'src/app/services/equipment/equipment.service';

@Component({
  selector: 'app-services',
  templateUrl: './services.component.html',
  styleUrls: ['./services.component.scss']
})
export class ServicesComponent implements OnInit {
  displayedColumns: string[] = [
    'id',
    'vrstaOpremeUslugeNaziv',
    'naziv',
    'opis',
    'cijena',
    'action'
  ];

  selectedType: any = 0;
  types: IVrstaOpremeUslugeData[] = [
    { id: 0, naziv: 'Sve usluge', jeOprema: false}
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
    this.getAllServices();
  }

  getAllServices() {
    // najprije dohvatimo sve opreme i vrste opreme zatim filtriramo opremu po vrsti opreme
    this._equipmentService.getAllServices().subscribe({
      next: (res) => {
        this._equipmentService.getTypesOfServices().subscribe({
          next: (res2) => {
            //dodaj u types sve vrste opreme koje već nisu u types
            res2.forEach((type: any) => {
              if(!this.types.find((t: any) => t.id === type.id)){
                this.types.push(type);
              }
            });
            // ako je odabrana vrsta opreme, filtriramo opremu po vrsti opreme
            if(this.selectedType !== 0){
              this.dataSource = new MatTableDataSource(res.filter((service: any) => service.vrstaOpremeUslugeId === this.selectedType));
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

  deleteService(id: number) {
    if (confirm('Jeste li sigurni da želite obrisati uslugu?')) {
      this._equipmentService.deleteEquipment(id).subscribe({
        next: (res) => {
          this.snackBar.open('Usluga je uspješno obrisana!', 'U redu', {
            duration: 3000,
          });
          this.getAllServices();
        },
        error: (err) => {
          this.snackBar.open('Greška prilikom brisanja usluge!', 'Zatvori', {
            duration: 3000,
          });
        },
      });
    }
  }

  openEditForm(data: any) {
    const dialogRef = this._dialog.open(ServiceDialogComponent, {
      data,
    });

    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllServices();
        }
      },
    });
  }

  openAddForm() {
    const dialogRef = this._dialog.open(ServiceDialogComponent);
    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllServices();
        }
      },
    });
  }
  
}



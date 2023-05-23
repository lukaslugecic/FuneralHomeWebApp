import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EquipmentDialogComponent } from 'src/app/components/dialogs/equipment-dialog/equipment-dialog.component';
import { FuneralDialogComponent } from 'src/app/components/dialogs/funeral-dialog/funeral-dialog.component';
import { Router } from '@angular/router';
import { FuneralService } from 'src/app/services/funeral/funeral.service';

@Component({
  selector: 'app-funerals',
  templateUrl: './funerals.component.html',
  styleUrls: ['./funerals.component.scss']
})
export class FuneralsComponent implements OnInit {
  displayedColumns: string[] = [
    'id',
    'imePok',
    'prezimePok',
    'datumPogreba',
    'kremacija',
    'ime',
    'prezime',
    'ukupnaCijena',
    'datumUgovaranja',
    'action'
    ];

  dataSource!: MatTableDataSource<any>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private _dialog: MatDialog,
    private router: Router,
    private _funeralService: FuneralService,
    private readonly snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.getAllFunerals();
  }

  getAllFunerals() {
    this._funeralService.getAllFunereals().subscribe({
      next: (res) => {
        this.dataSource = new MatTableDataSource(res);
        this.dataSource.sort = this.sort;
        this.dataSource.paginator = this.paginator;
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

  deleteFuneral(id: number) {
    if (confirm('Jeste li sigurni da želite obrisati pogreb?')) {
      this._funeralService.deleteFuneral(id).subscribe({
        next: (res) => {
          this.snackBar.open('Pogreb je uspješno obrisan!', 'U redu', {
            duration: 3000,
          });
          this.getAllFunerals();
        },
        error: (err) => {
          this.snackBar.open('Greška prilikom brisanja pogreba!', 'Zatvori', {
            duration: 3000,
          });
        },
      });
    }
  }

  openEditForm(data: any) {
    const dialogRef = this._dialog.open(FuneralDialogComponent, {
      data,
    });

    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllFunerals();
        }
      },
    });
  }

  openAddForm() {
    const dialogRef = this._dialog.open(FuneralDialogComponent);
    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllFunerals();
        }
      },
    });
  }
  

  redirectToDetails(id: number) {
    this.router.navigate([`/funerals/${id}`]);
  }
  
}



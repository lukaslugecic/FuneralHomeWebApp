import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DeathDialogComponent } from 'src/app/components/dialogs/death-dialog/death-dialog.component';
import { DeathService } from 'src/app/services/death/death.service';
import { InsuranceService } from 'src/app/services/insurance/insurance.service';
import { InsuranceDialogComponent } from 'src/app/components/dialogs/insurance-dialog/insurance-dialog.component';

@Component({
  selector: 'app-insurance',
  templateUrl: './insurance.component.html',
  styleUrls: ['./insurance.component.scss']
})
export class InsuranceComponent implements OnInit {
  displayedColumns: string[] = [
    'id',
    'ime',
    'prezime',
    'datumUgovaranja',
    'placanjeNaRate',
    'action'
    ];

  dataSource!: MatTableDataSource<any>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private _dialog: MatDialog,
    private _insuranceService: InsuranceService,
    private readonly snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.getAllInsurances();
  }

  getAllInsurances() {
    this._insuranceService.getAllInsurances().subscribe({
      next: (res) => {
        console.log(res);
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

  deleteEquipment(id: number) {
    if (confirm('Jeste li sigurni da želite obrisati osiguranje?')) {
      this._insuranceService.deleteInsurance(id).subscribe({
        next: (res) => {
          this.snackBar.open('Osiguranje uspješno obrisano!', 'U redu', {
            duration: 3000,
          });
          this.getAllInsurances();
        },
        error: (err) => {
          this.snackBar.open('Greška prilikom brisanja osiguranja!', 'Zatvori', {
            duration: 3000,
          });
        },
      });
    }
  }

  openEditForm(data: any) {
    const dialogRef = this._dialog.open(InsuranceDialogComponent, {
      data,
    });

    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllInsurances();
        }
      },
    });
  }

  openAddForm() {
    const dialogRef = this._dialog.open(InsuranceDialogComponent);
    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllInsurances();
        }
      },
    });
  }
  
}



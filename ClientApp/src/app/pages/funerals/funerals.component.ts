import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/auth/auth.service';
import { EmployeeService } from 'src/app/services/employee/employee.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { EquipmentDialogComponent } from 'src/app/components/dialogs/equipment-dialog/equipment-dialog.component';
import { FuneralDialogComponent } from 'src/app/components/dialogs/funeral-dialog/funeral-dialog.component';

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
    'action'
    ];

  dataSource!: MatTableDataSource<any>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private _dialog: MatDialog,
    private readonly authService: AuthService,
    private readonly employeeService: EmployeeService,
    private readonly snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.getAllFunerals();
  }

  getAllFunerals() {
    this.employeeService.getAllFunereals().subscribe({
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
    if (confirm('Jeste li sigurni da želite obrisati opremu?')) {
      this.employeeService.deleteEquipment(id).subscribe({
        next: (res) => {
          this.snackBar.open('Oprema je uspješno obrisana!', 'U redu', {
            duration: 3000,
          });
          this.getAllFunerals();
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
  
}



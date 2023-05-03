import { Component, OnChanges, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/auth/auth.service';
import { EmployeeService } from 'src/app/services/employee/employee.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { UserDialogComponent } from 'src/app/components/dialogs/user-dialog/user-dialog.component';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-all-users',
  templateUrl: './all-users.component.html',
  styleUrls: ['./all-users.component.scss']
})
export class AllUsersComponent implements OnInit, OnChanges {

  selectedType: any = 'S';

  displayedColumns: string[] = [
    'id',
    'ime',
    'prezime',
    'oib',
    'datumRodenja',
    'adresa',
    'mail',
    'vrstaKorisnika',
    'action'
    ];

  dataSource!: MatTableDataSource<any>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private _dialog: MatDialog,
    private readonly userService: UserService,
    private readonly snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.getAllUsers();
  }

  ngOnChanges(changes: any): void {
    this.getAllUsers();
  }

  getAllUsers() {
    console.log(this.selectedType);
    this.userService.getAllUsers().subscribe({
      next: (res) => {
        console.log(res);
        if(this.selectedType !== 'S'){
          console.log('filterRazodS');
          this.dataSource = new MatTableDataSource(res.filter((user: any) => user.vrstaKorisnika === this.selectedType));
        } else {
          console.log('filterS');
          this.dataSource = new MatTableDataSource(res);
        }
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
    if (confirm('Jeste li sigurni da želite obrisati korisnika?')) {
      this.userService.deleteUser(id).subscribe({
        next: (res) => {
          this.snackBar.open('Korisnik je uspješno obrisan!', 'U redu', {
            duration: 3000,
          });
          this.getAllUsers();
        },
        error: (err) => {
          this.snackBar.open('Greška prilikom brisanja korisnika!', 'Zatvori', {
            duration: 3000,
          });
        },
      });
    }
  }

  openEditForm(data: any) {
    const dialogRef = this._dialog.open(UserDialogComponent, {
      data,
    });

    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllUsers();
        }
      },
    });
  }

  openAddForm() {
    const dialogRef = this._dialog.open(UserDialogComponent);
    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllUsers();
        }
      },
    });
  }
}



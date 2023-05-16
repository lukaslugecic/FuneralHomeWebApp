import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { UserDialogComponent } from 'src/app/components/dialogs/user-dialog/user-dialog.component';
import { AuthService } from 'src/app/services/auth/auth.service';
import { DeathService } from 'src/app/services/death/death.service';
import { FuneralService } from 'src/app/services/funeral/funeral.service';
import { InsuranceService } from 'src/app/services/insurance/insurance.service';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  userDataColumns: string[] = [
    'ime',
    'prezime',
    'oib',
    'adresa',
    'datumRodenja',
    'email',
    'action'
  ];

  deathColumns: string[] = [
    'id',
    'imePok',
    'prezimePok',
    'oibpok',
    'datumRodenjaPok',
    'datumSmrtiPok'
  ];

  funeralColumns: string[] = [
    'imePok',
    'prezimePok',
    'datumPogreba',
    'kremacija',
    'ukupnaCijena',
  ];

  insuranceColumns: string[] = [
    'id',
    'datumUgovaranja',
    'placanjeNaRate',
    'brojRata',
    'nazivPaketa',
    'cijenaPaketa',
    'action'
  ];

  id: number = this._authService.userValue?.id as number;

  constructor(
    private _snackBar: MatSnackBar,
    private _dialog: MatDialog,
    private _authService: AuthService,
    private _userService: UserService,
    private _deathService: DeathService,
    private _funeralService: FuneralService,
    private _insuranceService: InsuranceService
  ) {}


  dataSourceUser!: MatTableDataSource<any>;
  dataSourceDeaths!: MatTableDataSource<any>;
  dataSourceFunerals!: MatTableDataSource<any>;
  dataSourceInsurance!: MatTableDataSource<any>;
  userData: any[] = [];
  deathData: any[] = [];
  funeralData: any[] = [];
  insuranceData: any[] = [];

  ngOnInit(): void {
    this.getAllData();
  }

  getAllData(){
    this.userData = [];
    this.deathData = [];
    this.funeralData  = [];
    this.insuranceData = [];
    this._userService.getUserById(this.id).subscribe({
      next: (res) => {
        this.userData.push(res);
        this.dataSourceUser = new MatTableDataSource(this.userData);
      }
    });

    this._deathService.getAllDeathsByUserId(this.id).subscribe({
      next: (res) => {
        this.deathData = res;
        this.dataSourceDeaths = new MatTableDataSource(this.deathData);
      }
    });

    this._funeralService.getAllFuneralsByUserId(this.id).subscribe({
      next: (res) => {
        this.funeralData = res;
        this.dataSourceFunerals = new MatTableDataSource(this.funeralData);
      }
    });

    this._insuranceService.getInsurancesByUserId(this.id).subscribe({
      next: (res) => {
        this.insuranceData = res;
        this.dataSourceInsurance = new MatTableDataSource(this.insuranceData);
      }
    });
  }


  openEditUserData(data : any){
    const dialogRef = this._dialog.open(UserDialogComponent, {
      data: data
    }); 
    
  }

  removeInsurance(id: number){
    if(confirm("Jeste li sigurni da želite obrisati osiguranje?")){
      this._insuranceService.deleteInsurance(id).subscribe({
        next: (res) => {
          this._snackBar.open('Osiguranje uspješno obrisano!', 'U redu', {
            duration: 3000,
          });
          this.getAllData();
        },
        error: (err) => {
          this._snackBar.open('Greška prilikom brisanja osiguranja!', 'U redu', {
            duration: 3000,
          });
        }
      });
    }
  }
}

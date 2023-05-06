import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { AddEquipmentDialogComponent } from 'src/app/components/dialogs/add-equipment-dialog/add-equipment-dialog.component';
import { AddServiceDialogComponent } from 'src/app/components/dialogs/add-service-dialog/add-service-dialog.component';
import { DeathDialogComponent } from 'src/app/components/dialogs/death-dialog/death-dialog.component';
import { FuneralDialogComponent } from 'src/app/components/dialogs/funeral-dialog/funeral-dialog.component';
import { FuneralService } from 'src/app/services/funeral/funeral.service';

@Component({
  selector: 'app-funeral-detail',
  templateUrl: './funeral-detail.component.html',
  styleUrls: ['./funeral-detail.component.scss']
})
export class FuneralDetailComponent implements OnInit {
  funeralId: number | undefined;

  funeralColumns: string[] = [
    'id',
    'datumPogreba',
    'kremacija',
    'ime',
    'prezime',
    'ukupnaCijena',
    'action'
  ];

  deathColumns: string[] = [
    'id',
    'imePok',
    'prezimePok',
    'oibpok',
    'datumRodenjaPok',
    'datumSmrtiPok',
    'action'
  ];

  equipmentColumns: string[] = [
    'id',
    'vrstaOpremeNaziv',
    'naziv',
    'cijena',
    'kolicina',
    'add',
    'action'
    ];

  serviceColumns: string[] = [
    'id',
    'vrstaUslugeNaziv',
    'naziv',
    'cijena',
    'action'
  ];

  constructor(
    private _snackBar: MatSnackBar,
    private _dialog: MatDialog,
    private readonly _funeralService: FuneralService,
    private readonly route: ActivatedRoute,
  ) { }

  dataSourceDeaths!: MatTableDataSource<any>;
  dataSourceEquipment!: MatTableDataSource<any>;
  dataSourceServices!: MatTableDataSource<any>;
  arrayOfDeaths: any[] = [];
  arrayOfEquipment: any[] = [];
  arrayOfServices: any[] = [];

  ngOnInit(): void {
    this.getAllInfo();
  }

  getAllInfo() {
    this.arrayOfDeaths = [];
    this.arrayOfEquipment = [];
    this.arrayOfServices = [];
    this.funeralId = Number(this.route.snapshot.paramMap.get('id'));
    this._funeralService.getFuneralDetailById(this.funeralId).subscribe({
      next: (res) => {
        this.arrayOfDeaths.push(res);
        this.arrayOfEquipment = res.pogrebOprema;
        this.arrayOfServices = res.pogrebUsluga;
        this.dataSourceDeaths = new MatTableDataSource(this.arrayOfDeaths);
        this.dataSourceEquipment = new MatTableDataSource(this.arrayOfEquipment);
        this.dataSourceServices = new MatTableDataSource(this.arrayOfServices);
      }
    });
  }

  openEditDeathForm(data: any) {
    const dialogRef = this._dialog.open(DeathDialogComponent, {
      data,
    });

    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllInfo();
        }
      },
    });
  }

  deleteFuneral(id: number) {
    if (confirm('Jeste li sigurni da želite obrisati opremu?')) {
      this._funeralService.deleteFuneral(id).subscribe({
        next: (res) => {
          this._snackBar.open('Oprema je uspješno obrisana!', 'U redu', {
            duration: 3000,
          });
          // redirect to /funerals
          
        },
        error: (err) => {
          this._snackBar.open('Greška prilikom brisanja opreme!', 'Zatvori', {
            duration: 3000,
          });
        },
      });
    }
  }

  openEditFuneralForm(data: any) {
    const dialogRef = this._dialog.open(FuneralDialogComponent, {
      data,
    });

    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllInfo();
        }
      },
    });
  }

  addEquipment() {
    const dialogRef = this._dialog.open(AddEquipmentDialogComponent, {
      data: this.funeralId,
    });
    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllInfo();
        }
      },
    });
  }

  removeEquipment(data: any) {
    console.log(data);
    this._funeralService.removeEquipment(this.funeralId!, data).subscribe({
      next: (res) => {
        this.getAllInfo();
      },
      error: (err) => {
        this._snackBar.open('Greška prilikom uklanjanja opreme!', 'U redu', {
          duration: 3000,
        });
      }
    });
  }

  addService() {
    const dialogRef = this._dialog.open(AddServiceDialogComponent, {
      data: this.funeralId,
    });
    dialogRef.afterClosed().subscribe({
      next: (val) => {
        if (val) {
          this.getAllInfo();
        }
      },
    });
  }

  removeService(data: any) {
    this._funeralService.removeService(this.funeralId!, data).subscribe({
      next: (res) => {
        this.getAllInfo();
      }
    });
  }

  incrementEquipment(id: number){
    this._funeralService.incrementEquipment(this.funeralId!, id).subscribe({
      next: (res) => {
        this.getAllInfo();
      },
      error: (err) => {
        this._snackBar.open('Nema više opreme na zalihi', 'Zatvori', {
          duration: 3000,
        });
      },
    });
  }

  decrementEquipment(id: number){
    this._funeralService.decrementEquipment(this.funeralId!, id).subscribe({
      next: (res) => {
        this.getAllInfo();
      }
    });
  }

}

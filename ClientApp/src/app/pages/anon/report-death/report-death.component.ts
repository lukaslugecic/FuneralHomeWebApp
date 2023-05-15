import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-report-death',
  templateUrl: './report-death.component.html',
  styleUrls: ['./report-death.component.scss']
})
export class ReportDeathComponent implements OnInit {

  constructor(private _router: Router) { }

  ngOnInit(): void {
  }

  openAddForm() {
    // preusmjeri na "/report-death/form" s Routerom
    this._router.navigate(['/report-death/form']);
  }
}

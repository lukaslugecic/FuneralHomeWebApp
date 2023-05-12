import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-organize-funeral',
  templateUrl: './organize-funeral.component.html',
  styleUrls: ['./organize-funeral.component.scss']
})
export class OrganizeFuneralComponent implements OnInit {

  constructor(private _router: Router) { }

  ngOnInit(): void {
  }

  openAddForm() {
    // preusmjeri na "/report-death/form" s Routerom
    this._router.navigate(['/report-death/form']);
  }
}

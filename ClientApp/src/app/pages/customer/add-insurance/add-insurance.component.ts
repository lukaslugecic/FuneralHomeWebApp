import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-insurance',
  templateUrl: './add-insurance.component.html',
  styleUrls: ['./add-insurance.component.scss']
})
export class AddInsuranceComponent implements OnInit {

  constructor(private _router: Router) { }

  ngOnInit(): void {
  }

  openAddForm() {
    this._router.navigate(['/organize-funeral/form']);
  }
}

import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-report-death',
  templateUrl: './report-death.component.html',
  styleUrls: ['./report-death.component.scss']
})
export class ReportDeathComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  openAddForm() {
    console.log("openAddDialog");
  }
}

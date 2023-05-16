import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-report-death',
  templateUrl: './report-death.component.html',
  styleUrls: ['./report-death.component.scss']
})
export class ReportDeathComponent implements OnInit {
  public user$ = this._authService.user$;
  anon = true;
  constructor(
    private _router: Router,
    private _authService: AuthService
  ) { }

  ngOnInit(): void {
    if(this._authService.isLoggedIn()){
      this.anon = false;
    }
  }

  openAddForm() {
    if(!this.anon){
      this._router.navigate(['/report-death/form']);
    } else {
      this._router.navigate(['/register']);
    }
  }
}

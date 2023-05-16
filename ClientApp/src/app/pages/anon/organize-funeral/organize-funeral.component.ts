import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-organize-funeral',
  templateUrl: './organize-funeral.component.html',
  styleUrls: ['./organize-funeral.component.scss']
})
export class OrganizeFuneralComponent implements OnInit {
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
      this._router.navigate(['/organize-funeral/form']);
    } else {
      this._router.navigate(['/register']);
    }
  }
}

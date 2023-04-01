import { Component, OnInit } from '@angular/core';
import { IKorisnik } from 'src/app/interfaces/korisnik-data';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public user$ = this.authService.user$;
  korisnici: IKorisnik[] = [];
  constructor(private readonly authService: AuthService) {}

  ngOnInit() : void{
    this.authService
    .getKorisnici()
    .subscribe((result : IKorisnik[]) => (this.korisnici = result));
  }

  fun() : void{
    console.log(this.korisnici);
  }
}

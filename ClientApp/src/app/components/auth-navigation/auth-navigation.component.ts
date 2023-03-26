import { Component } from '@angular/core';

@Component({
  selector: 'app-auth-navigation',
  templateUrl: './auth-navigation.component.html',
  styleUrls: ['./auth-navigation.component.scss'],
})
export class AuthNavigationComponent {
  public links = [
    { title: 'Naslovna', path: '/' },
    { title: 'Prijava', path: '/login' },
    { title: 'Registracija', path: '/register' },
  ];
}

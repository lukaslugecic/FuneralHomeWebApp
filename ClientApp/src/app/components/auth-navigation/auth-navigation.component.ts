import { Component } from '@angular/core';

@Component({
  selector: 'app-auth-navigation',
  templateUrl: './auth-navigation.component.html',
  styleUrls: ['./auth-navigation.component.scss'],
})
export class AuthNavigationComponent {
  public links = [
    { title: 'Oprema', path: '/equipment-catalog' },
    { title: 'Usluge', path: '/services-catalog' },
    { title: 'Prijava smrtnog slučaja', path: '/report-death/info' },
    { title: 'Organizacija pogreba', path: '/organize-funeral' },
    { title: 'Osiguranje', path: '/add-insurance' },
    { title: 'Prijava', path: '/login' }
  ];
}

import { Component} from '@angular/core';

@Component({
  selector: 'app-main-navigation',
  templateUrl: './main-navigation.component.html',
  styleUrls: ['./main-navigation.component.scss']
})
export class MainNavigationComponent{

  constructor() { }

  public links = {
    admin: [
      { title: 'Naslovna', path: '/' },
      { title: 'Admin', path: '/admin' },
      { title: 'Profil', path: '/profile' },
    ],
    nonAdmin: [
      { title: 'Naslovna', path: '/' },
      { title: 'Kalendar', path: '/patient' },
      { title: 'Profil', path: '/profile' },
    ],
  };

}

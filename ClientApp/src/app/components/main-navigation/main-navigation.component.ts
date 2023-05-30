import { Component, OnDestroy} from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth/auth.service';

@Component({
  selector: 'app-main-navigation',
  templateUrl: './main-navigation.component.html',
  styleUrls: ['./main-navigation.component.scss']
})
export class MainNavigationComponent implements OnDestroy{
  private readonly subscription = new Subscription();
  public user$ = this.authService.user$;

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  public links = {
    admin: [
      { title: 'Korisnici', path: '/users' },
    ],
    employee: [
      { title: 'Oprema', path: '/equipment' },
      { title: 'Usluge', path: '/services' },
      { title: 'Pogrebi', path: '/funerals' },
      { title: 'Smrtni slučajevi', path: '/deaths' },
      { title: 'Osiguranja', path: '/insurances' },
    ],
    customer: [
      { title: 'Ponuda opreme', path: '/equipment-catalog' },
     // { title: 'Usluge', path: '/services-catalog' },
      { title: 'Prijava smrtnog slučaja', path: '/report-death/info' },
      { title: 'Organizacija pogreba', path: '/organize-funeral' },
      { title: 'Osiguranje', path: '/add-insurance' },
      { title: 'Moji podaci', path: '/profile' },
    ]
  };

  public onLogoutClick() {
    const logoutSubscription = this.authService.logout();
    this.subscription.add(logoutSubscription);
  }

  public ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}

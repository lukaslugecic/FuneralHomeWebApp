import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../../services/auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class CustomerGuard implements CanActivate {
  constructor(
    private router: Router,
    private authService: AuthService
  ) {}


  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot):
    Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
      const user = this.authService.userValue;
      if(user?.vrstaKorisnika === 'K') {
        return true;
      }
      if(!user){
        this.router.navigate(['/login'], { queryParams: {returnUrl: state.url}});
      } else {
        this.router.navigate(['/']);
      }
      
      return false;
  }
  
}

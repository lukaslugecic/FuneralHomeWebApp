import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IKorisnik } from '../../interfaces/korisnik-data';
import { ILoginData } from '../../interfaces/login-data';
import { IRegisterData } from '../../interfaces/register-data';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private url = 'Korisnik';
  private readonly _user$ = new BehaviorSubject<IKorisnik | null>(null);
  public user$: Observable<IKorisnik | null>;

  constructor(private http: HttpClient, private router: Router) {
    this._user$ = new BehaviorSubject(JSON.parse(localStorage.getItem('user')!));
    this.user$ = this._user$.asObservable();
  }

  public get userValue() {
    return this._user$.value;
  }

  public getKorisnici() : Observable<IKorisnik[]> {
    return this.http.get<IKorisnik[]>(`${environment.apiUrlHttps}/${this.url}`);
  }

  public register(data: IRegisterData) {
    return this.http.post<IKorisnik>(`${environment.apiUrlHttps}/${this.url}`, data).pipe(
      tap((resp) => {
        this._user$.next(resp);
      })
    );
  }

  public login(data: ILoginData) : Observable<string>{
    return this.http.post<any>(`${environment.apiUrlHttps}/${this.url}/login`, data)
      .pipe(map(user => {
        localStorage.setItem('user', JSON.stringify(user));
        this._user$.next(user);
        return user;
      }));
  }

  public logout() {
    localStorage.removeItem('user');
    this._user$.next(null);
    this.router.navigate(['/login']);
  }
  
}

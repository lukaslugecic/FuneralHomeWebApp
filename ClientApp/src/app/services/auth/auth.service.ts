import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { IKorisnik } from '../../interfaces/korisnik-data';
import { ILoginData } from '../../interfaces/login-data';
import { IRegisterData } from '../../interfaces/register-data';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly _user$ = new BehaviorSubject<IKorisnik | null>(null);
  public user$ = this._user$
    .asObservable()
    .pipe(tap((user) => (this.id = user?.id)));
  public id?: number;

  constructor(private http: HttpClient) {}

  public getUser() {
    return this.http.get<IKorisnik>('/api/Korisnik').pipe(
      tap((resp) => {
        this._user$.next(resp);
      })
    );
  }


  public login(data: ILoginData) {
    return this.http.post<IKorisnik>('/api/login', data).pipe(
      tap((resp) => {
        this._user$.next(resp);
      })
    );
  }

  public register(data: IRegisterData) {
    return this.http.post<IKorisnik>('/api/Korisnik', data).pipe(
      tap((resp) => {
        this._user$.next(resp);
      })
    );
  }

  public logout() {
    return this.http.get('/api/logout').pipe(
      tap(() => {
        this._user$.next(null);
      })
    );
  }
}

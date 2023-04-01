import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IKorisnik } from '../../interfaces/korisnik-data';
import { ILoginData } from '../../interfaces/login-data';
import { IRegisterData } from '../../interfaces/register-data';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  /*
  private readonly _user$ = new BehaviorSubject<IUser | null>(null);
  public user$ = this._user$
    .asObservable()
    .pipe(tap((user) => (this.id = user?.id)));
  public id?: number;

  constructor(private http: HttpClient) {}

  public getUser() {
    return this.http.get<IUser>('/api/user').pipe(
      tap((resp) => {
        this._user$.next(resp);
      })
    );
  }

  public getPatientDoctorId(): Observable<any> {
    return this.http
      .get('/api/user/doctor')
      .pipe(tap((resp) => console.log(resp)));
  }

  public login(data: ILoginData) {
    return this.http.post<IUser>('/api/login', data).pipe(
      tap((resp) => {
        this._user$.next(resp);
      })
    );
  }

  public register(data: IRegisterData) {
    return this.http.post<IUser>('/api/register', data).pipe(
      tap((resp) => {
        this._user$.next(resp);
      })
    );
  }

  public createDoctor(data: IDoctorNurseData) {
    return this.http.post<IUser>('/api/register/doctor', data).pipe(
      tap((resp) => {
        this._user$.next(resp);
      })
    );
  }

  public createNurse(data: IDoctorNurseData) {
    return this.http.post<IUser>('/api/register/nurse', data).pipe(
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
  */

  private url = 'Korisnik';
  private readonly _user$ = new BehaviorSubject<IKorisnik | null>(null);

  constructor(private http: HttpClient) {}

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
    return this.http.post(`${environment.apiUrlHttps}/${this.url}/login`, data, { responseType: 'text'});
    /*
    .pipe(
      tap((resp) => {
       this._user$.next(resp);
      })
    );
    */
  }

  public logout() {
    return this.http.get('/api/logout').pipe(
      tap(() => {
        this._user$.next(null);
      })
    );
  }
  
}

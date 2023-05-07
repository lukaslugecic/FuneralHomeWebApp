import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IKorisnik } from '../../interfaces/korisnik-data';
import { IRegisterData } from 'src/app/interfaces/register-data';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private url = 'Korisnik';

  constructor(private http: HttpClient) { }

  public getAllUsers() : Observable<IKorisnik[]> {
    return this.http.get<IKorisnik[]>(`${environment.apiUrlHttps}/${this.url}`);
  }

  public updateUser(id: number, data: IRegisterData) {
    return this.http.put<IRegisterData>(`${environment.apiUrlHttps}/${this.url}/${id}`, data);
  }

  public addUser(data: IRegisterData) {
    return this.http.post<IRegisterData>(`${environment.apiUrlHttps}/${this.url}`, data);
  }

  public deleteUser(id: number) {
    return this.http.delete<IKorisnik>(`${environment.apiUrlHttps}/${this.url}/${id}`);
  }
  
  public getAllUsersWithoutInsurance() : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/Korisnik/WithoutInsurance`);
  }
}

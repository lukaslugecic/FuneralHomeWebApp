import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DeathService {

  constructor(private http: HttpClient) { }

  public getAllDeaths() : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/SmrtniSlucaj`);
  }

  public deleteDeath(id: number) : Observable<any> {
    return this.http.delete(`${environment.apiUrlHttps}/SmrtniSlucaj/${id}`);
  }
}

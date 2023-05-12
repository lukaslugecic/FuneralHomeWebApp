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

  public getAllDeathsWithoutFuneral() : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/SmrtniSlucaj/WithoutFuneral`);
  }

  public getAllDeathsWithoutFuneralByUserId(id: number) : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/SmrtniSlucaj/WithoutFuneral/${id}`);
  }

  public addDeath(data: any) : Observable<any> {
    return this.http.post(`${environment.apiUrlHttps}/SmrtniSlucaj`, data);
  }

  public updateDeath(id: number, data: any) : Observable<any> {
    return this.http.put(`${environment.apiUrlHttps}/SmrtniSlucaj/${id}`, data);
  }

  public deleteDeath(id: number) : Observable<any> {
    return this.http.delete(`${environment.apiUrlHttps}/SmrtniSlucaj/${id}`);
  }
}

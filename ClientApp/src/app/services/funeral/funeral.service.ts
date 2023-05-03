import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FuneralService {

  constructor(private http: HttpClient) { }

  public getAllFunereals() : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/Pogreb/PogrebSmrtniSlucaj`);
  }

  public getFuneralDetailById(id: number) : Observable<any> {
    return this.http.get<any>(`${environment.apiUrlHttps}/Pogreb/Aggregate/${id}`);
  }

  public deleteFuneral(id: number) : Observable<any> {
    return this.http.delete<any>(`${environment.apiUrlHttps}/Pogreb/${id}`);
  }
}

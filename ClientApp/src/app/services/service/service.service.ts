import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IVrstaUslugeData } from 'src/app/interfaces/vrsta-usluge-data';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ServiceService {

  constructor(private http: HttpClient) { }

  public getAllServices() : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/Usluga`);
  }

  public getAllServicesByType(type: number) : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/Oprema/Vrste/${type}`);
  }

  public getTypesOfServices() : Observable<Array<IVrstaUslugeData>> {
    return this.http.get<Array<IVrstaUslugeData>>(`${environment.apiUrlHttps}/VrstaUsluge`);
  }

  public updateService(id: number, data: any) : Observable<any> {
    return this.http.put<any>(`${environment.apiUrlHttps}/Usluga/${id}`, data);
  }

  public addService(data: any) : Observable<any> {
    return this.http.post<any>(`${environment.apiUrlHttps}/Usluga`, data);
  }
  
  public deleteService(id: number) : Observable<any> {
    return this.http.delete<any>(`${environment.apiUrlHttps}/Usluga/${id}`);
  }
}

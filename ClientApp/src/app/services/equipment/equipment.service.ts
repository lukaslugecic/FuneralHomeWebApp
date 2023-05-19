import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IVrstaOpremeUslugeData } from 'src/app/interfaces/vrsta-opreme-usluge-data';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EquipmentService {

  constructor(private http: HttpClient) { }

  
  public getAllEquipment() : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/OpremaUsluga/Oprema`);
  }

  public getAllServices() : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/OpremaUsluga/Usluge`);
  }

  /*
  public getAllEquipmentByType(type: number) : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/OpremaUsluge/Vrste/${type}`);
  }
  */

  public getTypesOfEquipment() : Observable<Array<IVrstaOpremeUslugeData>> {
    return this.http.get<Array<IVrstaOpremeUslugeData>>(`${environment.apiUrlHttps}/VrstaOpremeUsluge/Oprema`);
  }

  public getTypesOfServices() : Observable<Array<IVrstaOpremeUslugeData>> {
    return this.http.get<Array<IVrstaOpremeUslugeData>>(`${environment.apiUrlHttps}/VrstaOpremeUsluge/Usluge`);
  }

  public updateEquipment(id: number, data: any) : Observable<any> {
    return this.http.put<any>(`${environment.apiUrlHttps}/OpremaUsluge/${id}`, data);
  }

  public addEquipment(data: any) : Observable<any> {
    return this.http.post<any>(`${environment.apiUrlHttps}/OpremaUsluge`, data);
  }

  public deleteEquipment(id: number) : Observable<any> {
    return this.http.delete<any>(`${environment.apiUrlHttps}/OpremaUsluge/${id}`);
  }
}

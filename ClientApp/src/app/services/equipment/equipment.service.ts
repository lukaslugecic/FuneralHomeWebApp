import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IVrstaOpremeData } from 'src/app/interfaces/vrsta-opreme-data';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EquipmentService {

  constructor(private http: HttpClient) { }

  public getAllEquipment() : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/Oprema`);
  }

  public getAllEquipmentByType(type: number) : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/Oprema/Vrste/${type}`);
  }

  public getTypesOfEquipment() : Observable<Array<IVrstaOpremeData>> {
    return this.http.get<Array<IVrstaOpremeData>>(`${environment.apiUrlHttps}/VrstaOpreme`);
  }

  public updateEquipment(id: number, data: any) : Observable<any> {
    return this.http.put<any>(`${environment.apiUrlHttps}/Oprema/${id}`, data);
  }

  public addEquipment(data: any) : Observable<any> {
    return this.http.post<any>(`${environment.apiUrlHttps}/Oprema`, data);
  }

  public deleteEquipment(id: number) : Observable<any> {
    return this.http.delete<any>(`${environment.apiUrlHttps}/Oprema/${id}`);
  }
}

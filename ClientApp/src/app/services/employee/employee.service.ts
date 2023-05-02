import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { IVrstaUslugeData } from 'src/app/interfaces/vrsta-usluge-data';
import { IVrstaOpremeData } from 'src/app/interfaces/vrsta-opreme-data';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http: HttpClient) { }

  // Equipment

  public getAllEquipment() : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/Oprema`);
  }

  public getAllEquipmentByType(type: number) : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/Oprema/Vrste/${type}`);
  }

  getTypesOfEquipment() : Observable<Array<IVrstaOpremeData>> {
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

  // Service

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





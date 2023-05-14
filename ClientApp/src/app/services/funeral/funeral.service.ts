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

  public getFuneralByDeathId(id: number) : Observable<any> {
    return this.http.get<any>(`${environment.apiUrlHttps}/Pogreb/SmrtniSlucaj/${id}`);
  }

  public getFuneralDetailById(id: number) : Observable<any> {
    return this.http.get<any>(`${environment.apiUrlHttps}/Pogreb/Aggregate/${id}`);
  }

  public deleteFuneral(id: number) : Observable<any> {
    return this.http.delete<any>(`${environment.apiUrlHttps}/Pogreb/${id}`);
  }

  public addFuneral(data: any) : Observable<any> {
    return this.http.post<any>(`${environment.apiUrlHttps}/Pogreb`, data);
  }

  public updateFuneral(id: number, data: any) : Observable<any> {
    return this.http.put<any>(`${environment.apiUrlHttps}/Pogreb/${id}`, data);
  }

  public upadateFuneralDeath(id: number, data: any) : Observable<any> {
    return this.http.put<any>(`${environment.apiUrlHttps}/Pogreb/PogrebSmrtniSlucaj/${id}`, data);
  }

  public addFuneralDeath(data: any) : Observable<any> {
    return this.http.post<any>(`${environment.apiUrlHttps}/Pogreb/PogrebSmrtniSlucaj`, data);
  }

  public addEquipment(id: number, data : any) : Observable<any> {
    return this.http.post<any>(`${environment.apiUrlHttps}/Pogreb/AddOprema/${id}`, data);
  }

  public removeEquipment(id: number, data : any) : Observable<any> {
    return this.http.post<any>(`${environment.apiUrlHttps}/Pogreb/RemoveOprema/${id}`, data);
  }

  public incrementEquipment(id: number, opremaId : number) : Observable<any> {
    return this.http.post<any>(`${environment.apiUrlHttps}/Pogreb/IncrementOprema/${id}?opremaId=${opremaId}`, null);
  }

  public decrementEquipment(id: number, opremaId : number) : Observable<any> {
    return this.http.post<any>(`${environment.apiUrlHttps}/Pogreb/DecrementOprema/${id}?opremaId=${opremaId}`, null);
  }
  
  public addService(id: number, data : any) : Observable<any> {
    return this.http.post<any>(`${environment.apiUrlHttps}/Pogreb/AddUsluga/${id}`, data);
  }

  public removeService(id: number, data : any) : Observable<any> {
    return this.http.post<any>(`${environment.apiUrlHttps}/Pogreb/RemoveUsluga/${id}`, data);
  }

  public addFuneralWithEquipmentAndServices(data : any) : Observable<any> {
    return this.http.put<any>(`${environment.apiUrlHttps}/Pogreb/AddPogreb`, data);
  }
}

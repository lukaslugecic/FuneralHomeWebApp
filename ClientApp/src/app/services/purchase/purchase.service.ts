import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PurchaseService {

  constructor(private http: HttpClient) { }

  public getAllPurchases() : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/Kupnja`);
  }

  public getAllPurchasesByUserId(id: number) : Observable<Array<any>> {
    return this.http.get<Array<any>>(`${environment.apiUrlHttps}/Kupnja/Korisnik/${id}`);
  }

  public addPurchase(data: any) : Observable<any> {
    return this.http.post(`${environment.apiUrlHttps}/Kupnja`, data);
  }

  public updatePurchase(id: number, data: any) : Observable<any> {
    return this.http.put(`${environment.apiUrlHttps}/Kupnja/${id}`, data);
  }

  public deletePurchase(id: number) : Observable<any> {
    return this.http.delete(`${environment.apiUrlHttps}/Kupnja/${id}`);
  }


  public addEquipment(id: number, data : any) : Observable<any> {
    return this.http.post<any>(`${environment.apiUrlHttps}/Kupnja/AddOprema/${id}`, data);
  }

  public removeEquipment(id: number, data : any) : Observable<any> {
    return this.http.post<any>(`${environment.apiUrlHttps}/Kupnja/RemoveOprema/${id}`, data);
  }

  public incrementEquipment(id: number, opremaId : number) : Observable<any> {
    return this.http.post<any>(`${environment.apiUrlHttps}/Kupnja/IncrementOprema/${id}?opremaId=${opremaId}`, null);
  }

  public decrementEquipment(id: number, opremaId : number) : Observable<any> {
    return this.http.post<any>(`${environment.apiUrlHttps}/Kupnja/DecrementOprema/${id}?opremaId=${opremaId}`, null);
  }
  
  public addPurchaseWithItems(data : any) : Observable<any> {
    return this.http.put<any>(`${environment.apiUrlHttps}/Kupnja/AddKupnja`, data);
  }
}

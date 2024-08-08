import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { data } from '../models/data';



const endpoint = 'http://localhost:5093/api/Data/';
const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class TimeService {

  constructor(private http: HttpClient) { }

  getAnualAmount(year?:number): Observable<data> {
    return this.http.get<data>(endpoint + year);
  }

  GetSalesByMonth() {
    return this.http.get<any>(endpoint + "SalesByMonth");
  }
  GetSalesByQuarter() {
    return this.http.get<any>(endpoint + "SalesByQuarter");
  }
  GetSalesBySemester() {
    return this.http.get<any>(endpoint + "SalesBySemester");
  }
}

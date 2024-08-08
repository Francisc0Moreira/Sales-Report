import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';

const endpoint = 'http://localhost:5093/api/region/';
const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};


@Injectable({
  providedIn: 'root'
})
export class RegionService {

  constructor(private http: HttpClient) { }


  GetLocalsByYear() {
    return this.http.get<any>(endpoint + "localsyears");
  }

  GetLocalsByMonth() {
    return this.http.get<any>(endpoint + "localsMonth");
  }

  GetLocalsByQuarter() {
    return this.http.get<any>(endpoint + "localsQuarter");
  }
  GetLocalsBySemester() {
    return this.http.get<any>(endpoint + "localsSemester");
  }

  GetCountrysYear() {
    return this.http.get<any>(endpoint + "countrys");
  }

}

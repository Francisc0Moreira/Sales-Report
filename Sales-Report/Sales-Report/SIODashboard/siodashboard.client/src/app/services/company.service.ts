import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { Company } from '../models/company';


const endpoint = 'http://localhost:5093/api/company/';
const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};


@Injectable({
  providedIn: 'root'
})
export class CompanyService {

  constructor(private http: HttpClient) { }


  getCompany(): Observable<Company> {
    return this.http.get<Company>(endpoint);
  }



}

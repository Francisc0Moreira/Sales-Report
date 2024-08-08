import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';

const endpoint = 'http://localhost:5093/api/client/';
const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class ClientsService {

  constructor(private http: HttpClient) { }

  getAllClientsValues() {
    return this.http.get(endpoint);
  }

  getClientsCount(): Observable<number> {
    return this.http.get<number>(endpoint + "clientsCount");
  }

  getAllMonths() {
    return this.http.get<any>(endpoint + "clientsMonth");
  }

  getAllProducts() {
    return this.http.get<any>(endpoint + "clientsProduct");
  }
}


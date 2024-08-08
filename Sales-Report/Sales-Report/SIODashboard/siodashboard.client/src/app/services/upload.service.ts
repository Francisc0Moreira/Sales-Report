import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';


const endpoint = 'http://localhost:5093/api/upload/RunSaft';
const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
};


@Injectable({
  providedIn: 'root'
})
export class UploadService {

  constructor(private http: HttpClient) { }


  executeSaft(saft: FormData) {
    return this.http.post<any>(endpoint, saft);
  }


}

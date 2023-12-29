import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConsultasService {

  private apiURL = "https://localhost:7189/api/";

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  constructor(private httpClient: HttpClient) { }

  find(cedula:string): Observable<any> {
    return this.httpClient.get(this.apiURL + 'Asegurados/consultar/' + cedula)
  }

  findCodigo(codigo:string): Observable<any> {
    return this.httpClient.get(this.apiURL + 'Seguros/consultar/' + codigo)
  }
}

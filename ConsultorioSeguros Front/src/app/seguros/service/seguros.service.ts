import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import {  Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { Seguros } from '../interface/seguros';

@Injectable({
  providedIn: 'root'
})
export class SegurosService {

  private apiURL = "https://localhost:7189/api/";

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  constructor(private httpClient: HttpClient) { }

  getAll(): Observable<any> {
  
    return this.httpClient.get(this.apiURL + 'Seguros')
    
  }

  create(seguro:Seguros): Observable<any> {
  
    return this.httpClient.post(this.apiURL + 'Seguros/ingresar', JSON.stringify(seguro), this.httpOptions)
    
  }

  find(id:number): Observable<any> {
  
    return this.httpClient.get(this.apiURL + 'Seguros/' + id)
    
  }
  

  update(id:number, seguro:Seguros): Observable<any> {
  
    return this.httpClient.put(this.apiURL + 'Seguros/' + id, JSON.stringify(seguro), this.httpOptions)
  }

  delete(id:number){
    return this.httpClient.delete(this.apiURL + 'Seguros/' + id, this.httpOptions)

  }
}

import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import {  Observable, throwError } from 'rxjs';
import { Asegurados } from '../interface/Asegurados.interface';


@Injectable({
  providedIn: 'root'
})
export class AseguradoServiceService {

  private apiURL = "https://localhost:7189/api/";

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  }

  constructor(private httpClient: HttpClient) { }

  getAll(): Observable<any> {
  
    return this.httpClient.get(this.apiURL + 'Asegurados')
    
  }

  findForId(id:number): Observable<any> {
  
    return this.httpClient.get(this.apiURL + 'Asegurados/consultarById/' + id)
  }

  create(asegurado:Asegurados): Observable<any> {
  
    return this.httpClient.post(this.apiURL + 'Asegurados/ingresar', JSON.stringify(asegurado), this.httpOptions)
    
  }

  find(id:number): Observable<any> {
  
    return this.httpClient.get(this.apiURL + 'Asegurados/' + id)
    
  }

  findAsegurosWithoutSeguros(id:number): Observable<any> {
    
    return this.httpClient.get(this.apiURL + 'Seguros/consultarSeguros/' + id)
  }

  update(id:number, asegurado:Asegurados): Observable<any> {
  
    return this.httpClient.put(this.apiURL + 'Asegurados/' + id, JSON.stringify(asegurado), this.httpOptions)
  }

  delete(id:number){
    return this.httpClient.delete(this.apiURL + 'Asegurados/' + id, this.httpOptions)

  }

  uploadFile(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    return this.httpClient.post(this.apiURL + 'Asegurados/upload', formData);
  }

  assignSegurosToAsegurado(aseguradoId: number, segurosIds: number[]): Observable<any> {
    const url = `${this.apiURL}Asegurados/AddSeguroToAsegurado/${aseguradoId}`;
    return this.httpClient.post(url, JSON.stringify(segurosIds), this.httpOptions);
  }

  deleteSegurosToAsegurado(aseguradoId: number, segurosIds: number[]): Observable<any> {
    const url = `${this.apiURL}Asegurados/DeleteSeguroToAsegurado/${aseguradoId}`;
    return this.httpClient.post(url, JSON.stringify(segurosIds), this.httpOptions);
  }
}

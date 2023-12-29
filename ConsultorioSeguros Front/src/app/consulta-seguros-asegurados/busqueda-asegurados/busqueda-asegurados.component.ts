import { Component } from '@angular/core';
import { ConsultasService } from '../service/consultas.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AseguradoServiceService } from 'src/app/asegurados/service/asegurado-service.service';
import { Asegurados } from 'src/app/asegurados/interface/Asegurados.interface';

@Component({
  selector: 'app-busqueda-asegurados',
  templateUrl: './busqueda-asegurados.component.html',
  styleUrls: ['./busqueda-asegurados.component.css']
})
export class BusquedaAseguradosComponent {

  response!: any;
  searchText = '';
  message? : string;
  errorResponse?: boolean;
  responseAsegurado!: any;
  dataAsegurado: Asegurados[] = [];

  constructor(public consultarService: ConsultasService, private router: Router, private toastr: ToastrService, public aseguradoService: AseguradoServiceService) { }

  searchAsegurado(){
    this.consultarService.find(this.searchText).subscribe((data: any)=>{
      this.response = data.data;
      this.message = data.message;
      this.errorResponse = data.error;
      this.toastr.clear();
      this.aseguradoService.getAll().subscribe((data: any)=>{
        this.responseAsegurado = data.data;
        data.data.forEach((element: any) => {
          if(element.cedula == this.searchText) {
            this.aseguradoService.find(element.id).subscribe((data: any)=>{
              this.dataAsegurado = data.data;
              console.log(this.dataAsegurado);
            })
          }
        });
      })

      if(this.errorResponse) {
        this.toastr.error(this.message);
      } else {
        this.toastr.success(this.message);      
      }
      

    });
  }

  numericOnly(event: any): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    if(event.target.value.length > 9) return false;
    return true;
  }

}

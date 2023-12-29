import { Component } from '@angular/core';
import { ConsultasService } from '../service/consultas.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-busqueda-seguros',
  templateUrl: './busqueda-seguros.component.html',
  styleUrls: ['./busqueda-seguros.component.css']
})
export class BusquedaSegurosComponent {

  response!: any;
  searchText = '';
  message? : string;
  errorResponse?: boolean;

  constructor(public consultarService: ConsultasService, private toastr: ToastrService) { }

  searchSeguro(){
    this.consultarService.findCodigo(this.searchText).subscribe((data: any)=>{
      this.response = data.data;
      this.message = data.message;
      this.errorResponse = data.error;
      this.toastr.clear();
      if(this.errorResponse) {
        this.toastr.error(this.message);
      } else {
        this.toastr.success(this.message);
      }

    });
  }

  limitLetter(event: any): boolean {
    if(event.target.value.length > 100) return false;
    return true;
  }
  
  blockPaste(event: ClipboardEvent): void {
    event.preventDefault();
  }
}

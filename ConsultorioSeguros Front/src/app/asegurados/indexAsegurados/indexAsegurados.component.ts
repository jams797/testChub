import { Component } from '@angular/core';
import { Asegurados } from '../interface/Asegurados.interface';
import { ResponseJson } from '../interface/ResponseJson.interface';
import { AseguradoServiceService } from '../service/asegurado-service.service';
import * as bootstrap from 'bootstrap';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-index',
  templateUrl: './indexAsegurados.component.html',
  styleUrls: ['./indexAsegurados.component.css']
})
export class IndexAseguradosComponent {
  response?: ResponseJson;
  asegurado: Asegurados[] = [];
  selectedId?: number;
  responseDelete?: ResponseJson;

  constructor(public aseguradoService: AseguradoServiceService, private toastr: ToastrService, private router: Router){ }

  ngOnInit(): void {
    this.aseguradoService.getAll().subscribe((data: ResponseJson)=>{
      this.response = data;
      this.asegurado = this.response.data;
    })
  }

  deleteAsegurado(id:number){
    this.aseguradoService.delete(id).subscribe((res: any) => {
      //this.asegurado = this.asegurado.filter(item => item.id !== id);
      this.responseDelete = res;
      if (this.responseDelete) {
        this.toastr.clear();
        res = this.responseDelete;
        if(this.responseDelete.error) {
          this.toastr.error(this.responseDelete.message);
        } else {
          this.toastr.success(this.responseDelete.message);
          this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
            this.router.navigateByUrl('asegurados/index');
          }); 
        }
      }
      
    })
  }

  openModal(id: number) {
    this.selectedId = id;
    var deleteModalElement = document.getElementById('deleteModal');
    if (deleteModalElement) {
      var myModal = new bootstrap.Modal(deleteModalElement);
      myModal.show();
    }
  }

  confirmDelete() {
    this.deleteAsegurado(this.selectedId || 0);
    
  }

}

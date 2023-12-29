import { Component } from '@angular/core';
import { Seguros } from '../interface/seguros';
import { SegurosService } from '../service/seguros.service';
import { ResponseJson } from '../interface/ResponseJson.interface';
import * as bootstrap from 'bootstrap';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent {
  response?: ResponseJson;
  seguros: Seguros[] = [];
  selectedId? : number;

  constructor(public seguroService: SegurosService){ }

  ngOnInit(): void {
    this.seguroService.getAll().subscribe((data: ResponseJson)=>{
      this.response = data;
      this.seguros = this.response.data;
    })  
  }

  deleteSeguro(id:number){
    this.seguroService.delete(id).subscribe(res => {
         this.seguros = this.seguros.filter(item => item.id !== id);
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
    this.deleteSeguro(this.selectedId || 0);
    
  }

}

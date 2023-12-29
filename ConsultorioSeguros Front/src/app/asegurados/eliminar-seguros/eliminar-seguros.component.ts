import { Component } from '@angular/core';
import { Seguros } from 'src/app/seguros/interface/seguros';
import { ResponseJson } from '../interface/ResponseJson.interface';
import { Asegurados } from '../interface/Asegurados.interface';
import { AseguradoServiceService } from '../service/asegurado-service.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SegurosService } from 'src/app/seguros/service/seguros.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-eliminar-seguros',
  templateUrl: './eliminar-seguros.component.html',
  styleUrls: ['./eliminar-seguros.component.css']
})
export class EliminarSegurosComponent {

  aseguradoId?: number;
  segurosIds?: number[];
  seguros: Seguros[] = [];
  response?: ResponseJson;
  asegurado: Asegurados[] = [];


  constructor(public aseguroService: AseguradoServiceService, public seguroService: SegurosService,private router: Router,  private route: ActivatedRoute,
    private toastr: ToastrService) { 
      this.segurosIds = [];
    }

    ngOnInit(): void {
      this.route.params.subscribe(params => {
        this.aseguradoId = parseInt(this.route.snapshot.paramMap.get('aseguradoId') || '', 10);

        this.aseguroService.find(this.aseguradoId).subscribe((data: ResponseJson)=>{
          this.response = data;
          this.asegurado = data.data;
        });


        this.aseguroService.findForId(this.aseguradoId).subscribe(
          (data) => {
            this.seguros = data.data;
          });
      });
    }

    onCheckboxChange(e: Event) {
      const input = e.target as HTMLInputElement;
      const id = Number(input.value);

      if (input.checked) {
        this.segurosIds?.push(id);
      } else {
        const index = this.segurosIds?.indexOf(id);
        if (index !== undefined && index > -1) {
          this.segurosIds?.splice(index, 1);
        }
      }
    }

    deleteSeguros(){
      if (this.aseguradoId !== undefined && this.segurosIds !== undefined) {
        this.aseguroService.deleteSegurosToAsegurado(this.aseguradoId, this.segurosIds).subscribe((data: ResponseJson)=>{
          this.toastr.clear();
          if(data.error) this.toastr.error(data.message);
          if(!data.error) this.toastr.success(data.message);
          this.router.navigateByUrl('asegurados/index');
        })
      } else {
        this.toastr.error('Debe seleccionar al menos un seguro');
      }
    }


}

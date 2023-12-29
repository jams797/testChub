import { Component } from '@angular/core';
import { AseguradoServiceService } from '../service/asegurado-service.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { ResponseJson } from '../interface/ResponseJson.interface';
import { SegurosService } from 'src/app/seguros/service/seguros.service';
import { Seguros } from 'src/app/seguros/interface/seguros';
import { Asegurados } from '../interface/Asegurados.interface';

@Component({
  selector: 'app-agregar-seguros',
  templateUrl: './agregar-seguros.component.html',
  styleUrls: ['./agregar-seguros.component.css']
})
export class AgregarSegurosComponent {

  aseguradoId?: number;
  segurosIds?: number[];
  seguros: Seguros[] = [];
  response?: ResponseJson;
  asegurado: Asegurados[] = [];
  message?: string;
  errorData?: boolean;
  seguroWithAsegurado: Seguros[] = [];
  seguroCombined: Seguros[] = []; 
  seguroEnd: Seguros[] = [];


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

          this.seguroService.getAll().subscribe(
            (data) => {
              this.seguros = data.data;
              if(this.aseguradoId !== undefined){
                this.aseguroService.findForId(this.aseguradoId).subscribe(
                  (data) => {
                    this.seguroWithAsegurado = data.data;
                    if(this.seguroWithAsegurado != null){
                      this.seguroCombined = this.seguros.concat(this.seguroWithAsegurado);      
                    const countMap = new Map();
                    this.seguroCombined.forEach(item => {
                      countMap.set(item.id, (countMap.get(item.id) || 0) + 1);
                    });
            
                    this.seguroEnd = this.seguroCombined.filter(item => countMap.get(item.id) === 1);
                    }
                    else{
                      this.seguroEnd = this.seguros;
                    }
                  });
              }
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

    addSeguros(){
      if (this.aseguradoId !== undefined && this.segurosIds !== undefined) {
        this.aseguroService.assignSegurosToAsegurado(this.aseguradoId, this.segurosIds).subscribe((data: ResponseJson)=>{
          this.message = data.message;
          this.errorData = data.error;
          this.toastr.clear();
          if(this.errorData) this.toastr.error(this.message);
          if(!this.errorData) this.toastr.success(this.message);
          this.router.navigateByUrl('asegurados/index');
          
        })
      } else {
        this.toastr.error('Debe seleccionar al menos un seguro');
      }
    }
}

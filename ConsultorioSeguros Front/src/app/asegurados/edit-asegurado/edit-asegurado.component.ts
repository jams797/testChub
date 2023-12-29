import { Component } from '@angular/core';
import { ResponseJson } from '../interface/ResponseJson.interface';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Asegurados } from '../interface/Asegurados.interface';
import { AseguradoServiceService } from '../service/asegurado-service.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit-asegurado',
  templateUrl: './edit-asegurado.component.html',
  styleUrls: ['./edit-asegurado.component.css']
})
export class EditAseguradoComponent {

  aseguradoId!: number;
  response!: ResponseJson;
  form!: FormGroup;
  asegurado: Asegurados[] = [];
  message!: string;
  errorData!: boolean;

  constructor(public aseguradoService: AseguradoServiceService, private route: ActivatedRoute, private router: Router,
    private toastr: ToastrService) 
    { }

    ngOnInit(): void {
      this.route.params.subscribe(params => {
        this.aseguradoId = parseInt(this.route.snapshot.paramMap.get('aseguradoId') || '', 10);
        this.aseguradoService.find(this.aseguradoId).subscribe((data: ResponseJson)=>{
          this.asegurado = data.data;

          
  
          this.form.patchValue({
            cedula: this.asegurado[0].cedula,
            nombre: this.asegurado[0].nombre,
            correo: this.asegurado[0].correo,
            telefono: this.asegurado[0].telefono,
            edad: this.asegurado[0].edad
          });
          
          })
        });
  
        this.form = new FormGroup({
          cedula: new FormControl('', [Validators.required]),
          nombre: new FormControl('', Validators.required),
          correo: new FormControl('', [Validators.required, Validators.email]),
          telefono: new FormControl('', Validators.required),
          edad: new FormControl({value: '', disabled: true}, [Validators.required, Validators.pattern("^[0-9]*$")])
        });
    }
    
  
    get f(){
      return this.form.controls;
    }
  
    submit(){
      let formValues = this.form.value;
      formValues.edad = this.form.get('edad')?.value;
    
      this.aseguradoService.update(this.aseguradoId, formValues).subscribe((data: ResponseJson) => {
        this.message = data.message;
        this.errorData = data.error;
        this.toastr.clear();
        if(this.errorData) this.toastr.error(this.message);
        if(!this.errorData) {
          this.toastr.success(this.message);
          this.router.navigateByUrl('asegurados/index');
        }
      })
    }

    numericOnly(event: any): boolean {
      const charCode = (event.which) ? event.which : event.keyCode;
      if (charCode > 31 && (charCode < 48 || charCode > 57)) {
          return false;
      }
      if(event.target.value.length > 9) return false;
      return true;
    }
  
    limitLetter(event: any): boolean {
      if(event.target.value.length > 100) return false;
      return true;
    }
  
    numericLenght(event: any): boolean {
      if(event.target.value.length > 1) return false;
      return true;
    }
  
    blockPaste(event: ClipboardEvent): void {
      event.preventDefault();
    }
}

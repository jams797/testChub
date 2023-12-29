import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ResponseJson } from '../interface/ResponseJson.interface';
import { AseguradoServiceService } from '../service/asegurado-service.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-asegurado',
  templateUrl: './create-asegurado.component.html',
  styleUrls: ['./create-asegurado.component.css']
})
export class CreateAseguradoComponent {

  form!: FormGroup;
  message!: string;
  errorData!: boolean;
  response!: ResponseJson;

  constructor(public aseguradoService: AseguradoServiceService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.form = new FormGroup({
      cedula: new FormControl('', [Validators.required, Validators.pattern("^[0-9]*$")]),
      nombre: new FormControl('', Validators.required),
      correo: new FormControl('', [Validators.required, Validators.email]),
      telefono: new FormControl('', Validators.required),
      edad: new FormControl('', [Validators.required, Validators.pattern("^[0-9]*$")])
    });

  }

  get f(){
    return this.form.controls;
  }

  submit(){
    this.aseguradoService.create(this.form.value).subscribe((data:ResponseJson) => {
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

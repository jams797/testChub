import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { SegurosService } from '../service/seguros.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ResponseJson } from '../interface/ResponseJson.interface';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.css']
})
export class CreateComponent {
  form!: FormGroup;
  message!: string;
  errorData!: boolean;
  response!: ResponseJson;

  constructor(public seguroServie: SegurosService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.form = new FormGroup({
      nombre: new FormControl('', [Validators.required]),
      codigo: new FormControl('', Validators.required),
      sumaAsegurada: new FormControl('', Validators.required),
      prima: new FormControl('', Validators.required),
      ramo: new FormControl('', Validators.required),
    });
  }

  get f(){
    return this.form.controls;
  }

  submit(){
    this.seguroServie.create(this.form.value).subscribe((data:ResponseJson) => {
      this.message = data.message;
      this.errorData = data.error;
      this.toastr.clear();
      if(this.errorData) this.toastr.error(this.message);
      if(!this.errorData) {
        this.toastr.success(this.message);
        this.router.navigateByUrl('seguros/index');
      }
    })
  }

  decimalFilter(event: any) {
    const reg = /^-?\d*(\.\d{0,2})?$/;
    let input = event.target.value + String.fromCharCode(event.charCode);

    if (!reg.test(input)) {
        event.preventDefault();
    }
}



limitLetter(event: any): boolean {
if(event.target.value.length > 100) return false;
return true;
}

blockPaste(event: ClipboardEvent): void {
  event.preventDefault();
} 

}

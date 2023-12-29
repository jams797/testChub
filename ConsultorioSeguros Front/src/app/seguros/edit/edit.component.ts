import { Component } from '@angular/core';
import { Seguros } from '../interface/seguros';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { SegurosService } from '../service/seguros.service';
import { ToastrService } from 'ngx-toastr'
import { ActivatedRoute, Router } from '@angular/router';
import { ResponseJson } from '../interface/ResponseJson.interface';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class EditComponent {

  seguroId!: number;
  response!: ResponseJson;
  form!: FormGroup;
  seguro: Seguros[] = [];
  message!: string;
  errorData!: boolean;

  constructor(public seguroService: SegurosService, private route: ActivatedRoute, private router: Router,
    private toastr: ToastrService) 
    { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.seguroId = parseInt(this.route.snapshot.paramMap.get('seguroId') || '', 10);
      this.seguroService.find(this.seguroId).subscribe((data: ResponseJson)=>{
        this.seguro = data.data;


        this.form.patchValue({
          nombre: this.seguro[0].nombre,
          codigo: this.seguro[0].codigo,
          sumaAsegurada: this.seguro[0].sumaAsegurada,
          prima: this.seguro[0].prima,
          ramo: this.seguro[0].ramo
        });
    });
  });
  
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
    console.log(this.form.value);
    this.seguroService.update(this.seguroId, this.form.value).subscribe((data: ResponseJson) => {
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
 
 blockPaste(event: ClipboardEvent): void {
  event.preventDefault();
  }

 limitLetter(event: any): boolean {
  if(event.target.value.length > 100) return false;
  return true;
  }
}

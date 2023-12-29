import { Component } from '@angular/core';
import { AseguradoServiceService } from '../service/asegurado-service.service';
import { ResponseJson } from '../interface/ResponseJson.interface';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-file-asegurado',
  templateUrl: './create-file-asegurado.component.html',
  styleUrls: ['./create-file-asegurado.component.css']
})
export class CreateFileAseguradoComponent {

  response?: ResponseJson;
  message?: string;
  errorData?: boolean;

  constructor(public aseguradoService: AseguradoServiceService, private toastr: ToastrService, private router: Router){ }  
  
  uploadFile(event: any) {
    let file = event.target.files[0];
    this.aseguradoService.uploadFile(file).subscribe((data: ResponseJson)=>{
      this.response = data;
      this.message = this.response.message;
      this.errorData = this.response.error;
      this.toastr.clear();
      if(this.errorData) this.toastr.error(this.message);
      if(!this.errorData) {
        this.toastr.success(this.message);
        this.router.navigateByUrl('asegurados/index');
      }
    })  
  }
}

import { Component } from '@angular/core';
import { ResponseJson } from '../interface/ResponseJson.interface';
import { Asegurados } from '../interface/Asegurados.interface';
import { AseguradoServiceService } from '../service/asegurado-service.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-view-asegurados',
  templateUrl: './view-asegurados.component.html',
  styleUrls: ['./view-asegurados.component.css']
})
export class ViewAseguradosComponent {
  aseguradoId!: number;
  response!: ResponseJson;
  asegurado: Asegurados[] = [];

  constructor(public aseguradoService: AseguradoServiceService, private route: ActivatedRoute, private router: Router) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.aseguradoId = parseInt(this.route.snapshot.paramMap.get('aseguradoId') || '', 10);

      this.aseguradoService.find(this.aseguradoId).subscribe((data: ResponseJson)=>{
        this.response = data;
        this.asegurado = data.data;
      });

    });
  }

}

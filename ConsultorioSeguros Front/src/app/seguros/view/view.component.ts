import { Component } from '@angular/core';
import { ResponseJson } from '../interface/ResponseJson.interface';
import { Seguros } from '../interface/seguros';
import { SegurosService } from '../service/seguros.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-view',
  templateUrl: './view.component.html'
})
export class ViewComponent {

  seguroId!: number;
  response!: ResponseJson;
  seguro: Seguros[] = [];

  constructor(public segurosService: SegurosService, private route: ActivatedRoute, private router: Router) { }


  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.seguroId = parseInt(this.route.snapshot.paramMap.get('seguroId') || '', 10);

      this.segurosService.find(this.seguroId).subscribe((data: ResponseJson)=>{
        this.response = data;
        this.seguro = data.data;
      });

    });
  }

}

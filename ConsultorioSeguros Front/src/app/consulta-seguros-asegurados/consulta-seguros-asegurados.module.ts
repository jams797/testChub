import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms'; 
import { ReactiveFormsModule } from '@angular/forms';

import { IndexComponentConsulta } from './index/index.component';
import { BusquedaSegurosComponent } from './busqueda-seguros/busqueda-seguros.component';
import { BusquedaAseguradosComponent } from './busqueda-asegurados/busqueda-asegurados.component';



@NgModule({
  declarations: [
    IndexComponentConsulta,
    BusquedaSegurosComponent,
    BusquedaAseguradosComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class ConsultaSegurosAseguradosModule { }

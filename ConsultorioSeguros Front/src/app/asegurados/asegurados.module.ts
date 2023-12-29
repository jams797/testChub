import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {RouterModule} from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';

import { IndexAseguradosComponent } from './indexAsegurados/indexAsegurados.component';
import { ViewAseguradosComponent } from './view-asegurados/view-asegurados.component';
import { EditAseguradoComponent } from './edit-asegurado/edit-asegurado.component';
import { CreateAseguradoComponent } from './create-asegurado/create-asegurado.component';
import { CreateFileAseguradoComponent } from './create-file-asegurado/create-file-asegurado.component';
import { AgregarSegurosComponent } from './agregar-seguros/agregar-seguros.component';
import { EliminarSegurosComponent } from './eliminar-seguros/eliminar-seguros.component';




@NgModule({
  declarations: [
    IndexAseguradosComponent,
    ViewAseguradosComponent,
    EditAseguradoComponent,
    CreateAseguradoComponent,
    CreateFileAseguradoComponent,
    AgregarSegurosComponent,
    EliminarSegurosComponent,

  ],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    MatFormFieldModule,
  ]
})
export class AseguradosModule { }

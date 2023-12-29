import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexComponent } from './seguros/index/index.component';
import { ViewComponent } from './seguros/view/view.component';
import { CreateComponent } from './seguros/create/create.component';
import { EditComponent } from './seguros/edit/edit.component';
import { IndexAseguradosComponent } from './asegurados/indexAsegurados/indexAsegurados.component';
import { ViewAseguradosComponent } from './asegurados/view-asegurados/view-asegurados.component';
import { CreateAseguradoComponent } from './asegurados/create-asegurado/create-asegurado.component';
import { EditAseguradoComponent } from './asegurados/edit-asegurado/edit-asegurado.component';
import { CreateFileAseguradoComponent } from './asegurados/create-file-asegurado/create-file-asegurado.component';
import { AgregarSegurosComponent } from './asegurados/agregar-seguros/agregar-seguros.component';
import { IndexComponentConsulta } from './consulta-seguros-asegurados/index/index.component';
import { EliminarSegurosComponent } from './asegurados/eliminar-seguros/eliminar-seguros.component';

const routes: Routes = [
  { path: 'seguros', redirectTo: '/seguros/index', pathMatch: 'full' },
  { path: 'seguros/index', component: IndexComponent},
  { path: 'seguros/:seguroId/view', component: ViewComponent},
  { path: 'seguros/create', component: CreateComponent},
  { path: 'seguros/:seguroId/edit', component: EditComponent},
  { path: 'asegurados', redirectTo: '/asegurados/index', pathMatch: 'full' },
  { path: 'asegurados/index', component: IndexAseguradosComponent},
  { path: 'asegurados/:aseguradoId/view', component: ViewAseguradosComponent},
  { path: 'asegurados/create', component: CreateAseguradoComponent},
  { path: 'asegurados/:aseguradoId/edit', component: EditAseguradoComponent},
  { path: 'asegurados/aseguradoUpload', component: CreateFileAseguradoComponent},
  { path: 'asegurados/:aseguradoId/agregarSeguros', component: AgregarSegurosComponent},
  { path: 'asegurados/:aseguradoId/eliminarSeguros', component: EliminarSegurosComponent},
  { path: '**', component: IndexComponentConsulta},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

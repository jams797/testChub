import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import {RouterModule} from '@angular/router';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule, ToastNoAnimationModule } from 'ngx-toastr';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SegurosModule } from './seguros/seguros.module';
import { AseguradosModule } from './asegurados/asegurados.module';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { ConsultaSegurosAseguradosModule } from './consulta-seguros-asegurados/consulta-seguros-asegurados.module';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    ToastNoAnimationModule.forRoot(),
    RouterModule,
    AppRoutingModule,
    SegurosModule,
    AseguradosModule,
    ConsultaSegurosAseguradosModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

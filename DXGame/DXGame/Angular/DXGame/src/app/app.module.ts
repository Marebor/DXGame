import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';

import { CardsService } from './cards.service';
import { CardsAdministrationComponent } from './cards-administration/cards-administration.component';

@NgModule({
  declarations: [
    AppComponent,
    CardsAdministrationComponent
  ],
  imports: [
    BrowserModule,
    HttpModule
  ],
  providers: [
	  CardsService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

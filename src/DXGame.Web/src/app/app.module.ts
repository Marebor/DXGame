import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { GameContextService } from './game-center/game-context.service';
import { PlayerService } from './player/player.service';
import { AppPreloadingStrategy } from './app-preloading-strategy';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { WelcomeComponent } from './welcome/welcome.component';
import { PlayroomService } from './playroom/playroom.service';

@NgModule({
  declarations: [
    AppComponent,
    WelcomeComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule
  ],
  providers: [
    AppPreloadingStrategy,
    PlayerService,
    PlayroomService,
    GameContextService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

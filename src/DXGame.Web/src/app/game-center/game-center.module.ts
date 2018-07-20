import { GameCenterRoutingModule } from './game-center-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GameCenterComponent } from './game-center.component';

@NgModule({
  imports: [
    CommonModule,
    GameCenterRoutingModule
  ],
  declarations: [GameCenterComponent]
})
export class GameCenterModule { }

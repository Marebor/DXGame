import { FormsModule } from '@angular/forms';
import { PlayerRoutingModule } from './player-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayerCreationComponent } from './player-creation/player-creation.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    PlayerRoutingModule
  ],
  declarations: [PlayerCreationComponent]
})
export class PlayerModule { }

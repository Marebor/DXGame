import { PlayerRoutingModule } from './player-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayerCreationComponent } from './player-creation/player-creation.component';
import { PlayerService } from './player.service';

@NgModule({
  imports: [
    CommonModule,
    PlayerRoutingModule
  ],
  declarations: [PlayerCreationComponent],
  providers: [PlayerService]
})
export class PlayerModule { }

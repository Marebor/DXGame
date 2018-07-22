import { PlayroomService } from './playroom.service';
import { PlayroomRoutingModule } from './playroom-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayroomCreationComponent } from './playroom-creation/playroom-creation.component';

@NgModule({
  imports: [
    CommonModule,
    PlayroomRoutingModule
  ],
  declarations: [PlayroomCreationComponent]
})
export class PlayroomModule { }

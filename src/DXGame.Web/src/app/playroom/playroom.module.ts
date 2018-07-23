import { PlayroomRoutingModule } from './playroom-routing.module';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PlayroomCreationComponent } from './playroom-creation/playroom-creation.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    PlayroomRoutingModule
  ],
  declarations: [PlayroomCreationComponent]
})
export class PlayroomModule { }

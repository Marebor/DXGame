import { FormsModule } from '@angular/forms';
import { 
  MatListModule, 
  MatButtonModule, 
  MatIconModule, 
  MatFormFieldModule, 
  MatInputModule, 
  MatCheckboxModule 
} from '@angular/material';
import { PlayerRoutingModule } from './player-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlayerCreationComponent } from './player-creation/player-creation.component';
import { PlayersListComponent } from './players-list/players-list.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MatListModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    PlayerRoutingModule
  ],
  declarations: [PlayerCreationComponent, PlayersListComponent]
})
export class PlayerModule { }

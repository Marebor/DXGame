import { PlayerCreationComponent } from './player-creation/player-creation.component';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { PlayersListComponent } from './players-list/players-list.component';

const ROUTES: Routes = [
  { 
    path: 'create-player', 
    component: PlayerCreationComponent
  },
  {
    path: '',
    component: PlayersListComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(ROUTES)
  ],
  exports: [
    RouterModule
  ]
})
export class PlayerRoutingModule { }

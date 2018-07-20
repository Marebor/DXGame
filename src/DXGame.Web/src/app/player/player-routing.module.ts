import { PlayerCreationComponent } from './player-creation/player-creation.component';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

const ROUTES: Routes = [
  { 
    path: 'create-player', 
    component: PlayerCreationComponent
  },
  {
    path: '',
    redirectTo: '/create-player',
    pathMatch: 'full'
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

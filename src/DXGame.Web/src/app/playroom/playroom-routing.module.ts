import { PlayroomCreationComponent } from './playroom-creation/playroom-creation.component';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

const ROUTES: Routes = [
  { 
    path: 'create-playroom', 
    component: PlayroomCreationComponent
  },
  {
    path: '',
    redirectTo: '/create-playroom',
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
export class PlayroomRoutingModule { }

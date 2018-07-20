import { AppPreloadingStrategy } from './app-preloading-strategy';
import { WelcomeComponent } from './welcome/welcome.component';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

const ROUTES: Routes = [
  { 
    path: 'welcome', 
    component: WelcomeComponent
  },
  {
    path: 'game-center',
    loadChildren: './game-center/game-center.module#GameCenterModule'
  },
  {
    path: 'players',
    loadChildren: './player/player.module#PlayerModule'
  },
  {
    path: '',
    redirectTo: '/welcome',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [
    RouterModule.forRoot(ROUTES, { preloadingStrategy: AppPreloadingStrategy })
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule { }

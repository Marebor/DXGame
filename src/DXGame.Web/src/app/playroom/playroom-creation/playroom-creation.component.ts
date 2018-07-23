import { Player } from './../../player/player';
import { GameContextService } from '../../game-center/game-context.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { PlayroomService } from '../playroom.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-playroom-creation',
  templateUrl: './playroom-creation.component.html',
  styleUrls: ['./playroom-creation.component.css']
})
export class PlayroomCreationComponent {
  private activePlayer: Player;

  constructor(private gameContext: GameContextService, private playroomService: PlayroomService) { }

  createPlayroom(name: string, isPrivate: boolean, password?: string) {
    if (!this.gameContext.activePlayer) {
      return;
    }
    this.playroomService
    .createPlayroom(name, this.gameContext.activePlayer.id, isPrivate, password)
    .subscribe(
      response => this.gameContext.switchPlayroom(response),
      error => console.log(error)
    )
  }
}

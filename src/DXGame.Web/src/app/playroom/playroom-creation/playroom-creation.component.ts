import { Player } from './../../player/player';
import { GameContextService } from '../../game-center/game-context.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { PlayroomService } from '../playroom.service';
import { Subscription } from 'rxjs';
import { Playroom } from '../playroom';

@Component({
  selector: 'app-playroom-creation',
  templateUrl: './playroom-creation.component.html',
  styleUrls: ['./playroom-creation.component.css']
})
export class PlayroomCreationComponent {
  private activePlayer: Player;
  private model: Playroom = new Playroom("", "", false, "");
  private password: string;

  constructor(private gameContext: GameContextService, private playroomService: PlayroomService) { }

  onSubmit() {
    if (!this.gameContext.activePlayer) {
      return;
    }
    this.playroomService
    .createPlayroom(this.model.name, this.gameContext.activePlayer.id, this.model.isPrivate, this.password)
    .subscribe(
      response => { this.gameContext.switchPlayroom(response); },
      error => console.log(error)
    )
    this.model = new Playroom("", "", false, "");
    this.password = null;
  }
}

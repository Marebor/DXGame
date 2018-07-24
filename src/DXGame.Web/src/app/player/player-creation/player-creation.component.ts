import { Player } from '../player';
import { GameContextService } from '../../game-center/game-context.service';
import { PlayerService } from '../player.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-player-creation',
  templateUrl: './player-creation.component.html',
  styleUrls: ['./player-creation.component.css']
})
export class PlayerCreationComponent {
  private model: Player = new Player(null, false);
  private password: string;

  constructor(private playerService: PlayerService, private gameContext: GameContextService) { }

  onSubmit() {
    this.playerService.createPlayer(this.model.name, this.model.protectedWithPassword, this.password).subscribe(
      response => this.gameContext.switchPlayer(response),
      error => console.log(error)
    )
  }
}

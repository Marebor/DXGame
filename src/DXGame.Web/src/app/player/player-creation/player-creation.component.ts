import { GameContextService } from './../../game-center/game-context.service';
import { PlayerService } from '../player.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-player-creation',
  templateUrl: './player-creation.component.html',
  styleUrls: ['./player-creation.component.css']
})
export class PlayerCreationComponent implements OnInit {

  constructor(private playerService: PlayerService, private gameContext: GameContextService) { }

  ngOnInit() {
  }

  createPlayer(name: string) {
    this.playerService.createPlayer(name).subscribe(
      response => this.gameContext.switchPlayer(response),
      error => console.log(error)
    )
  }
}

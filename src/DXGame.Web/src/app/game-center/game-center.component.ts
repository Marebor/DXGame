import { Player } from './../player/player';
import { Playroom } from './../playroom/playroom';
import { GameContextService } from './game-context.service';
import { PlayroomService } from '../playroom/playroom.service';
import { PlayerService } from '../player/player.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-game-center',
  templateUrl: './game-center.component.html',
  styleUrls: ['./game-center.component.css']
})
export class GameCenterComponent implements OnInit {

  players: Player[] = [];

  constructor(private playerService: PlayerService, private playroomService: PlayroomService, 
    private gameContext: GameContextService) { }

  ngOnInit() {
    this.playerService.localPlayers$.subscribe(
      next => this.players = next,
      error => console.log(error)
    )
  }

  trackPlayroomById(index: number, playroom: Playroom) {
    return playroom.id;
  }

  trackPlayerById(index: number, player: Player) {
    return player.id;
  }
}
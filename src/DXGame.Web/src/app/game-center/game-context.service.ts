import { PlayroomService } from '../playroom/playroom.service';
import { PlayerService } from '../player/player.service';
import { Playroom } from '../playroom/playroom';
import { Injectable } from '@angular/core';
import { Player } from '../player/player';
import { Observable, Subject, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GameContextService {
  public localPlayers: Player[] = [];
  public activePlayer: Player;

  constructor() { }

  switchPlayer(player: Player) {
    console.log("Switching player to: " + JSON.stringify(player));
    if (!this.localPlayers.find(p => p.id === player.id)) {
      this.localPlayers.push(player);
    }
    this.activePlayer = player;
  }

  switchPlayroom(playroom: Playroom) {
    if (!this.activePlayer) {
      throw new Error("No player selected.");
    }
    console.log("Switching playroom to: " + JSON.stringify(playroom))
    if (!this.activePlayer.playrooms.find(p => p.id == playroom.id)) {
      this.activePlayer.playrooms.push(playroom);
    }
    this.activePlayer.activePlayroom = playroom;
  }
}

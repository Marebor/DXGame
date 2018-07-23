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
  private playersCache = [];
  public activePlayer: Player;

  constructor(private playerService: PlayerService, private playroomService: PlayroomService) { }

  private cachePlayer() {
    let val = JSON.stringify(this.activePlayer);
    let index = this.playersCache.findIndex(c => c.id == this.activePlayer.id);
    if (index !== -1) {
      this.playersCache[index] = val;
    }
    else {
      this.playersCache.push(val);
    }
  }

  switchPlayer(player: Player) {
    console.log("Switching player to: " + JSON.stringify(player))
    if (this.activePlayer) {
      this.cachePlayer();
    }
    this.activePlayer = player;
  }

  switchPlayroom(playroom: Playroom) {
    console.log("Switching playroom to: " + JSON.stringify(playroom))
    if (this.activePlayer && !this.activePlayer.playrooms.find(p => p.id == playroom.id)) {
      this.activePlayer.playrooms.push(playroom);
    }
    this.activePlayer.activePlayroom = playroom;
  }
}

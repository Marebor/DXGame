import { PlayroomService } from '../playroom/playroom.service';
import { PlayerService } from '../player/player.service';
import { Playroom } from '../playroom/playroom';
import { Injectable } from '@angular/core';
import { Player } from '../player/player';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GameContextService {
  private activePlayerSubject$: Subject<Player>;
  private playersCache = [];
  public activePlayer: Player;
  public activePlayer$: Observable<Player>;

  constructor(private playerService: PlayerService, private playroomService: PlayroomService) {
    this.activePlayerSubject$ = new Subject<Player>();
    this.activePlayer$ = this.activePlayerSubject$;
  }

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
    this.activePlayerSubject$.next(this.activePlayer);
  }

  switchPlayroom(playroom: Playroom) {
    this.activePlayer.activePlayroom = playroom;
  }
}

import { Player } from './player';
import { Observable, of, throwError, Subject } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  private localPlayers: Player[] = [];
  public localPlayers$: Observable<Player[]> = of(this.localPlayers);

  constructor() { }

  createPlayer(name: string) : Observable<Player> {
    if (this.localPlayers.find(p => p.name == name)) {
      return throwError(new Error("Player already exists"));
    }
    else {
      let player = new Player(name);
      this.localPlayers.push(player);
      return of(player);
    }
  }
}

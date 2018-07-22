import { Player } from './player';
import { Observable, of, throwError, Subject } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  private existingPlayers: Player[] = [];
  private localPlayersSubject$: Subject<Player[]> = new Subject<Player[]>();
  public localPlayers$: Observable<Player[]> = of(this.existingPlayers);

  constructor() { }

  browsePlayers() {
    return of(this.existingPlayers);
  }

  createPlayer(name: string) : Observable<Player> {
    if (this.existingPlayers.find(p => p.name == name)) {
      return throwError(new Error("Player already exists"));
    }
    else {
      let player = new Player(name);
      this.existingPlayers.push(player);
      return of(player);
    }
  }
}

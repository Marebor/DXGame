import { Player } from './player';
import { Observable, of, throwError, Subject } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  public localPlayers: Player[] = [];

  constructor() { }

  createPlayer(name: string, protectedWithPassword: boolean, password?: string) : Observable<Player> {
    if (this.localPlayers.find(p => p.name == name)) {
      return throwError(new Error("Player already exists"));
    }
    if (protectedWithPassword && !password) {
      return throwError(new Error("Invalid password"))
    }
    else {
      let player = new Player(name, protectedWithPassword);
      this.localPlayers.push(player);
      return of(player);
    }
  }
}

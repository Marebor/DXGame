import { Observable, of, throwError, never } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  existingPlayers: string[] = [];

  constructor() { }

  createPlayer(name: string) : Observable<any> {
    if (this.existingPlayers.find(el => el == name)) {
      return throwError(new Error("Player already exists"));
    }
    else {
      this.existingPlayers.push(name);
      return of(name);
    }
  }
}

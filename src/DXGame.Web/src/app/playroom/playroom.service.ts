import { GameContextService } from '../game-center/game-context.service';
import { Injectable } from '@angular/core';
import { Playroom } from './playroom';
import { throwError, of, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PlayroomService {
  private localPlayrooms: Playroom[] = [];

  constructor() { }

  browsePlayrooms() {
    return of(this.localPlayrooms);
  }

  findPlayroomByName(name: string) {
    return of(this.localPlayrooms.filter(p => p.name.indexOf(name) !== -1));
  }

  createPlayroom(name: string, owner: string, isPrivate: boolean, password: string) {
    if (this.getPlayroom(name)) {
      return throwError(new Error("Playroom already exists."))
    }
    else {
      let playroom = new Playroom(name, owner, isPrivate, password);
      this.localPlayrooms.push(playroom);
      return of(playroom);
    }
  }

  getPlayroom(name: string) {
    return this.localPlayrooms.find(p => p.name == name);
  }
}
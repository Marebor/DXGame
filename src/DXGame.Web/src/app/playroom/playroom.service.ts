import { Injectable } from '@angular/core';
import { Playroom } from './playroom';
import { throwError, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PlayroomService {
  existingPlayrooms: Playroom[] = [];

  constructor() { }

  browsePlayrooms() {
    return of(this.existingPlayrooms);
  }

  findPlayroomByName(name: string) {
    return of(this.existingPlayrooms.filter(p => p.name.indexOf(name) !== -1));
  }

  createPlayroom(name: string, owner: string, isPrivate: boolean, password: string) {
    if (this.getPlayroom(name)) {
      return throwError(new Error("Playroom already exists."))
    }
    else {
      let playroom = new Playroom(name, owner, isPrivate, password);
      return of(playroom);
    }
  }

  getPlayroom(name: string) {
    return this.existingPlayrooms.find(p => p.name == name);
  }
}

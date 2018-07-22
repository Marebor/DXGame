import { GameContextService } from '../../game-center/game-context.service';
import { Component, OnInit } from '@angular/core';
import { PlayroomService } from '../playroom.service';

@Component({
  selector: 'app-playroom-creation',
  templateUrl: './playroom-creation.component.html',
  styleUrls: ['./playroom-creation.component.css']
})
export class PlayroomCreationComponent implements OnInit {

  constructor(private gameContext: GameContextService, private playroomService: PlayroomService) { }

  ngOnInit() {
  }

  createPlayroom(name: string, isPrivate: boolean, password?: string) {
    this.playroomService
    .createPlayroom(name, this.gameContext.activePlayer.id, isPrivate, password)
    .subscribe(
      response => console.log(`Created player: ${JSON.stringify(response)}`),
      error => console.log(error)
    )
  }
}

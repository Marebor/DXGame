import { PlayerService } from './../player.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-player-creation',
  templateUrl: './player-creation.component.html',
  styleUrls: ['./player-creation.component.css']
})
export class PlayerCreationComponent implements OnInit {

  constructor(private playerService: PlayerService) { }

  ngOnInit() {
  }

  createPlayer(name: string) {
    this.playerService.createPlayer(name).subscribe(
      response => console.log(`Created user: ${name}`),
      error => console.log(error)
    )
  }
}

import { Component, OnInit } from '@angular/core';

import { CardsService } from '../cards.service';
import { Card } from '../card';

@Component({
  selector: 'app-cards-administration',
  templateUrl: './cards-administration.component.html',
  styleUrls: ['./cards-administration.component.css']
})
export class CardsAdministrationComponent implements OnInit {

  private cards: Card[] = [];

  constructor(private cardsService: CardsService) { }

  ngOnInit() {
    this.cardsService.getAllCards()
      .then(cards => this.cards = cards)
      .catch(error => null);
  }
}

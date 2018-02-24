import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';

import 'rxjs/add/operator/toPromise';

import { Card } from './card';

@Injectable()
export class CardsService {
	
    private cardsURL: string = "http://localhost:65432/api/cards";

	  constructor(private http: Http) { }
	
	  private handleError(error: any): Promise<any> {
		  console.error('An error occurred: ', error.message || error); // for demo purposes only
		  return Promise.reject(error.message || error);
	  }

    getAllCards(): Promise<Card[]> {
      return this.http.get(this.cardsURL)
			  .toPromise()
			  .then(response => response.json() as Card[])
			  .catch(error => this.handleError(error));
	  }
	
	  getCard(ID: number): Promise<Card> {
		  return this.http.get(`${this.cardsURL}/${ID}`)
			  .toPromise()
			  .then(response => response.json() as Card)
			  .catch(error => this.handleError(error));
    }


    addCards(images: File[]): Promise<Card[]> {
      let form = new FormData();
      let i: number = 0;
      for (let image of images) {
        form.append(`image${++i}`, image);
      }
      return this.http.post(this.cardsURL, form)
        .toPromise()
        .then(response => response.json() as Card[])
        .catch(error => this.handleError(error));
    }

    deleteCard(ID: number): Promise<void> {
      return this.http.delete(`${this.cardsURL}/${ID}`)
        .toPromise()
        .then(response => null)
        .catch(error => this.handleError(error));
    }
}

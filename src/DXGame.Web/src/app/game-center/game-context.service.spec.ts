import { TestBed, inject } from '@angular/core/testing';

import { GameContextService } from './game-context.service';

describe('GameContextService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GameContextService]
    });
  });

  it('should be created', inject([GameContextService], (service: GameContextService) => {
    expect(service).toBeTruthy();
  }));
});

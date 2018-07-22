import { TestBed, inject } from '@angular/core/testing';

import { PlayroomService } from './playroom.service';

describe('PlayroomService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PlayroomService]
    });
  });

  it('should be created', inject([PlayroomService], (service: PlayroomService) => {
    expect(service).toBeTruthy();
  }));
});

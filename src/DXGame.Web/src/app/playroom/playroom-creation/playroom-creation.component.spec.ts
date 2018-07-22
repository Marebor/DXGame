import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayroomCreationComponent } from './playroom-creation.component';

describe('PlayroomCreationComponent', () => {
  let component: PlayroomCreationComponent;
  let fixture: ComponentFixture<PlayroomCreationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlayroomCreationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayroomCreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

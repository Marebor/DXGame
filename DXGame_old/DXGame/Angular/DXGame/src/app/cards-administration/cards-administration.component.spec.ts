import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CardsAdministrationComponent } from './cards-administration.component';

describe('CardsAdministrationComponent', () => {
  let component: CardsAdministrationComponent;
  let fixture: ComponentFixture<CardsAdministrationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardsAdministrationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardsAdministrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});

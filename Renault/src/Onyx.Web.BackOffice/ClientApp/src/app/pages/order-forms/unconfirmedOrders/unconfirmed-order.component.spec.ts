import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UnconfirmedOrderComponent } from './unconfirmed-order.component';

describe('UnconfirmedOrderComponent', () => {
  let component: UnconfirmedOrderComponent;
  let fixture: ComponentFixture<UnconfirmedOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UnconfirmedOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UnconfirmedOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

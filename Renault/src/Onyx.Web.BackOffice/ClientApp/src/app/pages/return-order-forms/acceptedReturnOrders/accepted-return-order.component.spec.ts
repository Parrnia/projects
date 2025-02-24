import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AcceptedReturnOrderComponent } from './accepted-return-order.component';

describe('AcceptedReturnOrderComponent', () => {
  let component: AcceptedReturnOrderComponent;
  let fixture: ComponentFixture<AcceptedReturnOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AcceptedReturnOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AcceptedReturnOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderTotalsComponent } from './order-total.component';

describe('OrderTotalsComponent', () => {
  let component: OrderTotalsComponent;
  let fixture: ComponentFixture<OrderTotalsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrderTotalsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderTotalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CostRefundedOrderComponent } from './cost-refunded-order.component';

describe('CostRefundedOrderComponent', () => {
  let component: CostRefundedOrderComponent;
  let fixture: ComponentFixture<CostRefundedOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CostRefundedOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CostRefundedOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

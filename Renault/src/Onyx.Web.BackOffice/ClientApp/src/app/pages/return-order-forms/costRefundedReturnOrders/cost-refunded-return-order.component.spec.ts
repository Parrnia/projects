import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CostRefundedReturnOrderComponent } from './cost-refunded-return-order.component';

describe('CostRefundedReturnOrderComponent', () => {
  let component: CostRefundedReturnOrderComponent;
  let fixture: ComponentFixture<CostRefundedReturnOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CostRefundedReturnOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CostRefundedReturnOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmedPaymentOrderComponent } from './payment-confirmed-order.component';

describe('ConfirmedPaymentOrderComponent', () => {
  let component: ConfirmedPaymentOrderComponent;
  let fixture: ComponentFixture<ConfirmedPaymentOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConfirmedPaymentOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConfirmedPaymentOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

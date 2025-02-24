import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UnconfirmedPaymentOrderComponent } from './payment-unconfirmed-order.component';

describe('UnconfirmedPaymentOrderComponent', () => {
  let component: UnconfirmedPaymentOrderComponent;
  let fixture: ComponentFixture<UnconfirmedPaymentOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UnconfirmedPaymentOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UnconfirmedPaymentOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

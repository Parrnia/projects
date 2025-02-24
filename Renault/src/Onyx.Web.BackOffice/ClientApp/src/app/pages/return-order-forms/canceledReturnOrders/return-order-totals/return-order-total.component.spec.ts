import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReturnOrderTotalsComponent } from './return-order-total.component';

describe('ReturnOrderTotalsComponent', () => {
  let component: ReturnOrderTotalsComponent;
  let fixture: ComponentFixture<ReturnOrderTotalsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReturnOrderTotalsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReturnOrderTotalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

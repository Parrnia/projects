import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RejectedReturnOrderComponent } from './rejected-return-order.component';

describe('RejectedReturnOrderComponent', () => {
  let component: RejectedReturnOrderComponent;
  let fixture: ComponentFixture<RejectedReturnOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RejectedReturnOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RejectedReturnOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllConfirmedReturnOrderComponent } from './all-confirmed-return-order.component';

describe('AllConfirmedReturnOrderComponent', () => {
  let component: AllConfirmedReturnOrderComponent;
  let fixture: ComponentFixture<AllConfirmedReturnOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllConfirmedReturnOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllConfirmedReturnOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

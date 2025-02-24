import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReturnOrderItemOptionComponent } from './return-order-item-option.component';

describe('ReturnOrderItemOptionComponent', () => {
  let component: ReturnOrderItemOptionComponent;
  let fixture: ComponentFixture<ReturnOrderItemOptionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReturnOrderItemOptionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReturnOrderItemOptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

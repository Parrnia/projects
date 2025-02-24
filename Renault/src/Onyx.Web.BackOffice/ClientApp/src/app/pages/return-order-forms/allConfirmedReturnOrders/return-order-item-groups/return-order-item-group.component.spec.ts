import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReturnOrderItemGroupComponent } from './return-order-item-group.component';

describe('ReturnOrderItemsComponent', () => {
  let component: ReturnOrderItemGroupComponent;
  let fixture: ComponentFixture<ReturnOrderItemGroupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReturnOrderItemGroupComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReturnOrderItemGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

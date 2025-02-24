import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReturnOrderItemsComponent } from './return-order-item.component';

describe('ReturnOrderItemsComponent', () => {
  let component: ReturnOrderItemsComponent;
  let fixture: ComponentFixture<ReturnOrderItemsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReturnOrderItemsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReturnOrderItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

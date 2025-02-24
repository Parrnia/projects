import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReturnOrderInfoComponent } from './return-order-info.component';

describe('ReturnOrderInfoComponent', () => {
  let component: ReturnOrderInfoComponent;
  let fixture: ComponentFixture<ReturnOrderInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReturnOrderInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReturnOrderInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

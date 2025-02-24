import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CanceledReturnOrderComponent } from './canceled-return-order.component';

describe('CanceledReturnOrderComponent', () => {
  let component: CanceledReturnOrderComponent;
  let fixture: ComponentFixture<CanceledReturnOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CanceledReturnOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CanceledReturnOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

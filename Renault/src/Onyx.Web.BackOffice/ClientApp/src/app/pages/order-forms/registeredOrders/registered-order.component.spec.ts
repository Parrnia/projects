import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisteredOrderComponent } from './registered-order.component';

describe('RegisteredOrderComponent', () => {
  let component: RegisteredOrderComponent;
  let fixture: ComponentFixture<RegisteredOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegisteredOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegisteredOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisteredReturnOrderComponent } from './registered-return-order.component';

describe('RegisteredReturnOrderComponent', () => {
  let component: RegisteredReturnOrderComponent;
  let fixture: ComponentFixture<RegisteredReturnOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegisteredReturnOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RegisteredReturnOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

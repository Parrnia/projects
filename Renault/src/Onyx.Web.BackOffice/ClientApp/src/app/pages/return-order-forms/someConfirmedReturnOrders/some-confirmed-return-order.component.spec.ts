import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SomeConfirmedReturnOrderComponent } from './some-confirmed-return-order.component';

describe('SomeConfirmedReturnOrderComponent', () => {
  let component: SomeConfirmedReturnOrderComponent;
  let fixture: ComponentFixture<SomeConfirmedReturnOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SomeConfirmedReturnOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SomeConfirmedReturnOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

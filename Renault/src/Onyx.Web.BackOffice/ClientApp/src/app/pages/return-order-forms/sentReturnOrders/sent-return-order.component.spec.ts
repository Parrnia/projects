import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SentReturnOrderComponent } from './sent-return-order.component';

describe('SentReturnOrderComponent', () => {
  let component: SentReturnOrderComponent;
  let fixture: ComponentFixture<SentReturnOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SentReturnOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SentReturnOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

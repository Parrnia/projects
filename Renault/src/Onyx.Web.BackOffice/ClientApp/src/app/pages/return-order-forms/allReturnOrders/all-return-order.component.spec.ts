import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllReturnOrderComponent } from './all-return-order.component';

describe('AllReturnOrderComponent', () => {
  let component: AllReturnOrderComponent;
  let fixture: ComponentFixture<AllReturnOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllReturnOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllReturnOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

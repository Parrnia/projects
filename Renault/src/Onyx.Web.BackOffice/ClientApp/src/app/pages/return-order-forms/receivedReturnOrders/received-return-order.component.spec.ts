import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceivedReturnOrderComponent } from './received-return-order.component';

describe('ReceivedReturnOrderComponent', () => {
  let component: ReceivedReturnOrderComponent;
  let fixture: ComponentFixture<ReceivedReturnOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReceivedReturnOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReceivedReturnOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

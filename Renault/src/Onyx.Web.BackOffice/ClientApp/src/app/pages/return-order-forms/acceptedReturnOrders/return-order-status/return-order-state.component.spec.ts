import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReturnOrderStateComponent } from './return-order-state.component';

describe('ReturnOrderStateComponent', () => {
  let component: ReturnOrderStateComponent;
  let fixture: ComponentFixture<ReturnOrderStateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReturnOrderStateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReturnOrderStateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

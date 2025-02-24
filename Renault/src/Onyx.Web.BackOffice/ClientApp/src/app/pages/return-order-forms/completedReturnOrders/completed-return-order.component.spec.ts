import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompletedReturnOrderComponent } from './completed-return-order.component';

describe('CompletedReturnOrderComponent', () => {
  let component: CompletedReturnOrderComponent;
  let fixture: ComponentFixture<CompletedReturnOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CompletedReturnOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CompletedReturnOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

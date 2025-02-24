import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderWorkflowComponent } from './order-workflow.component';

describe('OrderWorkflowComponent', () => {
  let component: OrderWorkflowComponent;
  let fixture: ComponentFixture<OrderWorkflowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrderWorkflowComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OrderWorkflowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

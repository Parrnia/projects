import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowOrderComponent } from './workflow-order.component';

describe('WorkflowOrderComponent', () => {
  let component: WorkflowOrderComponent;
  let fixture: ComponentFixture<WorkflowOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkflowOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkflowOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

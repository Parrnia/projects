import { ComponentFixture, TestBed } from '@angular/core/testing';
import { WorkflowReturnOrderComponent } from './workflow-return-order.component';


describe('WorkflowReturnOrderComponent', () => {
  let component: WorkflowReturnOrderComponent;
  let fixture: ComponentFixture<WorkflowReturnOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkflowReturnOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkflowReturnOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

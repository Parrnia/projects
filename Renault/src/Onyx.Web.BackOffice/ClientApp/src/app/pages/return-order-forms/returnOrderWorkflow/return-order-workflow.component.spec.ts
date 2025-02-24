import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReturnOrderWorkflowComponent } from './return-order-workflow.component';


describe('ReturnOrderWorkflowComponent', () => {
  let component: ReturnOrderWorkflowComponent;
  let fixture: ComponentFixture<ReturnOrderWorkflowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReturnOrderWorkflowComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReturnOrderWorkflowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

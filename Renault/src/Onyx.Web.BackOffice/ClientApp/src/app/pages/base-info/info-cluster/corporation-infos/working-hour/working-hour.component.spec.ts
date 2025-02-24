import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkingHourComponent } from './working-hour.component';

describe('WorkingHoursComponent', () => {
  let component: WorkingHourComponent;
  let fixture: ComponentFixture<WorkingHourComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WorkingHourComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WorkingHourComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

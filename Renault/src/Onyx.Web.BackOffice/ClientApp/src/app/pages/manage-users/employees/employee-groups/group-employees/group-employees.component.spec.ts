import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupEmployeesComponent } from './group-employees.component';

describe('GroupEmployeesComponent', () => {
  let component: GroupEmployeesComponent;
  let fixture: ComponentFixture<GroupEmployeesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GroupEmployeesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GroupEmployeesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

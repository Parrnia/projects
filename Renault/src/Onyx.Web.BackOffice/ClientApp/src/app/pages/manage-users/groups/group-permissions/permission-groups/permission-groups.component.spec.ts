import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PermissionGroupsComponent } from './permission-groups.component';

describe('PermissionGroupsComponent', () => {
  let component: PermissionGroupsComponent;
  let fixture: ComponentFixture<PermissionGroupsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PermissionGroupsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PermissionGroupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

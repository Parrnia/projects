import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TypeGroupsComponent } from './type-groups.component';

describe('TypeGroupsComponent', () => {
  let component: TypeGroupsComponent;
  let fixture: ComponentFixture<TypeGroupsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TypeGroupsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TypeGroupsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

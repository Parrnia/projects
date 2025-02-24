import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PreparedOrderComponent } from './prepared-order.component';

describe('PreparedOrderComponent', () => {
  let component: PreparedOrderComponent;
  let fixture: ComponentFixture<PreparedOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PreparedOrderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PreparedOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

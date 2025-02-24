import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CountingUnitTypeComponent } from './counting-unit-type.component';

describe('CountingUnitTypeComponent', () => {
  let component: CountingUnitTypeComponent;
  let fixture: ComponentFixture<CountingUnitTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CountingUnitTypeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CountingUnitTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

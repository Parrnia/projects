import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CountingUnitComponent } from './counting-unit.component';

describe('CountingUnitComponent', () => {
  let component: CountingUnitComponent;
  let fixture: ComponentFixture<CountingUnitComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CountingUnitComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CountingUnitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OptionColorsComponent } from './option-colors.component';

describe('OptionColorsComponent', () => {
  let component: OptionColorsComponent;
  let fixture: ComponentFixture<OptionColorsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OptionColorsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OptionColorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

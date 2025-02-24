import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OptionMaterialsComponent } from './option-materials.component';

describe('OptionMaterialsComponent', () => {
  let component: OptionMaterialsComponent;
  let fixture: ComponentFixture<OptionMaterialsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OptionMaterialsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OptionMaterialsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CorporationInfoComponent } from './corporation-info.component';

describe('CorporationInfoComponent', () => {
  let component: CorporationInfoComponent;
  let fixture: ComponentFixture<CorporationInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CorporationInfoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CorporationInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

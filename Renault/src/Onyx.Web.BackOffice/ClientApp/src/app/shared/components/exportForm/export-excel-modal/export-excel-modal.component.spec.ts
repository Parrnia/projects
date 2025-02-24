import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportExcelModalComponent } from './export-excel-modal.component';

describe('ExportExcelModalComponent', () => {
  let component: ExportExcelModalComponent;
  let fixture: ComponentFixture<ExportExcelModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExportExcelModalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExportExcelModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

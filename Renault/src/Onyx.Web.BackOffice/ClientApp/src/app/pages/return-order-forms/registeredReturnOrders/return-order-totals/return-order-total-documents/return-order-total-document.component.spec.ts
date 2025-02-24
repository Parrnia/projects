import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReturnOrderTotalDocumentComponent } from './return-order-total-document.component';

describe('ReturnOrderTotalDocumentComponent', () => {
  let component: ReturnOrderTotalDocumentComponent;
  let fixture: ComponentFixture<ReturnOrderTotalDocumentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReturnOrderTotalDocumentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReturnOrderTotalDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReturnOrderItemDocumentComponent } from './return-order-item-document.component';

describe('ReturnOrderItemDocumentComponent', () => {
  let component: ReturnOrderItemDocumentComponent;
  let fixture: ComponentFixture<ReturnOrderItemDocumentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReturnOrderItemDocumentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReturnOrderItemDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

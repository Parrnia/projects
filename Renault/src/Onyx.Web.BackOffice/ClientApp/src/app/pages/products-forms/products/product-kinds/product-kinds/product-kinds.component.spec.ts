import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductKindsComponent } from './product-kinds.component';

describe('ProductKindsComponent', () => {
  let component: ProductKindsComponent;
  let fixture: ComponentFixture<ProductKindsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductKindsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductKindsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

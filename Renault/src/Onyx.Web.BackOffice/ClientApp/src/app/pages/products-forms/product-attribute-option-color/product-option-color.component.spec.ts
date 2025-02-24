import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductAttributeOptionBaseComponent } from './product-attribute-option-base.component';

describe('ProductAttributeOptionBaseComponent', () => {
  let component: ProductAttributeOptionBaseComponent;
  let fixture: ComponentFixture<ProductAttributeOptionBaseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductAttributeOptionBaseComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductAttributeOptionBaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

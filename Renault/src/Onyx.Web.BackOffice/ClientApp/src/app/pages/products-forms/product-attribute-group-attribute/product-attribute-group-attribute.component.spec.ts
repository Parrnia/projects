import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductAttributeGroupAttributeComponent } from './product-attribute-group-attribute.component';

describe('ProductAttributeGroupAttributeComponent', () => {
  let component: ProductAttributeGroupAttributeComponent;
  let fixture: ComponentFixture<ProductAttributeGroupAttributeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductAttributeGroupAttributeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductAttributeGroupAttributeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

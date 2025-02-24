import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductAttributeTypesComponent } from './product-attribute-types.component';

describe('ProductAttributeTypesComponent', () => {
  let component: ProductAttributeTypesComponent;
  let fixture: ComponentFixture<ProductAttributeTypesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductAttributeTypesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductAttributeTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

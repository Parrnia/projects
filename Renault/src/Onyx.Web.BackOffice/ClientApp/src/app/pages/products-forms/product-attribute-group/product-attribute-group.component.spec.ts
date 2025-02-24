import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductAttributeGroupComponent } from './product-attribute-group.component';

describe('ProductAttributeGroupComponent', () => {
  let component: ProductAttributeGroupComponent;
  let fixture: ComponentFixture<ProductAttributeGroupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductAttributeGroupComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductAttributeGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

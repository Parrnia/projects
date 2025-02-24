import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductOptionRoleComponent } from './product-option-role.component';

describe('ProductOptionRoleComponent', () => {
  let component: ProductOptionRoleComponent;
  let fixture: ComponentFixture<ProductOptionRoleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductOptionRoleComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductOptionRoleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductAttributeOptionComponent } from './product-attribute-option.component';



describe('ProductAttributeOptionComponent', () => {
  let component: ProductAttributeOptionComponent;
  let fixture: ComponentFixture<ProductAttributeOptionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductAttributeOptionComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductAttributeOptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

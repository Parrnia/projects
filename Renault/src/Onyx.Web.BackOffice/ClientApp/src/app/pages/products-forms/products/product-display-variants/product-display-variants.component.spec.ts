import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ProductDisplayVariantsComponent } from './product-display-variants.component';


describe('ProductDisplayVariantsComponent', () => {
  let component: ProductDisplayVariantsComponent;
  let fixture: ComponentFixture<ProductDisplayVariantsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductDisplayVariantsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProductDisplayVariantsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

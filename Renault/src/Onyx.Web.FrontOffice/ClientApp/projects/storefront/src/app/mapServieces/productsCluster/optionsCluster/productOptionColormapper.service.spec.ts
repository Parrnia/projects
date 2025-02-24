import { TestBed } from '@angular/core/testing';
import { ProductOptionColormapperService } from './productOptionColormapper.service';



describe('ProductOptionColormapperService', () => {
  let service: ProductOptionColormapperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProductOptionColormapperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

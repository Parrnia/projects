import { TestBed } from '@angular/core/testing';
import { ProductOptionMaterialmapperService } from './productOptionMaterialmapper.service';



describe('ProductOptionMaterialmapperService', () => {
  let service: ProductOptionMaterialmapperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProductOptionMaterialmapperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

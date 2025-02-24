import { TestBed } from '@angular/core/testing';

import { ProductBrandmapperService } from './product-brand-mapper.service';

describe('ProductBrandmapperService', () => {
  let service: ProductBrandmapperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProductBrandmapperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

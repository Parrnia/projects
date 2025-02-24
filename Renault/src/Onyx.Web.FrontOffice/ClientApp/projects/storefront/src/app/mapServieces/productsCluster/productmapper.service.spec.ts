import { TestBed } from '@angular/core/testing';

import { ProductmapperService } from './productmapper.service';

describe('ProductmapperService', () => {
  let service: ProductmapperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProductmapperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { BrandmapperService } from './brandmapper.service';

describe('BrandmapperService', () => {
  let service: BrandmapperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BrandmapperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

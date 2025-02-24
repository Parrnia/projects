import { TestBed } from '@angular/core/testing';

import { VehiclemapperService } from './vehiclemapper.service';

describe('VehiclemapperService', () => {
  let service: VehiclemapperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VehiclemapperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

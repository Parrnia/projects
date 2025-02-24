import { TestBed } from '@angular/core/testing';

import { VehicleBrandmapperService } from './vehicle-brand-mapper.service';

describe('VehicleBrandmapperService', () => {
  let service: VehicleBrandmapperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VehicleBrandmapperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});


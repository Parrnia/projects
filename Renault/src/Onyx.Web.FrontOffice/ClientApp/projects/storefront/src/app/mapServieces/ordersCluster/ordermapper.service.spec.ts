import { TestBed } from '@angular/core/testing';

import { OrdermapperService } from './ordermapper.service';

describe('OrdermapperService', () => {
  let service: OrdermapperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OrdermapperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

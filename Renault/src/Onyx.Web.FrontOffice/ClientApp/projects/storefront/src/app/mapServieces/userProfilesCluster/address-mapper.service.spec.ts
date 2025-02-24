import { TestBed } from '@angular/core/testing';

import { AddressMapperService } from './address-mapper.service';

describe('AddressMapperService', () => {
  let service: AddressMapperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AddressMapperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

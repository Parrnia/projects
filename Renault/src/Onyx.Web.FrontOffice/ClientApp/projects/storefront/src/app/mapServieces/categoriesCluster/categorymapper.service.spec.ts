import { TestBed } from '@angular/core/testing';

import { CategorymapperService } from './categorymapper.service';

describe('CategorymapperService', () => {
  let service: CategorymapperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CategorymapperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';
import { ReviewmapperService } from './reviewmapper.service';



describe('ReviewmapperService', () => {
  let service: ReviewmapperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ReviewmapperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

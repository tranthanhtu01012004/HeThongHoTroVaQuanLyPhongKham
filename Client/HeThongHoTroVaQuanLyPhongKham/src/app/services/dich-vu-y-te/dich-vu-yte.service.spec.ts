import { TestBed } from '@angular/core/testing';
import { DichVuYTeService } from './dich-vu-yte.service';



describe('DichVuYTeServiceService', () => {
  let service: DichVuYTeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DichVuYTeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

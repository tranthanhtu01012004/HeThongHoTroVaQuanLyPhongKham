import { TestBed } from '@angular/core/testing';

import { DonThuocService } from './don-thuoc.service';

describe('DonThuocService', () => {
  let service: DonThuocService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DonThuocService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

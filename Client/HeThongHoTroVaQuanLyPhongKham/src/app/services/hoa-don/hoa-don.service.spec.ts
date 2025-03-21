import { TestBed } from '@angular/core/testing';

import { HoaDonService } from './hoa-don.service';

describe('HoaDonService', () => {
  let service: HoaDonService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HoaDonService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

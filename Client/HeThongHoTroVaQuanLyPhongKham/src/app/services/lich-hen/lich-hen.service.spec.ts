import { TestBed } from '@angular/core/testing';

import { LichHenService } from './lich-hen.service';

describe('LichHenService', () => {
  let service: LichHenService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LichHenService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

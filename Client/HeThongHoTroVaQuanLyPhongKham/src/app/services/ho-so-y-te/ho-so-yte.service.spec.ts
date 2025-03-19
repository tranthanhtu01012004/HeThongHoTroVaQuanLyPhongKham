import { TestBed } from '@angular/core/testing';

import { HoSoYTeService } from './ho-so-yte.service';

describe('HoSoYTeService', () => {
  let service: HoSoYTeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HoSoYTeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

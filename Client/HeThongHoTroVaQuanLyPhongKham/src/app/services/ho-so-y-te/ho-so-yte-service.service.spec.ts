import { TestBed } from '@angular/core/testing';

import { HoSoYTeServiceService } from './ho-so-yte-service.service';

describe('HoSoYTeServiceService', () => {
  let service: HoSoYTeServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HoSoYTeServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

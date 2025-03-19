import { TestBed } from '@angular/core/testing';

import { TrieuChungService } from './trieu-chung.service';

describe('TrieuChungService', () => {
  let service: TrieuChungService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TrieuChungService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

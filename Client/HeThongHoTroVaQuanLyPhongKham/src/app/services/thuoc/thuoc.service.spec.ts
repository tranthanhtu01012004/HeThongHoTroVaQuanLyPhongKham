import { TestBed } from '@angular/core/testing';

import { ThuocService } from './thuoc.service';

describe('ThuocService', () => {
  let service: ThuocService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ThuocService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

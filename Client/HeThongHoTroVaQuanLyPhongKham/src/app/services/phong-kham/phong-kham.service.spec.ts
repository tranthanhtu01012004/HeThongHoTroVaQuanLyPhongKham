import { TestBed } from '@angular/core/testing';

import { PhongKhamService } from './phong-kham.service';

describe('PhongKhamService', () => {
  let service: PhongKhamService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PhongKhamService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

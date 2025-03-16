import { TestBed } from '@angular/core/testing';

import { PhongKhamNhanVienService } from './phong-kham-nhan-vien.service';

describe('PhongKhamNhanVienService', () => {
  let service: PhongKhamNhanVienService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PhongKhamNhanVienService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

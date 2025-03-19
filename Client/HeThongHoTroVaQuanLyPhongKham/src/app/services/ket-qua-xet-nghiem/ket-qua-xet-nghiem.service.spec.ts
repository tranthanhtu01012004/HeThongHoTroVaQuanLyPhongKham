import { TestBed } from '@angular/core/testing';

import { KetQuaXetNghiemService } from './ket-qua-xet-nghiem.service';

describe('KetQuaXetNghiemService', () => {
  let service: KetQuaXetNghiemService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(KetQuaXetNghiemService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

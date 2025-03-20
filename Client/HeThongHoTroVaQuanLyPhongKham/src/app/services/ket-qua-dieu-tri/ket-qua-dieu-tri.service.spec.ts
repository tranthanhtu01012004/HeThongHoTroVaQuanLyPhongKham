import { TestBed } from '@angular/core/testing';

import { KetQuaDieuTriService } from './ket-qua-dieu-tri.service';

describe('KetQuaDieuTriService', () => {
  let service: KetQuaDieuTriService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(KetQuaDieuTriService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

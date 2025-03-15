import { TestBed } from '@angular/core/testing';

import { VaiTroService } from './vai-tro.service';

describe('VaiTroService', () => {
  let service: VaiTroService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VaiTroService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

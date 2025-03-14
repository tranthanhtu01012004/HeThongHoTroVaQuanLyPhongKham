import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DichVuYTeComponent } from './dich-vu-y-te.component';

describe('DichVuYTeComponent', () => {
  let component: DichVuYTeComponent;
  let fixture: ComponentFixture<DichVuYTeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DichVuYTeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DichVuYTeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

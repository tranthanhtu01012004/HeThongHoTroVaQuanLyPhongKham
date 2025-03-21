import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HoaDonComponent } from './hoa-don.component';

describe('HoaDonComponent', () => {
  let component: HoaDonComponent;
  let fixture: ComponentFixture<HoaDonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HoaDonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HoaDonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

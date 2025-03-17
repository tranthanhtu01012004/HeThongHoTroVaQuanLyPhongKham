import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuanLyLichHenComponent } from './quan-ly-lich-hen.component';

describe('QuanLyLichHenComponent', () => {
  let component: QuanLyLichHenComponent;
  let fixture: ComponentFixture<QuanLyLichHenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QuanLyLichHenComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QuanLyLichHenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

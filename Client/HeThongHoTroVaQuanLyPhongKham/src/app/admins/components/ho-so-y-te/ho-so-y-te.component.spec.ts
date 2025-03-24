import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HoSoYTeComponent } from './ho-so-y-te.component';

describe('HoSoYTeComponent', () => {
  let component: HoSoYTeComponent;
  let fixture: ComponentFixture<HoSoYTeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HoSoYTeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HoSoYTeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

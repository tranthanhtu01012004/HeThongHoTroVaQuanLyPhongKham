import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DichVuComponent } from './dich-vu.component';

describe('DichVuComponent', () => {
  let component: DichVuComponent;
  let fixture: ComponentFixture<DichVuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DichVuComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DichVuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

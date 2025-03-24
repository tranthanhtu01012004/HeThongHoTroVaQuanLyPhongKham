import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ThuocComponent } from './thuoc.component';

describe('ThuocComponent', () => {
  let component: ThuocComponent;
  let fixture: ComponentFixture<ThuocComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ThuocComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ThuocComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FilterAppointmentsComponent } from './appointment-filter.component';

;

describe('AppointmentFilterComponent', () => {
  let component: FilterAppointmentsComponent;
  let fixture: ComponentFixture<FilterAppointmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FilterAppointmentsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FilterAppointmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

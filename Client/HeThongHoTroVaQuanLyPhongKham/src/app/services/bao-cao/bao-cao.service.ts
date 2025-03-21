import { Injectable } from '@angular/core';
import { BaseApiService } from '../base-api-service/base-api.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DoanhThuReportDTO } from '../../interfaces/bao-cao/DoanhThuReportDTO';
import { LichHenReportDTO } from '../../interfaces/bao-cao/LichHenReportDTO';
import { DonThuocReportDTO } from '../../interfaces/bao-cao/DonThuocReportDTO';
import { DichVuYTeReportDTO } from '../../interfaces/bao-cao/DichVuYTeReportDTO';

@Injectable({
  providedIn: 'root'
})
export class BaoCaoService extends BaseApiService {
  private endpoint = '/reports';
  
  constructor(http: HttpClient) {
    super(http);
  }
// Thống kê doanh thu
thongKeDoanhThu(tuNgay: Date, denNgay: Date, trangThaiThanhToan?: string): Observable<DoanhThuReportDTO> {
  let params = new HttpParams()
    .set('tuNgay', tuNgay.toISOString())
    .set('denNgay', denNgay.toISOString());
  
  if (trangThaiThanhToan) {
    params = params.set('trangThaiThanhToan', trangThaiThanhToan);
  }

  return this.http.get<DoanhThuReportDTO>(`${this.apiBaseUrl}${this.endpoint}/revenues`, { params });
}

// Thống kê lịch hẹn
thongKeLichHen(tuNgay: Date, denNgay: Date): Observable<LichHenReportDTO> {
  const params = new HttpParams()
    .set('tuNgay', tuNgay.toISOString())
    .set('denNgay', denNgay.toISOString());

  return this.http.get<LichHenReportDTO>(`${this.apiBaseUrl}${this.endpoint}/appointments`, { params });
}

// Thống kê đơn thuốc
thongKeDonThuoc(tuNgay: Date, denNgay: Date): Observable<DonThuocReportDTO> {
  const params = new HttpParams()
    .set('tuNgay', tuNgay.toISOString())
    .set('denNgay', denNgay.toISOString());

  return this.http.get<DonThuocReportDTO>(`${this.apiBaseUrl}${this.endpoint}/prescriptions`, { params });
}

// Thống kê dịch vụ y tế
thongKeDichVuYTe(tuNgay: Date, denNgay: Date): Observable<DichVuYTeReportDTO> {
  const params = new HttpParams()
    .set('tuNgay', tuNgay.toISOString())
    .set('denNgay', denNgay.toISOString());

  return this.http.get<DichVuYTeReportDTO>(`${this.apiBaseUrl}${this.endpoint}/healthcare-services`, { params });
}
}

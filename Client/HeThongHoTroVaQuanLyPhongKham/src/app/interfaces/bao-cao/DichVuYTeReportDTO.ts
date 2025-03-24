export interface DichVuYTeReportDTO {
  tuNgay: Date;
  denNgay: Date;
  soLuongTheoDichVu: { [key: string]: number };
  tongSoBenhNhan: number;
}
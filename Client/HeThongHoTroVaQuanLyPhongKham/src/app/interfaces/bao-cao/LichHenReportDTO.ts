export interface LichHenReportDTO {
    tuNgay: Date;
    denNgay: Date;
    soLuongTheoTrangThai: { [key: string]: number };
    tongSoLichHen: number;
  }
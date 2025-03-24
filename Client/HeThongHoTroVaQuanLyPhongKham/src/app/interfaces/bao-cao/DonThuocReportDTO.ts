export interface DonThuocReportDTO {
    tuNgay: Date;
    denNgay: Date;
    soLuongTheoBenhNhan: { [key: number]: number };
    tongSoDonThuoc: number;
  }
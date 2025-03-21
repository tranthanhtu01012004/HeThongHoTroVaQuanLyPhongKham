import { IHoaDon } from "../hoa-don/IHoaDon";

export interface DoanhThuReportDTO {
    tongDoanhThu: number;
    soHoaDon: number;
    trangThaiThanhToan: string;
    tuNgay: Date;
    denNgay: Date;
    danhSachHoaDon: IHoaDon[];
  }
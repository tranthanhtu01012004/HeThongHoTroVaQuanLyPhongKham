export interface IHoaDon {
    maHoaDon: number;
    maLichHen: number;
    maHoSoYTe?: number;
    tongTien: number;
    trangThaiThanhToan: 'Đã thanh toán' | 'Chưa thanh toán';
    ngayThanhToan?: string | null;
  }
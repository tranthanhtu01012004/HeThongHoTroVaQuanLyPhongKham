import { IChiTietThuoc } from "../chi-tiet-thuoc/IChiTietThuoc";

export interface IDonThuoc {
    maDonThuoc: number;
    maHoSoYTe: number;
    maHoaDon: number | null;
    ngayKeDon: string;
    chiTietThuocList: IChiTietThuoc[];
  }
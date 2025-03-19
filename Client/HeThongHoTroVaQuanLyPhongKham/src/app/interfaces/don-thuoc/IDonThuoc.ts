import { IChiTietThuoc } from "../chi-tiet-thuoc/IChiTietThuoc";

export interface IDonThuoc {
    maDonThuoc: number;
    maHoSoYTe: number;
    maHoaDon: number;
    ngayKeDon: string;
    chiTietThuocList: IChiTietThuoc[];
  }
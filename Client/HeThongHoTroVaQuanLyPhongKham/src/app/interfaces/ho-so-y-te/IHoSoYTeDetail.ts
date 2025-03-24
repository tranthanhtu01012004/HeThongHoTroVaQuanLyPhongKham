import { IDonThuoc } from "../don-thuoc/IDonThuoc";
import { IKetQuaDieuTri } from "../ket-qua-deu-tri/IKetQuaDieuTri";
import { IKetQuaXetNghiem } from "../ket-qua-xet-nghiem/IKetQuaXetNghiem";
import { ITrieuChung } from "../trieu-chung/ITrieuChung";

export interface IHoSoYTeDetail {
    maHoSoYTe: number;
    maBenhNhan: number;
    chuanDoan: string;
    phuongPhapDieuTri: string;
    lichSuBenh: string;
    trieuChung: ITrieuChung[];
    ketQuaXetNghiem: IKetQuaXetNghiem[];
    donThuoc: IDonThuoc[];
    ketQuaDieuTri: IKetQuaDieuTri[];
  }
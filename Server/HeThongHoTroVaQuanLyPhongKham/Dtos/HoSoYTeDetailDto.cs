namespace HeThongHoTroVaQuanLyPhongKham.Dtos
{
    public class HoSoYTeDetailDto
    {
        public int MaHoSoYTe { get; set; }
        public int MaBenhNhan { get; set; }
        public string ChuanDoan { get; set; }
        public string PhuongPhapDieuTri { get; set; }
        public string LichSuBenh { get; set; }
        public List<TrieuChungDTO> TrieuChung { get; set; } = new List<TrieuChungDTO>();
        public List<KetQuaXetNghiemDTO> KetQuaXetNghiem { get; set; } = new List<KetQuaXetNghiemDTO>();
        public List<DonThuocDTO> DonThuoc { get; set; } = new List<DonThuocDTO>();
        public List<KetQuaDieuTriDTO> KetQuaDieuTri { get; set; } = new List<KetQuaDieuTriDTO>();
    }
}

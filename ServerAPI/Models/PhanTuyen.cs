namespace ServerAPI.Models
{
    public class PhanTuyen
    {
        public int IDPhieu { get; set; }
        public int IDKhachHang { get; set; }
        public int IDKyThu { get; set; }
        public int IDTuyenThu { get; set; }
        public int IDNhanVien { get; set; }
        public int MauSoPhieu { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime NgayThu { get; set; }
    }
}

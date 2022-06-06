namespace ServerAPI.Models
{
    public class NhanVien
    {
        public int Id { get; set; }
        public string MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string GioiTinh { get; set; }
        public string SoDienThoai { get; set; }
        public DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string CCCD { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
    }
}

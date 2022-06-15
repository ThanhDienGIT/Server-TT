using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuThuController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PhieuThuController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
               select PhieuThu.IDPhieu,KhachHang.IDKhachHang,KhachHang.HoTenKH,TuyenThu.IDTuyenThu,TuyenThu.TenTuyenThu,KyThu.IDKyThu,KyThu.TenKyThu,
					KyThu.Thang,KyThu.Nam,NhanVien.IDNhanVien,NhanVien.HoTen,PhieuThu.MauSoPhieu,PhieuThu.NgayTao,PhieuThu.NgayThu,XaPhuong.IDXaPhuong,XaPhuong.TenXaPhuong,QuanHuyen.IDQuanHuyen,QuanHuyen.TenQuanHuyen,LoaiKhachHang.TenLoai,LoaiKhachHang.Gia
                from PhieuThu
                inner join KhachHang
                on PhieuThu.IDKhachHang = KhachHang.IDKhachHang
                inner join TuyenThu
                on PhieuThu.IDTuyenThu = TuyenThu.IDTuyenThu
                inner join KyThu
                on PhieuThu.IDKyThu = KyThu.IDKyThu
                FULL OUTER join NhanVien
                on PhieuThu.IDNhanVien = NhanVien.IDNhanVien
                inner join XaPhuong
                on KhachHang.IDXaPhuong = XaPhuong.IDXaPhuong
                inner join QuanHuyen
                on XaPhuong.IDQuanHuyen = QuanHuyen.IDQuanHuyen
                inner join LoaiKhachHang
                on KhachHang.IDLoaiKhachHang = LoaiKhachHang.IDLoaiKhachHang
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        //getbyID
        [HttpGet("{id}")]
        public JsonResult GetByID(int id)
        {
            string query = @"select PhieuThu.IDPhieu,KhachHang.HoTenKH,TuyenThu.TenTuyenThu,KyThu.TenKyThu,
					            KyThu.Thang,KyThu.Nam,NhanVien.HoTen,PhieuThu.MauSoPhieu,
                                PhieuThu.NgayTao,PhieuThu.NgayThu from dbo.PhieuThu
                                 inner join KhachHang
                                    on PhieuThu.IDKhachHang = KhachHang.IDKhachHang
                                    inner join TuyenThu
                                    on PhieuThu.IDTuyenThu = TuyenThu.IDTuyenThu
                                    inner join KyThu
                                    on PhieuThu.IDKyThu = KyThu.IDKyThu
                                    inner join NhanVien
                                    on PhieuThu.IDNhanVien = NhanVien.IDNhanVien
                                where IDPhieu =" + id;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        //get Ky Thu
        [HttpGet("kythu")]
        public JsonResult GetKyThu()
        {
            string query = @"select distinct KyThu.IDKyThu,KyThu.TenKyThu from KyThu";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }
        //get KH
        [HttpGet("khachhang")]
        public JsonResult GetKhachHang()
        {
            string query = @"
               select distinct KhachHang.IDKhachHang,KhachHang.HoTenKH from KhachHang where TrangThai = 1                
            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }
        //get KH id
        [HttpGet("khachhang/{id}")]
        public JsonResult GetInfoKHByID(int id)
        {
            string query = @"select Diachi, IDTuyenThu 
                from dbo.KhachHang inner join XaPhuong on KhachHang.IDXaPhuong = XaPhuong.IDXaPhuong
                where IDKhachHang =" + id;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }
        //insert
        [HttpPost]
        public JsonResult Post(PhieuThu pt)
        {
            string getMaxIDPhieuQuery = "select IDENT_CURRENT('PhieuThu') + 1";
            int maxIDPhieu = 0;
            DataTable dt = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(getMaxIDPhieuQuery, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    dt.Load(myReader);
                    myReader.Close();
                }
                maxIDPhieu = int.Parse(dt.Rows[0][0].ToString());
                string maSoPhieu = "PT" + dt.Rows[0][0].ToString() + "MKH" + pt.IDKhachHang + "D" + DateTime.Today.ToString("ddMMyyyy");
                string query = @"insert into PhieuThu values (" + pt.IDKhachHang + @",
                        " + pt.IDTuyenThu + @"," + pt.IDKyThu + @",null,
                        '" + maSoPhieu + @"',GETDATE(),null)";
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }
     
        //delete
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                DELETE FROM PhieuThu 
                WHERE IDPhieu = '" + id + @"'
                ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}

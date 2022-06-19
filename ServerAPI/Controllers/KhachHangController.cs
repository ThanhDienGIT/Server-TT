using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using ServerAPI.Models;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public KhachHangController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                select KhachHang.IDKhachHang, KhachHang.HoTenKH, KhachHang.MaKhachHang, KhachHang.CCCD, KhachHang.NgayCap, KhachHang.NgayTao, 
                    KhachHang.NgayChinhSua,KhachHang.DiaChi, KhachHang.IDXaPhuong, XaPhuong.TenXaPhuong, XaPhuong.IDQuanHuyen, QuanHuyen.TenQuanHuyen,
                    KhachHang.IDLoaiKhachHang, LoaiKhachHang.TenLoai, KhachHang.TrangThai, PhanTuyen.IDNhanVien
                from KhachHang
                inner join XaPhuong
                on KhachHang.IDXaPhuong = XaPhuong.IDXaPhuong
                inner join QuanHuyen
                on XaPhuong.IDQuanHuyen = QuanHuyen.IDQuanHuyen
                inner join LoaiKhachHang
				on KhachHang.IDLoaiKhachHang = LoaiKhachHang.IDLoaiKhachHang
				inner join TuyenThu 
				on XaPhuong.IDTuyenThu = TuyenThu.IDTuyenThu
				inner join PhanTuyen 
				on PhanTuyen.IDTuyenThu = TuyenThu.IDTuyenThu
            ";
            DataTable table = new DataTable();
            
            SqlDataReader myReader;
            string sqlDataSource = _configuration.GetConnectionString("DBCon");

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
            myReader.Close();

            return new JsonResult(table);
        }
        //lay thong tin khach hang tu idNhanVien
        [HttpGet("{idNhanVien}")]
        public JsonResult GetByStatus(int idNhanVien)
        {
            string query = @"
                select KhachHang.IDKhachHang, KhachHang.HoTenKH, KhachHang.MaKhachHang, KhachHang.CCCD, KhachHang.NgayCap, KhachHang.NgayTao, 
                    KhachHang.NgayChinhSua,KhachHang.DiaChi, KhachHang.IDXaPhuong, XaPhuong.TenXaPhuong, XaPhuong.IDQuanHuyen, QuanHuyen.TenQuanHuyen,
                    KhachHang.IDLoaiKhachHang, LoaiKhachHang.TenLoai, KhachHang.TrangThai, PhanTuyen.IDNhanVien, TuyenThu.TenTuyenThu
                from KhachHang
                inner join XaPhuong
                on KhachHang.IDXaPhuong = XaPhuong.IDXaPhuong
                inner join QuanHuyen
                on XaPhuong.IDQuanHuyen = QuanHuyen.IDQuanHuyen
                inner join LoaiKhachHang
				on KhachHang.IDLoaiKhachHang = LoaiKhachHang.IDLoaiKhachHang
				inner join TuyenThu 
				on XaPhuong.IDTuyenThu = TuyenThu.IDTuyenThu
				inner join PhanTuyen 
				on PhanTuyen.IDTuyenThu = TuyenThu.IDTuyenThu
                where PhanTuyen.IDNhanVien = " + idNhanVien
            ;
            DataTable table = new DataTable();

            SqlDataReader myReader;
            string sqlDataSource = _configuration.GetConnectionString("DBCon");

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
            myReader.Close();

            return new JsonResult(table);
        }

        

        [HttpPost]
        public JsonResult Post(KhachHang kh)
        {
            string maKH = "KH";

            string checkQuery = @"select * from KhachHang where CCCD = '" + kh.CCCD + "'";

            string getIDQuery = @"SELECT IDENT_CURRENT('KhachHang') + 1";

            string sqlDataSource = _configuration.GetConnectionString("DBCon");

            DataTable checkCCCD = new DataTable();

            DataTable maxID = new DataTable();
           
            DataTable table = new DataTable();

            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                      
               
                using (SqlCommand myCommand = new SqlCommand(checkQuery, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    checkCCCD.Load(myReader);
                    myReader.Close();
                }
                
                if (checkCCCD.Rows.Count > 0)
                {
                    myCon.Close();
                    return new JsonResult("CCCD đã tồn tại");                
                }
                else
                {
                    using (SqlCommand myCommand = new SqlCommand(getIDQuery, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        maxID.Load(myReader);
                        myReader.Close();
                    }

                    string MaxIDKhachHang = maxID.Rows[0][0].ToString();
                    int SoMaxIDKhachHang = 6 - MaxIDKhachHang.Length;

                    for (int i = 0; i < (SoMaxIDKhachHang); i++)
                    {
                        maKH = String.Concat(maKH, "0");
                    }
                    maKH = String.Concat(maKH, MaxIDKhachHang);

                    string query = @"insert into dbo.KhachHang values
                    (" + kh.IDXaPhuong + @"," + kh.IDLoaiKhachHang + @",'" + maKH + @"',N'" + kh.HoTenKH + @"',N'" + kh.DiaChi + @"','" + kh.CCCD + @"','" + kh.NgayCap + @"','"
                    + kh.NgayTao + @"',null,1)";

                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                        return new JsonResult("Thêm Khách Hàng Thành Công");
                    }
                }              
            }
        }

        [HttpPut]
        public JsonResult Put(KhachHang kh)
        {
            string checkQuery = @"select * from KhachHang where CCCD = '" + kh.CCCD + "' and idKhachHang !="+ kh.IDKhachHang;

            string sqlDataSource = _configuration.GetConnectionString("DBCon");

            DataTable checkCCCD = new DataTable();

            string query = @"update KhachHang 
                            set IDXaPhuong = '" + kh.IDXaPhuong + "', HoTenKH = N'" + kh.HoTenKH + "', DiaChi = N'" + kh.DiaChi + "', CCCD = '" + kh.CCCD + "', NgayCap = '" + kh.NgayCap + "', NgayChinhSua = '" + kh.NgayChinhSua + "', IDLoaiKhachHang = '" + kh.IDLoaiKhachHang  + "' where IDKhachHang =" + kh.IDKhachHang ;
            DataTable table = new DataTable();

            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                using (SqlCommand myCommand = new SqlCommand(checkQuery, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    checkCCCD.Load(myReader);
                    myReader.Close();
                }

                if (checkCCCD.Rows.Count > 0)
                {
                    myCon.Close();
                    return new JsonResult("CCCD đã tồn tại");
                }
                else
                {
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                        return new JsonResult("Chỉnh Sửa Thành Công");
                    }
                }
            }
        }

        [HttpPut("{id}")]
        public JsonResult PutStatus(int id)
        {
            string sqlDataSource = _configuration.GetConnectionString("DBCon");

            string query = @"update KhachHang set TrangThai = 0 where IDKhachHang =" + id;

            DataTable table = new DataTable();

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
                        return new JsonResult("Xoá Thành Công Khách Hàng");
                    }
                }
            }
    }
}

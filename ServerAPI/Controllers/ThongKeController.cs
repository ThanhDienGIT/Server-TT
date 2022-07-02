using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ThongKeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from KyThu order by Nam,Thang";
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

        [HttpGet("GetPhieuThu")]
        public JsonResult GetPhieuThu()
        {
            string query = @"select * from PhieuThu";
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

        [HttpGet("GetNamKyThu")]
        public JsonResult GetNamKyThu()
        {
            string query = @"select DISTINCT Nam from KyThu";
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

        [HttpGet("GetCountNhanVien")]
        public JsonResult GetCountNhanVien()
        {
            string query = @"select count(IDNhanVien) as NhanVien from NhanVien";
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;
            int NhanVien = 0;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    if (myReader.Read())
                    {
                        NhanVien = myReader.GetInt32(0);
                    }
                   
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(NhanVien);
        }
        [HttpGet("GetCountTuyenThu")]
        public JsonResult GetCountTuyenThu()
        {
            string query = @"select count(IDTuyenThu) as TuyenThu from TuyenThu";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;
            int TuyenThu = 0;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    if (myReader.Read())
                    {
                        TuyenThu = myReader.GetInt32(0);
                    }

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(TuyenThu);
        }
        [HttpGet("GetCountKhachHang")]
        public JsonResult GetCountKhachHang()
        {
            string query = @"select count(IDKhachHang) as KhachHang from KhachHang";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;
            int KhachHang = 0;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    if (myReader.Read())
                    {
                        KhachHang = myReader.GetInt32(0);
                    }

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(KhachHang);
        }
        [HttpGet("GetCountKyThu")]
        public JsonResult GetCountKyThu()
        {
            string query = @"select count(IDKyThu) as KyThu from KyThu";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;
            int KyThu = 0;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    if (myReader.Read())
                    {
                        KyThu = myReader.GetInt32(0);
                    }

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(KyThu);
        }

        [HttpGet("GetCustomer")]
        public JsonResult GetCustomer()
        {
            string query = @"select kh.IDKhachHang , kh.HoTenKH , tt.TenTuyenThu , nv.MaNhanVien ,
                             nv.HoTen , kh.DiaChi , kh.TrangThai , xp.TenXaPhuong ,qh.TenQuanHuyen
                             from KhachHang as kh , Nhanvien as nv , XaPhuong as xp , 
                             QuanHuyen as qh , TuyenThu as tt , PhanTuyen as pt
                             where kh.IDXaPhuong = xp.IDXaPhuong and qh.IDQuanHuyen = xp.IDQuanHuyen and nv.IDNhanVien = pt.IDNhanVien and 
                             pt.IDTuyenThu = tt.IDTuyenThu  and pt.IDQuanHuyen = qh.IDQuanHuyen";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;
            ;
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


    }
}

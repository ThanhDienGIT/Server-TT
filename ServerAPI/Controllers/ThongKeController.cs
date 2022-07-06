﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Text;

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

        [HttpGet("GetKyThu")]
        public JsonResult GetKyThu()
        {
            string query = @"select * from KyThu";
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
        [HttpGet("getminday")]
        public JsonResult getminday()
        {
            string query = @"select min(NgayThu) as ngaynhonhat from PhieuThu";

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

        [HttpGet("GetQuanHuyen")]
        public JsonResult GetQuanHuyen()
        {
            string query = @"select QuanHuyen.IDQuanHuyen, QuanHuyen.TenQuanHuyen from QuanHuyen order by IDQuanHuyen ";
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

        [HttpGet("getXaPhuongTheoQuanHuyen/{ad}")]
        public JsonResult GetXaPhuongTheoQuanHuyen(string ad)
        {
            string query = "select a.TenXaPhuong " +
                "from XaPhuong as a , QuanHuyen as b  " +
                "where a.IDQuanHuyen = b.IDQuanHuyen and b.TenQuanHuyen = @username ";
            SqlParameter username = new SqlParameter("username", SqlDbType.NVarChar);
            username.Value = ad;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.Add(username);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpGet("getTuyenThuTheoQuanHuyen/{ad}")]
        public JsonResult getTuyenThuTheoQuanHuyen(string ad)
        {
            string query = 
                "select tt.TenTuyenThu " +
                "from TuyenThu as tt , PhanTuyen as pt , QuanHuyen as qh " +
                "where tt.IDTuyenThu = pt.IDTuyenThu and pt.IDQuanHuyen = qh.IDQuanHuyen and qh.TenQuanHuyen = @username ";
            SqlParameter username = new SqlParameter("username", SqlDbType.NVarChar);
            username.Value = ad;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.Add(username);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }



        [HttpGet("GetTuyenThu")]
        public JsonResult GetTuyenThu()
        {
            string query = @" select DISTINCT tt.TenTuyenThu
                        from PhieuThu as pt , TuyenThu as tt , NhanVien as nv , PhanTuyen as phantuyen
                        where pt.IDTuyenThu = tt.IDTuyenThu";
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


        [HttpGet("GetCustomer")]
        public JsonResult GetCustomer()
        {
            string query = @"select DISTINCT 
                            a.MaKhachHang, a.HoTenKH,a.DiaChi ,a.TrangThai, b.TenXaPhuong ,
                            c.TenQuanHuyen ,e.TenTuyenThu,q.TenKyThu,q.Thang,q.Nam , d.NgayThu ,d.NgayTao
                            from 
                            KhachHang as a , XaPhuong as b , QuanHuyen as c , 
                            PhieuThu as d , TuyenThu as e , KyThu as q 
                            where 
                            a.IDXaPhuong = b.IDXaPhuong and c.IDQuanHuyen = b.IDQuanHuyen and 
                            d.IDKhachHang = a.IDKhachHang and d.IDTuyenThu = e.IDTuyenThu and
                            q.IDKyThu = d.IDKyThu ORDER BY q.TenKyThu,d.NgayThu,e.TenTuyenThu,c.TenQuanHuyen,b.TenXaPhuong DESC";
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



        [HttpGet("GetCustomerall/{ngaybatdau}/{ngayketthuc}/{tenquanhuyen}/{tenxaphuong}/{tenkythu}/{tentuyenthu}")]
        public JsonResult GetCustomerifsall(string ngaybatdau,string ngayketthuc,
         string tenquanhuyen,string tenxaphuong,string tenkythu,string tentuyenthu)
        {         
            string selectform = @"select DISTINCT 
                            a.MaKhachHang, a.HoTenKH,a.DiaChi ,a.TrangThai, b.TenXaPhuong ,
                            c.TenQuanHuyen ,e.TenTuyenThu,q.TenKyThu,q.Thang,q.Nam , d.NgayThu ,d.NgayTao
                            from 
                            KhachHang as a , XaPhuong as b , QuanHuyen as c , 
                            PhieuThu as d , TuyenThu as e , KyThu as q 
                            where 
                            a.IDXaPhuong = b.IDXaPhuong and c.IDQuanHuyen = b.IDQuanHuyen and 
                            d.IDKhachHang = a.IDKhachHang and d.IDTuyenThu = e.IDTuyenThu and
                            q.IDKyThu = d.IDKyThu";
            string whereString = @"";
            string orderByString = @" ORDER BY q.TenKyThu,d.NgayThu,e.TenTuyenThu,c.TenQuanHuyen,b.TenXaPhuong";

            whereString = string.Concat(whereString, " and d.NgayTao between " +"'" + ngaybatdau + "'" + " and " + "'" + ngayketthuc + "'");

            if(tenquanhuyen !=  "noquanhuyen")
            {
                whereString = string.Concat(whereString, " and c.TenQuanHuyen = " + "N'", tenquanhuyen, "' ");
            }
            if (tenxaphuong != "noxaphuong")
            {
                whereString = string.Concat(whereString, " and b.TenXaPhuong = "+ "N'", tenxaphuong, "' ");
            }
            if (tenkythu != "nokythu")
            {
                whereString = string.Concat(whereString, " and q.TenKyThu = " + "N'", tenkythu, "' ");
            }
            if (tentuyenthu != "notuyenthu")
            {
                whereString = string.Concat(whereString, " and e.TenTuyenThu = " + "N'", tentuyenthu, "' ");
            }
            
            string query = string.Concat(selectform, whereString, orderByString);
            Console.WriteLine(query);
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

        [HttpGet("GetCustomerDaThu/{ngaybatdau}/{ngayketthuc}/{tenquanhuyen}/{tenxaphuong}/{tenkythu}/{tentuyenthu}")]
        public JsonResult GetCustomerDaThu(string ngaybatdau, string ngayketthuc,
         string tenquanhuyen, string tenxaphuong, string tenkythu, string tentuyenthu)
        {


            string selectform = @"select DISTINCT 
                            a.MaKhachHang, a.HoTenKH,a.DiaChi ,a.TrangThai, b.TenXaPhuong ,
                            c.TenQuanHuyen ,e.TenTuyenThu,q.TenKyThu,q.Thang,q.Nam , d.NgayThu ,d.NgayTao
                            from 
                            KhachHang as a , XaPhuong as b , QuanHuyen as c , 
                            PhieuThu as d , TuyenThu as e , KyThu as q 
                            where 
                            a.IDXaPhuong = b.IDXaPhuong and c.IDQuanHuyen = b.IDQuanHuyen and 
                            d.IDKhachHang = a.IDKhachHang and d.IDTuyenThu = e.IDTuyenThu and
                            q.IDKyThu = d.IDKyThu";
            string whereString = @"";
            string orderByString = @" ORDER BY q.TenKyThu,d.NgayThu,e.TenTuyenThu,c.TenQuanHuyen,b.TenXaPhuong";

            whereString = string.Concat(whereString, " and d.NgayThu between " + "'" + ngaybatdau + "'" + " and " + "'" + ngayketthuc + "'");

            if (tenquanhuyen != "noquanhuyen")
            {
                whereString = string.Concat(whereString, " and c.TenQuanHuyen = " + "N'", tenquanhuyen, "' ");
            }
            if (tenxaphuong != "noxaphuong")
            {
                whereString = string.Concat(whereString, " and b.TenXaPhuong = " + "N'", tenxaphuong, "' ");
            }
            if (tenkythu != "nokythu")
            {
                whereString = string.Concat(whereString, " and q.TenKyThu = " + "N'", tenkythu, "' ");
            }
            if (tentuyenthu != "notuyenthu")
            {
                whereString = string.Concat(whereString, " and e.TenTuyenThu = " + "N'", tentuyenthu, "' ");
            }

            string query = string.Concat(selectform, whereString, orderByString);
            Console.WriteLine(query);
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
        [HttpGet("GetCustomerChuaThu/{ngaybatdau}/{ngayketthuc}/{tenquanhuyen}/{tenxaphuong}/{tenkythu}/{tentuyenthu}")]
        public JsonResult GetCustomerChuaThu(string ngaybatdau, string ngayketthuc,
         string tenquanhuyen, string tenxaphuong, string tenkythu, string tentuyenthu)
        {


            string selectform = @"select DISTINCT 
                            a.MaKhachHang, a.HoTenKH,a.DiaChi ,a.TrangThai, b.TenXaPhuong ,
                            c.TenQuanHuyen ,e.TenTuyenThu,q.TenKyThu,q.Thang,q.Nam , d.NgayThu ,d.NgayTao
                            from 
                            KhachHang as a , XaPhuong as b , QuanHuyen as c , 
                            PhieuThu as d , TuyenThu as e , KyThu as q 
                            where 
                            a.IDXaPhuong = b.IDXaPhuong and c.IDQuanHuyen = b.IDQuanHuyen and 
                            d.IDKhachHang = a.IDKhachHang and d.IDTuyenThu = e.IDTuyenThu and
                            q.IDKyThu = d.IDKyThu";
            string whereString = @"";
            string orderByString = @" ORDER BY q.TenKyThu,d.NgayThu,e.TenTuyenThu,c.TenQuanHuyen,b.TenXaPhuong";

            whereString = string.Concat(whereString, " and d.NgayTao between " + "'" + ngaybatdau + "'" + " and " + "'" + ngayketthuc + "'" +" and d.Ngaythu " + "is null"  );

            if (tenquanhuyen != "noquanhuyen")
            {
                whereString = string.Concat(whereString, " and c.TenQuanHuyen = " + "N'", tenquanhuyen, "' ");
            }
            if (tenxaphuong != "noxaphuong")
            {
                whereString = string.Concat(whereString, " and b.TenXaPhuong = " + "N'", tenxaphuong, "' ");
            }
            if (tenkythu != "nokythu")
            {
                whereString = string.Concat(whereString, " and q.TenKyThu = " + "N'", tenkythu, "' ");
            }
            if (tentuyenthu != "notuyenthu")
            {
                whereString = string.Concat(whereString, " and e.TenTuyenThu = " + "N'", tentuyenthu, "' ");
            }

            string query = string.Concat(selectform, whereString, orderByString);
            Console.WriteLine(query);
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















    }
}

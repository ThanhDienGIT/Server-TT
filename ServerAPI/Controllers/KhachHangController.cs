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
                order by KhachHang.TrangThai desc
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
                where PhanTuyen.IDNhanVien = " + idNhanVien +
                "order by KhachHang.TrangThai desc"

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

            string getIDKyThuQuery = @"select IDKyThu from KyThu where Thang = Month (SYSDATETIME()) and Nam = Year (SYSDATETIME())";

            string getIDQuery = @"SELECT IDENT_CURRENT('KhachHang') + 1";

            string getIDTuyenThuQuery = @"select IDTuyenThu from XaPhuong where IDTuyenThu IS NOT NULL and IDXaPhuong = " + kh.IDXaPhuong;

            string sqlDataSource = _configuration.GetConnectionString("DBCon");

            DataTable checkCCCD = new DataTable();

            DataTable maxID = new DataTable();

            DataTable IDTuyenThu = new DataTable();

            DataTable maxIDKyThu = new DataTable();

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

                using (SqlCommand myCommand = new SqlCommand(getIDTuyenThuQuery, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    IDTuyenThu.Load(myReader);
                    myReader.Close();
                }

                if (IDTuyenThu.Rows.Count == 0)
                {
                    myCon.Close();
                    return new JsonResult("Không Thể Thêm Khách Hàng Do Chưa tồn tại tuyến thu ở địa chỉ hiện tại");                
                }
                else
                {
                    if (checkCCCD.Rows.Count == 0)
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

                        string formattedNgayCap = kh.NgayCap.ToString("yyyy-MM-dd");

                        string query = @"insert into dbo.KhachHang values
                    (" + kh.IDXaPhuong + @"," + kh.IDLoaiKhachHang + @",'" + maKH + @"',N'" + kh.HoTenKH + @"',N'" + kh.DiaChi + @"','" + kh.CCCD + @"','" + formattedNgayCap + @"',GETDATE()"
                        +@",null,1)";
                        using (SqlCommand myCommand = new SqlCommand(query, myCon))
                        {
                            myReader = myCommand.ExecuteReader();
                            table.Load(myReader);
                            myReader.Close();
                                                   
                        }
                      
                        using (SqlCommand myCommand = new SqlCommand(getIDKyThuQuery, myCon))
                        {
                            myReader = myCommand.ExecuteReader();
                            maxIDKyThu.Load(myReader);
                            myReader.Close();
                        }
                        int SoMaxIDKyThu = 0;
                        if (maxIDKyThu.Rows.Count != 0)
                        {
                            SoMaxIDKyThu = int.Parse(maxIDKyThu.Rows[0][0].ToString());
                        }

                        if (SoMaxIDKyThu != 0)
                        {                      
                            int SoIDTuyenThu = int.Parse(IDTuyenThu.Rows[0][0].ToString());
                            int IDPhieu = 0;
                            string maSoPhieu = "PT";

                            string getIDPhieuQuery = "select IDENT_CURRENT('PhieuThu') + 1";

                            DataTable IDPhieuthu = new DataTable();

                            using (SqlCommand myCommand = new SqlCommand(getIDPhieuQuery, myCon))
                            {
                                myReader = myCommand.ExecuteReader();
                                IDPhieuthu.Load(myReader);
                                myReader.Close();
                            }

                            maSoPhieu = String.Concat(maSoPhieu, IDPhieuthu.Rows[0][0].ToString(),
                                    "MKH", MaxIDKhachHang, "D", DateTime.Today.ToString("ddMMyyyy"));

                            IDPhieu = int.Parse(IDPhieuthu.Rows[0][0].ToString());

                            string queryThemPhieuThu = @"insert into PhieuThu values (" + MaxIDKhachHang + @"," + SoIDTuyenThu + @"," + SoMaxIDKyThu + @",null,'" + maSoPhieu + "'," + kh.IDLoaiKhachHang + @",GETDATE(),null)";
                            
                            using (SqlCommand myCommand = new SqlCommand(queryThemPhieuThu, myCon))
                            {
                                myReader = myCommand.ExecuteReader();
                                myReader.Close();
                            }
                            myCon.Close();
                            return new JsonResult("Thêm Khách Hàng Và Phiếu Thu Thành Công");
                        }
                        else
                        {
                            myCon.Close();
                            return new JsonResult("Thêm Khách Hàng Thành Công");
                        }                   
                    }
                    else
                    {
                        myCon.Close();
                        return new JsonResult("Căn Cước Công Dân Đã Tồn Tại");
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
             
           

            string formattedNgayCap = kh.NgayCap.ToString("yyyy-MM-dd");

            string query = @"update KhachHang 
                            set IDXaPhuong = '" + kh.IDXaPhuong + "', HoTenKH = N'" + kh.HoTenKH + "', DiaChi = N'" + kh.DiaChi + "', CCCD = '" + kh.CCCD + "', NgayCap = '" + formattedNgayCap + "', NgayChinhSua = " + "GETDATE()" + ", IDLoaiKhachHang = '" + kh.IDLoaiKhachHang  + "' where IDKhachHang =" + kh.IDKhachHang ;
            Console.WriteLine(query);
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

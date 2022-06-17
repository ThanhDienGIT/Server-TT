using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TuyenThuController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TuyenThuController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from dbo.TuyenThu ORDER BY TuyenThu.IDTuyenThu DESC";
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

        [HttpGet("{nhanVien}/{quanHuyen}/{xaPhuong}")]
        public JsonResult GetByQuanHuyen(int nhanVien, int quanHuyen, int xaPhuong)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;

            string selectFromString = @"select distinct Tuyenthu.IDTuyenThu, Tuyenthu.MaTuyenThu, 
                    TuyenThu.TenTuyenThu, QuanHuyen.TenQuanHuyen, NhanVien.HoTen, 
                    convert(varchar, NgayBatDau, 103) as NgayBatDau, 
                    convert(varchar, NgayKetThuc, 103) as NgayKetThuc
                from dbo.TuyenThu 
                full outer join dbo.XaPhuong on TuyenThu.IDTuyenThu = XaPhuong.IDTuyenThu 
                full outer join dbo.PhanTuyen on TuyenThu.IDTuyenThu = PhanTuyen.IDTuyenThu 
	            full outer join dbo.QuanHuyen on QuanHuyen.IDQuanHuyen = PhanTuyen.IDQuanHuyen 
                    or QuanHuyen.IDQuanHuyen = XaPhuong.IDQuanHuyen
                full outer join dbo.NhanVien on PhanTuyen.IDNhanVien = NhanVien.IDNhanVien ";
            string whereString = @"where TuyenThu.IDTuyenThu is not null ";
            string orderByString = @"ORDER BY TuyenThu.IDTuyenThu DESC";

            if (nhanVien != -1)
            {
                whereString = String.Concat(whereString, " AND NhanVien.IDNhanVien = ", nhanVien, " ");
            }
            if (quanHuyen != -1)
            {
                whereString = String.Concat(whereString, " AND QuanHuyen.IDQuanHuyen = ", quanHuyen, " ");
            }
            if (xaPhuong != -1)
            {
                whereString = String.Concat(whereString, " AND XaPhuong.IDXaPhuong = ", xaPhuong, " ");
            }

            string query = String.Concat(selectFromString, whereString, orderByString);
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

            //if (quanHuyen == -1 && xaPhuong == -1)
            //{
            //    string query = @"select distinct Tuyenthu.IDTuyenThu, Tuyenthu.MaTuyenThu, 
            //            TuyenThu.TenTuyenThu, QuanHuyen.TenQuanHuyen, NhanVien.HoTen, 
            //            convert(varchar, NgayBatDau, 103) as NgayBatDau, 
            //            convert(varchar, NgayKetThuc, 103) as NgayKetThuc
            //        from dbo.TuyenThu 
            //        full outer join dbo.XaPhuong on TuyenThu.IDTuyenThu = XaPhuong.IDTuyenThu 
            //        full outer join dbo.PhanTuyen on TuyenThu.IDTuyenThu = PhanTuyen.IDTuyenThu 
	           //     full outer join dbo.QuanHuyen on QuanHuyen.IDQuanHuyen = PhanTuyen.IDQuanHuyen 
            //            or QuanHuyen.IDQuanHuyen = XaPhuong.IDQuanHuyen
            //        full outer join dbo.NhanVien on PhanTuyen.IDNhanVien = NhanVien.IDNhanVien
            //        where TuyenThu.IDTuyenThu is not null ORDER BY TuyenThu.IDTuyenThu DESC";
            //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            //    {
            //        myCon.Open();
            //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
            //        {
            //            myReader = myCommand.ExecuteReader();
            //            table.Load(myReader);

            //            myReader.Close();
            //            myCon.Close();
            //        }
            //    }
            //    return new JsonResult(table);
            //}
            //else if (quanHuyen == -1 && xaPhuong != -1)
            //{
            //    string query = @"select distinct Tuyenthu.IDTuyenThu, Tuyenthu.MaTuyenThu, 
            //            TuyenThu.TenTuyenThu, QuanHuyen.TenQuanHuyen, NhanVien.HoTen, 
            //            convert(varchar, NgayBatDau, 103) as NgayBatDau, 
            //            convert(varchar, NgayKetThuc, 103) as NgayKetThuc  
            //        from dbo.TuyenThu 
	           //     full outer join dbo.XaPhuong on TuyenThu.IDTuyenThu = XaPhuong.IDTuyenThu 
            //        full outer join dbo.PhanTuyen on TuyenThu.IDTuyenThu = PhanTuyen.IDTuyenThu 
	           //     full outer join dbo.QuanHuyen on QuanHuyen.IDQuanHuyen = PhanTuyen.IDQuanHuyen
            //            or QuanHuyen.IDQuanHuyen = XaPhuong.IDQuanHuyen
            //        full outer join dbo.NhanVien on PhanTuyen.IDNhanVien = NhanVien.IDNhanVien
            //        where XaPhuong.IDXaPhuong=" + xaPhuong + @" 
            //        AND TuyenThu.IDTuyenThu is not null ORDER BY TuyenThu.IDTuyenThu DESC";
            //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            //    {
            //        myCon.Open();
            //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
            //        {
            //            myReader = myCommand.ExecuteReader();
            //            table.Load(myReader);

            //            myReader.Close();
            //            myCon.Close();
            //        }
            //    }
            //    return new JsonResult(table);
            //}
            //else if (quanHuyen != -1 && xaPhuong == -1)
            //{
            //    string query = @"select distinct Tuyenthu.IDTuyenThu, Tuyenthu.MaTuyenThu,
            //            TuyenThu.TenTuyenThu, QuanHuyen.TenQuanHuyen, NhanVien.HoTen, 
            //            convert(varchar, NgayBatDau, 103) as NgayBatDau,
            //            convert(varchar, NgayKetThuc, 103) as NgayKetThuc 
            //        from dbo.TuyenThu 
            //        full outer join dbo.XaPhuong on TuyenThu.IDTuyenThu = XaPhuong.IDTuyenThu 
            //        full outer join dbo.PhanTuyen on TuyenThu.IDTuyenThu = PhanTuyen.IDTuyenThu 
	           //     full outer join dbo.QuanHuyen on QuanHuyen.IDQuanHuyen = PhanTuyen.IDQuanHuyen
            //            or QuanHuyen.IDQuanHuyen = XaPhuong.IDQuanHuyen
            //        full outer join dbo.NhanVien on PhanTuyen.IDNhanVien = NhanVien.IDNhanVien
            //        where QuanHuyen.IDQuanHuyen=" + quanHuyen +
            //        @" AND TuyenThu.IDTuyenThu is not null ORDER BY TuyenThu.IDTuyenThu DESC";
            //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            //    {
            //        myCon.Open();
            //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
            //        {
            //            myReader = myCommand.ExecuteReader();
            //            table.Load(myReader);

            //            myReader.Close();
            //            myCon.Close();
            //        }
            //    }
            //    return new JsonResult(table);
            //}
            //else //if(quanHuyen != -1 && xaPhuong != -1)
            //{
            //    string query = @"select distinct Tuyenthu.IDTuyenThu, Tuyenthu.MaTuyenThu,
            //            TuyenThu.TenTuyenThu, QuanHuyen.TenQuanHuyen, NhanVien.HoTen, 
            //            convert(varchar, NgayBatDau, 103) as NgayBatDau,
            //            convert(varchar, NgayKetThuc, 103) as NgayKetThuc 
            //        from dbo.TuyenThu 
            //        full outer join dbo.XaPhuong on TuyenThu.IDTuyenThu = XaPhuong.IDTuyenThu 
            //        full outer join dbo.PhanTuyen on TuyenThu.IDTuyenThu = PhanTuyen.IDTuyenThu 
	           //     full outer join dbo.QuanHuyen on QuanHuyen.IDQuanHuyen = PhanTuyen.IDQuanHuyen
            //            or QuanHuyen.IDQuanHuyen = XaPhuong.IDQuanHuyen
            //        full outer join dbo.NhanVien on PhanTuyen.IDNhanVien = NhanVien.IDNhanVien
            //        where XaPhuong.IDXaPhuong=" + xaPhuong + @" and QuanHuyen.IDQuanHuyen=" + quanHuyen +
            //        @" AND TuyenThu.IDTuyenThu is not null ORDER BY TuyenThu.IDTuyenThu DESC";
            //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            //    {
            //        myCon.Open();
            //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
            //        {
            //            myReader = myCommand.ExecuteReader();
            //            table.Load(myReader);

            //            myReader.Close();
            //            myCon.Close();
            //        }
            //    }
            //    return new JsonResult(table);
            //}
        }



        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"Update dbo.PhanTuyen set NgayKetThuc = convert(varchar, SYSDATETIME(), 23) 
                where IDTuyenThu = " + id;
            string query2 = @"Select * from dbo.TuyenThu
                full outer join dbo.PhanTuyen on TuyenThu.IDTuyenThu = PhanTuyen.IDTuyenThu 
                where NgayKetThuc is not null and TuyenThu.IDTuyenThu = " + id;

            DataTable dataTable = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query2, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    dataTable.Load(myReader);
                    myReader.Close();
                    if (dataTable.Rows.Count > 0)
                    {
                        return new JsonResult("Tuyến thu này đã kết thúc. Không thể thực hiện thao tác này");
                    }
                    else
                    {
                        string queryGetXaPhuong = "select IDXaPhuong from XaPhuong where IDTuyenThu = " + id;
                        DataTable tableXaPhuong = new DataTable();
                        using (SqlCommand getXaPhuongCMD = new SqlCommand(queryGetXaPhuong, myCon))
                        {
                            myReader = getXaPhuongCMD.ExecuteReader();
                            tableXaPhuong.Load(myReader);
                            myReader.Close();
                        }
                        for(int i = 0; i < tableXaPhuong.Rows.Count; i++)
                        {
                            int idXaPhuong = int.Parse(tableXaPhuong.Rows[i][0].ToString());
                            string querySetXaPhuong = "Update XaPhuong set IDTuyenThu = null " +
                                @"where IDXaPhuong =  " + idXaPhuong;
                            using (SqlCommand setXaPhuongCommand = new SqlCommand(querySetXaPhuong, myCon))
                            {
                                myReader = setXaPhuongCommand.ExecuteReader();
                                myReader.Close();
                            }
                        }
                        using (SqlCommand delTuyenThuCommand = new SqlCommand(query, myCon))
                        {
                            myReader = delTuyenThuCommand.ExecuteReader();
                            myReader.Close();
                            myCon.Close();
                        }
                        return new JsonResult("Tuyến thu đã được kết thúc thành công");
                    }
                }
            }
        }
    }
}

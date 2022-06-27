﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KyThuController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public KyThuController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from dbo.KyThu ORDER BY nam ASC, thang ASC";
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

        [HttpGet("years")]
        public JsonResult GetYears()
        {
            string query = @"select distinct Nam from dbo.KyThu";
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

        [HttpGet("{id}")]
        public JsonResult GetByID(int id)
        {
            string query = @"select * from dbo.KyThu where IDKyThu = " + id;
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

        [HttpGet("{thang}/{nam}")]
        public JsonResult GetByThangAndNam(int thang, int nam)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;

            if (nam == -1 && thang == -1)
            {
                string query = @"select * from dbo.KyThu ORDER BY nam ASC, thang ASC";
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
            else if (nam == -1 && thang != -1)
            {
                string query = @"select * from dbo.KyThu where thang=" + thang + @"ORDER BY nam ASC, thang ASC";
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
            else if (nam != -1 && thang == -1)
            {
                string query = @"select * from dbo.KyThu where nam=" + nam + @" ORDER BY nam ASC, thang ASC";
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
            else //if(nam != -1 && thang != -1)
            {
                string query = @"select * from dbo.KyThu where thang=" + thang +
                    @" and nam=" + nam + @" ORDER BY nam ASC, thang ASC";
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



        [HttpPost()]
        public JsonResult Post(KyThu kt)
        {
            string queryCheck = "Select * from dbo.KyThu where Thang = " + kt.Thang + @" and Nam =" + kt.Nam;
            DataTable dt = new DataTable();

            string query = @"insert into dbo.KyThu values
                (N'Kỳ thu tháng " + kt.Thang + @" năm " + kt.Nam + @"','" + kt.Thang + @"','" + kt.Nam + @"')";
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(queryCheck, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    dt.Load(myReader);
                    myReader.Close();
                }

                if (dt.Rows.Count > 0)
                {
                    myCon.Close();
                    return new JsonResult("Đã tồn tại kỳ thu tháng " + kt.Thang + " năm " + kt.Nam);
                }
                else
                {
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        myReader.Close();
                        myCon.Close();
                        return new JsonResult("Added Successfully");
                    }
                }
            }

        }


        [HttpPost("{status}")]
        public JsonResult Post(KyThu kt, bool status)
        {
            string queryCheck = "Select * from dbo.KyThu where Thang = " + kt.Thang + @" and Nam =" + kt.Nam;
            DataTable dt = new DataTable();
            string query = @"insert into dbo.KyThu values
                (N'Kỳ thu tháng " + kt.Thang + @" năm " + kt.Nam + @"','" + kt.Thang + @"','" + kt.Nam + @"')";
            string query2 = @"Select IDKyThu from dbo.KyThu where 
                Thang = " + kt.Thang + @" and Nam = " + kt.Nam;
            string query3 = @"Select IDKhachHang, IDTuyenThu, IDLoaiKhachHang from dbo.KhachHang join dbo.XaPhuong on 
                dbo.KhachHang.IDXaPhuong = dbo.XaPhuong.IDXaPhuong where TrangThai = 1";
            string sqlDataSource = _configuration.GetConnectionString("DBCon");

            DataTable table = new DataTable();
            DataTable table2 = new DataTable();
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(queryCheck, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    dt.Load(myReader);
                    myReader.Close();
                }
                if (dt.Rows.Count > 0)
                {
                    myCon.Close();
                    return new JsonResult("Đã tồn tại kỳ thu tháng " + kt.Thang + " năm " + kt.Nam);
                }
                else
                {
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();

                        myReader.Close();
                        myCon.Close();
                    }
                    if (status)
                    {
                        myCon.Open();
                        using (SqlCommand myCommand = new SqlCommand(query2, myCon))
                        {
                            myReader = myCommand.ExecuteReader();
                            table.Load(myReader);

                            myReader.Close();
                        }
                        using (SqlCommand myCommand = new SqlCommand(query3, myCon))
                        {
                            myReader = myCommand.ExecuteReader();
                            table2.Load(myReader);

                            myReader.Close();
                        }

                        int IDKyThu = int.Parse(table.Rows[0][0].ToString());
                        for (int i = 0; i < table2.Rows.Count; i++)
                        {
                            string getIDPhieuQuery = "select IDENT_CURRENT('PhieuThu') + 1";
                            string maSoPhieu = "PT";
                            DataTable tableID = new DataTable();
                            using (SqlCommand myCommand = new SqlCommand(getIDPhieuQuery, myCon))
                            {
                                myReader = myCommand.ExecuteReader();
                                tableID.Load(myReader);
                                myReader.Close();
                            }
                            string IDPhieu = tableID.Rows[0][0].ToString();
                            int IDKH = int.Parse(table2.Rows[i][0].ToString());
                            int IDTuyen = int.Parse(table2.Rows[i][1].ToString());
                            int IDMauSoPhieu = int.Parse(table2.Rows[i][2].ToString());
                            maSoPhieu = String.Concat(maSoPhieu, IDPhieu,
                                "MKH", IDKH, "D", DateTime.Today.ToString("ddMMyyyy"));
                            string query4 = @"insert into PhieuThu values (" + IDKH + @"," + IDTuyen + @",

                                " + IDKyThu + @",null,'"+ maSoPhieu + @"','" + IDMauSoPhieu + @"',GETDATE(),null)";

                            using (SqlCommand myCommand = new SqlCommand(query4, myCon))
                            {
                                myReader = myCommand.ExecuteReader();
                                myReader.Close();
                            }
                        }
                        myCon.Close();
                    }
                    return new JsonResult(new
                    {
                        severity = "success",
                        message = "Thêm kỳ thu thành công"
                    }
                    );
                }
            }
        }

        [HttpPut()]
        public JsonResult Put(KyThu kt)
        {
            string queryCheck = "Select * from dbo.KyThu where Thang = " + kt.Thang + @" and Nam =" + kt.Nam;
            DataTable dt = new DataTable();
            string query = @"update dbo.KyThu set TenKyThu = N'Kỳ thu tháng " + kt.Thang + @" năm " + kt.Nam + @"',
                Thang = " + kt.Thang + @", Nam = " + kt.Nam + @" where idKyThu = " + kt.IDKyThu;
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(queryCheck, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    dt.Load(myReader);
                    myReader.Close();
                }
                if (dt.Rows.Count > 0)
                {
                    myCon.Close();
                    return new JsonResult(new
                    {
                        severity = "warning",
                        message = "Không thể chỉnh sửa. Đã tồn tại kỳ thu tháng " + kt.Thang + " năm " + kt.Nam
                    }
                    );
                }
                else
                {
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        myReader.Close();
                        myCon.Close();
                        return new JsonResult(new
                        {
                            severity = "success",
                            message = "Cập nhật thông tin kỳ thu thành công"
                        }
                        );
                    }
                }
            }
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from dbo.KyThu where IDKyThu = " + id;
            string query2 = @"Select * from dbo.PhieuThu where NgayThu IS NOT NULL and IDKyThu = " + id;
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
                        return new JsonResult(new
                        {
                            severity = "warning",
                            message = "Tồn tại phiếu thu đã được thu trong kỳ thu này.Không thể xoá kỳ thu"
                        }
                        );
                    }
                    else
                    {
                        string query3 = "Delete from dbo.PhieuThu where IDKyThu = " + id;
                        using (SqlCommand delPhieuThuCommand = new SqlCommand(query3, myCon))
                        {
                            myReader = delPhieuThuCommand.ExecuteReader();

                            myReader.Close();
                        }
                        using (SqlCommand delKyThuCommand = new SqlCommand(query, myCon))
                        {
                            myReader = delKyThuCommand.ExecuteReader();

                            myReader.Close();
                            myCon.Close();
                        }
                        return new JsonResult(new
                        {
                            severity = "success",
                            message = "Xoá kỳ thu thành công"
                        }
                        );
                    }
                }
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Models;
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
                    TuyenThu.TenTuyenThu, QuanHuyen.IDQuanHuyen, QuanHuyen.TenQuanHuyen, 
                    NhanVien.IDNhanVien, NhanVien.HoTen, 
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
        }

        [HttpPost("{idQuanHuyen}")]
        public JsonResult Post(int idQuanHuyen, string[] xaPhuongChosenList)
        {
            string queryCheck = "Select TenXaPhuong from dbo.XaPhuong where IDTuyenThu is not null and (";
            string whereString = "";
            for(int i = 1; i <= xaPhuongChosenList.Length; i++)
            {
                if(i != xaPhuongChosenList.Length)
                {
                    whereString = string.Concat(whereString, "TenXaPhuong=N'", xaPhuongChosenList[i-1], "' or ");
                }
                else
                {
                    whereString = string.Concat(whereString, "TenXaPhuong=N'", xaPhuongChosenList[i-1], "')");
                }

            }
            queryCheck = string.Concat(queryCheck, whereString);

            DataTable dt = new DataTable();
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
                    string result = "Tồn tại xã phường đã có tuyến thu: ";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i != dt.Rows.Count - 1)
                        {
                            result = string.Concat(result, dt.Rows[i][0], ", ");
                        }
                        else
                        {
                            result = string.Concat(result, dt.Rows[i][0], ".");
                        }

                    }
                    myCon.Close();
                    return new JsonResult(result);
                }
                else
                {
                    string tenQuanHuyen = "";
                    string queryGetTenQuanHuyen = "Select TenQuanHuyen from QuanHuyen " +
                        "where IDQuanHuyen = " + idQuanHuyen;
                    DataTable tblTenQuanHuyen = new DataTable();
                    using (SqlCommand myCommand = new SqlCommand(queryGetTenQuanHuyen, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        tblTenQuanHuyen.Load(myReader);
                        myReader.Close();
                    }
                    tenQuanHuyen = tblTenQuanHuyen.Rows[0][0].ToString();

                    //Lay Viet tat ten Quan Huyen
                    string subStr;
                    int countSpace = 0;
                    for (int i = 0; i < tenQuanHuyen.Length; i++)
                    {
                        subStr = tenQuanHuyen.Substring(i, 1);
                        if (subStr == " ")
                            countSpace++;
                    }
                    int[] spaceIndex = new int[countSpace];
                    int curSpaceIndex = -1;
                    string maTuyenThu = "";
                    for (int i = 0; i < spaceIndex.Length; i++)
                    {
                        spaceIndex[i] = tenQuanHuyen.IndexOf(" ", curSpaceIndex + 1);
                        curSpaceIndex = spaceIndex[i];
                        maTuyenThu = string.Concat(maTuyenThu, 
                            tenQuanHuyen.Substring(curSpaceIndex + 1, 1));
                    }

                    string queryGetMax = "Select cast(max(" +
                            "substring(MaTuyenThu," + (maTuyenThu.Length + 1) + @",50)) as INT) + 1" 
                        + "from TuyenThu where MaTuyenThu like N'%"+ maTuyenThu + @"%'";
                    DataTable maxMaTuyenThu = new DataTable();
                    using (SqlCommand myCommand = new SqlCommand(queryGetMax, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        maxMaTuyenThu.Load(myReader);
                        myReader.Close();
                    }
                    string maxID = maxMaTuyenThu.Rows[0][0].ToString();
                    if(maxID.Length == 0)
                    {
                        maxID = "1";
                    }
                    for(int i=0; i<=3-maxID.Length; i++)
                    {
                        maxID = string.Concat("0",maxID);
                    }
                    maTuyenThu = string.Concat(maTuyenThu, maxID);
                    string tenTuyen = string.Join(" - ", xaPhuongChosenList);

                    //Insert Tuyen Thu
                    string queryInsertTT = "Insert into TuyenThu values(N'" + maTuyenThu + @"',N'"
                        + tenTuyen + @"')";
                    using (SqlCommand myCommand = new SqlCommand(queryInsertTT, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        myReader.Close();
                    }

                    //Update IDTuyenThu trong dbo.XaPhuong
                    string queryGetIDTuyenThu = "Select IDTuyenThu from TuyenThu " +
                        "where MaTuyenThu=N'" + maTuyenThu +"'";
                    DataTable tblIDTuyenThu = new DataTable();
                    using (SqlCommand myCommand = new SqlCommand(queryGetIDTuyenThu, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        tblIDTuyenThu.Load(myReader);
                        myReader.Close();
                    }
                    int idTuyenThu = int.Parse(tblIDTuyenThu.Rows[0][0].ToString());
                    string queryUpdate = "Update XaPhuong set IDTuyenThu = "+ idTuyenThu +@" where (";
                    string queryUpdateXP = string.Concat(queryUpdate, whereString);
                    using (SqlCommand myCommand = new SqlCommand(queryUpdateXP, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        myReader.Close();
                        myCon.Close();
                        return new JsonResult("Added Successfully");
                    }
                }
            }
        }

        [HttpPut()]
        public JsonResult Put(PhanTuyen pt)
        {
            string querySelect = "Select NgayBatDau, NgayKetThuc from dbo.PhanTuyen where IDTuyenThu = " + pt.IDTuyenThu;
            string queryInsert = "Insert into dbo.PhanTuyen values(" + pt.IDNhanVien
                + @"," + pt.IDTuyenThu + @"," + pt.IDQuanHuyen
                + @", convert(varchar, SYSDATETIME(), 23) ,null)";

            string queryUpdate1 = @"update dbo.PhanTuyen set IDNhanVien = " + pt.IDNhanVien
                + @",  NgayBatDau = convert(varchar, SYSDATETIME(), 23)
                where IDTuyenThu = " + pt.IDTuyenThu;
            string queryUpdate2 = @"update dbo.PhanTuyen set IDNhanVien = " + pt.IDNhanVien
                + @" where IDTuyenThu = " + pt.IDTuyenThu;

            DataTable dt = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(querySelect, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    dt.Load(myReader);
                    myReader.Close();
                }
                if (dt.Rows.Count > 0) 
                {
                    if (string.IsNullOrEmpty(dt.Rows[0][1].ToString()))
                    {
                        //Không thể update
                        myCon.Close();
                        return new JsonResult("Tuyến thu này đã kết thúc. Không thể chỉnh sửa");
                    }
                    else
                    {
                        //Có thể Update
                        if (string.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                        {
                            //Có Update NgayBD (Vì NgayBD Null)
                            using (SqlCommand myCommand = new SqlCommand(queryUpdate1, myCon))
                            {
                                myReader = myCommand.ExecuteReader();
                                myReader.Close();
                                myCon.Close();
                                return new JsonResult("Cập nhật tuyến thu thành công");
                            }
                        }
                        else
                        {
                            //Không Update NgayBD
                            using (SqlCommand myCommand = new SqlCommand(queryUpdate2, myCon))
                            {
                                myReader = myCommand.ExecuteReader();
                                myReader.Close();
                                myCon.Close();
                                return new JsonResult("Cập nhật tuyến thu thành công");
                            }
                        }
                    }
                }
                else
                {
                    //Insert
                    using (SqlCommand myCommand = new SqlCommand(queryInsert, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        myReader.Close();
                        myCon.Close();
                        return new JsonResult("Cập nhật tuyến thu thành công");
                    }
                }
            }
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
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

                        string queryCheckExist = "Select * from dbo.PhanTuyen where IDTuyenThu = " + id;
                        DataTable dt = new DataTable();
                        using (SqlCommand checkExistPhanTuyenCommand = new SqlCommand(queryCheckExist, myCon))
                        {
                            myReader = checkExistPhanTuyenCommand.ExecuteReader();
                            dt.Load(myReader);
                            myReader.Close();
                        }
                        if(dt.Rows.Count > 0)
                        {
                            string query = @"Update dbo.PhanTuyen 
                            set NgayKetThuc = convert(varchar, SYSDATETIME(), 23) 
                            where IDTuyenThu = " + id;
                            using (SqlCommand delTuyenThuCommand = new SqlCommand(query, myCon))
                            {
                                myReader = delTuyenThuCommand.ExecuteReader();
                                myReader.Close();
                                myCon.Close();
                            }
                            return new JsonResult("Tuyến thu đã được kết thúc thành công");
                        }
                        else
                        {
                            string query = @"Delete from dbo.TuyenThu where IDTuyenThu = " + id;
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
}

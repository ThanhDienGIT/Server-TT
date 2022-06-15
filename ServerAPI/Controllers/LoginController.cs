using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"Select * from NhanVien";
            DataTable table = new DataTable();
            clsDatabase.OpenConnection();
            SqlDataReader myReader;
            SqlCommand myCommand = new SqlCommand(query, clsDatabase.con);
            myReader = myCommand.ExecuteReader();
            table.Load(myReader);
            myReader.Close();
            clsDatabase.CloseConnection();
            return new JsonResult(table);
        }
        
          
        [HttpGet("{id}")]
        public JsonResult GetByID(string id)
        {
            string strSelectID = "select * from NhanVien where TaiKhoan = @username";
            clsDatabase.OpenConnection();
            SqlCommand sqlCommand = new SqlCommand(strSelectID, clsDatabase.con);
            SqlParameter username = new SqlParameter("username", SqlDbType.VarChar);
            username.Value = id;
            sqlCommand.Parameters.Add(username);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            int IDstaff = 0;
            if (sqlDataReader.Read())
            {
              IDstaff = sqlDataReader.GetInt32(0);
            }
            sqlDataReader.Close();
            clsDatabase.CloseConnection();
            return new JsonResult(IDstaff);
        }

        [HttpGet("getinfobyID/{ad}")]
        public JsonResult getinfobyID(int ad)
        {
            string strSelectID = "select * from NhanVien where IdNhanVien = @username";
            clsDatabase.OpenConnection();
            SqlCommand sqlCommand = new SqlCommand(strSelectID, clsDatabase.con);
            SqlParameter username = new SqlParameter("username", SqlDbType.Int);
            username.Value = ad;
            sqlCommand.Parameters.Add(username);
            SqlDataReader sqlDataReader1;
            DataTable data1 = new DataTable();
            sqlDataReader1= sqlCommand.ExecuteReader();
            data1.Load(sqlDataReader1);
            sqlDataReader1.Close();
            clsDatabase.CloseConnection();
            return new JsonResult(data1);
        }
        [HttpGet("getmastership/{ul}")]
        public JsonResult getmastership(int ul)
        {
            string strSelectID = "select * from PhanQuyen where IDNhanVien = @username";
            clsDatabase.OpenConnection();
            SqlCommand sqlCommand = new SqlCommand(strSelectID, clsDatabase.con);
            SqlParameter username = new SqlParameter("username", SqlDbType.Int);
            username.Value = ul;
            sqlCommand.Parameters.Add(username);
            SqlDataReader sqlDataReader2;
            DataTable data2 = new DataTable();
            sqlDataReader2 = sqlCommand.ExecuteReader();
            data2.Load(sqlDataReader2);
            sqlDataReader2.Close();
            clsDatabase.CloseConnection();
            return new JsonResult(data2);
        }

        [HttpPost]
        public JsonResult Post(Login login)
        {
            string query = @"Select * from NhanVien";
            DataTable table = new DataTable();
            clsDatabase.OpenConnection();
            SqlDataReader myReader;
            SqlCommand myCommand = new SqlCommand(query, clsDatabase.con);
            myReader = myCommand.ExecuteReader();
            bool tk = false;
            bool mk = false;
            while (myReader.Read())
            {
               if(myReader.GetString(10) == login.Username)
                {
                    tk = true;
                }
               if(myReader.GetString(11) == login.Password)
                {
                    mk = true;
                }
            }
            myReader.Close();
            if (tk == true && mk == true)
            {
                return new JsonResult("Connect");
            }
            else
            {
                return new JsonResult("Failed Connect");
            }
            
        }

        


    }
}

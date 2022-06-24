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
            string query = @"select * from dbo.NhanVien ";
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
        public JsonResult GetByID(string id)
        {
            string query = "select * from NhanVien where TaiKhoan = @username";
            SqlParameter username = new SqlParameter("username", SqlDbType.VarChar);
            username.Value = id;
            int IDstaff = 0;
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

                    if (myReader.Read())
                    {
                        IDstaff = myReader.GetInt32(0);
                    }
                    table.Load(myReader);

<<<<<<< HEAD
<<<<<<< HEAD
                    myReader.Close(); 
=======
                    myReader.Close();
>>>>>>> c5fdbcbdcf20295325ab91c7ba48a869c1d11039
=======
                    myReader.Close();
>>>>>>> b998ebfce2954506c4c42f949b6f6b946e50d037
                    myCon.Close();
                }
            }
            return new JsonResult(IDstaff);
        }

        [HttpGet("getinfobyID/{ad}")]
        public JsonResult getinfobyID(int ad)
        {
            string query = "select * from NhanVien where IdNhanVien = @username";
            SqlParameter username = new SqlParameter("username", SqlDbType.Int);
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


        [HttpGet("getmastership/{ul}")]
        public JsonResult getmastership(int ul)
        {
            string query = "select * from PhanQuyen where IDNhanVien = @username";
            SqlParameter username = new SqlParameter("username", SqlDbType.Int);
            username.Value = ul;
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

        [HttpPost]
        public JsonResult Post(Login login)
        {
            string query = @"select * from dbo.NhanVien ";
            bool UsernameTest = false;
            bool PasswordTest = false;
            string sqlDataSource = _configuration.GetConnectionString("DBCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        if (myReader.GetString(10) == login.Username)
                        {
                            UsernameTest = true;
                        }

                        if (myReader.GetString(11) == login.Password)
                        {
                            PasswordTest = true;
                        }
                    }
                    myReader.Close();
                    myCon.Close();
                }
            }
            if (UsernameTest == true && PasswordTest == true)
            {
                return new JsonResult("Connect");
            }
            else
            {
                return new JsonResult("Username Not Valid");
            }

        }




    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using ServerAPI.Models;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XaPhuongController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public XaPhuongController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select XaPhuong.IDXaPhuong, XaPhuong.TenXaPhuong, XaPhuong.IDQuanHuyen ,QuanHuyen.TenQuanHuyen 
                            from XaPhuong
                            join QuanHuyen on XaPhuong.IDQuanHuyen = QuanHuyen.IDQuanHuyen
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

        [HttpGet("getbyidemp/{idNhanVien}")]
        public JsonResult GetByStatus(int idNhanVien)
        {
            string query = @"
                select * from XaPhuong
                inner join PhanTuyen on XaPhuong.IDTuyenThu = PhanTuyen.IDTuyenThu
                where IDNhanVien = " + idNhanVien;
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
        public JsonResult Post(XaPhuong xp)
        {
            string
                query = @"insert into XaPhuong values(N'"+xp.TenXaPhuong+"',"+xp.IDQuanHuyen+",null);";
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

            return new JsonResult("Added Successfully");
        }

        [HttpDelete]
        public JsonResult Delete(int idXaPhuong)
        {
            string
                query = @"delete XaPhuong where IDXaPhuong = " + idXaPhuong;
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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using ServerAPI.Models;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"Select DepartmentId, DepartmentName from dbo.Department";
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

        [HttpPost]
        public JsonResult Post(Department dep)
        {
            string query = @"insert into dbo.Department values('"+ dep.DepartmentName +@"')";
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
    }
}

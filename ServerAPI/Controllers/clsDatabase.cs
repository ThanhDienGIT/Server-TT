using System.Data;
using System.Data.SqlClient;



namespace ServerAPI.Controllers


{
    public class clsDatabase
    {
        public static SqlConnection con;

        public static bool OpenConnection()
        {
            try
            {
                con = new SqlConnection("Data Source=DESKTOP-1R7APBJ\\SQLEXPRESS;Initial Catalog=QLMoiTruongDoThi; Integrated Security=true");
                con.Open();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public static bool CloseConnection()
        {
            try
            {
                con.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


    }
}
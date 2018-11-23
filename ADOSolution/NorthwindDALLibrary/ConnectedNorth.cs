using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace NorthwindDALLibrary
{
    public class ConnectedNorth
    {
        SqlConnection _northCon;
        public SqlDataReader GetCustomers()
        {
            SqlCommand customerCMD = null;
            try
            {
                customerCMD = new SqlCommand("Select * from Customers", _northCon);
                _northCon.Open();
                return customerCMD.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                if (_northCon.State == System.Data.ConnectionState.Open)
                    _northCon.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using NorthwindDALLibrary;
using System.Data.SqlClient;
using System.Data;

namespace NorthClientConApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CallDataReader();
            //CallScaler();
            //CallNonQuery();
           // CallUpdateQuery();
        }

        private static void CallUpdateQuery()
        {
            
            {
                SqlConnection northCon = null;
                try
                {
                    northCon = new SqlConnection(ConfigurationManager.ConnectionStrings["NorthConnection"].ConnectionString);

                    SqlParameter[] sqlparams = new SqlParameter[2];
                    sqlparams[0] = new SqlParameter("@City", "Airoli");
                    sqlparams[1] = new SqlParameter("@CustomerID", "AAA");

                    SqlCommand custCMD = new SqlCommand("Update Customers set City=@City where CustomerID=@CustomerID", northCon);
                    custCMD.Parameters.AddRange(sqlparams);
                    northCon.Open();
                    Console.WriteLine($"CustCMD: {custCMD.CommandText}");
                    int recEffected = custCMD.ExecuteNonQuery();
                    if (recEffected == 1)
                    {
                        Console.WriteLine("New Customer Created....");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    if (northCon.State == ConnectionState.Open)
                    {
                        northCon.Close();
                    }
                }
            }
        }

        private static void CallNonQuery()
        {
            SqlConnection northCon = null;
            try
            {
                northCon = new SqlConnection(ConfigurationManager.ConnectionStrings["NorthConnection"].ConnectionString);

                string custID ="AAA", compName = "GEP", contactName = "MN", city = "Mumbai", Country = "India";
                string newCust = string.Format("insert into Customers(CustomerID, CompanyName, ContactName, City, Country) values('" + custID + "','" + compName + "','" + contactName + "','" + city + "' ,'" + Country + "')");
                // NEVER NEVER USE CONCATENATION FOR SQL INJECTION
                SqlCommand custCMD = new SqlCommand(newCust, northCon);
                northCon.Open();
                Console.WriteLine($"custCMD: {custCMD.CommandText}");
                int recEffected = custCMD.ExecuteNonQuery();
                if (recEffected == 1)
                {
                    Console.WriteLine("New Customer created ......");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (northCon.State == ConnectionState.Open)
                {
                    northCon.Close();
                }
            }
        }

        private static void CallScaler()
        {
            string conStr = ConfigurationManager.ConnectionStrings["NorthConnection"].ConnectionString;

            SqlConnection northCon = null;
            try
            {
                northCon = new SqlConnection(conStr);
                SqlCommand scalerCMD = new SqlCommand("Select * from Categories", northCon);
                northCon.Open();
                Console.WriteLine($"Select * from Categories with Scaler -> {scalerCMD.ExecuteScalar()}");
                Console.WriteLine($"First: {scalerCMD.CommandText}");
                scalerCMD.CommandText = "Select Max(UnitPrice) from Products";
                Console.WriteLine($"Second: {scalerCMD.CommandText}");
                Console.WriteLine($"Select Max(UnitPrice) from Products with Scaler -> {scalerCMD.ExecuteScalar()}");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (northCon.State == ConnectionState.Open)
                {
                    northCon.Close();
                }
            }
        }

        static void CallDataReader()
        {
            SqlConnection northCon = new SqlConnection("Server=MUM02L2789; Initial Catalog=Northwind; User ID=sa; Password=Password123;");
            SqlCommand categoryCMD = null;
            try
            {
                categoryCMD = new SqlCommand("Select * from Categories", northCon);
                northCon.Open();
                SqlDataReader reader = categoryCMD.ExecuteReader();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.WriteLine($"Column Name: {reader.GetName(i)}");
                }
                Console.WriteLine("---------------------------------------------------------------------");
                int ctr = 1;
                while (reader.Read())
                {
                    Console.WriteLine($"{ctr++} -> {reader.GetValue(0)} : {reader.GetString(1).ToString()} : {reader["Description"]}");
                }
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
                SqlCommand empCMD = new SqlCommand("Select * from Employees", northCon);
                SqlDataReader empReader = empCMD.ExecuteReader();
                while (empReader.Read())
                {
                    Console.WriteLine($"{ctr++} -> {empReader["EmployeeID"]} : {empReader["FirstName"]}");
                }
                if (!empReader.IsClosed)
                {
                    empReader.Close();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (northCon.State == System.Data.ConnectionState.Open)
                {
                    northCon.Close();
                }
            }
            ConnectedNorth conNorth = new ConnectedNorth();

        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DisconnectedNorthConApp
{
    class Program
    {
        DataSet _northDS;
        DataTable _customersTable, _categoriesTable,_productTable,_accountsTable;
        SqlConnection _northCon;
        SqlDataAdapter _customerAdapter, _categoryAdapter,_productAdapter;
        SqlCommandBuilder _customerCB;
        DataRelation _catAndProdRelation;

        public Program()
        {
            _northCon = new SqlConnection(GetConnectionstring);
            _northDS = new DataSet();
        }

        void AddAccountTable()
        {
            
            DataTable AccountsTable = new DataTable("Accounts1");
            AccountsTable.Columns.Add( "AccountNumber",typeof(int));
            AccountsTable.Columns.Add( "HoldersName", typeof(string));
            AccountsTable.Columns.Add( "Balance",  typeof(decimal));

            DataRow dr = AccountsTable.NewRow();

            dr["AccountNumber"] = 101;
            dr["HoldersName"] = "Nikhil";
            dr["Balance"] = 1000;

            DataRow dr1 = AccountsTable.NewRow();

            dr1["AccountNumber"] = 102;
            dr1["HoldersName"] = "Kavya";
            dr1["Balance"] = 10000;

            DataRow dr2 = AccountsTable.NewRow();

            dr2["AccountNumber"] = 103;
            dr2["HoldersName"] = "ravi";
            dr2["Balance"] = 100000;

            AccountsTable.Rows.Add(dr);
            AccountsTable.Rows.Add(dr2);
            AccountsTable.Rows.Add(dr1);


            _northDS.Tables.Add(AccountsTable);

            _accountsTable = _northDS.Tables["Accounts1"];




        }

        public void LoadNorth()
        {
            try
            {
                _customerAdapter = new SqlDataAdapter("Select * from Customers", _northCon);
                _customerAdapter.FillSchema(_northDS, SchemaType.Source, "Customers");
                _customerAdapter.Fill(_northDS,"Customers");
                _customerCB = new SqlCommandBuilder(_customerAdapter);
                _customersTable = _northDS.Tables["Customers"];

                _categoryAdapter = new SqlDataAdapter("Select * from Categories", _northCon);
                _categoryAdapter.Fill(_northDS,"Categories");
                _categoriesTable = _northDS.Tables["Categories"];

                _productAdapter = new SqlDataAdapter("Select * from Products", _northCon);
                _productAdapter.Fill(_northDS, "Products");
                _productTable = _northDS.Tables["Products"];

                _catAndProdRelation = new DataRelation("CatAndProdRel", _categoriesTable.Columns["CategoryID"],
                    _productTable.Columns["CategoryID"]);

                _northDS.Relations.Add(_catAndProdRelation);
                CustNotNull();
                CustSetPrimaryKey();
                AddAccountTable();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
        }

        int DSTableCount
        {
            get { return _northDS.Tables.Count; }
        }
         string GetConnectionstring
        {  
            get
            {
                return  ConfigurationManager.ConnectionStrings["NorthConnection"].ConnectionString;
                

            }


        }

        void DisplayCustomers()
        {
            int ctr = 1;
            foreach (DataRow dr in _customersTable.Rows)
            {
                Console.WriteLine($"{ctr++}-> {dr["CustomerID"]}-- {dr["CompanyName"]}");

            }


        }

        void DisplayCategories()
        {
            int ctr = 1;
            foreach (DataRow dr in _categoriesTable.Rows)
            {
                Console.WriteLine($"{ctr++}-> {dr["CategoryID"]}-- {dr["CategoryName"]}");

            }
        }

        void DisplayProducts()
        {
            int ctr = 1;
            foreach (DataRow dr in _productTable.Rows)
            {
                Console.WriteLine($"{ctr++}-> {dr["SupplierID"]}-- " +
                    $"{dr["ProductName"]} -- {dr["CategoryID"]} -- {dr["UnitPrice"]}");

            }
        }

        void CustNotNull()
        {
          DataColumn companyNameColumn=  _customersTable.Columns["CompanyName"];
            companyNameColumn.AllowDBNull = false;
        }

        void CustSetPrimaryKey()
        {
            DataColumn[] dc = new DataColumn[1];
            dc[0] = _customersTable.Columns["CustomersID"];
            _customersTable.PrimaryKey = dc;
        }
        void AddNewCustomer()
        {
            DataRow dr = _customersTable.NewRow();
            dr["CustomerID"] = "BBB";
            dr["CompanyName"] = "Mumbai";
            dr["ContactName"] = "MN";
            dr["City"]= "Mumbai";
            dr["Country"] = "India";

            try
            {
                _customersTable.Rows.Add(dr);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
            

        }

        void CustEdit(string CustomerID)
        {
            DataRow[] dr =_customersTable.Select("CustomerID='" + CustomerID + "'");
            Console.WriteLine($"Editing customer with CustomerID: {CustomerID}");
            Console.WriteLine($" {dr[0]["CustomerID"]}--{dr[0]["CompanyName"]}--{dr[0]["City"]}");
            dr[0]["City"] = "Berlin";
            Console.WriteLine($" {dr[0]["CustomerID"]}--{dr[0]["CompanyName"]}--{dr[0]["City"]}");



        }
        void DisplayAccounts()
        {
            int ctr = 1;
            foreach (DataRow dr  in _accountsTable.Rows)
            {
                Console.WriteLine($"{ctr++}--{dr["AccountNumber"]}--{dr["HoldersName"]}--{dr["Balance"]} ");
            }

        }
        void SaveToDB()
        {
            try
            {
                //Console.WriteLine($"Insert:\n{_customerCB.GetInsertCommand().CommandText}");
               // Console.WriteLine($"Insert:\n{_customerCB.GetUpdateCommand().CommandText}");
                //Console.WriteLine($"Insert:\n{_customerCB.GetDeleteCommand().CommandText}");

                 _customerAdapter.Update(_northDS, "Customers");

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
        }

        static void Main(string[] args)
        {
            Program pp = new Program();
            pp.LoadNorth();
            //int ctr = 1;
            //for (int i = 0; i < pp._northDS.Tables[0].Rows.Count; i++)
            //{
            //    Console.WriteLine($"{ctr++}->CustID: {pp._northDS.Tables[0].Rows[i][0]}-- " +
            //        $"CompName: {pp._northDS.Tables[0].Rows[i][13]}");


            //}
           // pp.AddNewCustomer();
             pp.DisplayCustomers();
            // pp.DisplayCategories();
            //pp.DisplayProducts();

            // pp.DisplayAccounts();
           // pp.CustEdit("ALFKI");
            //pp.SaveToDB();
            //pp.DisplayCustomers();


            Console.WriteLine($"DS Table Count:{pp.DSTableCount}");
        }
    }
}

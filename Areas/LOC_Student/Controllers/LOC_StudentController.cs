using dbareas.Areas.LOC_City.Models;
using dbareas.Areas.LOC_Country.Models;
using dbareas.Areas.LOC_State.Models;
using dbareas.Areas.LOC_Student.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace dbareas.Areas.LOC_Student.Controllers
{
    [Area("LOC_Student")]
    [Route("LOC_Student/{Controller}/{action}")]
    public class LOC_StudentController : Controller
    {
        #region Index Action

        public IActionResult Index()
        {
            string connectionstr = "Data Source=SHUBHAM\\SQLEXPRESS01;Initial Catalog=22010101618;Integrated Security=True";
            Console.WriteLine(connectionstr);
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Country_SelectAll");
            DataTable dt = new DataTable();
            using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
            {
                dt.Load(dataReader);
            }
            List<CountryDdModel> dl = new List<CountryDdModel>();
            foreach (DataRow dr in dt.Rows)
            {
                CountryDdModel ddl = new CountryDdModel();
                ddl.CountryId = Convert.ToInt32(dr["CountryId"]);
                ddl.CountryName = dr["CountryName"].ToString();
                dl.Add(ddl);
            }
            ViewBag.Country = dl;
            List<StateDDL> loc_State = new List<StateDDL>();
            ViewBag.StateList = loc_State;
            List<CityDDL> loc_City = new List<CityDDL>();
            ViewBag.CityList = loc_City;
            return View();
        }

        #endregion

        #region Save Action

        public IActionResult Save(StudentModel student)
        {
            if (student.StudentName == null)
            {
                TempData["MSG"] = "name is empty";
                return View("ValidateForm");
            }
            else if (student.Studentage == 0 && (student.Studentage > 15 || student.Studentage < 60))
            {
                TempData["MSG"] = "enter Valid age";
                return View("ValidateForm");
            }
            else if (student.StudentEmail == null)
            {
                TempData["MSG"] = "email is empty";
                return View("ValidateForm");
            }
            else
            {
                return View("Index");
            }
        }

        #endregion

    public IActionResult ViewDemo()
    {
        return View();
    }

    public IActionResult ViewDemo1(String gname)
    {
        TempData["Name"] = gname;
        return View();
    }
  
        #region ValidateForm Action

        public IActionResult ValidateForm()
        {
            return View();
        }

        #endregion

        #region DropDownByCountry Action

        public IActionResult DropDownByCountry(int? CountryID)
        {
            string str = "Data Source=SHUBHAM\\SQLEXPRESS01;Initial Catalog=22010101618;Integrated Security=True";
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "STATE_DDL";
            cmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;
            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();

            List<StateDDL> list = new List<StateDDL>();
            foreach (DataRow dr in dt.Rows)
            {
                StateDDL dropdown = new StateDDL();
                dropdown.StateId = Convert.ToInt32(dr["StateID"]);
                dropdown.StateName = dr["StateName"].ToString();
              
                list.Add(dropdown);
                Console.WriteLine(list);
            }
            ViewBag.StateList = list;
            
            var vModel = list;
            return Json(vModel);
        }

        #endregion

        #region DropDownByState Action


        public IActionResult DropDownByState(int StateID)
        {
            string str = "Data Source=SHUBHAM\\SQLEXPRESS01;Initial Catalog=22010101618;Integrated Security=True";
               Console.Write(StateID);
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CITY_DDL";
            cmd.Parameters.Add("@StateID", SqlDbType.Int).Value = StateID;
            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            conn.Close();

            List<CityDDL> list = new List<CityDDL>();
            foreach (DataRow dr in dt.Rows)
            {
                CityDDL dropdown = new CityDDL();
                dropdown.CityId = Convert.ToInt32(dr["CityID"]);
                Console.Write(dropdown.CityId);
                dropdown.CityName = dr["CityName"].ToString();
                list.Add(dropdown);
            }
            ViewBag.StateList = list;
            var vModel = list;
            return Json(vModel);
        }

        #endregion
   

        public IConfiguration Configuration;
        public LOC_StudentController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #region STUDENT LIST
        public IActionResult StudentList()
        {
            string connectionString = this.Configuration.GetConnectionString("conStr");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "PR_Student_SelectAll";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = sqlCommand.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            sqlConnection.Close();
            return View(dataTable);
        }
        #endregion

        #region ADDEDIT
        public IActionResult StudentAddEdit(int? StudentID){
         FillBranch_DropDownMenu();
              string connectionstr = "Data Source=SHUBHAM\\SQLEXPRESS01;Initial Catalog=22010101618;Integrated Security=True";
            Console.WriteLine(connectionstr);
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Country_SelectAll");
            DataTable dt = new DataTable();
            using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
            {
                dt.Load(dataReader);
            }
            List<CountryDdModel> dl = new List<CountryDdModel>();
            foreach (DataRow dr in dt.Rows)
            {
                CountryDdModel ddl = new CountryDdModel();
                ddl.CountryId = Convert.ToInt32(dr["CountryId"]);
                ddl.CountryName = dr["CountryName"].ToString();
                dl.Add(ddl);
            }
            ViewBag.Country = dl;
            List<StateDDL> loc_State = new List<StateDDL>();
            ViewBag.StateList = loc_State;
            List<CityDDL> loc_City = new List<CityDDL>();
            ViewBag.CityList = loc_City;
           
            if (StudentID != null)
            {
                
                string con = this.Configuration.GetConnectionString("conStr");
                DataTable dts = new DataTable();
                SqlConnection sqlconnection = new SqlConnection(con);
                sqlconnection.Open();
                SqlCommand objcmd = sqlconnection.CreateCommand();
                objcmd.CommandType = CommandType.StoredProcedure;
                objcmd.CommandText = "SelectByPk_Student";
                objcmd.Parameters.AddWithValue("StudentID", StudentID);
                SqlDataReader sqldatareader = objcmd.ExecuteReader();
                dts.Load(sqldatareader);
                MST_StudentModel model = new MST_StudentModel();
                foreach (DataRow dr in dts.Rows)
                {
                    model.StudentID = Convert.ToInt32(dr["StudentID"]);
                    model.BranchID = Convert.ToInt32(dr["BranchID"]);
                    model.CityID = Convert.ToInt32(dr["CityID"]);
                    model.StudentName = dr["StudentName"].ToString();
                    model.Email = dr["Email"].ToString();
                    model.MobileNoStudent = Convert.ToString(dr["MobileNoStudent"]);
                    model.MobileNoFather = Convert.ToString(dr["MobileNoFather"]);
                    model.Address = dr["Address"].ToString();
                    model.BirthDate = Convert.ToDateTime(dr["BirthDate"]);
                    model.Age = Convert.ToInt32(dr["Age"]);
                    model.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    model.Gender = dr["Gender"].ToString();
                    model.Password = dr["Password"].ToString();
                }
                return View(model);
            }
            return View();
        }
        #endregion

        #region ADDEDIT METHOD
        public IActionResult AddEditMethod(MST_StudentModel model)
        {

            string connectionstr = this.Configuration.GetConnectionString("conStr");
            DataTable dt = new DataTable();
            SqlConnection sqlconnection = new SqlConnection(connectionstr);
            sqlconnection.Open();
            SqlCommand objcmd = sqlconnection.CreateCommand();
            objcmd.CommandType = CommandType.StoredProcedure;
            if (model.StudentID == null)
            {
                objcmd.CommandText = "PR_Student_Insert";
                objcmd.Parameters.AddWithValue("StudentName", model.StudentName);
                objcmd.Parameters.AddWithValue("CityID", model.CityID);
                objcmd.Parameters.AddWithValue("BranchID", model.BranchID);
                objcmd.Parameters.AddWithValue("MobileNoStudent", model.MobileNoStudent);
                objcmd.Parameters.AddWithValue("Email", model.Email);
                objcmd.Parameters.AddWithValue("MobileNoFather", model.MobileNoFather);
                objcmd.Parameters.AddWithValue("Address", model.Address);
                objcmd.Parameters.AddWithValue("BirthDate", model.BirthDate);
                objcmd.Parameters.AddWithValue("Age", model.Age);
                objcmd.Parameters.AddWithValue("IsActive", model.IsActive);
                objcmd.Parameters.AddWithValue("Gender", model.Gender);
                objcmd.Parameters.AddWithValue("Password", model.Password);
                objcmd.ExecuteNonQuery();
            }
            else
            {
                objcmd.CommandText = "PR_Student_Update";
                objcmd.Parameters.AddWithValue("StudentID", model.StudentID);
                objcmd.Parameters.AddWithValue("CityID", model.CityID);
                objcmd.Parameters.AddWithValue("BranchID", model.BranchID);
                objcmd.Parameters.AddWithValue("StudentName", model.StudentName);
                objcmd.Parameters.AddWithValue("MobileNoStudent", model.MobileNoStudent);
                objcmd.Parameters.AddWithValue("Email", model.Email);
                objcmd.Parameters.AddWithValue("MobileNoFather", model.MobileNoFather);
                objcmd.Parameters.AddWithValue("Address", model.Address);
                objcmd.Parameters.AddWithValue("BirthDate", model.BirthDate);
                objcmd.Parameters.AddWithValue("Age", model.Age);
                objcmd.Parameters.AddWithValue("IsActive", model.IsActive);
                objcmd.Parameters.AddWithValue("Gender", model.Gender);
                objcmd.Parameters.AddWithValue("Password", model.Password);
                objcmd.ExecuteNonQuery();
            }
            return RedirectToAction("StudentList");
        }
        #endregion

        #region DELETE STUDENT
        public IActionResult DeleteStudent(int StudentID)
        {
            string connectionString = this.Configuration.GetConnectionString("conStr");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "PR_Student_Delete";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@StudentID", StudentID);
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return RedirectToAction("StudentList");
        }
        #endregion
          public void FillBranch_DropDownMenu()
        {
            String ConnectionString = this.Configuration.GetConnectionString("conStr");
            List<MST_BranchDropDownModel> mst_BranchDropDowns = new List<MST_BranchDropDownModel>();
      
          
             SqlDatabase sqlDatabase = new SqlDatabase("Data Source=SHUBHAM\\SQLEXPRESS01;Initial Catalog=22010101618;Integrated Security=True");
            DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Branch_Select");
            DataTable dt = new DataTable();
            using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
            {
                dt.Load(dataReader);
            }
            List<MST_BranchDropDownModel> dl = new List<MST_BranchDropDownModel>();
            foreach (DataRow dr in dt.Rows)
            {
                MST_BranchDropDownModel ddl = new MST_BranchDropDownModel();
                ddl.BranchID = Convert.ToInt32(dr["BranchID"]);
                ddl.BranchName = dr["BranchName"].ToString();
                dl.Add(ddl);
            }
              ViewBag.BranchList = dl;
        }

        
    }
   
    }

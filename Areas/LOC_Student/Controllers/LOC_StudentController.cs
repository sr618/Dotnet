using dbareas.Areas.LOC_City.Models;
using dbareas.Areas.LOC_Country.Models;
using dbareas.Areas.LOC_State.Models;
using dbareas.Areas.LOC_Student.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
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
            List<StateDDL> loc_State = new
List<StateDDL>();
            ViewBag.StateList = loc_State;
            List<CityDDL> loc_City = new
           List<CityDDL>();
            ViewBag.CityList = loc_City;
            return View();
         
        }
        public IActionResult Save(StudentModel student)
        {
            if ( student.StudentName==null )
            {
                TempData["MSG"] = "name is empty";
                return View("ValidateForm");
            }
            else if(student.Studentage == 0 && (student.Studentage >15 || student.Studentage < 60))
            {
                TempData["MSG"] = "enter Valid age";
                return View("ValidateForm");
            }

        else    if (student.StudentEmail == null)
            {
                TempData["MSG"] = "email is empty";
                return View("ValidateForm");
            }
            else
            {
                return View("Index");
            }
        }
        public IActionResult ValidateForm()
        {
            return View();
        }
        public IActionResult DropDownByCountry(int? CountryID)
        {

            string str = "Data Source = SHUBHAM\\SQLEXPRESS01; Initial Catalog = 22010101618; Integrated Security = True";
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
            }
            ViewBag.StateList = list;
            var vModel = list;
            return Json(vModel);
        }
        public IActionResult DropDownByState(int? StateID)
        {
            string str = "Data Source=SHUBHAM\\SQLEXPRESS01;Initial Catalog=22010101618;Integrated Security=True";
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
                dropdown.CityName = dr["CityName"].ToString();
                list.Add(dropdown);
            }
            ViewBag.CityList = list;
            var vModel = list;
            return Json(vModel);
        }

    }
}

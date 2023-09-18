using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using dbareas.Areas.LOC_Country.Models;
using System.Data.SqlClient;

namespace dbareas.Areas.LOC_Country.Controllers
{
    [Area("LOC_Country")]
    [Route("LOC_Country/{Controller}/{action}")]
    public class LOC_CountryController : Controller
    {
        private IConfiguration Configuration;
        public LOC_CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        public IActionResult Index()
        {
            string connectionstr = this.Configuration.GetConnectionString("constr");
            Console.WriteLine(connectionstr);
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Country_SelectAll");
            DataTable dt = new DataTable();
            using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
            {
                dt.Load(dataReader);
            }


            return View(dt);
        }
        public IActionResult Delete(int id)
        {
            string connectionstr = this.Configuration.GetConnectionString("conStr");
            Console.WriteLine(connectionstr);
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            SqlConnection connection = new SqlConnection(connectionstr);

            connection.Open();
            SqlCommand dbCommand = new SqlCommand("PR_Country_DeleteByPK", connection);

            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.Parameters.AddWithValue("@CountryId", id);
            dbCommand.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Index", "LOC_Country", new { area = "LOC_Country" });
        }
        public IActionResult Save(CountryModel modelCountry)
        {

            string connectionstr = this.Configuration.GetConnectionString("conStr");
            Console.WriteLine(connectionstr);
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            SqlConnection connection = new SqlConnection(connectionstr);

            if (modelCountry.CountryId == 0)
            {

                connection.Open();
                SqlCommand dbCommand = new SqlCommand("PR_Country_Insert", connection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.Parameters.Add("@CountryName", SqlDbType.VarChar).Value = modelCountry.CountryName;
                dbCommand.Parameters.Add("@CountryCode", SqlDbType.VarChar).Value = modelCountry.CountryCode;
                dbCommand.ExecuteNonQuery();
                connection.Close();
                return RedirectToAction("Index");

            }
            else
            {
                connection.Open();
                SqlCommand dbCommand = new SqlCommand("PR_Country_UpdateByPK", connection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.Parameters.AddWithValue("@CountryId", modelCountry.CountryId);
                dbCommand.Parameters.Add("@CountryName", SqlDbType.VarChar).Value = modelCountry.CountryName;
                dbCommand.Parameters.Add("@CountryCode", SqlDbType.VarChar).Value = modelCountry.CountryCode;
                dbCommand.ExecuteNonQuery();
                connection.Close();
                return RedirectToAction("Index");
            }
        }
        
            public IActionResult AddEditCountry(int? id)
            {
                if (id == null)
                {
                    return View();
                }
                else
                {

                    String connectionstr = this.Configuration.GetConnectionString("conStr");
                    DataTable dt = new DataTable();
                    SqlConnection sqlConnection = new SqlConnection(connectionstr);
                    sqlConnection.Open();
                    SqlCommand ObjCmd = sqlConnection.CreateCommand();
                    ObjCmd.CommandType = CommandType.StoredProcedure;
                    ObjCmd.CommandText = "PR_Country_SelectByPK";
                    ObjCmd.Parameters.AddWithValue("CountryID", id);
                    SqlDataReader sqlDataReader = ObjCmd.ExecuteReader();
                    dt.Load(sqlDataReader);
                    CountryModel model = new CountryModel();
                    foreach (DataRow dr in dt.Rows)
                    {
                        model.CountryId = int.Parse(dr["CountryId"].ToString());
                        model.CountryName = dr["CountryName"].ToString();
                        model.CountryCode = dr["CountryCode"].ToString();
                    }
                    return View(model);
                }
            }
        }
    }

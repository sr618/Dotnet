using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using dbareas.Areas.LOC_City.Models;
using dbareas.Areas.LOC_State.Models;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace dbareas.Areas.LOC_City.Controllers
{
    [Area("LOC_City")]
    [Route("LOC_City/{Controller}/{action}")]

    public class LOC_CityController : Controller
    {
        private IConfiguration configuration;
        public LOC_CityController(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }
        public IActionResult Index()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("constr"));
            conn.Open();
            SqlCommand sqlCommand = conn.CreateCommand(); ;
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_City_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader dr = sqlCommand.ExecuteReader();
            dt.Load(dr);
    
            return View(dt);

        }
        public IActionResult AddEditCity(int? id)
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("constr"));
            conn.Open();
            SqlCommand sqlCommand = conn.CreateCommand(); ;
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "StateList";
            DataTable dt = new DataTable();
            SqlDataReader dr = sqlCommand.ExecuteReader();
            dt.Load(dr);
            List<StateDDL> stateDDL = new List<StateDDL>();
            foreach (DataRow data in dt.Rows)
            {
                StateDDL stateModel = new StateDDL();
                stateModel.StateId = Convert.ToInt32(data["StateId"]);
                stateModel.StateName = data["StateName"].ToString();
                stateDDL.Add(stateModel);
            }
            ViewBag.state = stateDDL;
            conn.Close();
            if (id == null)
            {
                return View();
            }
            else
            {
                DataTable dtx = new DataTable();
                SqlConnection sqlConnection = new SqlConnection("Data Source=SHUBHAM\\SQLEXPRESS01;Initial Catalog=22010101618;Integrated Security=True");
                sqlConnection.Open();
                SqlCommand ObjCmd = sqlConnection.CreateCommand();
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.CommandText = "PR_City_SelectByPK";
                ObjCmd.Parameters.AddWithValue("CityID", id);
                SqlDataReader sqlDataReader = ObjCmd.ExecuteReader();
                dtx.Load(sqlDataReader);
                CityModel model = new CityModel();
                foreach (DataRow drx in dtx.Rows)
                {
                    //     model.StateId = int.Parse(dr["StateId"].ToString());
                    model.CityName = drx["CityName"].ToString();
                    model.CityCode = drx["CityCode"].ToString();
                    model.StateId = Convert.ToInt32(drx["StateID"]);


                }
                return View(model);
            }
        }
        public IActionResult Save(CityModel modelCity)
        {

            string connectionstr = "Data Source=SHUBHAM\\SQLEXPRESS01;Initial Catalog=22010101618;Integrated Security=True";
            Console.WriteLine(connectionstr);
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            SqlConnection connection = new SqlConnection(connectionstr);

            if (modelCity.StateId == 0)
            {

                connection.Open();
                SqlCommand dbCommand = new SqlCommand("PR_City_Insert", connection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.Parameters.Add("@CityName", SqlDbType.VarChar).Value = modelCity.CityName;
                dbCommand.Parameters.Add("@CityCode", SqlDbType.VarChar).Value = modelCity.CityCode;
                dbCommand.Parameters.Add("@StateID", SqlDbType.VarChar).Value = modelCity.StateId;
                dbCommand.ExecuteNonQuery();
                connection.Close();
                return RedirectToAction("Index");

            }
            else
            {
                connection.Open();
                SqlCommand dbCommand = new SqlCommand("PR_City_UpdateByPK", connection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.Parameters.AddWithValue("@CityId", modelCity.StateId);
                dbCommand.Parameters.Add("@CityName", SqlDbType.VarChar).Value = modelCity.CityName;
                dbCommand.Parameters.Add("@CityCode", SqlDbType.VarChar).Value = modelCity.CityCode;
                dbCommand.Parameters.Add("@StateID", SqlDbType.VarChar).Value = modelCity.StateId;
                dbCommand.ExecuteNonQuery();
                connection.Close();
                return RedirectToAction("Index");
            }
        }


    }
}


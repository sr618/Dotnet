using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using dbareas.Areas.LOC_Branch.Models;
using System.Data.SqlClient;

namespace dbareas.Areas.LOC_Branch.Controllers
{
    [Area("LOC_Branch")]
    [Route("LOC_Branch/{Controller}/{action}")]
    public class LOC_BranchController : Controller
    {
        private IConfiguration Configuration;
        public LOC_BranchController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        public IActionResult Index()
        {
            string connectionstr = this.Configuration.GetConnectionString("constr");
            Console.WriteLine(connectionstr);
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_Branch_Select");
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
            SqlCommand dbCommand = new SqlCommand("PR_Branch_Delete", connection);

            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.Parameters.AddWithValue("@BranchId", id);
            dbCommand.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Index", "LOC_Branch", new { area = "LOC_Branch" });
        }
        public IActionResult Save(BranchModel modelBranch)
        {

            string connectionstr = this.Configuration.GetConnectionString("conStr");
            Console.WriteLine(connectionstr);
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            SqlConnection connection = new SqlConnection(connectionstr);

            if (modelBranch.BranchId == 0)
            {

                connection.Open();
                SqlCommand dbCommand = new SqlCommand("PR_Branch_Insert", connection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.Parameters.Add("@BranchName", SqlDbType.VarChar).Value = modelBranch.BranchName;
                dbCommand.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = modelBranch.BranchCode;
                dbCommand.ExecuteNonQuery();
                connection.Close();
                return RedirectToAction("Index");

            }
            else
            {
                connection.Open();
                SqlCommand dbCommand = new SqlCommand("PR_Branch_Update", connection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.Parameters.AddWithValue("@BranchId", modelBranch.BranchId);
                dbCommand.Parameters.Add("@BranchName", SqlDbType.VarChar).Value = modelBranch.BranchName;
                dbCommand.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = modelBranch.BranchCode;
                dbCommand.ExecuteNonQuery();
                connection.Close();
                return RedirectToAction("Index");
            }
        }

        public IActionResult AddEditBranch(int? id)
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
                ObjCmd.CommandText = "PR_Branch_SelectByPK";
                ObjCmd.Parameters.AddWithValue("BranchID", id);
                SqlDataReader sqlDataReader = ObjCmd.ExecuteReader();
                dt.Load(sqlDataReader);
                BranchModel model = new BranchModel();
                foreach (DataRow dr in dt.Rows)
                {
                    model.BranchId = int.Parse(dr["BranchId"].ToString());
                    model.BranchName = dr["BranchName"].ToString();
                    model.BranchCode = dr["BranchCode"].ToString();
                }
                return View(model);
            }
        }
    }
}

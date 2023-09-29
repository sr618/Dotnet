using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using dbareas.Areas.LOC_State.Models;
using dbareas.Areas.LOC_Country.Models;

namespace dbareas.Areas.LOC_State.Controllers
{
    [Area("LOC_State")]
    [Route("LOC_State/{Controller}/{action}")]
    public class LOC_StateController : Controller
    {
        #region Index Action

        public IActionResult Index()
        {
            string connectionstr = "Data Source=SHUBHAM\\SQLEXPRESS01;Initial Catalog=22010101618;Integrated Security=True";
            Console.WriteLine(connectionstr);
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            DbCommand dbCommand = sqlDatabase.GetStoredProcCommand("PR_State_SelectAll");
            DataTable dt = new DataTable();
            using (IDataReader dataReader = sqlDatabase.ExecuteReader(dbCommand))
            {
                dt.Load(dataReader);
            }
            return View(dt);
        }

        #endregion

        #region Delete Action

        public IActionResult Delete(int id)
        {
            string connectionstr = "Data Source=SHUBHAM\\SQLEXPRESS01;Initial Catalog=22010101618;Integrated Security=True";
            Console.WriteLine(connectionstr);
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            SqlConnection connection = new SqlConnection(connectionstr);

            connection.Open();
            SqlCommand dbCommand = new SqlCommand("PR_State_DeleteByPK", connection);

            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.Parameters.AddWithValue("@StateId", id);
            dbCommand.ExecuteNonQuery();
            connection.Close();
            return RedirectToAction("Index", "LOC_State", new { area = "LOC_State" });
        }

        #endregion

        #region AddEditState Action

        public IActionResult AddEditState(int? id)
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
            if (id == null)
            {
                return View();
            }
            else
            {
                DataTable dtx = new DataTable();
                SqlConnection sqlConnection = new SqlConnection(connectionstr);
                sqlConnection.Open();
                SqlCommand ObjCmd = sqlConnection.CreateCommand();
                ObjCmd.CommandType = CommandType.StoredProcedure;
                ObjCmd.CommandText = "PR_State_SelectByPK";
                ObjCmd.Parameters.AddWithValue("StateID", id);
                SqlDataReader sqlDataReader = ObjCmd.ExecuteReader();
                dtx.Load(sqlDataReader);
                StateModel model = new StateModel();
                foreach (DataRow dr in dtx.Rows)
                {
                    model.StateName = dr["StateName"].ToString();
                    model.StateCode = dr["StateCode"].ToString();
                    model.CountryID = dr["CountryID"].ToString();
                }
                return View(model);
            }
        }

        #endregion

        #region Save Action

        public IActionResult Save(StateModel modelState)
        {
            string connectionstr = "Data Source=SHUBHAM\\SQLEXPRESS01;Initial Catalog=22010101618;Integrated Security=True";
            Console.WriteLine(connectionstr);
            SqlDatabase sqlDatabase = new SqlDatabase(connectionstr);
            SqlConnection connection = new SqlConnection(connectionstr);

            if (modelState.StateId == 0)
            {
                connection.Open();
                SqlCommand dbCommand = new SqlCommand("PR_State_Insert", connection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.Parameters.Add("@StateName", SqlDbType.VarChar).Value = modelState.StateName;
                dbCommand.Parameters.Add("@StateCode", SqlDbType.VarChar).Value = modelState.StateCode;
                dbCommand.Parameters.Add("@CountryID", SqlDbType.VarChar).Value = modelState.CountryID;
                dbCommand.ExecuteNonQuery();
                connection.Close();
                return RedirectToAction("Index");
            }
            else
            {
                connection.Open();
                SqlCommand dbCommand = new SqlCommand("PR_State_UpdateByPK", connection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.Parameters.AddWithValue("@StateId", modelState.StateId);
                dbCommand.Parameters.Add("@StateName", SqlDbType.VarChar).Value = modelState.StateName;
                dbCommand.Parameters.Add("@StateCode", SqlDbType.VarChar).Value = modelState.StateCode;
                dbCommand.Parameters.Add("@CountryID", SqlDbType.VarChar).Value = modelState.CountryID;
                dbCommand.ExecuteNonQuery();
                connection.Close();
                return RedirectToAction("Index");
            }
        }

        #endregion
    }
}

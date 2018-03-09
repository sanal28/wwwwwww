using ETLCenterWebServices.Common_Library;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;

namespace ETLCenterWebServices
{
    /// <summary>
    /// Summary description for ETLCenterEmployeeService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ETLCenterEmployeeService : System.Web.Services.WebService
    {
        public string connection { get; set; }

        [WebMethod]
        public bool EmployeeCreation(int ID_Employee, string EmpName,string EmpPassword,
                                    string EmpEmail, int EmpIsActive,int EmpIsDelete,
                                    int EnteredBy,int ModifiedBy, int Action)
        {
            bool EmployeeCreationflag;
            try
            {
                connection = ConfigurationManager.ConnectionStrings[Constants.connection].ConnectionString;
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(Constants.UspEmployeeUpdate, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(Constants.ID_Employee, SqlDbType.Int).Value = ID_Employee;
                        cmd.Parameters.Add(Constants.EmpName, SqlDbType.NVarChar).Value = EmpName;
                        cmd.Parameters.Add(Constants.EmpPassword, SqlDbType.NVarChar).Value = EmpPassword;
                        cmd.Parameters.Add(Constants.EmpEmail, SqlDbType.NVarChar).Value = EmpEmail;
                        cmd.Parameters.Add(Constants.EmpIsActive, SqlDbType.Bit).Value = EmpIsActive;
                        cmd.Parameters.Add(Constants.EmpIsDelete, SqlDbType.Bit).Value = EmpIsDelete;
                        cmd.Parameters.Add(Constants.EnteredBy, SqlDbType.Int).Value = EnteredBy;
                        cmd.Parameters.Add(Constants.ModifiedBy, SqlDbType.Int).Value = ModifiedBy;
                        cmd.Parameters.Add(Constants.Action, SqlDbType.TinyInt).Value = Action;
                        con.Open();
                        EmployeeCreationflag = Convert.ToBoolean(cmd.ExecuteNonQuery());
                        return EmployeeCreationflag;

                    }
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {

            }

        }
        [WebMethod]
        public string SelectEmployeeDetails()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                connection = ConfigurationManager.ConnectionStrings[Constants.connection].ConnectionString;
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(Constants.UspEmployeeSelect, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;                
                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        DataTable dataTable = new DataTable();
                        dataTable.Load(dataReader);

                        con.Close();
                        if (dataTable.Rows.Count > 0)
                        {
                            return JsonConvert.SerializeObject(dataTable, Newtonsoft.Json.Formatting.Indented);
                        }
                        else
                        {
                            return "0";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                return "0";
            }
           
        }

    }
}

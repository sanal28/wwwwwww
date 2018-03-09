using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Xml;
using Newtonsoft.Json;
using ETLCenterWebServices.Common_Library;

namespace ETLCenterWebServices
{
    /// <summary>
    /// Summary description for ETLCenterLoginService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ETLCenterLoginService : System.Web.Services.WebService
    {
        public string connection { get; set; }
        [WebMethod]
        public string Login(string employeeEmail, string password)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                connection = ConfigurationManager.ConnectionStrings[Constants.connection].ConnectionString;
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(Constants.UspEmployeeValidLogin, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(Constants.EmpEmail, SqlDbType.VarChar).Value = employeeEmail;
                        cmd.Parameters.Add(Constants.EmpPassword, SqlDbType.VarChar).Value = password;
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
                            return JsonConvert.SerializeObject(0, Newtonsoft.Json.Formatting.Indented);
                        }               
                    }
                }   
            }
            catch (Exception ex)
            {
                CommonFunctions commonFun = new CommonFunctions();
                commonFun.LogError(HttpContext.Current, ex.Message, "Login", "ETLCenterLoginService");
                commonFun = null;
                return JsonConvert.SerializeObject(0, Newtonsoft.Json.Formatting.Indented);
            }
            finally
            {
            }           
        }

        [WebMethod]
        public bool ForgotPassword(string mailId)
        {
            bool mailFlag = false;
            try
            {
                Common_Library.CommonFunctions functionMail = new Common_Library.CommonFunctions();
                connection = ConfigurationManager.ConnectionStrings[Constants.connection].ConnectionString;
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(Constants.UspEmpForgotPassword, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(Constants.EmpEmail, SqlDbType.VarChar).Value = mailId;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataReader dataReader = cmd.ExecuteReader();
                        DataTable dataTable = new DataTable();
                        dataTable.Load(dataReader);
                        if (dataTable.Rows.Count > 0) 
                            {
                                string encryptForget = Crypto.Encryption((Convert.ToString(dataTable.Rows[0]["ID_Employee"])));
                                string msgBody = "<a id='resetLink' href='http://localhost:53097//Login/Reset?userId=" + encryptForget + "'> Reset Link </a>";
                                mailFlag = functionMail.SendEmail(mailId, "", "", "Reset Password", " Your Reset Password Link is  " +
                                          msgBody);
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions commonFun = new CommonFunctions();
                commonFun.LogError(HttpContext.Current, ex.Message, "forgotPassword", "ETLCenterLoginService");
                commonFun = null;
            }
            return mailFlag;
        }
        [WebMethod]
        public int RestPassword(string encryptId,string newPassword)
        {
            int passwdUpdateFlag;
            try
            {
                string userId = Crypto.Decrypt(encryptId);
                connection = ConfigurationManager.ConnectionStrings[Constants.connection].ConnectionString;
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(Constants.UspEmployeeResetPassword, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(Constants.userId, SqlDbType.VarChar).Value = userId;
                        cmd.Parameters.Add(Constants.EmpPassword, SqlDbType.NVarChar).Value = newPassword;
                        con.Open();
                        passwdUpdateFlag = cmd.ExecuteNonQuery();
                        return passwdUpdateFlag;

                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions commonFun = new CommonFunctions();
                commonFun.LogError(HttpContext.Current, ex.Message, "RestPassword", "ETLCenterLoginService");
                commonFun = null;
                return -1;
            }
            finally
            {
            }       

        }
        [WebMethod]
        public bool Support(string userName, string emailId, string message)
        {
            try
            {
                bool mailFlag=false;
                bool Supportflag=false;
                connection = ConfigurationManager.ConnectionStrings[Constants.connection].ConnectionString;
                using (SqlConnection con = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(Constants.UspELTSupportUpdate, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(Constants.ESEmail, SqlDbType.VarChar).Value = emailId;
                        cmd.Parameters.Add(Constants.ESName, SqlDbType.VarChar).Value = userName;
                        cmd.Parameters.Add(Constants.ESMessage, SqlDbType.VarChar).Value = message;
                        cmd.Parameters.Add(Constants.Action, SqlDbType.Int).Value = 1;
                        con.Open();
                        Supportflag = Convert.ToBoolean(cmd.ExecuteNonQuery());
                        if (Supportflag == true)
                        {
                            Common_Library.CommonFunctions functionMail = new Common_Library.CommonFunctions();


                            mailFlag = functionMail.SendEmail("sanal@nuvento.com", "", "", "ETL Support", " "+message+ "from "+"  "+ emailId
                                        );
                            return mailFlag;
                        }
                        else
                        {
                            return mailFlag;
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunctions commonFun = new CommonFunctions();
                commonFun.LogError(HttpContext.Current, ex.Message, "Support", "ETLCenterLoginService");
                commonFun = null;
                return false;
            }
            finally
            {
            }

        }
    }
}

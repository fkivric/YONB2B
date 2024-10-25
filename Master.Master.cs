using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YONB2B.Class;
using static YONB2B.Class.Siniflar;

namespace YONB2B
{
    public partial class Master : System.Web.UI.MasterPage
    {
        public static string ConnectionString = "Server=192.168.4.24;Database=VDB_YON01;User Id=sa;Password=MagicUser2023!;";
        public static string ConnectionString2 = "Server=192.168.4.24;Database=MDE_GENEL;User Id=sa;Password=MagicUser2023!;";
        SqlConnection sql = new SqlConnection(ConnectionString);
        SqlConnection sql2 = new SqlConnection(ConnectionString2);
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                var loginRes = (List<LoginObj>)Session["Login"];
                if (loginRes != null)
                {
                    username.InnerText = loginRes[0].SONAME;
                    if (loginRes[0].CURVAL == "YON")
                    {
                        userfirma.InnerText = loginRes[0].CURVAL;
                        Firma0.Visible = false;
                        Firma1.Visible = false;
                        Firma2.Visible = false;
                    }
                    else
                    {
                        userfirma.InnerText = DbQuery.GetValue($"select CURNAME from CURRENTS  where CURVAL = '{loginRes[0].CURVAL}'");
                        Yon0.Visible = false;
                        Yon1.Visible = false;
                        Yon2.Visible = false;
                        Yon3.Visible = false;
                        Yon4.Visible = false;
                    }
                    //var dt = DbQuery.Query($"select SONAME + ' ' + SOSURNAME as name, CURNAME from SOCIAL left outer join CURRENTS on CURID = SOCURID where SOCODE = '{loginRes[0].SOCODE}'", ConnectionString);
                }
                else
                {
                    Response.Redirect("NewLogin.aspx");
                }
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }
            else
            {
            }
        }
    }
}
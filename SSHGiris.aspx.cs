using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static YONB2B.Class.Siniflar;
using YONB2B.Class;

namespace YONB2B
{
    public partial class SSHGiris : System.Web.UI.Page
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
                    //string q = String.Format(@"select count(*) as toplam,sum(case when WPTREVAL is NULL or WPTREVAL = '0 'then 0 else 1 end) as aktif
                    //from PRODUCTS
                    //left outer join WAVEPRODUCTS on WPROID = PROID and WPROUNIQ = 8
                    //left outer join WAVEPROTREE on WPROUNIQ = WPTREUNIQ and WPROVAL = WPTREVAL
                    //left outer join PROSUPPLIER on PROSUPPROID = PROID
                    //left outer join CURRENTS on CURID = PROSUPCURID
                    //left outer join SOCIAL on SOCURID = CURID
                    //where PROSTS=1 and SOCODE = '{0}'", loginRes[0].SOCODE);
                    //string w = String.Format(@"select count(*)
                    //from PRODUCTS
                    //left outer join WAVEPRODUCTS on WPROID = PROID and WPROUNIQ = 9
                    //left outer join WAVEPROTREE on WPROUNIQ = WPTREUNIQ and WPROVAL = WPTREVAL
                    //left outer join PROSUPPLIER on PROSUPPROID = PROID
                    //left outer join CURRENTS on CURID = PROSUPCURID
                    //left outer join SOCIAL on SOCURID = CURID
                    //where PROSTS=1 
                    //and WPTREVAL = '1'");
                    //string x = q + String.Format($"\r\n and SOCODE = '{loginRes[0].SOCODE}'");

                    //var dt1 = DbQuery.Query(q, ConnectionString);
                    //var dt2 = DbQuery.GetValue(w);
                    //var dt3 = DbQuery.GetValue(x);
                    //double oran = double.Parse(dt1.Rows[0]["aktif"].ToString()) / double.Parse(dt1.Rows[0]["toplam"].ToString());
                    //double oran2 = double.Parse(dt1.Rows[0]["aktif"].ToString()) / double.Parse(dt2);
                    //stoktoplam.InnerText = dt1.Rows[0]["toplam"].ToString();
                    //stokgoup.InnerText = dt2;
                    //stokaktif.InnerText = dt1.Rows[0]["aktif"].ToString();
                    //yukselis.InnerText = oran.ToString().Substring(0, 4) + "%";
                    //aktiforan.Style["width"] = oran.ToString().Substring(0, 4) + "%";
                    //grouporan.InnerText = oran2.ToString() + "%";
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
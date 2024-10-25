using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
    public partial class DashBoard : System.Web.UI.Page
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
                    DataCek(loginRes[0].SOCODE);
                    string q = String.Format(@"select count(*) as toplam,sum(case when T.WPROVAL is NULL or T.WPROVAL = '0 'then 0 else 1 end) as aktif
                    from PRODUCTS
                    LEFT OUTER JOIN WAVEPRODUCTS N ON WPROID = PROID
                    LEFT OUTER JOIN WAVEPRODUCTS T ON T.WPROID = PROID AND T.WPROUNIQ = 8
                    LEFT OUTER JOIN WAVEPROTREE ON WPTREUNIQ = N.WPROUNIQ AND N.WPROVAL = WPTREVAL
                    LEFT OUTER JOIN CURRENTS ON CURVAL = WPTREVAL
                    LEFT OUTER JOIN SOCIAL ON SOCURID = CURID
                    WHERE PROSTS = 1 AND SOCODE = '{0}'", loginRes[0].SOCODE);
                    string w = String.Format(@"select count(*)
                    from PRODUCTS
                    left outer join WAVEPRODUCTS on WPROID = PROID and WPROUNIQ = 9
                    left outer join WAVEPROTREE on WPROUNIQ = WPTREUNIQ and WPROVAL = WPTREVAL
                    left outer join PROSUPPLIER on PROSUPPROID = PROID
                    left outer join CURRENTS on CURID = PROSUPCURID
                    left outer join SOCIAL on SOCURID = CURID
                    where PROSTS=1  and PROPROUID in (103,36)
                    and WPTREVAL = '1'");
                    string x = w + String.Format($"\r\n and SOCODE = '{loginRes[0].SOCODE}'");

                    var dt1 = DbQuery.Query(q, ConnectionString);
                    var dt2 = DbQuery.GetValue(w);
                    var dt3 = DbQuery.GetValue(x);
                    double oran = double.Parse(dt1.Rows[0]["aktif"].ToString()) / double.Parse(dt1.Rows[0]["toplam"].ToString());
                    double oran2 = double.Parse(dt1.Rows[0]["aktif"].ToString()) / double.Parse(dt2);
                    stoktoplam.InnerText = dt1.Rows[0]["toplam"].ToString();
                    stokgoup.InnerText = dt2;
                    stokaktif.InnerText = dt1.Rows[0]["aktif"].ToString();
                    if (oran > 0)
                    {
                        yukselis.InnerText = oran.ToString().Substring(0, 4) + "%";
                        aktiforan.Style["width"] = oran.ToString().Substring(0, 4) + "%";
                        grouporan.InnerText = oran2.ToString() + "%";
                    }
                    else
                    {
                        yukselis.InnerText = "0";
                        aktiforan.Style["width"] = "0%";
                        grouporan.InnerText = "0";
                    }
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
        void DataCek(string SOCODE)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = $"select \r\n" +
                    $"Markaid,Marka,\r\n" +
                    $"ToplamAdet,\r\n" +
                    $"netsinifaktif,\r\n" +
                    $"netaktif,\r\n" +
                    $"ResimYüklenen,\r\n" +
                    $"convert(int,round((case when netsinifaktif != 0 then netsinifaktif else 1 end /case when ToplamAdet != 0 then ToplamAdet else 1 end)*100,0)) as SinifOran,\r\n" +
                    $"convert(int,round((case when netaktif != 0 then netaktif else 1 end/case when netsinifaktif != 0 then netsinifaktif else 1 end)*100,0)) as AktifOran\r\n" +
                    $"from (\r\n" +
                    $"select markaa.WPTREVAL as Markaid,markaa.WPTRENAME as Marka ,\r\n" +
                    $"convert(numeric(18,2),count(*)) as ToplamAdet,\r\n" +
                    $"convert(numeric(18,2),sum(case when (netsinifa.WPTREVAL is null or netsinifa.WPTREVAL = '0') then 0 else 1 end)) as netsinifaktif,\r\n" +
                    $"convert(numeric(18,2),sum(case when (neta.WPTREVAL is null or neta.WPTREVAL = '0') then 0 else 1 end)) as netaktif,\r\n" +
                    $"convert(numeric(18,2),count(STOKID)) as ResimYüklenen\r\n" +
                    $"from PRODUCTS\r\n" +
                    $"LEFT OUTER JOIN WAVEPRODUCTS marka ON marka.WPROID = PROID and marka.WPROUNIQ = 1\r\n" +
                    $"LEFT OUTER JOIN WAVEPROTREE markaa on markaa.WPTREUNIQ = marka.WPROUNIQ and marka.WPROVAL = markaa.WPTREVAL\r\n" +
                    $"LEFT OUTER JOIN WAVEPRODUCTS firma on firma.WPROID = PROID and firma.WPROUNIQ = 7\r\n" +
                    $"LEFT OUTER JOIN WAVEPROTREE firmaa on firmaa.WPTREUNIQ = firma.WPROUNIQ and firmaa.WPTREVAL = firma.WPROVAL\r\n" +
                    $"LEFT OUTER JOIN WAVEPRODUCTS net on net.WPROID = PROID and net.WPROUNIQ = 8\r\n" +
                    $"LEFT OUTER JOIN WAVEPROTREE neta on neta.WPTREUNIQ = net.WPROUNIQ and neta.WPTREVAL = net.WPROVAL\r\n" +
                    $"LEFT OUTER JOIN WAVEPRODUCTS netsinif on netsinif.WPROID = PROID and netsinif.WPROUNIQ = 10\r\n" +
                    $"LEFT OUTER JOIN WAVEPROTREE netsinifa on netsinifa.WPTREUNIQ = netsinif.WPROUNIQ and netsinifa.WPTREVAL = netsinif.WPROVAL\r\n" +
                    $"LEFT OUTER JOIN CURRENTS ON CURVAL = firmaa.WPTREVAL\r\n" +
                    $"LEFT OUTER JOIN SOCIAL ON SOCURID = CURID\r\n" +
                    $"outer apply (select distinct STOKID from W.[Yonavm_Web_Siparis].[dbo].[EntegreFStokResimleri] where STOKID = PROID) as EntegreFStokResimleri\r\n" +
                    $"WHERE SOCODE = '{SOCODE}'\r\n" +
                    $"group by markaa.WPTREVAL,markaa.WPTRENAME) net";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                ProductRepeater.DataSource = reader;
                ProductRepeater.DataBind();
            }

        }
    }
}
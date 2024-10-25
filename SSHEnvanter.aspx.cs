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
    public partial class SSHEnvanter : System.Web.UI.Page
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
                    sorgu();
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
        private static List<Envanterlistesi> envanter = new List<Envanterlistesi>();
        private static List<Envanterlistesi> secili = new List<Envanterlistesi>();
        void sorgu()
        {

            Marka.Enabled = true;
            Marka.DataSource = null;
            Marka.Items.Clear();
            string q = String.Format(@"
            select '' as CURID,'  SEÇİNİZ...' as CURNAME
            union
            select CURID,upper(CURNAME) as CURNAME from CURRENTS where CURSUPPLIER = 1 and CURSTS = 1
            and exists (select * from PROSUPPLIER 
			            left outer join PRODUCTS on PROID = PROSUPPROID
			            where PROSUPCURID = CURID and PROPROUID = 36)
            order by 2");
            var dt = DbQuery.Query(q, ConnectionString);
            Marka.DataSource = dt;
            Marka.DataValueField = "CURID";
            Marka.DataTextField = "CURNAME";
            Marka.DataBind();
            Listele.Enabled = true;
        }
        void sorgu2()
        {

            string q = @"select * from PRODUCTS
                    left outer join WAVEPRODUCTS on WPROID = PROID and WPROUNIQ = '5'
                    left outer join WAVEPROTREE on WPROUNIQ = WPTREUNIQ and WPROVAL = WPTREVAL
                    left outer join PROSUPPLIER on PROSUPPROID = PROID
                    where 1 = 1 and PROPROUID in (36,103)";
            if (string.IsNullOrEmpty(StokAdi.Text) && StokAdi.Text != "")
            {
                q = q + String.Format(@"
                   and PRONAME like '%{0}%'", StokAdi.Text);
            }
            if (Marka.SelectedValue != "" && Marka.SelectedValue != "0")
            {
                q = q + String.Format(@"
                    and PROSUPCURID = '{0}'", Marka.SelectedValue);
            }
            if (Model.SelectedValue != "")
            {
                q = q + String.Format(@"
                    and WPTREVAL = '{0}'", Model.SelectedValue);
            }
            if (StokKodu.Text != "")
            {
                q = q + String.Format(@"
                    and PROVAL = '{0}'", StokKodu.Text);
            }
            envanter = DbQuery.Query(q, ConnectionString).DataTableToList<Envanterlistesi>();
            Urunler.DataSource = envanter;
            Urunler.DataBind();
        }
        protected void Listele_Click(object sender, EventArgs e)
        {
            row.Attributes["hidden"] = null;
            sorgu2();
        }
        protected void marka_SelectedIndexChanged(object sender, EventArgs e)
        {
            var CURID = Marka.SelectedValue;
            Model.Enabled = true;
            Model.DataSource = null;
            Model.Items.Clear();
            string q = String.Format(@"
            select '' as WPTREVAL,'  SEÇİNİZ...' as WPTRENAME
            union
            select distinct WPTREVAL,upper(WPTRENAME) as WPTRENAME from WAVEPROTREE
            left outer join WAVEPRODUCTS on WPROUNIQ = WPTREUNIQ and WPROVAL = WPTREVAL
            where WPROUNIQ = 5 
            and exists (select * from PROSUPPLIER 
			            left outer join PRODUCTS on PROID = PROSUPPROID
			            where PROID = WPROID and PROPROUID = 36  and PROSUPCURID = {0})
            order by 2", CURID);
            var dt = DbQuery.Query(q, ConnectionString);
            Model.DataSource = dt;
            Model.DataValueField = "WPTREVAL";
            Model.DataTextField = "WPTRENAME";
            Model.DataBind();
        }

        protected void Urunler_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Cells.Count > 1)
            {
                var ss = e.Row.RowType;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
            }

        }

        protected void Urunler_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                // Seçilen satırın indeksini al
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                // Satır verilerini almak için GridView'den ilgili satırı çek
                GridViewRow row = Urunler.Rows[rowIndex];

                // Hücre verilerini al
                string proid = row.Cells[1].Text; // PROID hücresi
                string proval = row.Cells[2].Text; // PROVAL hücresi
                string proname = row.Cells[3].Text; // PRONAME hücresi

                // İstediğiniz işlemleri burada yapabilirsiniz
                // Örneğin:
                // Verileri URL parametreleri olarak gönder
                string url = $"SSHOlusTurma.aspx?proid={Server.UrlEncode(proid)}&proval={Server.UrlEncode(proval)}&proname={Server.UrlEncode(proname)}";
                Response.Redirect(url);
            }

        }
    }
}
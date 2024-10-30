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
using System.Drawing.Printing;

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
                    sorgu1();
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
                // PostBack olduğunda sayfa numarasını al
                string pageIndexStr = Request["__EVENTARGUMENT"];
                if (int.TryParse(pageIndexStr, out int pageIndex))
                {
                    BindProductData(pageIndex);
                }
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
        void sorgu1()
        {
            string magaza = "";
            bool tek = true;
            var loginRes = (List<LoginObj>)Session["Login"];
            if (loginRes[0].DIVVAL.Length >= 3)
            {
                tek = false;
                var magazalar = DbQuery.Query($"select * from MDE_GENEL.dbo.FK_fn_Split('{loginRes[0].DIVVAL.ToString()}',',')", ConnectionString);
                for (int i = 0; i < magazalar.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        magaza = "'" + magazalar.Rows[i][0].ToString() + "'";
                    }
                    else
                    {
                        magaza = magaza + ",'" + magazalar.Rows[i][0].ToString() + "'";
                    }
                }

                string q = String.Format(@"
                select '' as DEPOVAL, 'SEÇİNİZ...' as DEPONAME
                union
                select distinct DIVVAL as DEPOVAL, DIVNAME as DEPONAME from DIVISON where DIVSTS = 1
                and DIVVAL in ({0})
                ", magaza);
                var dt = DbQuery.Query(q, ConnectionString);
                Depo.DataSource = dt;
                Depo.DataValueField = "DEPOVAL";
                Depo.DataTextField = "DEPONAME";
                Depo.DataBind();
                Depo.Enabled = true;
            }
            else
            {
                magaza = loginRes[0].DIVVAL.ToString();
                string q = String.Format(@"
                select distinct DIVVAL as DEPOVAL, DIVNAME as DEPONAME from DIVISON where DIVSTS = 1
                and DIVVAL = '{0}'
                ", magaza);
                var dt = DbQuery.Query(q, ConnectionString);
                Depo.DataSource = dt;
                Depo.DataValueField = "DEPOVAL";
                Depo.DataTextField = "DEPONAME";
                Depo.DataBind();
                Depo.SelectedValue = magaza;
            }
        }
        static string Sorgu = "";
        protected void Listele_Click(object sender, EventArgs e)
        {
            row.Attributes["hidden"] = null;
            BindProductData();
        }
        private const int PageSize = 20;
        private void BindProductData(int pageIndex = 1)
        {
            Sorgu = String.Format(@"select PROID,PROVAL,PRONAME,isnull(QUAN,0) as adet,
                        ROW_NUMBER() OVER (ORDER BY PRONAME) AS RowNum from PRODUCTS
                    left outer join WAVEPRODUCTS on WPROID = PROID and WPROUNIQ = '5'
                    left outer join WAVEPROTREE on WPROUNIQ = WPTREUNIQ and WPROVAL = WPTREVAL
                    left outer join PROSUPPLIER on PROSUPPROID = PROID
					outer apply (select sum(PINVQUAN) as QUAN from PROINV 
								inner join DEFSTORAGE on DSTORID = PINVSTORID
								where PINVPROID = PROID and DSTORDIVISON = '{0}'
								and PINVYEAR != '' and PINVMONTH != '') PROINV
                    where 1 = 1 and PROPROUID in (36,103) and len(PROVAL) > 6", Depo.SelectedValue);

            if (string.IsNullOrEmpty(StokAdi.Text) || StokAdi.Text != "")
            {
                Sorgu = Sorgu + String.Format(@"
                   and PRONAME like '%{0}%'", StokAdi.Text);
            }
            if (Marka.SelectedValue != "" || Marka.SelectedValue != "0")
            {
                Sorgu = Sorgu + String.Format(@"
                    and PROSUPCURID = '{0}'", Marka.SelectedValue);
            }
            if (Model.SelectedValue != "")
            {
                Sorgu = Sorgu + String.Format(@"
                    and WPTREVAL = '{0}'", Model.SelectedValue);
            }
            if (StokKodu.Text != "")
            {
                Sorgu = Sorgu + String.Format(@"
                    and PROVAL = '{0}'", StokKodu.Text);
            }
            if (QUAN.Checked)
            {
                Sorgu = Sorgu + @"
                    and QUAN <> 0";
            }
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                // İlk olarak toplam kayıt sayısını almak için sorgu
                string countQuery = String.Format(@"
                select count(*) from (
                {0}
                ) AS TotalCount", Sorgu);

                SqlCommand countCmd = new SqlCommand(countQuery, conn);

                int totalCount = (int)countCmd.ExecuteScalar();
                int totalPages = (int)Math.Ceiling((double)totalCount / PageSize);

                // İkinci sorgu: sayfalanmış verileri almak için
                string query = String.Format(@"
                SELECT * FROM (
                {0}
                ) AS Result 
                WHERE RowNum BETWEEN @start AND @end
                order by PRONAME", Sorgu);

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@start", (pageIndex - 1) * PageSize + 1);
                cmd.Parameters.AddWithValue("@end", pageIndex * PageSize);

                SqlDataReader reader = cmd.ExecuteReader();
                ProductRepeater.DataSource = reader;
                ProductRepeater.DataBind();

                litPagination.Text = GeneratePagination(totalPages, pageIndex);
            }
        }
        private string GeneratePagination(int totalPages, int currentPage)
        {
            int startPage = Math.Max(1, currentPage - 5);
            int endPage = Math.Min(totalPages, startPage + 9);

            string paginationHtml = "<div class='pagination'>";

            // Önceki sayfa bağlantısı
            if (currentPage > 1)
            {
                paginationHtml += $"<a href='javascript:void(0);' onclick='__doPostBack(\"{Marka.UniqueID}\", \"{currentPage - 1}\");'>Önceki</a>";
            }

            // Sayfa numaraları
            for (int i = startPage; i <= endPage; i++)
            {
                if (i == currentPage)
                {
                    paginationHtml += $"<a class='active'>{i}</a>";
                }
                else
                {
                    paginationHtml += $"<a href='javascript:void(0);' onclick='__doPostBack(\"{Marka.UniqueID}\", \"{i}\");'>{i}</a>";
                }
            }

            // Sonraki sayfa bağlantısı
            if (currentPage < totalPages)
            {
                paginationHtml += $"<a href='javascript:void(0);' onclick='__doPostBack(\"{Marka.UniqueID}\", \"{currentPage + 1}\");'>Sonraki</a>";
            }

            paginationHtml += "</div>";
            return paginationHtml;
        }

        //private string GeneratePagination(int totalPages, int currentPage)
        //{
        //    int startPage = Math.Max(1, currentPage - 5);
        //    int endPage = Math.Min(totalPages, startPage + 9);

        //    string paginationHtml = "<div class='pagination'>";

        //    // Önceki sayfa bağlantısı
        //    if (currentPage > 1)
        //    {
        //        paginationHtml += $"<a href='javascript:void(0);' onclick='{postBackFunctionName}(\"{currentPage - 1}\");'>Önceki</a>";
        //    }

        //    // Sayfa numaraları
        //    for (int i = startPage; i <= endPage; i++)
        //    {
        //        if (i == currentPage)
        //        {
        //            paginationHtml += $"<a class='active'>{i}</a>";
        //        }
        //        else
        //        {
        //            paginationHtml += $"<a href='javascript:void(0);' onclick='{postBackFunctionName}(\"{i}\");'>{i}</a>";
        //        }
        //    }

        //    // Sonraki sayfa bağlantısı
        //    if (currentPage < totalPages)
        //    {
        //        paginationHtml += $"<a href='javascript:void(0);' onclick='{postBackFunctionName}(\"{currentPage + 1}\");'>Sonraki</a>";
        //    }

        //    paginationHtml += "</div>";
        //    return paginationHtml;
        //}

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

        protected void Unnamed_CheckedChanged(object sender, EventArgs e)
        {
            if (QUAN.Checked)
            {
                EnvanterDurum.InnerText = "Envanterde Olanlar";
            }
            else
            {
                EnvanterDurum.InnerText = "Stokta Olmayanlar";
            }
        }
    }
}
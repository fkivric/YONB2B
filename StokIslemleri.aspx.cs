using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static YONB2B.Class.Siniflar;
using System.Net;
using YONB2B.Class;
using System.Collections;

namespace YONB2B
{
    public partial class StokIslemleri : System.Web.UI.Page
    {
        public static string ConnectionString = "Server=192.168.4.24;Database=VDB_YON01;User Id=sa;Password=MagicUser2023!;";
        public static string ConnectionString2 = "Server=192.168.4.24;Database=MDE_GENEL;User Id=sa;Password=MagicUser2023!;";
        SqlConnection sql = new SqlConnection(ConnectionString);
        SqlConnection sql2 = new SqlConnection(ConnectionString2);
        static string stoklistessorgu = @"select PROID,PROVAL,PRONAME,case when wptweb.WPTREVAL = '1' then 'Aktif' else 'Pasif' end as durum, resimadet from PRODUCTS
                left outer join WAVEPRODUCTS on WPROID = PROID and WPROUNIQ = '1'
                left outer join WAVEPROTREE n on n.WPTREUNIQ = WPROUNIQ and WPROVAL = n.WPTREVAL				
                left outer join WAVEPRODUCTS wpweb on wpweb.WPROID = PROID and wpweb.WPROUNIQ = '8'
                left outer join WAVEPROTREE wptweb on wptweb.WPTREUNIQ = wpweb.WPROUNIQ and wpweb.WPROVAL = wptweb.WPTREVAL
				outer apply (select count(*) as resimadet from W.[Yonavm_Web_Siparis].dbo.EntegreFStokResimleri where STOKID = PROID) resim
                where PROSTS = 1  and n.WPTREVAL = '{0}'";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var loginRes = (List<LoginObj>)Session["Login"];
                if (loginRes != null)
                {
                    string q = String.Format(@"select '0' as WPTREVAL, 'Seçiniz....!' as Adi union select distinct WPTREVAL,MDE_GENEL.dbo.CapitalizeWords(WPTRENAME) as Adi from PRODUCTS
                    left outer join WAVEPRODUCTS on WPROID = PROID and WPROUNIQ = 1
                    left outer join WAVEPROTREE on WPROUNIQ = WPTREUNIQ and WPROVAL = WPTREVAL
                    left outer join PROSUPPLIER on PROSUPPROID = PROID
                    left outer join CURRENTS on CURID = PROSUPCURID
                    left outer join SOCIAL on SOCURID = CURID
                    where SOCODE = '{0}'", loginRes[0].SOCODE);
                    marka.DataSource = DbQuery.Query(q, ConnectionString);
                    marka.DataValueField = "WPTREVAL";
                    marka.DataTextField = "Adi";
                    marka.DataBind();

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
                    BindProductData("", pageIndex);
                }
            }

        }
        protected void marka_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (marka.SelectedValue != "0")
            {
                BindProductData();
                //using (SqlConnection conn = sql)
                //{
                //    string q = String.Format(stoklistessorgu, marka.SelectedValue);
                //    SqlCommand cmd = new SqlCommand(q, conn);
                //    conn.Open();
                //    SqlDataReader reader = cmd.ExecuteReader();
                //    ProductRepeater.DataSource = reader;
                //    ProductRepeater.DataBind();
                //    conn.Close();
                //    //conn.Open();
                //    //SqlDataAdapter da = new SqlDataAdapter(q, conn);
                //    //DataTable dt = new DataTable();
                //    //da.Fill(dt);
                //    //Urunler.DataSource = dt;
                //    //Urunler.DataBind(); 

                //}
            }
            else
            {
                //Urunler.DataSource = null;
                //Urunler.DataBind();
            }
        }

        private const int PageSize = 20;
        private void BindProductData(string search = "", int pageIndex = 1)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                // İlk olarak toplam kayıt sayısını almak için sorgu
                string countQuery = @"
                SELECT COUNT(*) FROM (
                    SELECT PROID
                    FROM PRODUCTS
                    LEFT OUTER JOIN WAVEPRODUCTS ON WPROID = PROID AND WPROUNIQ = '1'
                    LEFT OUTER JOIN WAVEPROTREE n ON n.WPTREUNIQ = WPROUNIQ AND WPROVAL = n.WPTREVAL
                    LEFT OUTER JOIN WAVEPRODUCTS wpweb ON wpweb.WPROID = PROID AND wpweb.WPROUNIQ = '8'
                    LEFT OUTER JOIN WAVEPROTREE wptweb ON wptweb.WPTREUNIQ = wpweb.WPROUNIQ AND wpweb.WPROVAL = wptweb.WPTREVAL
                    OUTER APPLY (
                        SELECT COUNT(*) AS resimadet 
                        FROM W.[Yonavm_Web_Siparis].dbo.EntegreFStokResimleri 
                        WHERE STOKID = PROID
                    ) resim
                    WHERE PROSTS = 1 
                    AND n.WPTREVAL = @marka
                    AND (PRONAME LIKE @search OR PROID LIKE @search OR PROVAL LIKE @search)
                ) AS TotalCount";

                SqlCommand countCmd = new SqlCommand(countQuery, conn);
                countCmd.Parameters.AddWithValue("@marka", marka.SelectedValue);
                countCmd.Parameters.AddWithValue("@search", "%" + search + "%");

                int totalCount = (int)countCmd.ExecuteScalar();
                int totalPages = (int)Math.Ceiling((double)totalCount / PageSize);

                // İkinci sorgu: sayfalanmış verileri almak için
                string query = @"
                SELECT * FROM (
                    SELECT 
                        PROID,
                        PROVAL,
                        PRONAME,
                        CASE WHEN wptweb.WPTREVAL = '1' THEN 'Aktif' ELSE 'Pasif' END AS durum,
                        resimadet,
                        ROW_NUMBER() OVER (ORDER BY PROID) AS RowNum
                    FROM PRODUCTS
                    LEFT OUTER JOIN WAVEPRODUCTS ON WPROID = PROID AND WPROUNIQ = '1'
                    LEFT OUTER JOIN WAVEPROTREE n ON n.WPTREUNIQ = WPROUNIQ AND WPROVAL = n.WPTREVAL
                    LEFT OUTER JOIN WAVEPRODUCTS wpweb ON wpweb.WPROID = PROID AND wpweb.WPROUNIQ = '8'
                    LEFT OUTER JOIN WAVEPROTREE wptweb ON wptweb.WPTREUNIQ = wpweb.WPROUNIQ AND wpweb.WPROVAL = wptweb.WPTREVAL
                    OUTER APPLY (
                        SELECT COUNT(*) AS resimadet 
                        FROM W.[Yonavm_Web_Siparis].dbo.EntegreFStokResimleri 
                        WHERE STOKID = PROID
                    ) resim
                    WHERE PROSTS = 1 
                    AND n.WPTREVAL = @marka
                    AND (PRONAME LIKE @search OR PROID LIKE @search OR PROVAL LIKE @search)
                ) AS Result 
                WHERE RowNum BETWEEN @start AND @end";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@marka", marka.SelectedValue);
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");
                cmd.Parameters.AddWithValue("@start", (pageIndex - 1) * PageSize + 1);
                cmd.Parameters.AddWithValue("@end", pageIndex * PageSize);

                SqlDataReader reader = cmd.ExecuteReader();
                ProductRepeater.DataSource = reader;
                ProductRepeater.DataBind();

                litPagination.Text = GeneratePagination(totalPages, pageIndex);
            }
            //        using (SqlConnection conn = new SqlConnection(ConnectionString))
            //        {
            //            string query = String.Format(@"select PROID,PROVAL,PRONAME,case when wptweb.WPTREVAL = '1' then 'Aktif' else 'Pasif' end as durum, resimadet from PRODUCTS
            //            left outer join WAVEPRODUCTS on WPROID = PROID and WPROUNIQ = '1'
            //            left outer join WAVEPROTREE n on n.WPTREUNIQ = WPROUNIQ and WPROVAL = n.WPTREVAL				
            //            left outer join WAVEPRODUCTS wpweb on wpweb.WPROID = PROID and wpweb.WPROUNIQ = '8'
            //            left outer join WAVEPROTREE wptweb on wptweb.WPTREUNIQ = wpweb.WPROUNIQ and wpweb.WPROVAL = wptweb.WPTREVAL
            //outer apply (select count(*) as resimadet from W.[Yonavm_Web_Siparis].dbo.EntegreFStokResimleri where STOKID = PROID) resim
            //            where PROSTS = 1  and n.WPTREVAL = '{0}' AND PRONAME like @search", marka.SelectedValue);
            //            SqlCommand cmd = new SqlCommand(query, conn);
            //            cmd.Parameters.AddWithValue("@search", "%" + search + "%");
            //            conn.Open();
            //            int totalCount = (int)cmd.ExecuteScalar();
            //            int totalPages = (int)Math.Ceiling((double)totalCount / PageSize);

            //            query = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY PROID) AS RowNum, * FROM Products WHERE PRONAME LIKE @search OR PROID LIKE @search OR PROVAL LIKE @search) AS Result WHERE RowNum BETWEEN @start AND @end";
            //            cmd = new SqlCommand(query, conn);
            //            cmd.Parameters.AddWithValue("@search", "%" + search + "%");
            //            cmd.Parameters.AddWithValue("@start", (pageIndex - 1) * PageSize + 1);
            //            cmd.Parameters.AddWithValue("@end", pageIndex * PageSize);

            //            SqlDataReader reader = cmd.ExecuteReader();
            //            ProductRepeater.DataSource = reader;
            //            ProductRepeater.DataBind();

            //            litPagination.Text = GeneratePagination(totalPages, pageIndex);
            //        }
        }
        private string GeneratePagination(int totalPages, int currentPage)
        {
            int startPage = Math.Max(1, currentPage - 5);
            int endPage = Math.Min(totalPages, startPage + 9);

            string paginationHtml = "<div class='pagination'>";

            // Önceki sayfa bağlantısı
            if (currentPage > 1)
            {
                paginationHtml += $"<a href='javascript:void(0);' onclick='__doPostBack(\"{Search.UniqueID}\", \"{currentPage - 1}\");'>Önceki</a>";
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
                    paginationHtml += $"<a href='javascript:void(0);' onclick='__doPostBack(\"{Search.UniqueID}\", \"{i}\");'>{i}</a>";
                }
            }

            // Sonraki sayfa bağlantısı
            if (currentPage < totalPages)
            {
                paginationHtml += $"<a href='javascript:void(0);' onclick='__doPostBack(\"{Search.UniqueID}\", \"{currentPage + 1}\");'>Sonraki</a>";
            }

            paginationHtml += "</div>";
            return paginationHtml;
        }

        protected void Search_TextChanged(object sender, EventArgs e)
        {
            BindProductData(Search.Text);
            //using (SqlConnection conn = sql)
            //{
            //    string q = String.Format(stoklistessorgu+ @" AND PRONAME like '%{1}%'", marka.SelectedValue, Search.Text);
            //    SqlCommand cmd = new SqlCommand(q, conn);
            //    conn.Open();
            //    SqlDataReader reader = cmd.ExecuteReader();
            //    ProductRepeater.DataSource = reader;
            //    ProductRepeater.DataBind();
            //    conn.Close();

            //    //SqlDataAdapter da = new SqlDataAdapter(q, conn);
            //    //DataTable dt = new DataTable();
            //    //da.Fill(dt);
            //    //Urunler.DataSource = dt;
            //    //Urunler.DataBind();
            //}
        }
        //protected void Urunler_RowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    e.Row.Cells[1].Visible = false; // 1. sütun
        //    e.Row.Cells[2].Visible = false; // 2. sütun
        //}

        //protected void Urunler_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "Select")
        //    {
        //        // Seçilen satırın indeksini al
        //        int rowIndex = Convert.ToInt32(e.CommandArgument);

        //        // Satır verilerini almak için GridView'den ilgili satırı çek
        //        GridViewRow row = Urunler.Rows[rowIndex];

        //        // Hücre verilerini al
        //        string proid = row.Cells[1].Text; // PROID hücresi
        //        string proval = row.Cells[2].Text; // PROVAL hücresi
        //        string proname = row.Cells[3].Text; // PRONAME hücresi

        //        // İstediğiniz işlemleri burada yapabilirsiniz
        //        // Örneğin:
        //        // Verileri URL parametreleri olarak gönder
        //        string url = $"Guncelleme.aspx?proid={Server.UrlEncode(proid)}&proval={Server.UrlEncode(proval)}&proname={Server.UrlEncode(proname)}";
        //        Response.Redirect(url);
        //    }
        //}

    }
}
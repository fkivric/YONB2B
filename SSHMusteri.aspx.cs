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
using System.Configuration;
using System.Drawing.Printing;
using System.Data;
using System.Collections;

namespace YONB2B
{
    public partial class SSHMusteri : System.Web.UI.Page
    {
        public static string ConnectionString = "Server=192.168.4.24;Database=VDB_YON01;User Id=sa;Password=MagicUser2023!;";
        public static string ConnectionString2 = "Server=192.168.4.24;Database=MDE_GENEL;User Id=sa;Password=MagicUser2023!;";
        SqlConnection sql = new SqlConnection(ConnectionString);
        SqlConnection sql2 = new SqlConnection(ConnectionString2);
        public static string magaza = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var loginRes = (List<LoginObj>)Session["Login"];
                if (loginRes != null)
                {
                    bool tekmagaza = true;
                    if (loginRes[0].DIVVAL.Length >= 3)
                    {
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
                        tekmagaza = false;
                    }
                    else
                    {
                        magaza = loginRes[0].DIVVAL.ToString();
                    }
                    DataTable dt = new DataTable();
                    if (tekmagaza)
                    {
                        string q = String.Format(@"                        
                        select DIVVAL as DEPOVAL,DIVNAME as DEPONAME from DIVISON where DIVSTS = 1 and DIVVAL = '{0}'", magaza);
                        dt = DbQuery.Query(q, ConnectionString);
                    }
                    else
                    {
                        string q = String.Format(@"
                        select '' as DEPOVAL, 'SEÇİNİZ...' as DEPONAME
                        union
                        select DIVVAL,DIVNAME from DIVISON where DIVSTS = 1 and DIVVAL in ({0})", magaza);
                        dt = DbQuery.Query(q, ConnectionString);
                    }
                    Depo.DataSource = dt;
                    Depo.DataValueField = "DEPOVAL";
                    Depo.DataTextField = "DEPONAME";
                    Depo.DataBind();

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


        protected void Listele_Click(object sender, EventArgs e)
        {
            bool SearchValue = false;
            if (Depo.SelectedValue != "")
            {
                if (Search.Text != "")
                {
                    SearchValue = true;
                }
                if (TC.Text != "")
                {
                    SearchValue = true;
                }
                if(SearchValue)
                {
                    BindProductData(Search.Text, TC.Text);
                }
            }
            else
            {
                Liste.Visible=false;
            }
            
        }
        private const int PageSize = 20;
        private void BindProductData(string curname = "", string TC = "", int pageIndex = 1)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                // İlk olarak toplam kayıt sayısını almak için sorgu
                string countQuery = @"
                SELECT COUNT(*) FROM (
                select CURID,CURVAL,CURNAME,CUSIDTCNO from CURRENTS
                left outer join CURRENTSCHILD on CURCHID = CURID
                left outer join CUSIDENTITY ON CUSIDCURID = CURID
                left outer join DIVISON ON DIVVAL = CURDIVISON
                where CURSTS = 1 and CURCUSTOMER = 1
                and CUSIDTCNO = @marka and CURNAME like @search
                and exists (Select * from SALES s
			                where not exists (Select * from SALES i where i.SALCANSALID = s.SALID)
			                and s.SALID > 0
			                and s.SALCURID = CURID)
                ) AS TotalCount";

                SqlCommand countCmd = new SqlCommand(countQuery, conn);
                countCmd.Parameters.AddWithValue("@marka", TC);
                countCmd.Parameters.AddWithValue("@search", "%" + curname + "%");

                int totalCount = (int)countCmd.ExecuteScalar();
                int totalPages = (int)Math.Ceiling((double)totalCount / PageSize);
                sayfatoplam.Text = "Toplam Sayfa :" + totalPages.ToString();
                // İkinci sorgu: sayfalanmış verileri almak için
                string query = @"
                SELECT * FROM (
                    SELECT 
                        CURID,CURVAL,CURNAME,CUSIDTCNO,
                        ROW_NUMBER() OVER (ORDER BY CURID) AS RowNum
                    from CURRENTS
                left outer join CURRENTSCHILD on CURCHID = CURID
                left outer join CUSIDENTITY ON CUSIDCURID = CURID
                left outer join DIVISON ON DIVVAL = CURDIVISON
                where CURSTS = 1 and CURCUSTOMER = 1
                and CUSIDTCNO = @marka and CURNAME like @search
                and exists (Select * from SALES s
			                where not exists (Select * from SALES i where i.SALCANSALID = s.SALID)
			                and s.SALID > 0
			                and s.SALCURID = CURID)
                ) AS Result 
                WHERE RowNum BETWEEN @start AND @end
                order by CURNAME asc";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@marka", TC);
                cmd.Parameters.AddWithValue("@search", "%" + curname + "%");
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
        public static string cuscurid = "";
        protected void btnOpenModal_Click(object sender, EventArgs e)
        {
            // Tıklanan butona ait CDRID değerini al
            Button btn = (Button)sender;
            string cdrID = btn.CommandArgument;
            cuscurid = cdrID;
            BindProductData2(cdrID);
            ClientScript.RegisterStartupScript(this.GetType(), "OpenModal", "openModal();", true);
        }

        private void BindProductData2(string curid = "")
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                // İkinci sorgu: sayfalanmış verileri almak için
                string query = @"
                select PROID,PROVAL,PRONAME
                from SALES S
                left outer join INVOICE on SALID = INVSALID
                left outer join INVOICECHILD on INVCHINVID = INVID
                left outer join INVOICECHILDPROBH on INVCHPBHID = INVCHID
                left outer join PRODUCTSBEHAVE on PROBHID = INVCHPBHPROBHID
                left outer join ORDERS on ORDSALID = SALID
                left outer join ORDERSCHILD on ORDCHORDID = ORDID
                left outer join PRODUCTS on PROID = ORDCHPROID or PROID = PROBHPROID
                where SALCURID = @marka 
                and PROPROUID in (36,103)
                and SALAMOUNT > 0
                and SALID > 0
                and not exists (select * from SALES i where S.SALID = i.SALCANSALID)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@marka", curid);

                SqlDataReader reader = cmd.ExecuteReader();
                Urunler.DataSource = reader;
                Urunler.DataBind();
            }
        }
        protected void Kapat_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "closeModal", "closeModal();", true);
        }

        protected void SALES_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[2].Visible = false;
        }

        protected void SALES_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "closeModal2", "closeModal2();", true);
            if (e.CommandName == "Select")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = SALES.Rows[rowIndex];
                string selectedId = row.Cells[0].Text; // ID sütunu
                hdnSelectedId.Value = selectedId;
                BindProductData2(selectedId);

                // Popup'ı kapatma ve seçilen değeri ana sayfaya gönderme
                ClientScript.RegisterStartupScript(this.GetType(), "OpenModal2", "openModal2();", true);
            }
        }

        protected void btnOpenModal_Click1(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string cdrID = btn.CommandArgument;
            string url = $"SSHOlusTurma.aspx?proid={Server.UrlEncode(cdrID)}&cuscurid={Server.UrlEncode(cuscurid)}";
            Response.Redirect(url);
        }
    }
}
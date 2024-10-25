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
                    }
                    else
                    {
                        magaza = loginRes[0].DIVVAL.ToString();
                    }
                    string q = String.Format(@"
                    select '' as DEPOVAL, 'SEÇİNİZ...' as DEPONAME
                    union
                    select DIVVAL,DIVNAME from DIVISON where DIVSTS = 1 and DIVVAL in ({0})", magaza);
                    var dt = DbQuery.Query(q, ConnectionString);
                    Depo.DataSource = dt;
                    Depo.DataValueField = "DEPOVAL";
                    Depo.DataTextField = "DEPONAME";
                    Depo.DataBind();
                    if (tekmagaza)
                        Depo.SelectedValue = magaza;
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

        protected void magaza_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Depo.SelectedValue != "")
            {
                string q = String.Format(@"select ORDSALID,CDRCURID,CURVAL,CURNAME,
                ORDDATE,CDRDATE2,
                sum(ORDCHBALANCEQUAN) as ORDCHBALANCEQUAN,Convert(numeric(18,2),sum(ORDCHBALANCEQUAN*ORDCHPRICE)) as PRLPRICE,TESLIM.DIVNAME  as TESLIMDIVNAME,SATIS.DIVNAME as SATISDIVNAME,
                CURCHCOUNTY,
                DSHIPNAME,DCRWNAME
                from CUSDELIVER
                left outer join CUSDELCREW on CDCWCDRID = CDRID
                left outer join DEFCREW on DCRWVAL = CDCWDCRWVAL
                left outer join CURRENTS on CURID = CDRCURID
                left outer join CURRENTSCHILD on CURCHID = CURID
                left outer join DEFSTORAGE on DSTORID = CDRSTORID
                left outer join ORDERSCHILD on ORDCHID = CDRORDCHID
                left outer join ORDERS on ORDID = ORDCHORDID
                left outer join PRODUCTS on PROID = ORDCHPROID
                LEFT OUTER JOIN DEFSHIPMENT WITH (NOLOCK) ON DSHIPVAL = CDRSHIPVAL
                LEFT OUTER JOIN DIVISON TESLIM WITH (NOLOCK) ON TESLIM.DIVVAL = DSTORVAL AND ORDCHCOMPANY = TESLIM.DIVCOMPANY
                LEFT OUTER JOIN DIVISON SATIS WITH (NOLOCK) ON SATIS.DIVVAL = CDRSALEDIV AND ORDCHCOMPANY = SATIS.DIVCOMPANY
                WHERE CDRSTS IN (1) 
                AND ORDERSCHILD.ORDCHBEYOND = 0 
                AND CUSDELIVER.CDRKIND <> 1 
                AND CUSDELIVER.CDRRNDSTS = 1 
                AND CUSDELIVER.CDRSALID > 0 
                AND ORDCHBALANCEQUAN > 0
                AND PROPROUID in (103,36)
                AND (CDRSALEDIV in ({0}) OR DSTORDIVISON in ({0}))
                group by ORDSALID,CDRCURID,CURVAL,CURNAME,ORDDATE,TESLIM.DIVNAME,SATIS.DIVNAME,DSHIPNAME,CURCHCOUNTY,CDRDATE2,DCRWNAME", magaza);
                BindProductData();
            }
            else
            {

            }
        }

        private const int PageSize = 20;
        private void BindProductData(string search = "", int pageIndex = 1)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                // İlk olarak toplam kayıt sayısını almak için sorgu
                string countQuery = String.Format(@"
                SELECT COUNT(*) FROM (
                    select top 100 ORDSALID,CDRCURID,CURVAL,CURNAME,
                    ORDDATE,
                    TESLIMDIVNAME,SATISDIVNAME,
                    sum(bekleyen) as adet,
                    DSHIPNAME,DCRWNAME
                    from (
                    select distinct CDRID,ORDSALID,CDRCURID,CURVAL,CURNAME,
                    ORDDATE,
                    TESLIM.DIVNAME  as TESLIMDIVNAME,SATIS.DIVNAME as SATISDIVNAME,
                    convert(int,case when ORDCHBALANCEQUAN > 0 then ORDCHBALANCEQUAN else 0 end) as bekleyen,
                    DSHIPNAME,DCRWNAME
                    from CURRENTS with (nolock)
                    left outer join SALES with (nolock) on SALCURID = CURID
                    left outer join CUSDELIVER with (nolock) on CDRCURID = CURID
                    left outer join CUSDELCREW with (nolock) on CDCWCDRID = CDRID
                    left outer join DEFCREW with (nolock) on DCRWVAL = CDCWDCRWVAL
                    left outer join DEFSTORAGE with (nolock) on DSTORID = CDRSTORID
                    left outer join ORDERSCHILD with (nolock) on ORDCHID = CDRORDCHID
                    left outer join ORDERS with (nolock) on ORDID = ORDCHORDID
                    left outer join PRODUCTS with (nolock) on PROID = ORDCHPROID
                    LEFT OUTER JOIN DEFSHIPMENT WITH (NOLOCK) ON DSHIPVAL = CDRSHIPVAL
                    LEFT OUTER JOIN DIVISON TESLIM WITH (NOLOCK) ON TESLIM.DIVVAL = DSTORVAL AND ORDCHCOMPANY = TESLIM.DIVCOMPANY
                    LEFT OUTER JOIN DIVISON SATIS WITH (NOLOCK) ON SATIS.DIVVAL = CDRSALEDIV AND ORDCHCOMPANY = SATIS.DIVCOMPANY
                    where 1=1
                    AND SALDATE >= DATEADD(YEAR,-2,getdate())
                    AND PROPROUID in (103,36)
                    AND (CDRSALEDIV = '{0}' OR DSTORDIVISON = '{0}')
                    AND CURNAME LIKE @search
                    AND CDRSTS != 3
                    ) net
                    group by ORDSALID,CDRCURID,CURVAL,CURNAME,
                    ORDDATE,
                    TESLIMDIVNAME,SATISDIVNAME,DSHIPNAME,DCRWNAME
                ) AS TotalCount 
                option(fast 100)", Depo.SelectedValue);

                SqlCommand countCmd = new SqlCommand(countQuery, conn);
                countCmd.Parameters.AddWithValue("@search", "%" + search + "%");

                int totalCount = 100;// (int)countCmd.ExecuteScalar();
                int totalPages = (int)Math.Ceiling((double)totalCount / PageSize);
                sayfatoplam.Text = totalPages.ToString() + " / " + pageIndex ;

                // İkinci sorgu: sayfalanmış verileri almak için
                string query = String.Format(@"
                SELECT TOP 100 * FROM (
                    select top 100 ORDSALID,CDRCURID,CURVAL,CURNAME,
                    ORDDATE,
                    TESLIMDIVNAME,SATISDIVNAME,
                    sum(bekleyen) as adet,
                    DSHIPNAME,DCRWNAME,
                    ROW_NUMBER() OVER (ORDER BY ORDSALID) AS RowNum
                    from (
                    select distinct CDRID,ORDSALID,CDRCURID,CURVAL,CURNAME,
                    ORDDATE,
                    TESLIM.DIVNAME  as TESLIMDIVNAME,SATIS.DIVNAME as SATISDIVNAME,
                    convert(int,case when ORDCHBALANCEQUAN > 0 then ORDCHBALANCEQUAN else 0 end) as bekleyen,
                    DSHIPNAME,DCRWNAME
                    from CURRENTS with (nolock)
                    left outer join SALES with (nolock) on SALCURID = CURID
                    left outer join CUSDELIVER with (nolock) on CDRCURID = CURID
                    left outer join CUSDELCREW with (nolock) on CDCWCDRID = CDRID
                    left outer join DEFCREW with (nolock) on DCRWVAL = CDCWDCRWVAL
                    left outer join DEFSTORAGE with (nolock) on DSTORID = CDRSTORID
                    left outer join ORDERSCHILD with (nolock) on ORDCHID = CDRORDCHID
                    left outer join ORDERS with (nolock) on ORDID = ORDCHORDID
                    left outer join PRODUCTS with (nolock) on PROID = ORDCHPROID
                    LEFT OUTER JOIN DEFSHIPMENT WITH (NOLOCK) ON DSHIPVAL = CDRSHIPVAL
                    LEFT OUTER JOIN DIVISON TESLIM WITH (NOLOCK) ON TESLIM.DIVVAL = DSTORVAL AND ORDCHCOMPANY = TESLIM.DIVCOMPANY
                    LEFT OUTER JOIN DIVISON SATIS WITH (NOLOCK) ON SATIS.DIVVAL = CDRSALEDIV AND ORDCHCOMPANY = SATIS.DIVCOMPANY
                    where 1=1
                    AND SALDATE >= DATEADD(YEAR,-2,getdate())
                    AND PROPROUID in (103,36)
                    AND (CDRSALEDIV = '{0}' OR DSTORDIVISON = '{0}')
                    AND CURNAME Like @search
                    AND CDRSTS != 3
                    ) net
                    group by ORDSALID,CDRCURID,CURVAL,CURNAME,
                    ORDDATE,
                    TESLIMDIVNAME,SATISDIVNAME,DSHIPNAME,DCRWNAME
                ) AS Result 
                WHERE RowNum BETWEEN @start AND @end
                option(fast 100)", Depo.SelectedValue);

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");
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
                paginationHtml += $"<a href='javascript:void(0);' onclick='__doPostBack(\"{searchInput.UniqueID}\", \"{currentPage - 1}\");'>Önceki</a>";
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
                    paginationHtml += $"<a href='javascript:void(0);' onclick='__doPostBack(\"{searchInput.UniqueID}\", \"{i}\");'>{i}</a>";
                }
            }

            // Sonraki sayfa bağlantısı
            if (currentPage < totalPages)
            {
                paginationHtml += $"<a href='javascript:void(0);' onclick='__doPostBack(\"{searchInput.UniqueID}\", \"{currentPage + 1}\");'>Sonraki</a>";
            }

            paginationHtml += "</div>";
            return paginationHtml;
        }

        //private string GeneratePagination(int totalPages, int currentPage)
        //{
        //    string paginationHtml = "";
        //    for (int i = 1; i <= totalPages; i++)
        //    {
        //        if (i == currentPage)
        //        {
        //            paginationHtml += $"<a class='active'>{i}</a>";
        //        }
        //        else
        //        {
        //            paginationHtml += $"<a href='javascript:void(0);' onclick='__doPostBack(\"{searchInput.UniqueID}\", \"{i}\");'>{i}</a>";
        //        }
        //    }
        //    return paginationHtml;
        //}

        protected void searchInput_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = searchInput.Value;
            BindProductData(searchTerm);
        }
        protected void Search_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Secim_Click(object sender, EventArgs e)
        {

        }
    }
}
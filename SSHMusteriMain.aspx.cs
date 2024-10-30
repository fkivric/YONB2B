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
using System.Data;

namespace YONB2B
{
    public partial class SSHMusteriMain : System.Web.UI.Page
    {
        public static string ConnectionString = "Server=192.168.4.24;Database=VDB_YON01;User Id=sa;Password=MagicUser2023!;";
        public static string ConnectionString2 = "Server=192.168.4.24;Database=MDE_GENEL;User Id=sa;Password=MagicUser2023!;";
        SqlConnection sql = new SqlConnection(ConnectionString);
        SqlConnection sql2 = new SqlConnection(ConnectionString2);
        string SALID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var loginRes = (List<LoginObj>)Session["Login"];
                if (loginRes != null)
                {
                    SALID = Request.QueryString["SALID"];
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
            }
        }
        private void LoadData(string salID)
        {
            string q = String.Format(@"declare 
            @SALID		bigint,
            @salkind	char(1)

            select @SALID = SALID,@salkind = SALSHIPKIND from SALES S
            where SALCURID = {0}} and SALAMOUNT > 0
            and SALID > 0
            and not exists (select * from SALES i where S.SALID = i.SALCANSALID)           

            if @salkind = 'S'
            begin
	            select PROID,PROVAL,PRONAME from ORDERS
	            left outer join ORDERSCHILD on ORDCHORDID = ORDID
	            left outer join PRODUCTS on PROID = ORDCHPROID
	            Where ORDSALID = @SALID
            end
            else
            begin
	            --select PROID, PROVAL,PRONAME 
	            select *
	            from INVOICE
	            left outer join INVOICECHILD on INVCHINVID = INVID
	            left outer join INVOICECHILDPROBH on INVCHPBHID = INVCHID
	            left outer join PRODUCTSBEHAVE on PROBHID = INVCHPBHPROBHID
	            left outer join PRODUCTS on PROID = PROBHPROID
	            where INVSALID = @SALID
            end");
        }
        protected void btnOpenIframe_Click(object sender, EventArgs e)
        {// Tıklanan butona ait CDRID değerini al
            Button btn = (Button)sender;
            string cdrID = btn.CommandArgument;

            // CDRID değerini Session'da sakla
            Session["SelectedCDRID"] = cdrID;

            // Iframe'in src değerini güncellemek için JavaScript kodu çalıştır
            string script = $"document.getElementById('iframeID').src = 'SSHUrunislemleri.aspx?CDRID={cdrID}';";
            ScriptManager.RegisterStartupScript(this, GetType(), "updateIframe", script, true);
        }
    }
}
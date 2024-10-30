using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YONB2B
{
    public partial class SSHUrunislemleri : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string cdrID = Request.QueryString["CDRID"];

                if (string.IsNullOrEmpty(cdrID))
                {
                    // Eğer QueryString boşsa, Session'dan al
                    cdrID = Session["SelectedCDRID"] as string;
                }

                if (!string.IsNullOrEmpty(cdrID))
                {
                    // CDRID'ye göre verileri yükle
                    LoadData(cdrID);
                }
            }

        }
        private void LoadData(string cdrID)
        {
            // CDRID'ye göre gerekli verileri getir ve göster
            lblResult.InnerText = $"Seçilen Ürün No: {cdrID}";
        }
    }
}
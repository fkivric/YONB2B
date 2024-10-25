using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YONB2B
{
    public partial class Eror : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                // Hata detaylarını göster
                errorDetails.InnerText = exception.Message + "\n" + exception.StackTrace;
            }
        }
    }
}
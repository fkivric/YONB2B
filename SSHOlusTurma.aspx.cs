using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YONB2B.Class;
using static YONB2B.Class.Siniflar;

namespace YONB2B
{
    public partial class SSHOlusTurma : System.Web.UI.Page
    {
        public static string ConnectionString = "Server=192.168.4.24;Database=VDB_YON01;User Id=sa;Password=MagicUser2023!;";
        public static string ConnectionString2 = "Server=192.168.4.24;Database=MDE_GENEL;User Id=sa;Password=MagicUser2023!;";
        SqlConnection sql = new SqlConnection(ConnectionString);
        SqlConnection sql2 = new SqlConnection(ConnectionString2);
        static string ftpUrl = "ftp://192.168.4.21/SSH/";
        static string ftpUsername = "admin";
        static string ftpPassword = "Madam1367";
        public static string PROID = "";
        public static string PROVAL = "";
        public static string PRONAME = "";
        static string CURID = "";
        static string CUSCURID = "";
        DataTable dosyalar = new DataTable();
        static List<string> files = new List<string>();
        static List<string> newFiles = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var loginRes = (List<LoginObj>)Session["Login"];
                if (loginRes != null)
                {
                    PROID = Request.QueryString["proid"];
                    string q = String.Format(@"select PROVAL,PRONAME from PRODUCTS where PROID = {0}", PROID);
                    var dt = DbQuery.Query(q, ConnectionString);
                    PROVAL = dt.Rows[0]["PROVAL"].ToString();//Request.QueryString["proval"];
                    PRONAME = dt.Rows[0]["PRONAME"].ToString();//Request.QueryString["proname"];
                    CUSCURID = Request.QueryString["cuscurid"];
                    Stokkodu.InnerText = PROVAL + " :";
                    Stokadi.InnerText = PRONAME;
                    sorgu1();
                    customerDATE.Value = DateTime.Now.ToString("yyyy-MM-dd");
                    tedarikci(PROID);
                    BindGrid();
                    if (CUSCURID.Length > 2)
                    {
                        var Dp = DbQuery.Query($"select CURVAL,CURNAME from CURRENTS where CURID = '{CUSCURID}'", ConnectionString);
                        Musteri.Text = Dp.Rows[0]["CURVAL"].ToString() + Environment.NewLine + Dp.Rows[0]["CURNAME"].ToString();
                    }
                    else
                    {
                        var Dp = DbQuery.Query($"select DIVVAL,DIVNAME from DIVISON where DIVVAL = '{CUSCURID}'",ConnectionString);
                        Musteri.Text = Dp.Rows[0]["DIVVAL"].ToString() + Environment.NewLine + Dp.Rows[0]["DIVNAME"].ToString();
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
        void tedarikci(string _proid)
        {
            string q = @"select CURID,CURNAME from CURRENTS
            where CURSUPPLIER = 1 and CURSTS = 1
            order by 2";
            var dt = DbQuery.Query(q, ConnectionString);
            Firma.DataSource = dt;
            Firma.DataValueField = "CURID";
            Firma.DataTextField = "CURNAME";
            Firma.DataBind();

            string w = String.Format(@"select CURID from CURRENTS
            left outer join PROSUPPLIER on PROSUPCURID = CURID
            where PROSUPPROID = {0}", _proid);
            var firmaID = DbQuery.GetValue(w);
            Firma.SelectedValue = firmaID;
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

        public bool FTPCechkFolder(string folderPath)
        {
            string server = ftpUrl + CURID;//"ftp.example.com"; // FTP sunucu adresi
            string username = ftpUsername; //"Yon"; // FTP kullanıcı adı
            string password = ftpPassword;//"Yonavm123"; // FTP şifre
            try
            {
                // FTP sunucusuna bağlan
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create($"{server}/{folderPath}");
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpRequest.Credentials = new NetworkCredential(username, password);

                using (FtpWebResponse ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
                {
                    using (Stream responseStream = ftpResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            // Sunucuda belirtilen klasörün dosya listesini al
                            string fileList = reader.ReadToEnd();

                            // Klasörün var olup olmadığını kontrol et
                            bool folderExists = !string.IsNullOrEmpty(fileList);

                            // Sonucu kullanıcıya göster
                            if (folderExists)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (WebException)
            {
                return false;
            }
        }
        public void CreateFolderFTP(string FileNAme)
        {
            try
            {
                string path = FileNAme;
                string xmlPath = ftpUrl + "/" + path;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(xmlPath);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
            }
        }
        private List<string> LoadFtpFiles(string File)
        {
            List<string> resimler = new List<string>();
            if (!FTPCechkFolder(ftpUrl + File))
            {
                CreateFolderFTP(File);
            }
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl + File);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    while (!reader.EndOfStream)
                    {
                        string fileName = reader.ReadLine();
                        string onlyname = fileName.Replace(File + "/", "");
                        if (onlyname.EndsWith("jpg") || onlyname.EndsWith("jpeg") || onlyname.EndsWith("png"))
                            resimler.Add(onlyname);
                    }
                }
                return resimler;

            }
            catch (Exception)
            {
                return resimler;
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string strFileName;
            string strFilePath;
            string strFolder;
            string ftpFolder;
            string ftpFilePath;
            strFolder = Server.MapPath("./UploadedFiles/Resimler/" + PROID + "/");
            // Get the name of the file that is posted.
            strFileName = oFile.PostedFile.FileName;
            //strFileName = Path.GetFileName(strFileName);
            if (oFile.Value != "")
            {
                if (!Directory.Exists(Server.MapPath("./UploadedFiles/Resimler/" + PROID)))
                {
                    Directory.CreateDirectory(Server.MapPath("./UploadedFiles/Resimler/" + PROID));
                }
                // Create the directory if it does not exist.
                if (!Directory.Exists(strFolder))
                {
                    Directory.CreateDirectory(strFolder);
                }
                // Save the uploaded file to the server.
                strFilePath = strFolder + strFileName;
                if (File.Exists(strFilePath))
                {
                    lblUploadResult.Text = strFileName + " Bu Dosya Daha Önce Yüklenmiş.....!";
                }
                else
                {
                    oFile.PostedFile.SaveAs(strFilePath);
                }

                try
                {
                    ftpFolder = Server.MapPath("./UploadedFiles/Resimler/" + PROID + "/FTPGiden/");
                    ftpFilePath = ftpFolder + strFileName;
                    if (!Directory.Exists(ftpFolder))
                    {
                        Directory.CreateDirectory(ftpFolder);
                    }
                    if (!File.Exists(ftpFilePath))
                        oFile.PostedFile.SaveAs(ftpFilePath);

                    if (!FTPCechkFolder(PROID))
                    {
                        CreateFolderFTP(PROID);
                    }
                    string sira = "";
                    string qq = String.Format(@"select count(*) as adet from W.[Yonavm_Web_Siparis].dbo.EntegreFStokResimleri where STOKID = {0} ", PROID);
                    dosyalar = DbQuery.Query(qq, ConnectionString2);
                    if (dosyalar == null)
                    {
                        sira = "0";
                    }
                    else
                    {
                        sira = (int.Parse(dosyalar.Rows[0]["adet"].ToString()) + 1).ToString();
                    }
                    string newFileName = PROVAL + "_" + sira + Path.GetExtension(strFileName); // Örneğin "newFileName.ext"
                    string newFilePath = Path.Combine(ftpFolder, newFileName);

                    // Rename the file
                    File.Move(ftpFilePath, newFilePath);
                    // Upload the file to FTP server
                    string ftpFullUrl = ftpUrl + "/" + CURID + "/" + PROID + "/" + newFileName;
                    FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ftpFullUrl);
                    ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                    ftpRequest.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                    byte[] fileContents = File.ReadAllBytes(newFilePath);
                    ftpRequest.ContentLength = fileContents.Length;

                    using (Stream requestStream = ftpRequest.GetRequestStream())
                    {
                        requestStream.Write(fileContents, 0, fileContents.Length);
                    }

                    using (FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse())
                    {
                        string q = String.Format(@"insert into W.[Yonavm_Web_Siparis].dbo.EntegreFStokResimleri values ({0},'{1}')", PROID, newFileName);
                        DbQuery.insertquery(q, ConnectionString2);
                        lblUploadResult.Text = strFileName + " Dosya Kaydedildi";
                        newFiles.Clear();
                        newFiles = LoadFtpFiles(CURID + "/" + PROID);
                    }
                    // Delete the local file after upload
                    File.Delete(newFilePath);
                }
                catch (Exception ex)
                {
                    lblUploadResult.Text = lblUploadResult.Text + "\r\n" + ex.Message;
                }
            }
            else
            {
                lblUploadResult.Text = "Yüklenecek dosyayı seçmek için 'Gözat'a tıklayın.";
            }
            // Display the result of the upload.
            //File.Delete(ftpFilePath);
            frmConfirmation.Visible = true;
            //FileLoad.Visible = true;
            //Dosyalar();

        }

        protected void Kaydet_Click(object sender, EventArgs e)
        {

        }

        protected void SSHTuru_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SSHTuru.SelectedValue != "")
            {
                BindGrid(SSHTuru.SelectedValue);
                ClientScript.RegisterStartupScript(this.GetType(), "OpenModal", "openModal();", true);
            }
        }

        private void BindGrid(string filter = "")
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string query = "select * from SSHFAULTS where 1=1";
                if (!string.IsNullOrEmpty(filter))
                {
                    query += " and (SFAULNOTES like @Filter or SFAULSOURCE like @Filter or SFAULNAME like @Filter)";
                }
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        cmd.Parameters.AddWithValue("@Filter", "%" + filter + "%");
                    }
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        SSHFAULTS.DataSource = dt;
                        SSHFAULTS.DataBind();
                    }
                }
            }
        }

        protected void txtFilter_TextChanged(object sender, EventArgs e)
        {
            BindGrid(txtFilter.Text);
        }

        protected void SSHFAULTS_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = SSHFAULTS.Rows[rowIndex];
                string selectedId = row.Cells[0].Text; // ID sütunu
                // Popup'ı kapatma ve seçilen değeri ana sayfaya gönderme
                ClientScript.RegisterStartupScript(this.GetType(), "closeModal", "closeModal();", true);
            }

        }
        protected void SSHFAULTS_RowCreated(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[1].Visible = false;
        }

        protected void Kapat_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "closeModal", "closeModal();", true);
        }
    }
}
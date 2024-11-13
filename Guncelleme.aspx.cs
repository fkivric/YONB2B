using DG.MiniHTMLTextBox;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using YONB2B.Class;
using static YONB2B.Class.Siniflar;

namespace YONB2B
{
    public partial class Guncelleme : System.Web.UI.Page
    {
        public static string ConnectionString = "Server=192.168.4.24;Database=VDB_YON01;User Id=sa;Password=MagicUser2023!;";
        public static string ConnectionString2 = "Server=192.168.4.24;Database=MDE_GENEL;User Id=sa;Password=MagicUser2023!;";
        public static string ConnectionString3 = "Server=192.168.4.12;Database=Yonavm_Web_Siparis;User Id=sa;Password=ebrarsudenur;";
        SqlConnection sql = new SqlConnection(ConnectionString);
        SqlConnection sql2 = new SqlConnection(ConnectionString2);
        SqlConnection sql3 = new SqlConnection(ConnectionString3);
        static string ftpUrl = "ftp://192.168.4.21/WEBRESIMLER/";
        static string ftpUsername = "admin";
        static string ftpPassword = "Madam1367";
        public static string PROID = "";
        public static string PROVAL = "";
        public static string PRONAME = "";
        static string WPTREVAL = "";
        static string CURID = "";
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
                    //var ftp = (List<Ftp>)Session["FTP"];
                    //if (ftp != null)
                    //{
                    //    ftpUrl = ftp[0].VolFtpHost;
                    //    ftpUsername = ftp[0].VolFtpUser;
                    //    ftpPassword = ftp[0].VolFtpPass;
                    //}
                    // URL'den parametreleri al
                    PROID = Request.QueryString["proid"];
                    PROVAL = Request.QueryString["proval"];
                    PRONAME = Request.QueryString["proname"];
                    txtValue.Text = PRONAME;
                    txtNewFileName.Text = PRONAME;
                    YONKODU.Text = PROVAL;
                    CURID = DbQuery.GetValue($"select PROSUPCURID from PRODUCTS\r\nleft outer join WAVEPRODUCTS on WPROID = PROID and WPROUNIQ = 1\r\nleft outer join WAVEPROTREE on WPROUNIQ = WPTREUNIQ and WPROVAL = WPTREVAL\r\nleft outer join PROSUPPLIER on PROSUPPROID = PROID\r\nwhere PROID = '{PROID}'");
                    WPTREVAL = DbQuery.GetValue($"select WPROVAL from WAVEPRODUCTS where WPROID = {PROID} and WPROUNIQ = 1");
                    var classlar = DbQuery.Query($"SELECT isnull(DetayClass,'') as DetayClass,isnull(TeknikClass,'') as TeknikClass FROM VolantToTicimaxWebClass where Vol_WPTREVAL = '{WPTREVAL}'", ConnectionString3);
                    if (classlar != null)
                    {
                        txtDetayClass.Text = classlar.Rows[0]["DetayClass"].ToString();
                        txtTeknikClass.Text = classlar.Rows[0]["TeknikClass"].ToString();
                    }
                    var dtaciklama = DbQuery.Query($"select * from VolantToTicimaxStokAciklamaFirma where VOL_PROID = {PROID}", ConnectionString3);
                    if (dtaciklama != null)
                    {
                        HtmlToText htt = new HtmlToText();
                        var plainText = htt.ConvertHtml(dtaciklama.Rows[0]["TicimaxAciklama"].ToString());

                        Aciklama.Text = plainText; //dtaciklama.Rows[0]["TicimaxAciklama"].ToString();
                    }
                    files = LoadFtpFiles(CURID);
                    newFiles = LoadFtpFiles(CURID + "/" + PROID);
                    // İlk ListBox'ı güncelle
                    UpdateListBoxes();
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
                string script = "$(document).ready(function () { $('[id*=btnSubmit]').click(); });";
                ClientScript.RegisterStartupScript(this.GetType(), "load", script, true);
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
                        int index = fileName.IndexOf('/')+1;

                        string onlyname = fileName.Substring(index, fileName.Length-index);
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
        //internal void Dosyalar()
        //{
        //    string q = String.Format(@"select 
	       //      d.MB_CURID as CURID
	       //     ,CURNAME
	       //     ,FileTypeName
	       //     ,MB_FileName as FileName
        //    from MDE_GENEL.dbo.MB_BayiDosyaları d
        //    left outer join MDE_GENEL.dbo.MB_DosyaTipi t on t.id = MB_FileType
        //    left outer join VDB_YON01.dbo.CURRENTS c on c.CURID = d.MB_CURID
        //    where d.MB_CURID = {0}
        //    order by MB_FileType", PROID);
        //    var dt = DbQuery.Query(q, ConnectionString);
        //    if (dt != null)
        //    {
        //        Resimler.DataSource = dt;
        //        Resimler.DataBind();
        //    }
        //}
        public bool FTPCechkFolder(string folderPath)
        {
            string server = ftpUrl+CURID;//"ftp.example.com"; // FTP sunucu adresi
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
                    WebMsgBox.Show($"{strFileName} Bu Dosya Daha Önce Yüklenmiş.....!");
                    lblUploadResult.Text = " Yüklenmiş Resim Yüklenemez. Lütfen Kontrol Ediniz..!";
                }
                else
                {
                    oFile.PostedFile.SaveAs(strFilePath);


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
                            DbQuery.insertquery($"insert into EntegreFStokResimleri values ({PROID},'{newFileName}')", ConnectionString3);
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

                files = LoadFtpFiles(CURID);
                newFiles = LoadFtpFiles(CURID + "/" + PROID);
                // İlk ListBox'ı güncelle
                UpdateListBoxes();
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
        protected void btnUpload2_Click(object sender, EventArgs e)
        {
            string strFileName;
            string strFilePath;
            string strFolder;
            string ftpFolder;
            string ftpFilePath;
            strFolder = Server.MapPath("./UploadedFiles/Resimler/" + PROID + "/");
            // Get the name of the file that is posted.
            strFileName = oFile2.PostedFile.FileName;
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
                    lblUploadResult2.Text = strFileName + " Bu Dosya Daha Önce Yüklenmiş.....!";
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
                        lblUploadResult2.Text = strFileName + " Dosya Kaydedildi";
                        newFiles.Clear();
                        newFiles = LoadFtpFiles(CURID + "/" + PROID);
                        UpdateListBoxes();
                    }
                    // Delete the local file after upload
                    File.Delete(newFilePath);
                }
                catch (Exception ex)
                {
                    lblUploadResult2.Text = lblUploadResult2.Text + "\r\n" + ex.Message;
                }
            }
            else
            {
                lblUploadResult2.Text = "Yüklenecek dosyayı seçmek için 'Gözat'a tıklayın.";
            }
            // Display the result of the upload.
            //File.Delete(ftpFilePath);
            frmConfirmation2.Visible = true;
            //FileLoad.Visible = true;
            //Dosyalar();

        }
        protected void fileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelViewer.Visible = true;
            panel1.Visible = true;
            // Retrieve the URL from ViewState

            string Filename = fileList.SelectedValue;
            string uploadedFileUrl = ftpUrl + CURID +"/"+ Filename;
            try
            {
                if (Filename.EndsWith("pdf") == true || Filename.EndsWith("PDF") == true)
                {
                    WebClient ftpClient = new WebClient();
                    ftpClient.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                    byte[] imageByte = ftpClient.DownloadData(uploadedFileUrl);


                    var tempFileName = Path.GetTempFileName().Replace("tmp", "pdf");

                    System.IO.File.WriteAllBytes(tempFileName, imageByte);

                    string webFolderPath = Server.MapPath("~/TempImages/");
                    if (!Directory.Exists(webFolderPath))
                    {
                        Directory.CreateDirectory(webFolderPath);
                    }
                    string webFilePath = Path.Combine(webFolderPath, Path.GetFileName(tempFileName));
                    File.Copy(tempFileName, webFilePath, true);

                    string relativeFilePath = "~/TempImages/" + Path.GetFileName(tempFileName);

                    // Use iframe to display the PDF
                    string pdfIframe = $"<iframe src='{ResolveUrl(relativeFilePath)}' type='application/pdf' width='600' height='500'></iframe>";
                    iframe.Src = $"{ResolveUrl(relativeFilePath)}";
                    pdfViewerPlaceHolder.Controls.Clear();
                    pdfViewerPlaceHolder.Controls.Add(new Literal { Text = pdfIframe });
                    iframe.Visible = false;
                    imgViewer.Visible = false;
                    pdfViewerPlaceHolder.Visible = false;
                    ResimEsle.Visible = true;

                }
                else if (Filename.EndsWith("jpg") || Filename.EndsWith("jpeg") || Filename.EndsWith("png"))
                {
                    //System.Threading.Thread.Sleep(5000);
                    WebClient ftpClient = new WebClient();
                    ftpClient.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                    byte[] imageByte = ftpClient.DownloadData(uploadedFileUrl);


                    var tempFileName = Path.GetTempFileName();
                    System.IO.File.WriteAllBytes(tempFileName, imageByte);

                    // Save the file in a web accessible folder
                    string webFolderPath = Server.MapPath("~/TempImages/");
                    if (!Directory.Exists(webFolderPath))
                    {
                        Directory.CreateDirectory(webFolderPath);
                    }

                    string webFilePath = Path.Combine(webFolderPath, Path.GetFileName(tempFileName) + ".jpg");
                    File.Copy(tempFileName, webFilePath, true);

                    // Set the ImageUrl to the web accessible file path
                    imgViewer.ImageUrl = "~/TempImages/" + Path.GetFileName(webFilePath);
                    imgViewer.Visible = true;
                    ResimEsle.Visible = true;

                }
                else
                {
                    imgViewer.Visible = false;
                    pdfViewerPlaceHolder.Visible = false;
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda yükleme ekranını gizle
                ScriptManager.RegisterStartupScript(this, GetType(), "hideLoading", "hideLoading();", true);
                //FileLoad.Visible = true;
                //FileLoad.Text = "Error: " + ex.Message;
            }
        }
        protected void tamlandı_Click(object sender, EventArgs e)
        {
           
        }
        private void UpdateListBoxes()
        {
            fileList.DataSource = files;
            fileList.DataBind();

            // İkinci ListBox'ı güncelle
            newFileList.DataSource = newFiles;
            newFileList.DataBind();
        }
        protected void ResimEsle_Click(object sender, EventArgs e)
        {
            // Seçili dosyayı al
            string selectedFile = fileList.SelectedValue;

            // Eğer bir dosya seçildiyse
            if (!string.IsNullOrEmpty(selectedFile))
            {
                // Seçili dosyayı newFiles listesine ekle
                newFiles.Add(selectedFile);

                // files listesinden kaldır
                files.Remove(selectedFile);

                // ListBox'ları güncelle
                UpdateListBoxes();
            }

        }
        protected void ResimCıkar_Click(object sender, EventArgs e)
        {
            // Seçili dosyayı al
            string selectedFile = newFileList.SelectedValue;
            if (CheckFileExists(ftpUrl + CURID + "/" + PROID + "/", ftpUsername, ftpPassword, selectedFile))
            {
                try
                {
                    string sira = "0";

                    // Dosyanın uzantısını al
                    string fileExtension = Path.GetExtension(selectedFile).ToLower();

                    sira = GetFileCountOnFtp(ftpUrl + CURID, ftpUsername, ftpPassword, "").ToString();
                    // Taşınacak dosyanın yeni adı ve yolu
                    string newFileName = sira + fileExtension; // Yeni dosya adı
                    string destinationPath = CURID + "/" + newFileName; // Hedef dizin

                    // Dosyayı taşıma işlemi
                    if (MoveFileToFtp(ftpUrl, ftpUsername, ftpPassword, selectedFile, destinationPath))
                    {
                        files = LoadFtpFiles(CURID);
                        newFiles = LoadFtpFiles(CURID + "/" + PROID);
                        UpdateListBoxes();
                        // Başarılı mesajı
                        //Response.Write("<script>alert('Dosya başarıyla taşındı.');</script>");
                    }
                    else
                    {
                        // Hata mesajı
                        //Response.Write("<script>alert('Dosya taşınırken hata oluştu.');</script>");
                    }

                    // Başarılı öğeleri listeden kaldır
                }
                catch (Exception)
                {
                }
            }
            else
            {
                // Eğer bir dosya seçildiyse
                if (!string.IsNullOrEmpty(selectedFile))
                {
                    // Seçili dosyayı newFiles listesine ekle
                    newFiles.Remove(selectedFile);

                    // files listesinden kaldır
                    files.Add(selectedFile);

                    // ListBox'ları güncelle
                    UpdateListBoxes();
                }
            }
        }
        private bool MoveFileToFtp(string ftpServer, string ftpUser, string ftpPass, string sourceFile, string destinationPath)
        {
            try
            {
                // Dosyayı indir
                WebClient webClient = new WebClient();
                webClient.Credentials = new NetworkCredential(ftpUser, ftpPass);
                byte[] fileData = webClient.DownloadData(ftpServer +CURID + "/" + sourceFile);

                // Yeni dosyayı yükle
                webClient.UploadData(ftpServer + destinationPath, WebRequestMethods.Ftp.UploadFile, fileData);

                // Eski dosyayı sil
                FtpWebRequest deleteRequest = (FtpWebRequest)WebRequest.Create(ftpServer + CURID +  "/" + sourceFile);
                deleteRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                deleteRequest.Credentials = new NetworkCredential(ftpUser, ftpPass);
                using (FtpWebResponse response = (FtpWebResponse)deleteRequest.GetResponse()) { }

                return true;
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama veya hata mesajı işlemleri
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private int GetFileCountOnFtp(string ftpServer, string ftpUser, string ftpPass, string directoryPath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServer + directoryPath);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(ftpUser, ftpPass);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    int fileCount = 0;

                    while (reader.ReadLine() != null)
                    {
                        fileCount++;
                    }

                    return fileCount;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1; // Hata durumunda -1 döndür
            }
        }
        public static bool CheckFileExists(string ftpUrl, string ftpUsername, string ftpPassword, string fileName)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl + fileName);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(ftpUsername, ftpPassword);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {                    
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false; // Hata durumunda -1 döndür
            }
        }
        protected void ResimBagla_Click(object sender, EventArgs e)
        {
            try
            {
                // Geçici bir liste oluşturun
                List<ListItem> itemsToRemove = new List<ListItem>();
                foreach (ListItem item in newFileList.Items)
                {
                    string sira = "0";
                    // Seçili dosyayı al
                    string selectedFileName = item.ToString();

                    if (!CheckFileExists(ftpUrl + CURID + "/", ftpUsername, ftpPassword, selectedFileName))
                    {
                        // Dosyanın uzantısını al
                        string fileExtension = Path.GetExtension(selectedFileName).ToLower();

                        if (GetFileCountOnFtp(ftpUrl + CURID + "/" + PROID, ftpUsername, ftpPassword, "") != -1)
                        {
                            sira = GetFileCountOnFtp(ftpUrl + CURID + "/" + PROID, ftpUsername, ftpPassword, "").ToString();
                        }
                        else
                        {
                            sira = "0";
                        }
                        // Taşınacak dosyanın yeni adı ve yolu
                        string newFileName = PROVAL + "_" + sira + fileExtension; // Yeni dosya adı
                        string destinationPath = CURID + "/" + PROID + "/" + newFileName; // Hedef dizin

                        // Dosyayı taşıma işlemi
                        if (MoveFileToFtp(ftpUrl, ftpUsername, ftpPassword, selectedFileName, destinationPath))
                        {
                            // Başarılı mesajı
                            //Response.Write("<script>alert('Dosya başarıyla taşındı.');</script>");
                            item.Text = "Başarılı";
                            itemsToRemove.Add(item);
                        }
                        else
                        {
                            // Hata mesajı
                            //Response.Write("<script>alert('Dosya taşınırken hata oluştu.');</script>");
                        }
                    }
                }
                // Başarılı öğeleri listeden kaldır
                foreach (var listItem in itemsToRemove)
                {
                    newFileList.Items.Remove(listItem);
                }
                UpdateListBoxes();
            }
            catch (Exception)
            {
            }
        }
        protected void YeniResim_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(txtNewFileName.Text) || txtNewFileName.Text != "")
            //{
            //    uploadarea.Visible = true;
            //    uploadarea2.Visible = true;
            //    Kaydet.Visible = true;
            //    panelViewer.Visible = false;
            //    panelViewer2.Visible = false;
            //    YeniResim.Visible = false;
            //}
        }
        protected void newFileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelViewer2.Visible = true;
            panel2.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "showLoading", "showLoading();", true);
            // Retrieve the URL from ViewState

            string Filename = newFileList.SelectedValue;
            string uploadedFileUrl = ftpUrl + CURID + "/"+PROID +"/" + Filename;
            try
            {
                if (Filename.EndsWith("pdf") == true || Filename.EndsWith("PDF") == true)
                {
                    WebClient ftpClient = new WebClient();
                    ftpClient.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                    byte[] imageByte = ftpClient.DownloadData(uploadedFileUrl);


                    var tempFileName = Path.GetTempFileName().Replace("tmp", "pdf");

                    System.IO.File.WriteAllBytes(tempFileName, imageByte);

                    string webFolderPath = Server.MapPath("~/TempImages/");
                    if (!Directory.Exists(webFolderPath))
                    {
                        Directory.CreateDirectory(webFolderPath);
                    }
                    string webFilePath = Path.Combine(webFolderPath, Path.GetFileName(tempFileName));
                    File.Copy(tempFileName, webFilePath, true);

                    string relativeFilePath = "~/TempImages/" + Path.GetFileName(tempFileName);

                    // Use iframe to display the PDF
                    string pdfIframe = $"<iframe src='{ResolveUrl(relativeFilePath)}' type='application/pdf' width='600' height='500'></iframe>";
                    iframe2.Src = $"{ResolveUrl(relativeFilePath)}";
                    pdfViewerPlaceHolder2.Controls.Clear();
                    pdfViewerPlaceHolder2.Controls.Add(new Literal { Text = pdfIframe });
                    iframe2.Visible = true;
                    pdfViewerPlaceHolder2.Visible = true;
                    ResimCıkar.Visible = true;

                }
                else if (Filename.EndsWith("jpg") || Filename.EndsWith("jpeg") || Filename.EndsWith("png"))
                {
                    //System.Threading.Thread.Sleep(5000);
                    WebClient ftpClient = new WebClient();
                    ftpClient.Credentials = new NetworkCredential(ftpUsername, ftpPassword);
                    byte[] imageByte = ftpClient.DownloadData(uploadedFileUrl);


                    var tempFileName = Path.GetTempFileName();
                    System.IO.File.WriteAllBytes(tempFileName, imageByte);

                    // Save the file in a web accessible folder
                    string webFolderPath = Server.MapPath("~/TempImages/");
                    if (!Directory.Exists(webFolderPath))
                    {
                        Directory.CreateDirectory(webFolderPath);
                    }

                    string webFilePath = Path.Combine(webFolderPath, Path.GetFileName(tempFileName) + ".jpg");
                    File.Copy(tempFileName, webFilePath, true);

                    // Set the ImageUrl to the web accessible file path
                    imgViewer2.ImageUrl = "~/TempImages/" + Path.GetFileName(webFilePath);
                    imgViewer2.Visible = true;
                    ResimCıkar.Visible = true;

                }
                else
                {
                    imgViewer2.Visible = false;
                    pdfViewerPlaceHolder2.Visible = false;
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda yükleme ekranını gizle
                ScriptManager.RegisterStartupScript(this, GetType(), "hideLoading", "hideLoading();", true);
                //FileLoad.Visible = true;
                //FileLoad.Text = "Error: " + ex.Message;
            }

        }
        protected void Kaydet_Click(object sender, EventArgs e)
        {
            string htmlContent = Request.Form["Aciklama"];
            var loginRes = (List<LoginObj>)Session["Login"];
            if (txtValue.Text != txtNewFileName.Text)
            {
                DbQuery.insertquery($"update PRODUCTS set PROSUPNAME = MDE_GENEL.dbo.CapitalizeWords({txtNewFileName.Text}) where PROID = {PROID}", ConnectionString);
                txtNewFileName.ReadOnly = true;
            }
            if (YONKODU.Text != FirmaKodu.Text && FirmaKodu.Text != "")
            {
                var barkod = DbQuery.GetValue($"select count(*) from PRODUCTSBAR where PROBARPROID = {PROID} and PROBARID = '{FirmaKodu.Text}'");
                if (barkod == "")
                DbQuery.insertquery($"insert into PRODUCTSBAR values ('{txtNewFileName.Text})',{PROID},0,0,NULL)", ConnectionString);
                FirmaKodu.ReadOnly = true;
            }
            try
            {
                // Geçici bir liste oluşturun
                List<ListItem> itemsToRemove = new List<ListItem>();
                foreach (ListItem item in newFileList.Items)
                {
                    string sira = "0";
                    // Seçili dosyayı al
                    string selectedFileName = item.ToString();

                    if (!CheckFileExists(ftpUrl + CURID + "/" + PROID + "/", ftpUsername, ftpPassword, selectedFileName))
                    {
                        // Dosyanın uzantısını al
                        string fileExtension = Path.GetExtension(selectedFileName).ToLower();

                        if (GetFileCountOnFtp(ftpUrl + CURID + "/" + PROID, ftpUsername, ftpPassword, "") != -1)
                        {
                            sira = GetFileCountOnFtp(ftpUrl + CURID + "/" + PROID, ftpUsername, ftpPassword, "").ToString();
                        }
                        else
                        {
                            sira = "0";
                        }
                        // Taşınacak dosyanın yeni adı ve yolu
                        string newFileName = PROVAL + "_" + sira + fileExtension; // Yeni dosya adı
                        string destinationPath = CURID + "/" + PROID + "/" + newFileName; // Hedef dizin

                        // Dosyayı taşıma işlemi
                        if (MoveFileToFtp(ftpUrl, ftpUsername, ftpPassword, selectedFileName, destinationPath))
                        {
                            // Başarılı mesajı
                            DbQuery.insertquery($"insert into EntegreFStokResimleri values ({PROID},'{destinationPath})'", ConnectionString3);

                        }
                        else
                        {
                            // Hata mesajı
                            //Response.Write("<script>alert('Dosya taşınırken hata oluştu.');</script>");
                        }
                    }
                    else
                    {
                        string fileExtension = Path.GetExtension(selectedFileName).ToLower();
                        string newFileName = PROVAL + "_" + sira + fileExtension; // Yeni dosya adı
                        string destinationPath = CURID + "/" + PROID + "/" + newFileName; // Hedef dizin
                        var kontrol = DbQuery.GetValue($"select count(*) from W.[Yonavm_Web_Siparis].dbo.EntegreFStokResimleri where RESIMADI = '{destinationPath}'");
                        if (kontrol == "0")
                        {
                            DbQuery.insertquery($"insert into EntegreFStokResimleri values ({PROID},'{destinationPath}')", ConnectionString3);
                        }
                    }
                }
                // Başarılı öğeleri listeden kaldır
                foreach (var listItem in itemsToRemove)
                {
                    newFileList.Items.Remove(listItem);
                }
                UpdateListBoxes();
            }
            catch (Exception)
            {
            }
            if (Aciklama.Text != "" || string.IsNullOrEmpty(Aciklama.Text))
            {
                try
                {
                    string TicimaxAciklama = "Site URL Adresi = " + txtUrl.Text + Environment.NewLine +" Ürün Açıklaması = "+ Environment.NewLine + Aciklama.Text;
                    Dictionary<string, string> aciklama = new Dictionary<string, string>();
                    aciklama.Add("@id", PROID);
                    aciklama.Add("@aciklama", TicimaxAciklama);
                    aciklama.Add("@User", loginRes[0].SOCODE);
                    aciklama.Add("@ReturnDesc", "");
                    var sonuc = DbQuery.Insert3("StokAciklama", aciklama);
                    string message = "Açıklama "+sonuc+" İşlem başarılı!";
                    // JavaScript kodunu oluştur
                    string script = $@"
                    <script type='text/javascript'>
                        $(document).ready(function () {{
                            $('#modalMessage').val('{message}');
                            showModal();
                        }});
                    </script>";

                    // Scripti sayfaya ekle
                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "showModal",
                        script,
                        false
                    );
                    //lblMessage.Text = message;
                    //ClientScript.RegisterStartupScript(this.GetType(), "modal", "$('#modalMessage').text('" + message + "'); $('#myModal').modal('show');", true);
                }
                catch (Exception)
                {
                }
            }
        }

        protected void UrlCek_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text;

            // HttpClient kullanarak web sitesine istek gönderiyoruz
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    string htmlContent = httpClient.GetStringAsync(url).Result;

                    // HtmlAgilityPack ile HTML içeriğini analiz ediyoruz
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    if (txtTeknikClass.Text != "")
                    {
                        // Hedef class adı
                        string TeknikClass = txtTeknikClass.Text;

                        // Belirli bir class adını içeren HTML elementlerini seçiyoruz
                        HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes($"//*[contains(@class, '{TeknikClass}')]");
                        Aciklama.Text = "";
                        if (nodes != null)
                        {
                            foreach (HtmlNode node in nodes)
                            {
                                // Seçilen elementleri burada işleyebilirsiniz
                                Aciklama.Text += node.OuterHtml;
                                //Console.WriteLine(node.OuterHtml);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"'{TeknikClass}' sınıfı içeren element bulunamadı.");
                        }
                    }
                    if (txtDetayClass.Text != "")
                    {
                        string DetayClass = txtDetayClass.Text;

                        // Belirli bir class adını içeren HTML elementlerini seçiyoruz
                        HtmlNodeCollection nodes2 = doc.DocumentNode.SelectNodes($"//*[contains(@class, '{DetayClass}')]");
                        if (nodes2 != null)
                        {
                            if (nodes2.Count >= 3)
                            {
                                HashSet<string> nodeler = new HashSet<string>();
                                foreach (HtmlNode node in nodes2)
                                {
                                    if (nodeler.Add(node.OuterHtml))
                                    {
                                        Aciklama.Text += node.OuterHtml;
                                    }
                                    // Seçilen elementleri burada işleyebilirsiniz
                                    //Console.WriteLine(node.OuterHtml);

                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"'{DetayClass}' sınıfı içeren element bulunamadı.");
                        }
                    }
                    DbQuery.insertquery(String.Format("update VolantToTicimaxWebClass set DetayClass = '{0}',TeknikClass = '{2}' where Vol_WPTREVAL = '{1}'", txtDetayClass.Text, WPTREVAL, txtTeknikClass.Text),ConnectionString3);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hata oluştu: {ex.Message}");
                }
            }
            List<string> imageLinks = GetImageLinksFromUrl(url);


            foreach (string link in imageLinks)
            {
                Console.WriteLine(link);
            }
        }
        static List<string> GetImageLinksFromUrl(string url)
        {
            List<string> imageLinks = new List<string>();

            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = web.Load(url);

                foreach (HtmlNode imgNode in doc.DocumentNode.SelectNodes("//img[@src]"))
                {
                    string src = imgNode.GetAttributeValue("src", "");
                    if (!string.IsNullOrEmpty(src))
                    {
                        if (Uri.IsWellFormedUriString(src, UriKind.Absolute))
                        {
                            imageLinks.Add(src);
                        }
                        else if (Uri.TryCreate(new Uri(url), src, out Uri absoluteUri))
                        {
                            imageLinks.Add(absoluteUri.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }

            return imageLinks;
        }

        protected void Kapat_Click(object sender, EventArgs e)
        {
            Response.Redirect("StokIslemleri.aspx");
        }
    }
}

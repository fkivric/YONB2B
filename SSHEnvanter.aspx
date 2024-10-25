<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SSHEnvanter.aspx.cs" Inherits="YONB2B.SSHEnvanter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .profile-picture {
            opacity: 0.75;
            height: 400px;
            width: 100%;
            position: relative;
            overflow: hidden;
            /* default image */
            /*background-image: url("img/arac-ruhsati-500-330.jpg");*/
            background-position: center;
            background-repeat: no-repeat;
            background-size: cover;
            box-shadow: 0 8px 6px -6px black;
        }

        .file-uploader {
            /* make it invisible */
            opacity: 0;
            /* make it take the full height and width of the parent container */
            height: 100%;
            width: 100%;
            cursor: pointer;
            /* make it absolute */
            position: absolute;
            top: 0%;
            left: 0%;
        }

        .upload-icon {
            position: absolute;
            top: 45%;
            left: 50%;
            transform: translate(-50%, -50%);
            /* initial icon state */
            opacity: 0;
            transition: opacity 0.3s ease;
            color: #ccc;
            -webkit-text-stroke-width: 2px;
            -webkit-text-stroke-color: #bbb;
        }
        /* toggle icon state */
        .profile-picture:hover .upload-icon {
            opacity: 1;
        }

        .profile-picture2 {
            opacity: 0.75;
            height: 250px;
            width: 400px;
            position: relative;
            overflow: hidden;
            /* default image */
            /*background-image: url("img/tramer.png");*/
            background-position: center;
            background-repeat: no-repeat;
            background-size: cover;
            box-shadow: 0 8px 6px -6px black;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-12 col-lg-12">
                <div class="card">
                    <div class="card-header">
                        Firma ve Stok Seçimi
                    </div>
                    <div class="row m-0 row-group text-center border-top border-light-3">
                        <div class="col-md-6">
                            <label class="form-label" for="StokKodu">Stok Kodu</label>
                            <br />
                            <asp:TextBox runat="server" ID="StokKodu" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label" for="StokAdi">Stok Adı</label>
                            <br />
                            <asp:TextBox runat="server" ID="StokAdi" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row m-0 row-group text-center border-top border-light-3 align-items-center">
                        <div class="col-md-6">
                            <label class="form-label" for="Marka">Marka</label>
                            <br />
                            <asp:DropDownList runat="server" ID="Marka" class="form-select form-control color-dropdown" OnSelectedIndexChanged="marka_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="">Seçiniz...</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label" for="Model">Model</label>
                            <br />
                            <asp:DropDownList runat="server" ID="Model" class="form-select form-control color-dropdown">
                                <asp:ListItem Value="">Seçiniz...</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <br />
                    <asp:Button runat="server" ID="Listele" CssClass="form-control d-block btn waves-effect waves-light btn-primary" OnClick="Listele_Click" Text="Listele" />
                </div>

                <div runat="server" id="row" class="row" hidden="hidden">
                    <div class="col-12 col-lg-12">
                        <div class="card">
                            <div class="card-header">
                                Güncellenecek Ürün Listesi
                            </div>
                            <div class="row m-0 row-group text-center border-top border-light-3">
                                <div class="col-12">
                                    <div class="col-md-6">
                                    </div>
                                </div>
                            </div>

                            <div class="row m-0 row-group text-center border-top border-light-3">
                                <div class="col-12">
                                    <div class="col-md-6">
                                        <asp:GridView runat="server" ID="Urunler"
                                            AutoGenerateColumns="False"
                                            ShowHeader="true"
                                            OnRowCreated="Urunler_RowCreated"
                                            OnRowCommand="Urunler_RowCommand">
                                            <Columns>
                                                <asp:ButtonField ButtonType="Button" Text="Seç" HeaderText="Güncelle" CommandName="Select" />
                                                <asp:BoundField DataField="PROID" />
                                                <asp:BoundField DataField="PROVAL" />
                                                <asp:BoundField DataField="PRONAME" HeaderText="Stok Adı" ItemStyle-CssClass="full-width" HeaderStyle-Font-Size="XX-Large" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!--End Row-->

            <!--End Dashboard Content-->

            <!--start overlay-->
            <div class="overlay toggle-menu"></div>
            <!--end overlay-->

        </div>
        <!-- End container-fluid-->
        <!--Start Back To Top Button-->
        <a href="javaScript:void();" class="back-to-top"><i class="fa fa-angle-double-up"></i></a>
        <!--End Back To Top Button-->
        <style>
            /* PRONAME Sütununun Tam Genişlikte Olması */
            .full-width {
                width: 100%; /* Sütunun genişliğini %100 yapar */
            }

            .full-width2 {
                width: 10%; /* Sütunun genişliğini %100 yapar */
            }
            /* Buton Stili */
            .gridview-button {
                width: 100%; /* Butonun genişliği satırı kaplayacak şekilde ayarlanır */
                padding: 10px; /* Buton içindeki boşluk */
            }
        </style>
    </div>

    <script src="assets/js/index.js"></script>
    <script>
          function upload() {

              const fileUploadInput = document.querySelector('.file-uploader');
              const filetext = document.getElementById('uploaderName1');
              /// Validations ///
              if (!fileUploadInput.value) {
                  return;
              }

              // using index [0] to take the first file from the array
              const image = fileUploadInput.files[0];
              // check if the file selected is not an image file
              if (!image.type.includes('image') && image.type != 'application/pdf') {
                  return alert('Sadece resim Dosyası Seçimine izin verilir!');
              }

              // check if size (in bytes) exceeds 10 MB
              if (image.size > 10_000_000) {
                  return alert('Maksimum yükleme boyutu 10MB!');
              }

              /// Display the image on the screen ///
              if (image.type === 'application/pdf') {
                  const profilePicture = document.querySelector('.profile-picture');
                  profilePicture.style.backgroundImage = `url('img/pdf.jpg')`; // Adjust the path as needed
                  filetext.textContent = `Seçilen PDF dosyası: ${file.name}`;
              }
              else if (image.type.includes('image')) {
                  const fileReader = new FileReader();
                  fileReader.readAsDataURL(image);

                  fileReader.onload = (fileReaderEvent) => {
                      const profilePicture = document.querySelector('.profile-picture');
                      profilePicture.style.backgroundImage = `url(${fileReaderEvent.target.result})`;
                      nodes.text = fileUploadInput.value;
                  }
                  filetext.text = fileUploadInput.files[0];
                  // upload image to the server or the cloud
              }
          }
    </script>
</asp:Content>

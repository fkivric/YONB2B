<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SSHOlusTurma.aspx.cs" Inherits="YONB2B.SSHOlusTurma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--checkbox style--%>
    <style>
        .checkbox-wrapper-10 .tgl {
            display: none;
        }

            .checkbox-wrapper-10 .tgl,
            .checkbox-wrapper-10 .tgl:after,
            .checkbox-wrapper-10 .tgl:before,
            .checkbox-wrapper-10 .tgl *,
            .checkbox-wrapper-10 .tgl *:after,
            .checkbox-wrapper-10 .tgl *:before,
            .checkbox-wrapper-10 .tgl + .tgl-btn {
                box-sizing: border-box;
            }

                .checkbox-wrapper-10 .tgl::-moz-selection,
                .checkbox-wrapper-10 .tgl:after::-moz-selection,
                .checkbox-wrapper-10 .tgl:before::-moz-selection,
                .checkbox-wrapper-10 .tgl *::-moz-selection,
                .checkbox-wrapper-10 .tgl *:after::-moz-selection,
                .checkbox-wrapper-10 .tgl *:before::-moz-selection,
                .checkbox-wrapper-10 .tgl + .tgl-btn::-moz-selection,
                .checkbox-wrapper-10 .tgl::selection,
                .checkbox-wrapper-10 .tgl:after::selection,
                .checkbox-wrapper-10 .tgl:before::selection,
                .checkbox-wrapper-10 .tgl *::selection,
                .checkbox-wrapper-10 .tgl *:after::selection,
                .checkbox-wrapper-10 .tgl *:before::selection,
                .checkbox-wrapper-10 .tgl + .tgl-btn::selection {
                    background: none;
                }

                .checkbox-wrapper-10 .tgl + .tgl-btn {
                    outline: 0;
                    display: block;
                    width: 100%;
                    height: 2em;
                    position: relative;
                    cursor: pointer;
                    -webkit-user-select: none;
                    -moz-user-select: none;
                    -ms-user-select: none;
                    user-select: none;
                }

                    .checkbox-wrapper-10 .tgl + .tgl-btn:after,
                    .checkbox-wrapper-10 .tgl + .tgl-btn:before {
                        position: relative;
                        display: block;
                        content: "";
                        width: 50%;
                        height: 100%;
                    }

                    .checkbox-wrapper-10 .tgl + .tgl-btn:after {
                        left: 0;
                    }

                    .checkbox-wrapper-10 .tgl + .tgl-btn:before {
                        display: none;
                    }

                .checkbox-wrapper-10 .tgl:checked + .tgl-btn:after {
                    left: 50%;
                }

        .checkbox-wrapper-10 .tgl-flip + .tgl-btn {
            padding: 2px;
            transition: all 0.2s ease;
            font-family: sans-serif;
            perspective: 100px;
        }

            .checkbox-wrapper-10 .tgl-flip + .tgl-btn:after,
            .checkbox-wrapper-10 .tgl-flip + .tgl-btn:before {
                display: inline-block;
                transition: all 0.4s ease;
                width: 100%;
                text-align: center;
                position: absolute;
                line-height: 2em;
                font-weight: bold;
                color: #fff;
                position: absolute;
                top: 0;
                left: 0;
                -webkit-backface-visibility: hidden;
                backface-visibility: hidden;
                border-radius: 4px;
            }

            .checkbox-wrapper-10 .tgl-flip + .tgl-btn:after {
                content: attr(data-tg-on);
                background: #02C66F;
                transform: rotateY(-180deg);
            }

            .checkbox-wrapper-10 .tgl-flip + .tgl-btn:before {
                background: #FF3A19;
                content: attr(data-tg-off);
            }

            .checkbox-wrapper-10 .tgl-flip + .tgl-btn:active:before {
                transform: rotateY(-20deg);
            }

        .checkbox-wrapper-10 .tgl-flip:checked + .tgl-btn:before {
            transform: rotateY(180deg);
        }

        .checkbox-wrapper-10 .tgl-flip:checked + .tgl-btn:after {
            transform: rotateY(0);
            left: 0;
            background: #7FC6A6;
        }

        .checkbox-wrapper-10 .tgl-flip:checked + .tgl-btn:active:after {
            transform: rotateY(20deg);
        }
    </style>

    <script src="assets/js/calculater.js"></script>
    <%--Takvimi açmak için tıklama simülasyonu--%>
    <style type="text/css">
        .profile-picture {
            opacity: 0.75;
            min-height: 400px;
            height: 100%;
            width: 100%;
            /* default image */
            /*background-image: url("img/arac-ruhsati-500-330.jpg");*/
            background-position: center;
            background-repeat: no-repeat;
            background-size: contain;
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
            opacity: 1;
            transition: opacity 0.3s ease;
            color: #ccc;
            -webkit-text-stroke-width: 2px;
            -webkit-text-stroke-color: #bbb;
        }
        /* toggle icon state */
        .profile-picture:hover .upload-icon {
            opacity: 1;
        }

        .ListBox {
            color: white;
            height: 100%;
            width: 100%;
        }

        .viewer-panel {
            position: relative;
        }

        .full-image {
            width: 100%;
            height: 100%;
            object-fit: contain; /* Görseli orantılı bir şekilde boyutlandırır */
        }

        /* İsteğe bağlı: Tam ekran için iframe'yi de doldurmak istiyorsanız */
        iframe {
            width: 100%;
            height: 100%;
        }
    </style>
    <script type="text/javascript">
        function openDatePicker() {
            var dateInput = document.getElementById('<%= customerDATE.ClientID %>');
            dateInput.click(); // Takvimi açmak için tıklama simülasyonu
        }
    </script>
    <%--model--%>
    <style>
        .modal {
            display: none;
            position: fixed;
            width: 100%;
            height: 100%;
            overflow: auto;
            background-color: rgba(0, 0, 0, 0.5);
        }

        .modal-content {
            background-color: #fefefe;
            margin: 15% auto;
            padding: 25px;
            border: 1px solid #888;
            width: 90%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header text">
                        <span runat="server" id="Stokkodu">Stok Kodu</span>
                        <span runat="server" id="Stokadi">Stok Adı</span>
                    </div>
                    <div class="row m-0 row-group text-center border-top border-light-3">
                        <div class="col-12 col-lg-4">
                            <div class="p-3">
                                <label class="form-label" for="Depo">İşlem Mağazası</label>
                                <br />
                                <asp:DropDownList runat="server" ID="Depo" class="form-select form-control color-dropdown">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-12 col-lg-4">
                            <div class="p-3">
                                <label class="form-label" for="Firma">Tedarikci Firma Adı:</label>
                                <br />
                                <asp:DropDownList runat="server" ID="Firma" class="form-select form-control color-dropdown">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-12 col-lg-4">
                            <div class="p-3">
                                <label class="form-label" for="Tarih">İşlem Tarihi</label>
                                <br />

                                <input class="col-md-4" type="date" id="customerDATE" runat="server" />
                                <%--                                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                                     <a href="javascript:OpenPopupPage('Calendar.aspx','<%= txtDate.ClientID %>','<%= Page.IsPostBack %>');">--%>
                                <img src="assets/images/icon-calendar.gif" />
                            </div>
                        </div>
                    </div>
                    <div class="row m-0 row-group text-center border-top border-light-3">
                        <div class="col-12 col-lg-4">
                            <div class="p-3">
                                <label class="form-label" for="SSHTuru">Hata Kodu</label>
                                <br />
                                <asp:DropDownList runat="server" ID="SSHTuru" class="form-select form-control color-dropdown" OnSelectedIndexChanged="SSHTuru_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="Parça Arızası" Text="Parça Arızası"></asp:ListItem>
                                    <asp:ListItem Value="Ürün Eksiği" Text="Ürün Eksiği"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-12 col-lg-4">
                            <div class="p-3">
                                <label class="form-label" for="Depo">SSH Muhattabı</label>
                                <br />
                                <asp:DropDownList runat="server" ID="TamirYeri" class="form-select form-control color-dropdown">
                                    <asp:ListItem Value="0" Text="Tedarikci yapacak"></asp:ListItem>
                                    <asp:ListItem Value=" 1" Text="Yetkili Servis Yapacak"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-12 col-lg-4">
                            <div class="p-3">
                                <label class="form-label" for="cb5">Garanti Durumu</label>
                                <br />
                                <div class="checkbox-wrapper-10">
                                    <input class="tgl tgl-flip" id="cb5" type="checkbox" checked="checked" />
                                    <label class="tgl-btn" data-tg-off="Eski" data-tg-on="Garantili Ürün" for="cb5"></label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card">
                    <div class="row m-0 row-group text-center border-top border-light-3">
                        <div class="col-12 col-lg-6">
                            <div class="p-3">
                                <div runat="server" id="uploadarea" class="card-body">
                                    <h4 class="card-title">Resim Ekleme</h4>
                                    <div class="row gx-1 gy-1 align-items-center">
                                        <div runat="server" class="row col-12">
                                            <div runat="server" id="resim1" class="col-md-12">
                                                <div class="card">
                                                    <div>
                                                        <div runat="server" id="Div1" class="number"></div>
                                                    </div>
                                                    <div runat="server" id="profilepicture" class="profile-picture">
                                                        <h1 class="upload-icon">
                                                            <i class="fa fa-plus fa-2x" aria-hidden="true"></i>
                                                        </h1>
                                                        <input
                                                            id="oFile"
                                                            type="file"
                                                            runat="server"
                                                            class="file-uploader"
                                                            onchange="upload()"
                                                            accept="image/*,application/pdf" />
                                                    </div>
                                                    <div class="iconBox">
                                                        <i class="fa fa-eye" aria-hidden="true"></i>
                                                        <label id="uploaderName1"></label>
                                                    </div>
                                                    <asp:Button ID="btnUpload" type="submit" Text="Ekle" runat="server" OnClick="btnUpload_Click"></asp:Button>
                                                    <asp:Panel ID="frmConfirmation" Visible="False" runat="server">
                                                        <asp:Label ID="lblUploadResult" runat="server"></asp:Label>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-12 col-lg-6">
                            <div class="p-3">
                                <h4 class="card-title">SSH Açıklaması</h4>
                                <textarea runat="server" id="Aciklama" class="textarea" placeholder="Enter text ..." style="width: 100%; height: 400px"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <asp:Button runat="server" ID="Kaydet" CssClass="btn btn-success form-control" Text="Kaydet" OnClick="Kaydet_Click" Visible="false" />
                </div>
            </div>
        </div>

        <div id="myModal" class="modal">
            <h5>Aranacak Kelime</h5>
            <div class="modal-content">
                <div class="modal-header text-center">
                    <a href="javascript:void(0)" class="text-success">
                        <span>
                            <img class="mr-2" src="assets/images/Yon%20Logo.png"
                                alt="" height="40">
                            <img src="assets/images/Yon%20Text.png" alt=""
                                height="40">
                        </span>
                    </a>
                </div>
                <div class="modal-body">
                    <label for="txtFilter">Arama</label>
                    <asp:TextBox ID="txtFilter" runat="server" OnTextChanged="txtFilter_TextChanged" AutoPostBack="true" BorderStyle="Solid" BorderColor="#000000" />
                    <div class="form-group">
                        <asp:GridView runat="server" ID="SSHFAULTS"
                            AutoGenerateColumns="false"
                            CssClass="gridView"
                            OnRowCreated="SSHFAULTS_RowCreated"
                            OnRowCommand="SSHFAULTS_RowCommand">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <Columns>
                                <asp:ButtonField ItemStyle-CssClass="button" CommandName="Select" Text="Seç" ButtonType="Button" HeaderText="Seçim" />
                                <asp:BoundField DataField="SFAULVAL" />
                                <asp:BoundField ItemStyle-CssClass="td1" DataField="SFAULNOTES" HeaderText="MALZEME" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField ItemStyle-CssClass="td1" DataField="SFAULSOURCE" HeaderText="KAYNAK" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField ItemStyle-CssClass="td1" DataField="SFAULNAME" HeaderText="PROBLEM" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </asp:GridView>
                        <asp:HiddenField ID="hdnSelectedId" runat="server" />
                    </div>
                    <div class="row gx-3 gy-2 align-items-center form-group text-center">
                        <asp:Button runat="server" ID="Kapat" CssClass="btn btn-outline-danger" Width="100%" Text="Kapat" OnClick="Kapat_Click" />
                    </div>
                </div>
            </div>
        </div>
        <!-- /.modal-dialog -->
    </div>
    <script>
        function openModal() {
            document.getElementById("myModal").style.display = "block";
        }

        function closeModal() {
            document.getElementById("myModal").style.display = "none";
        }

        window.onclick = function (event) {
            var modal = document.getElementById("myModal");
            if (event.target == modal) {
                modal.style.display = "none";
            }
        }
    </script>
    <style>
        /* GridView kontrolünüz için genel stil */
        .gridView {
            width: 100%; /* GridView'in tam genişlikte olmasını sağlar */
            table-layout: fixed; /* Sütun genişliklerini korur */
            border-collapse: collapse;
            color: black;
            font-size: 12px;
        }

            .gridView .td1 .td2 {
                word-wrap: break-word;
                white-space: normal;
                overflow-wrap: break-word;
            }

            .gridView .button {
                width: 100%;
            }

        .td1 {
            table-layout: fixed;
            word-wrap: break-word; /* Uzun kelimeleri satır sonuna sarmak için */
            white-space: normal; /* Satır sonu karakterlerini ve boşlukları işleyin */
            overflow-wrap: break-word; /* Uzun kelimeleri alt satıra geçirin */
            /*border: solid;*/
            /* max-width: 150px; İsteğe bağlı: sütun genişliğini sınırlandırın */
        }

        .td2 {
            table-layout: auto;
            /*border: none;*/
        }
    </style>
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
                profilePicture.style.backgroundImage = `url('assets/images/pdf.png')`; // Adjust the path as needed
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

<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SSHEnvanter.aspx.cs" Inherits="YONB2B.SSHEnvanter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table {
            width: 100%;
            border-collapse: collapse;
        }

            .table th, .table td {
                padding: 8px;
                text-align: left; /* Sütunların sola hizalanması */
            }
        .pagination {
            display: flex;
            justify-content: center;
            margin-top: 20px;
        }

            .pagination a {
                margin: 0 5px;
                padding: 8px 12px;
                border: 1px solid #ccc;
                text-decoration: none;
                color: #007bff;
                border-radius: 5px;
                transition: background-color 0.3s;
            }

                .pagination a.active {
                    background-color: #007bff;
                    color: white;
                }

                .pagination a:hover {
                    background-color: #0056b3;
                    color: white;
                }

        @media (max-width: 768px) {
            .table thead {
                display: none; /* Başlıkları gizle */
                text-align: left; /* Sütunların sola hizalanması */
            }

            .table tr {
                display: block; /* Satırları blok hale getir */
                margin-bottom: 25px; /* Satırlar arasında boşluk */
            }
            .table td {
                display: flex;
                /*justify-content: space-between;*/ /* Hücre içindeki içerikleri yan yana yerleştir */
                justify-content: flex-start; /* İçeriği sola hizala */
                border: none; /* Kenarları kaldır */
                padding: 5px;
                word-wrap: break-word; /* Uzun kelimeleri satır sonuna sarmak için */
                white-space: normal; /* Satır sonu karakterlerini ve boşlukları işleyin */
                overflow-wrap: break-word; /* Uzun kelimeleri alt satıra geçirin */
            }

                .table td:before {
                    content: attr(data-label); /* Hücre öncesi içerik olarak etiket ekle */
                    font-weight: bold; /* Etiketi kalın yap */
                    flex-basis: 40%; /* Etiket genişliği */
                }
            .pagination {
                flex-direction: column; /* Dikey hizalama */
                align-items: center; /* Merkezde hizala */
            }

            .pagination a {
                margin: 5px 0; /* Dikeyde boşluk ver */
                padding: 6px 10px; /* Küçük buton boyutu */
            }
        }
    </style>
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
                    <div class="card-header text-center">
                        <h2>Ürün Seçimi</h2>
                    </div>
                    <div class="row m-0 row-group text-center border-top border-light-3">
                        <div class="col-12 col-lg-4">
                            <div class="p-3">
                                <label class="form-label" for="Depo">İşlem Mağazası</label>
                                <asp:DropDownList runat="server" ID="Depo" class="form-select form-control color-dropdown">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="p-3">
                                <label class="form-label" for="Marka">Marka</label>
                                <asp:DropDownList runat="server" ID="Marka" class="form-select form-control color-dropdown" OnSelectedIndexChanged="marka_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="">Seçiniz...</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="p-3">
                                <label class="form-label" for="Model">Model</label>
                                <asp:DropDownList runat="server" ID="Model" class="form-select form-control color-dropdown">
                                    <asp:ListItem Value="">Seçiniz...</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row m-0 row-group text-center border-top border-light-3 align-items-center">
                        <div class="col-md-4">
                            <div class="p-3">
                                <label class="form-label" for="StokKodu">Stok Kodu</label>
                                <asp:TextBox runat="server" ID="StokKodu" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="p-3">
                                <label class="form-label" for="StokAdi">Stok Adı</label>
                                <asp:TextBox runat="server" ID="StokAdi" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="p-3">
                                <label runat="server" id="EnvanterDurum" class="form-label" for="switch">Envanterde Olanlar</label>
                                <br />
                                <label id="switch" class="switch">
                                    <input runat="server" type="checkbox" onchange="toggleText()" id="QUAN" checked/>
                                    <%--<asp:CheckBox runat="server" ID="QUAN" Checked="true" onchange="toggleText()" />--%>
                                    <span class="slider round"></span>
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="row m-0 row-group text-center border-top border-light-3">
                        <div class="col-12 col-lg-12">
                            <div class="p-3">
                                <asp:Button runat="server" ID="Listele" CssClass="form-control d-block btn waves-effect waves-light btn-primary" OnClick="Listele_Click" Text="Listele" />
                            </div>
                        </div>
                    </div>
                </div>
                <div runat="server" id="row" class="card" hidden="hidden">
                    <div class="row m-0 row-group text-center border-top border-light-3">
                        <div class="col-12">
                            <div class="p-3">
                                <div class="table-responsive">
                                    <asp:Repeater ID="ProductRepeater" runat="server">
                                        <HeaderTemplate>
                                            <table id="zero_config" class="table table-hover table-flush" border="1">
                                                <thead>
                                                    <tr>
                                                        <th>Stok Kodu</th>
                                                        <th>Stok Adı</th>
                                                        <th>Envanter Adet</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td data-label="Stok Kodu">
                                                    <a href='SSHOlusTurma.aspx?proid=<%# Eval("PROID") %>&cuscurid=<%# Depo.SelectedValue %>&proname=<%# Eval("PRONAME") %>'><%# Eval("PROVAL") %></a>
                                                </td>
                                                <td data-label="Stok Adı=">                                                                
                                                    <a href='SSHOlusTurma.aspx?proid=<%# Eval("PROID") %>&cuscurid=<%# Depo.SelectedValue %>&proname=<%# Eval("PRONAME") %>'><%# Eval("PRONAME") %></a>
                                                </td>
                                                <td data-label="Envanter Adeti=">                                                                
                                                    <a href='SSHOlusTurma.aspx?proid=<%# Eval("PROID") %>&cuscurid=<%# Depo.SelectedValue %>&proname=<%# Eval("PRONAME") %>'><%# Eval("adet") %></a>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
                                            </table>               
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <div class="pagination" id="pagination">
                                        <asp:Literal ID="litPagination" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-12">
                            <asp:GridView runat="server" ID="Urunler"
                                AutoGenerateColumns="False"
                                ShowHeader="true"
                                OnRowCreated="Urunler_RowCreated"
                                OnRowCommand="Urunler_RowCommand">
                                <Columns>
                                    <asp:ButtonField ButtonType="Button" Text="Seç" HeaderText="Güncelle" CommandName="Select" />
                                    <asp:BoundField DataField="PROID" />
                                    <asp:BoundField DataField="PROVAL" />
                                    <asp:BoundField DataField="PRONAME" HeaderText="Stok Adı" ItemStyle-CssClass="full-width"/>
                                    <asp:BoundField DataField="adet" HeaderText="Adet" ItemStyle-CssClass="full-width" DataFormatString="{0:C2}"/>
                                </Columns>
                            </asp:GridView>
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
        <style>
            .switch {
                position: relative;
                display: inline-block;
                width: 60px;
                height: 34px;
            }

                .switch input {
                    opacity: 0;
                    width: 0;
                    height: 0;
                }

            .slider {
                position: absolute;
                cursor: pointer;
                top: 0;
                left: 0;
                right: 0;
                bottom: 0;
                background-color: #ccc;
                -webkit-transition: .4s;
                transition: .4s;
            }

                .slider:before {
                    position: absolute;
                    content: "";
                    height: 26px;
                    width: 26px;
                    left: 4px;
                    bottom: 4px;
                    background-color: white;
                    -webkit-transition: .4s;
                    transition: .4s;
                }

            input:checked + .slider {
                background-color: #2196F3;
            }

            input:focus + .slider {
                box-shadow: 0 0 1px #2196F3;
            }

            input:checked + .slider:before {
                -webkit-transform: translateX(26px);
                -ms-transform: translateX(26px);
                transform: translateX(26px);
            }

            /* Rounded sliders */
            .slider.round {
                border-radius: 34px;
            }

                .slider.round:before {
                    border-radius: 50%;
                }
        </style>
    </div>
    <script type="text/javascript">
        function myPostBackFunction(page) {
            alert("Sayfa: " + page + " tıklandı!");
            // Burada sayfa geçişi için gerekli işlemi yapabilirsiniz.
            // Örneğin AJAX ile sayfa içeriklerini güncelleyebilirsiniz.
        }
    </script>
    <script src="assets/js/index.js"></script>
    <script type="text/javascript">
        function toggleText() {
            var checkbox = document.getElementById('<%= QUAN.ClientID %>');
            var envanterDurumLabel = document.getElementById('<%= EnvanterDurum.ClientID %>');
            if (checkbox.checked) {
                envanterDurumLabel.innerText = "Envanterde Olanlar";
            } else {
                envanterDurumLabel.innerText = "Envanterde Olmayanlar";
            }
        }
    </script>
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

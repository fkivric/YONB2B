<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SSHMusteri.aspx.cs" Inherits="YONB2B.SSHMusteri" EnableEventValidation="true" %>

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
        .table2 {
            width: 100%;
            border-collapse: collapse;
        }

            .table2 th, .table2 td {
                padding: 8px;
                text-align: left; /* Sütunların sola hizalanması */
                color:black;
                border:solid;
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
            .table2 thead {
                display: none; /* Başlıkları gizle */
                text-align: left; /* Sütunların sola hizalanması */
            }

            .table2 tr {
                display: block; /* Satırları blok hale getir */
                margin-bottom: 25px; /* Satırlar arasında boşluk */
            }

            .table2 td {
                display: flex;
                /*justify-content: space-between;*/ /* Hücre içindeki içerikleri yan yana yerleştir */
                justify-content: flex-start; /* İçeriği sola hizala */
                border: none; /* Kenarları kaldır */
                padding: 5px;
                word-wrap: break-word; /* Uzun kelimeleri satır sonuna sarmak için */
                white-space: normal; /* Satır sonu karakterlerini ve boşlukları işleyin */
                overflow-wrap: break-word; /* Uzun kelimeleri alt satıra geçirin */
            }

                .table2 td:before {
                    content: attr(data-label); /* Hücre öncesi içerik olarak etiket ekle */
                    font-weight: bold; /* Etiketi kalın yap */
                    flex-basis: 40%; /* Etiket genişliği */
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-12 col-lg-12">
                <div class="card">
                    <div class="card-header">
                        Kurulum Bekleyen Müşteri Listesi
                    </div>
                    <div class="row m-0 row-group text-center border-top border-light-3">
                        <div class="col-12 col-lg-4">
                            <div class="p-3">
                                <label for="marka">Nağaza Seçimi</label>
                                <asp:DropDownList runat="server" ID="Depo" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-12 col-lg-4">
                            <div class="p-3">
                                <label for="Search">Müşteri Ad & Soyadından Arama</label>
                                <asp:TextBox runat="server" ID="Search" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-12 col-lg-4">
                            <div class="p-3">
                                <label for="Search">Müşteri Ad & Soyadından Arama</label>
                                <asp:TextBox runat="server" ID="TC" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-12">
                            <asp:Button runat="server" ID="Listele" Text="Göster" OnClick="Listele_Click" CssClass="btn btn-info form-control" />
                        </div>
                    </div>
                </div>
                <div runat="server" id="Liste" class="row">
                    <div class="col-12 col-lg-12">
                        <div class="card">
                            <div class="card-header">
                                Mağaza Mobilya Alan Müşteri Listesi (İLK 100)
                                <div class="row m-0 row-group text-center border-top border-light-3">
                                    <div class="col-12">
                                        <div class="p-3">
                                            <div class="table-responsive">
                                                <asp:Repeater ID="ProductRepeater" runat="server">
                                                    <HeaderTemplate>
                                                        <table id="zero_config" class="table table-hover table-flush" border="1">
                                                            <thead>
                                                                <tr>
                                                                    <th>Müşteri No</th>
                                                                    <th>Müşteri Adı (Seç)</th>
                                                                    <th>Müşteri Tc</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:Button ID="btnOpenModal" runat="server"
                                                                    Text="Aç" CommandArgument='<%# Eval("CURID") %>'
                                                                    OnClick="btnOpenModal_Click" />
                                                            </td>
                                                            <td data-label="Müşteri No"><%# Eval("CURVAL") %>
                                                                <%--<a href='SSHMusteriMain.aspx?CURID=<%# Eval("CDRCURID")%>&SALID=<%# Eval("ORDSALID") %>'></a>--%>
                                                            </td>
                                                            <td data-label="Müşteri Adı"><%# Eval("CURNAME") %></td>
                                                            <td data-label="Müşteri Tc"><%# Eval("CUSIDTCNO") %></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        </tbody>
                                                        </table>               
                                                    </FooterTemplate>
                                                </asp:Repeater>
                                                <asp:Label runat="server" ID="sayfatoplam" Text="Toplam Sayfa :"></asp:Label>
                                                <div class="pagination" id="pagination">
                                                    <asp:Literal ID="litPagination" runat="server"></asp:Literal>
                                                </div>
                                            </div>
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
        </div>
    </div>

    <div id="myModal2" class="modal">
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
                <div class="form-group">
                    <asp:GridView runat="server" ID="SALES"
                        AutoGenerateColumns="false" OnRowCreated="SALES_RowCreated" OnRowCommand="SALES_RowCommand"
                        CssClass="gridView">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <Columns>
                            <asp:ButtonField ItemStyle-CssClass="button" CommandName="Select" Text="Seç" ButtonType="Button" HeaderText="Seçim" />
                            <asp:BoundField DataField="SALID" />
                            <asp:BoundField DataField="CURID" />
                            <asp:BoundField ItemStyle-CssClass="td1" DataField="CURVAL" HeaderText="Müşteri No" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField ItemStyle-CssClass="td1" DataField="CURNAME" HeaderText="Müşteri Adı" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField ItemStyle-CssClass="td1" DataField="SALDATE" HeaderText="Satış Tarihi" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:d}" />
                            <asp:BoundField ItemStyle-CssClass="td1" DataField="SATISDIVNAME" HeaderText="Satış Mağaza" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField ItemStyle-CssClass="td1" DataField="TESLIMDIVNAME" HeaderText="Teslim Eden" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField ItemStyle-CssClass="td1" DataField="QUAN" HeaderText="Adet" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left" DataFormatString="{0:C2}" />
                        </Columns>
                    </asp:GridView>
                    <asp:HiddenField ID="hdnSelectedId" runat="server" />
                </div>
                <div class="row gx-3 gy-2 align-items-center form-group text-center">
                    <%--<asp:Button runat="server" ID="Kapat" CssClass="btn btn-outline-danger" Width="100%" Text="Kapat" OnClick="Kapat_Click" />--%>
                </div>
            </div>
        </div>
    </div>
    <div id="myModal" class="modal">
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
                <div class="table-responsive">
                        <asp:Repeater ID="Urunler" runat="server">
                            <HeaderTemplate>
                                <table id="zero_config" class="table2 table-hover table-flush" border="1">
                                    <thead>
                                        <tr>
                                            <th>Seçim</th>
                                            <th>Stok Kodu</th>
                                            <th>Stok Adı</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnOpenModal" runat="server" Width="100%"
                                            Text="SSH İşleme Devam Et" CommandArgument='<%# Eval("PROID") %>'
                                            OnClick="btnOpenModal_Click1" />
                                    </td>
                                    <td data-label="Stok Kodu"><%# Eval("PROVAL") %></td>
                                    <td data-label="Stok Adı"><%# Eval("PRONAME") %></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                                </table>               
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
            </div>
            <asp:Button runat="server" ID="Kapat" CssClass="btn btn-outline-danger"  Width="50%" Text="Kapat" OnClick="Kapat_Click" />
        </div>
    </div>
    <a href="javaScript:void();" class="back-to-top"><i class="fa fa-angle-double-up"></i></a>
    <script>
        function openModal() {
            document.getElementById("myModal").style.display = "block";
            // Arka planın kaymasını engelle
            document.body.style.overflow = 'hidden';
        }
    
        function closeModal() {
            document.getElementById("myModal").style.display = "none";
            // Arka plan kaydırmasını geri getir
            document.body.style.overflow = '';
        }
        window.onclick = function (event) {
            var modal = document.getElementById("myModal");
            if (event.target == modal) {
                modal.style.display = "none";
            }
        }
    </script>
    <style>
    .modal {
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        max-width: 90%; /* Ekranın %90'ını kaplayacak */
        max-height: 90vh; /* Yükseklik olarak %90 viewport yüksekliği */
        overflow-y: auto; /* Yüksekliği aşarsa dikey kaydırma ekle */
        background-color: white;
        border-radius: 8px;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        z-index: 1050;
    }

    </style>
    <!--End Back To Top Button-->
</asp:Content>

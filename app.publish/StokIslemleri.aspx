<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="StokIslemleri.aspx.cs" Inherits="YONB2B.StokIslemleri" %>

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
                        <div class="col-12 col-lg-6">
                            <div class="p-3">
                                <label for="marka">Marka Seçimi</label>
                                <asp:DropDownList runat="server" ID="marka" CssClass="form-control" OnSelectedIndexChanged="marka_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-12 col-lg-6">
                            <div class="p-3">
                                <label for="Search">İsimden Arama</label>
                                <asp:TextBox runat="server" ID="Search" CssClass="form-control" OnTextChanged="Search_TextChanged" AutoPostBack="true"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-12 col-lg-12">

                        <div class="card">
                            <div class="card-header">
                                Güncellenecek Ürün Listesi
                                <div class="row m-0 row-group text-center border-top border-light-3">
                                    <div class="col-12">
                                        <div class="p-3">
                                            <div class="table-responsive">
                                                <asp:Repeater ID="ProductRepeater" runat="server">
                                                    <HeaderTemplate>
                                                        <table id="zero_config" class="table table-hover table-flush" border="1">
                                                            <thead>
                                                                <tr>
                                                                    <th>Yön Stok Adı (Seç)</th>
                                                                    <th>Sitede Aktif</th>
                                                                    <th>Tanımlı Resim Adeti</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td data-label="Adı (Seç)=">
                                                                <a href='Guncelleme.aspx?proid=<%# Eval("PROID") %>&proval=<%# Eval("PROVAL") %>&proname=<%# Eval("PRONAME") %>'><%# Eval("PRONAME") %></a>
                                                            </td>
                                                            <td data-label="Aktif="><%# Eval("durum") %></td>
                                                            <td data-label="Resim Adeti="><%# Eval("resimadet") %></td>
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
        </div>
    </div>
    <script src="assets/js/index.js"></script>
</asp:Content>

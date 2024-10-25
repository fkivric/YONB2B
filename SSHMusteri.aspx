<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SSHMusteri.aspx.cs" Inherits="YONB2B.SSHMusteri" %>

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
                        Kurulum Bekleyen Müşteri Listesi
                    </div>
                    <div class="row m-0 row-group text-center border-top border-light-3">
                        <div class="col-12 col-lg-6">
                            <div class="p-3">
                                <label for="marka">Nağaza Seçimi</label>
                                <asp:DropDownList runat="server" ID="Depo" CssClass="form-control" OnSelectedIndexChanged="magaza_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-12 col-lg-6">
                            <div class="p-3">
                                <label for="Search">Müşteri Adından Arama</label>
                                <asp:TextBox runat="server" ID="Search" CssClass="form-control" OnTextChanged="Search_TextChanged" AutoPostBack="true"></asp:TextBox>
                                <input type="text" id="searchInput" runat="server" placeholder="Arama yap..." onkeyup="performSearch();" aria-controls="zero_config" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row">
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
                                                                    <th>Satış Tarihi</th>
                                                                    <th>Satış Mağaza</th>
                                                                    <th>Teslim Depo</th>
                                                                    <th>Araç</th>
                                                                    <th>Teslimatcı</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td data-label="Müşteri No"><%# Eval("CURVAL") %></td>
                                                            <td data-label="Müşteri Adı (Seç)">
                                                                <button onclick="openModal('<%# Eval("CURNAME") %>', '<%# Eval("ORDSALID") %>', '<%# Eval("CDRCURID") %>')" class="btn btn-primary">
                                                                    <%# Eval("CURNAME") %>
                                                                </button>
                                                            </td>
                                                            <%--<td data-label="Yön Stok Adı">
                                                                <a href='Guncelleme.aspx?proid=<%# Eval("PROID") %>&proval=<%# Eval("PROVAL") %>&proname=<%# Eval("PRONAME") %>'><%# Eval("PRONAME") %></a>
                                                            </td>--%>
                                                            <td data-label="Satış Tarihi"><%# Eval("ORDDATE") %></td>
                                                            <td data-label="Satış Mağaza"><%# Eval("SATISDIVNAME") %></td>
                                                            <td data-label="Teslim Depo"><%# Eval("TESLIMDIVNAME") %></td>
                                                            <td data-label="Araç"><%# Eval("DSHIPNAME") %></td>
                                                            <td data-label="Teslimatcı"><%# Eval("DCRWNAME") %></td>
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
<!-- Modal HTML -->
<div id="productModal" class="modal">
    <div class="modal-content">
        <span class="close" onclick="closeModal()">&times;</span>
        <h2 id="modalTitle"></h2>
        <p><strong>Product ID:</strong> <span id="modalProductId"></span></p>
        <p><strong>Product Value:</strong> <span id="modalProductValue"></span></p>
    </div>
</div>
<script type="text/javascript">
    function openModal(proname, productId, productValue) {
        document.getElementById("modalTitle").innerText = CURNAME;
        document.getElementById("modalProductId").innerText = ORDSALID;
        document.getElementById("modalProductValue").innerText = CDRCURID;
        document.getElementById("productModal").style.display = "block";
    }

    function closeModal() {
        document.getElementById("productModal").style.display = "none";
    }

    // Modal'ı kapatmak için tıklama olayını dinleme
    window.onclick = function (event) {
        const modal = document.getElementById("productModal");
        if (event.target === modal) {
            closeModal();
        }
    }
</script>

<!-- Modal CSS -->
<style>
    .modal {
        display: none; /* Başlangıçta gizli */
        position: fixed;
        z-index: 1;
        left: 0;
        top: 0;
        width: 100%;
        height: 100%;
        overflow: auto;
        background-color: rgb(0,0,0);
        background-color: rgba(0,0,0,0.4);
        padding-top: 60px;
    }

    .modal-content {
        background-color: #fefefe;
        margin: 5% auto;
        padding: 20px;
        border: 1px solid #888;
        width: 80%; /* Genişlik */
    }

    .close {
        color: #aaa;
        float: right;
        font-size: 28px;
        font-weight: bold;
    }

    .close:hover,
    .close:focus {
        color: black;
        text-decoration: none;
        cursor: pointer;
    }

    /* Buton stilleri */
    .btn {
        padding: 8px 12px;
        border: none;
        border-radius: 5px;
        color: white;
        background-color: #007bff;
        cursor: pointer;
        transition: background-color 0.3s;
    }

    .btn:hover {
        background-color: #0056b3;
    }
</style>
    <a href="javaScript:void();" class="back-to-top"><i class="fa fa-angle-double-up"></i></a>
    <!--End Back To Top Button-->
    <script type="text/javascript">
        function performSearch() {
            var input = document.getElementById('<%= searchInput.ClientID %>').value;
            __doPostBack('<%= searchInput.UniqueID %>', input);
        }
    </script>
</asp:Content>

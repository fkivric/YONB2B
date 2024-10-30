<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SSHMusteriMain.aspx.cs" Inherits="YONB2B.SSHMusteriMain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-12 col-lg-12">
                <div class="card">
                    <div class="card-header">
                        Mağaza Mobilya Alan Müşteri Listesi (İLK 100)
                    </div>
                    <div class="row m-0 row-group text-center border-top border-light-3">
                        <div class="col-12">
                            <div class="p-3">
                                <div class="table-responsive">
                                    <asp:Repeater ID="ProductRepeater" runat="server">
                                        <HeaderTemplate>
                                            <table id="zero_config" class="table table-hover table-flush" border="1">
                                                <thead>
                                                    <tr>
                                                        <th>Ürün No</th>
                                                        <th>Ürün Adı (Seç)</th>
                                                        <th>Satış Tarihi</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnOpenIframe" runat="server" 
                                                                Text="Aç" CommandArgument='<%# Eval("CDRID") %>' 
                                                                OnClick="btnOpenIframe_Click" />
                                                </td>
                                                <td data-label="Ürün No"><%# Eval("PROVAL") %></td>
                                                <td data-label="Ürün Adı (Seç)"><%# Eval("PRONAME") %></td>
                                                <td data-label="Satış Tarihi"><%# Eval("SALDATE") %></td>
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
                <div class="card">
                    <div class="row m-0 row-group text-center border-top border-light-3">
                        <iframe id="iframeID" src="SSHUrunislemleri.aspx?" style="width: 100%; height: 500px;"></iframe>
                    </div>
                </div>
            </div>
        </div>
        <!--End Dashboard Content-->

        <!--start overlay-->
        <div class="overlay toggle-menu"></div>
        <!--end overlay-->

    </div>
    <!-- End container-fluid-->
            <!--Start Back To Top Button-->
</asp:Content>

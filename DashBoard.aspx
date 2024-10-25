<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="YONB2B.DashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="card mt-3">
            <div class="card-content">
                <div class="card-header" style="text-align: center; font-size: xx-large;">Ürün İşlem Detay listesi</div>
                <div class="row row-group m-0">
                    <div class="col-12 col-lg-6 col-xl-4 border-light">
                        <div class="card-body">
                            <h5 runat="server" id="stoktoplam" class="text-white mb-0">9526 <span class="float-right"><i class="fa fa-shopping-cart"></i></span></h5>
                            <div class="progress my-3" style="height: 3px;">
                                <div class="progress-bar" style="width: 100%"></div>
                            </div>
                            <p class="mb-0 text-white small-font">Aktif Stoklar <span class="float-right">100% <i class="zmdi zmdi-long-arrow-up"></i></span></p>
                        </div>
                    </div>
                    <div class="col-12 col-lg-6 col-xl-4 border-light">
                        <div class="card-body">
                            <h5 runat="server" id="stokgoup" class="text-white mb-0">6200 <span class="float-right"><i class="fa fa-eye"></i></span></h5>
                            <div class="progress my-3" style="height: 3px;">
                                <div runat="server" id="grouporan" class="progress-bar" style="width: 55%"></div>
                            </div>
                            <p class="mb-0 text-white small-font">Firma Ürün Grubu <span class="float-right">+5.2% <i class="zmdi zmdi-long-arrow-up"></i></span></p>
                        </div>
                    </div>
                    <div class="col-12 col-lg-6 col-xl-4 border-light">
                        <div class="card-body">
                            <h5 runat="server" id="stokaktif" class="text-white mb-0">8323 <span class="float-right"><i class="fa fa-usd"></i></span></h5>
                            <div class="progress my-3" style="height: 3px;">
                                <div runat="server" id="aktiforan" class="progress-bar" style="width: 55%"></div>
                            </div>
                            <p class="mb-0 text-white small-font">İnternete Açık Stoklar <span runat="server" id="yukselis" class="float-right">+1.2% <i class="zmdi zmdi-long-arrow-up"></i></span></p>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-12 col-lg-12">
                <div class="card">
                    <div class="card-header">
                        Marka Bazlı İşlem Durumu
                        <div class="card-action">
                        </div>
                        <div class="table-responsive">
                            <table class="table table-hover align-items-center table-flush">
                                <thead>
                                    <tr>
                                        <th>MARKA</th>
                                        <th>TOPLAM ADET</th>
                                        <th>Web'de Yüklü Adet</th>
                                        <th>Oran</th>
                                        <th>Webde Aktif Adet</th>
                                        <th>Oran</th>
                                        <th>Resim Eşleşmesi Yapılan</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="ProductRepeater" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <a href='ProductDetail.aspx?id=<%# Eval("Markaid") %>'><%# Eval("Marka") %></a>
                                                </td>
                                                <td><%# Eval("ToplamAdet") %></td>
                                                <td><%# Eval("netsinifaktif") %></td>
                                                <td>
                                                    <div class="progress shadow" style="height: 3px;">
                                                        <div class="progress-bar" role="progressbar" style="width: <%#Eval("SinifOran")%>%"></div>
                                                    </div>
                                                </td>
                                                <td><%# Eval("netaktif") %></td>
                                                <td>
                                                    <div class="progress shadow" style="height: 3px;">
                                                        <div class="progress-bar" role="progressbar" style="width: <%#Eval("AktifOran")%>%"></div>
                                                    </div>
                                                </td>
                                                <td><%# Eval("ResimYüklenen") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
<%--                 <table class="table align-items-center table-flush table-borderless">
                  <thead>
                   <tr>
                     <th>Product</th>
                     <th>Photo</th>
                     <th>Product ID</th>
                     <th>Amount</th>
                     <th>Date</th>
                     <th>Shipping</th>
                   </tr>
                   </thead>
                   <tbody><tr>
                    <td>Iphone 5</td>
                    <td><img src="https://via.placeholder.com/110x110" class="product-img" alt="product img"></td>
                    <td>#9405822</td>
                    <td>$ 1250.00</td>
                    <td>03 Aug 2017</td>
					<td><div class="progress shadow" style="height: 3px;">
                          <div class="progress-bar" role="progressbar" style="width: 90%"></div>
                        </div></td>
                   </tr>

                   <tr>
                    <td>Earphone GL</td>
                    <td><img src="https://via.placeholder.com/110x110" class="product-img" alt="product img"></td>
                    <td>#9405820</td>
                    <td>$ 1500.00</td>
                    <td>03 Aug 2017</td>
					<td><div class="progress shadow" style="height: 3px;">
                          <div class="progress-bar" role="progressbar" style="width: 60%"></div>
                        </div></td>
                   </tr>

                   <tr>
                    <td>HD Hand Camera</td>
                    <td><img src="https://via.placeholder.com/110x110" class="product-img" alt="product img"></td>
                    <td>#9405830</td>
                    <td>$ 1400.00</td>
                    <td>03 Aug 2017</td>
					<td><div class="progress shadow" style="height: 3px;">
                          <div class="progress-bar" role="progressbar" style="width: 70%"></div>
                        </div></td>
                   </tr>

                   <tr>
                    <td>Clasic Shoes</td>
                    <td><img src="https://via.placeholder.com/110x110" class="product-img" alt="product img"></td>
                    <td>#9405825</td>
                    <td>$ 1200.00</td>
                    <td>03 Aug 2017</td>
					<td><div class="progress shadow" style="height: 3px;">
                          <div class="progress-bar" role="progressbar" style="width: 100%"></div>
                        </div></td>
                   </tr>

                   <tr>
                    <td>Hand Watch</td>
                    <td><img src="https://via.placeholder.com/110x110" class="product-img" alt="product img"></td>
                    <td>#9405840</td>
                    <td>$ 1800.00</td>
                    <td>03 Aug 2017</td>
					<td><div class="progress shadow" style="height: 3px;">
                          <div class="progress-bar" role="progressbar" style="width: 40%"></div>
                        </div></td>
                   </tr>
				   
				   <tr>
                    <td>Clasic Shoes</td>
                    <td><img src="https://via.placeholder.com/110x110" class="product-img" alt="product img"></td>
                    <td>#9405825</td>
                    <td>$ 1200.00</td>
                    <td>03 Aug 2017</td>
					<td><div class="progress shadow" style="height: 3px;">
                          <div class="progress-bar" role="progressbar" style="width: 100%"></div>
                        </div></td>
                   </tr>

                 </tbody></table>--%>
                        </div>
                    </div>
                </div>
            </div>
            <!--End Row-->
        </div>
	</div><!--End Row-->
</asp:Content>

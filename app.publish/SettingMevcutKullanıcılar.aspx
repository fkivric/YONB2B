<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SettingMevcutKullanıcılar.aspx.cs" Inherits="YONB2B.SettingMevcutKullanıcılar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- start loader -->
    <div id="pageloader-overlay" class="visible incoming">
        <div class="loader-wrapper-outer">
            <div class="loader-wrapper-inner">
                <div class="loader"></div>
            </div>
        </div>
    </div>
    <!-- end loader -->

    <!-- Start wrapper-->
    <div id="wrapper">
        <div class="clearfix"></div>
        <div class="content-wrapper">
            <div class="container-fluid">
                <div class="row mt-3">
                    <div class="col-lg-12">

                        <div class="card">
                            <div class="card-body">
                                <div class="row-mb-3">
                                    <div class="row mb-3">
                                        <label for="inputText" class="col-sm-2 ">İşlem Yapıalcak Bayi</label>
                                        <asp:DropDownList ID="Bayiler" runat="server" CssClass="form-control btn btn-outline-secondary" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="Bayiler_SelectedIndexChanged"></asp:DropDownList>
                                        <br />
                                        <label for="inputText" class="col-sm-2 ">İşlem Yapıalcak Kullanıcı</label>
                                        <asp:DropDownList ID="Users" runat="server" CssClass="form-control btn btn-outline-secondary" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="Users_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <!-- General Form Elements -->
                                    <div class="row mb-3">
                                        <label for="OFFICURNAME" class="col-sm-2 ">Kullanıcı Ad Soyad</label>
                                        <div class="col-sm-10">
                                            <input type="text" runat="server" id="OFFICURNAME" class="form-control" />
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <label for="OFFCUREMAIL" class="col-sm-2 col-form-label">E-mail</label>
                                        <div class="col-sm-10">
                                            <input type="email" runat="server" id="OFFCUREMAIL" class="form-control" placeholder="@" required />
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <label for="SOENTERKEY" class="col-sm-2">Parola</label>
                                        <div class="col-sm-10">
                                            <input type="password" runat="server" id="SOENTERKEY" class="form-control" />
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <label for="OFFCURPHONE" class="col-sm-2 col-form-label">Telefon Numası</label>
                                        <div class="col-sm-10">
                                            <input type="number" runat="server" id="OFFCURPHONE" class="form-control" maxlength="10" placeholder="5xxxxxxxxx" required />
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <label for="OFFCURGSM" class="col-sm-2 col-form-label">GSM Numası</label>
                                        <div class="col-sm-10">
                                            <input type="number" runat="server" id="OFFCURGSM" class="form-control" maxlength="10" placeholder="5xxxxxxxxx" required />
                                        </div>
                                    </div>
                                    <div class="row mb-3">
                                        <label for="OFFCURNOTES" class="col-sm-2 col-form-label">Kullanıcı Notu</label>
                                        <div class="col-sm-10">
                                            <textarea runat="server" id="OFFCURNOTES" class="form-control" style="height: 100px"></textarea>
                                        </div>
                                    </div>
                                    <fieldset class="row mb-3">
                                        <label class="col-sm-2 col-form-label">Pozisyon</label>
                                        <div class="col-sm-10">
                                            <label class="form-check-label" for="OFFCURPOSITION">Seçiniz..</label>
                                            <select runat="server" id="OFFCURPOSITION" class="form-select form-control" multiple-aria-label="multiple select example">
                                                <option selected="selected" value="YÖNETİCİ">YÖNETİCİ</option>
                                                <option value="YETKİLİ">YETKİLİ</option>
                                                <option value="SAHİBİ">FİRMA SAHİBİ</option>
                                            </select>
                                        </div>
                                    </fieldset>
                                    <div class="row mb-3">
                                        <label class="col-sm-2 col-form-label"></label>
                                        <div class="col-sm-10">
                                            <asp:Button runat="server" ID="Kaydet" CssClass="btn btn-outline-primary form-control" OnClick="Kaydet_Click" Text="Kullanıcı Güncelle" />
                                        </div>
                                    </div>
                                    <!-- End General Form Elements -->
                                </div>
                            </div>
                        </div>                    
                    </div>
                </div>
                <!--start overlay-->
                <div class="overlay toggle-menu"></div>
                <!--end overlay-->
            </div>
            <!-- End container-fluid-->
        </div>
        <!--End content-wrapper-->
        <!--Start Back To Top Button-->
        <a href="javaScript:void();" class="back-to-top"><i class="fa fa-angle-double-up"></i></a>
        <!--End Back To Top Button-->
    </div>
</asp:Content>

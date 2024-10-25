<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SettingYeniKullanıcı.aspx.cs" Inherits="YONB2B.SettingYeniKullanıcı" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function SadeceRakam(e, allowedchars) {
            var key = e.charCode == undefined ? e.keyCode : e.charCode;
            if ((/^[0-9]+$/.test(String.fromCharCode(key))) || key == 0 || key == 13 || isPassKey(key, allowedchars)) { return true; }
            else { return false; }
        }
        function isPassKey(key, allowedchars) {
            if (allowedchars != null) {
                for (var i = 0; i < allowedchars.length; i++) {
                    if (allowedchars[i] == String.fromCharCode(key))
                        return true;
                }
            }
            return false;
        }
        function SadeceRakamBlur(e, clear) {
            var nesne = e.target ? e.target : e.srcElement;
            var val = nesne.value;
            val = val.replace(/^\s+|\s+$/g, "");
            if (clear) val = val.replace(/\s{2,}/g, " ");
            nesne.value = val;

        }
    </script>
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
    <div class="content-wrapper">
        <div class="container-fluid">
            <div class="row">
                <div class="col-xl-12">
                    <div class="details">
                        <div class="card">
                            <div class="card-body">
                                <div class="row mb-3">
                                    <label for="OFFICURNAME" class="col-sm-2 ">Kullanıcı Ad</label>
                                    <div class="col-sm-10">
                                        <input type="text" runat="server" id="OFFICURNAME" class="form-control" />
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <label for="OFFICURSURNAME" class="col-sm-2 ">Kullanıcı Soyad</label>
                                    <div class="col-sm-10">
                                        <input type="text" runat="server" id="OFFICURSURNAME" class="form-control" />
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
                                        <input type="number" runat="server" id="OFFCURPHONE" class="form-control" maxlength="10" placeholder="5xxxxxxxxx" required onblur="SadeceRakam()" />
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <label for="OFFCURGSM" class="col-sm-2 col-form-label">GSM Numası</label>
                                    <div class="col-sm-10">
                                        <input type="number" runat="server" id="OFFCURGSM" class="form-control" maxlength="10" placeholder="5xxxxxxxxx" required onblur="SadeceRakam()" />
                                    </div>
                                </div>
                                <div class="row mb-3">
                                    <label for="OFFCURNOTE" class="col-sm-2 col-form-label">Kullanıcı Notu</label>
                                    <div class="col-sm-10">
                                        <textarea runat="server" id="OFFCURNOTE" class="form-control" style="height: 100px"></textarea>
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
                                        <asp:Button runat="server" ID="Kaydet" CssClass="btn btn-outline-primary form-control" OnClick="Kaydet_Click" Text="Kullanıcı Ekle" />
                                    </div>
                                </div>
                                <!-- End General Form Elements -->
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

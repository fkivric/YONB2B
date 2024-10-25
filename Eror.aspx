<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Eror.aspx.cs" Inherits="YONB2B.Eror" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="error-container">
        <h2>Bir Hata Oluştu</h2>
        <p>Üzgünüz, beklenmedik bir hata ile karşılaştık. Lütfen daha sonra tekrar deneyin.</p>
        <p><strong>Hata Detayları:</strong></p>
        <pre runat="server" id="errorDetails"></pre>
    </div>
</asp:Content>

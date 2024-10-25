<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Guncelleme.aspx.cs" Inherits="YONB2B.Guncelleme" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
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

        .profile-picture2 {
            opacity: 0.75;
            height: 400px;
            width: 100%;
            position: relative;
            overflow: hidden;
            /* default image */
            /*background-image: url("img/arac-ruhsati-500-330.jpg");*/
            background-position: center;
            background-repeat: no-repeat;
            background-size: contain;
            box-shadow: 0 8px 6px -6px black;
        }

        .file-uploader2 {
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

        .upload-icon2 {
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
        .profile-picture2:hover .upload-icon2 {
            opacity: 1;
        }

        .profile-picture3 {
            opacity: 0.75;
            height: 250px;
            width: 400px;
            position: relative;
            overflow: hidden;
            /* default image */
            /*background-image: url("img/kimlik.png");*/
            background-position: center;
            background-repeat: no-repeat;
            background-size: cover;
            box-shadow: 0 8px 6px -6px black;
        }

        .file-uploader3 {
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

        .upload-icon3 {
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
        .profile-picture3:hover .upload-icon3 {
            opacity: 1;
        }


        .profile-picture4 {
            opacity: 0.75;
            height: 250px;
            width: 400px;
            position: relative;
            overflow: hidden;
            /* default image */
            /*background-image: url("img/Ekbelge.png");*/
            background-position: center;
            background-repeat: no-repeat;
            background-size: cover;
            box-shadow: 0 8px 6px -6px black;
        }

        .file-uploader4 {
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

        .upload-icon4 {
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
        .profile-picture4:hover .upload-icon4 {
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
            object-fit: fill; /*  contain; Görseli orantılı bir şekilde boyutlandırır */
        }

        /* İsteğe bağlı: Tam ekran için iframe'yi de doldurmak istiyorsanız */
        iframe {
            width: 100%;
            height: 100%;
        }

        .Listeler {
            background-color: transparent; /* Arka planı şeffaf yapar */
            color: white; /* Yazı rengini siyah yapar */
            width: 100%;
            font-size: medium;
        }

            /* Seçili öğenin stili */
            .Listeler option:checked {
                background-color: black; /* Seçili öğenin arka plan rengi siyah */
                color: white; /* Seçili öğenin yazı rengi beyaz */
                font-size: medium;
            }

            /* Tüm seçenekler için stil */
            .Listeler option {
                background-color: transparent; /* Normal seçeneklerin arka planı şeffaf */
                color: white; /* Normal seçeneklerin yazı rengi siyah */
                width: 100%;
                font-size: medium;
            }

        .editor{
            color:black;
        }
        .ck-source-editing-area,
        .ck-editor__editable {
            min-height: calc(65vh - 250px);
        }

        .ck-editor__main {
            height: calc(65vh - 250px);
        }
    </style>

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
                    <div class="card-header text-center">
                        Güncellenecek Ürün
                    </div>
                    <div class="row m-0 row-group text-center border-top border-light-3">
                        <div class="col-12 col-lg-6">
                            <div class="p-1">
                                <label for="Search">Yön Ürün Adı</label>
                                <asp:TextBox runat="server" ID="txtValue" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                <label for="YONKODU">Yön Ürün Kodu</label>
                                <asp:TextBox runat="server" ID="YONKODU" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-12 col-lg-6">
                            <div class="p-1">
                                <label for="Search">Web Ürün Adı</label>
                                <asp:TextBox runat="server" ID="txtNewFileName" CssClass="form-control" BackColor="Black"></asp:TextBox>
                                <label for="FirmaKodu">Ürün Kodu & Barkod Kodu</label>
                                <asp:TextBox runat="server" ID="FirmaKodu" CssClass="form-control" BackColor="Black"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row m-0 row-group text-center border-top border-light-3">
                        <div class="col-12 col-lg-6">
                            <div class="p-3 text-center">
                                <label for="fileList">Eşleşmemeiş Resimler</label>
                                <asp:ListBox ID="fileList" runat="server" CssClass="Listeler" OnSelectedIndexChanged="fileList_SelectedIndexChanged" AutoPostBack="true" />
                                <div runat="server" id="panel1" visible="false">
                                    <asp:Panel ID="panelViewer" runat="server" CssClass="viewer-panel" Style="width: 100%; height: 100vh;" Visible="false">
                                        <asp:Image ID="imgViewer" runat="server" CssClass="full-image" Visible="False" />
                                        <iframe runat="server" id="iframe" src="#" frameborder="0" visible="false"></iframe>
                                        <asp:PlaceHolder ID="pdfViewerPlaceHolder" runat="server" Visible="false"></asp:PlaceHolder>
                                    </asp:Panel>
                                </div>
                                <asp:Button runat="server" ID="ResimEsle" OnClick="ResimEsle_Click" Text="Resim Seç" CssClass="btn btn-info form-control" BackColor="YellowGreen" Visible="false" />
                            </div>
                            <div runat="server" id="resim1">
                                <div class="card">
                                    <div>
                                        <div runat="server" id="Div1" class="number"></div>
                                        <div class="cardName">Yeni Resim Ekleme</div>
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
                                    <asp:Button ID="btnUpload" type="submit" Text="Yükle" runat="server" OnClick="btnUpload_Click"></asp:Button>
                                    <asp:Panel ID="frmConfirmation" Visible="False" runat="server">
                                        <asp:Label ID="lblUploadResult" runat="server"></asp:Label>
                                    </asp:Panel>
                                </div>

                            </div>
                        </div>
                        <div class="col-12 col-lg-6">
                            <div class="p-3">
                                <label for="newFileList">Atanmış Resimler</label>
                                <asp:ListBox ID="newFileList" runat="server" CssClass="Listeler" OnSelectedIndexChanged="newFileList_SelectedIndexChanged" AutoPostBack="true" />
                                <div runat="server" id="panel2" visible="false">
                                    <asp:Panel ID="panelViewer2" runat="server" CssClass="viewer-panel" Style="width: 100%; height: 100vh; overflow: hidden;" Visible="false">
                                        <asp:Image ID="imgViewer2" runat="server" CssClass="full-image" Visible="False" />
                                        <iframe runat="server" id="iframe2" src="#" frameborder="0" visible="false"></iframe>
                                        <asp:PlaceHolder ID="pdfViewerPlaceHolder2" runat="server" Visible="false"></asp:PlaceHolder>
                                    </asp:Panel>
                                </div>
                                <asp:Button runat="server" ID="ResimCıkar" OnClick="ResimCıkar_Click" Text="Seçili Resimi Çıkar" CssClass="btn btn-danger form-control" BackColor="Red" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card">
                    <div runat="server" id="onizle" class="row m-0 row-group text-center border-top border-light-3">
                        <div class="col-12 col-lg-6">
                            <label for="Aciklama">Ürün Özellikleri</label>
                            <p for="Aciklama">
                                <ul>
                                    <li style="margin-left: 0px;">Direk yazmak için önce lütfen "Font Color" dügmesi ile siyah yapınız
                                    </li>
                                    <li style="margin-left: 0px;">web sitesinden direk kodu olarak eklemek için "Source" düğmesine basın
                                    </li>
                                </ul>
                            </p>
                            <asp:TextBox ID="Aciklama" runat="server" TextMode="MultiLine" CssClass="editor"></asp:TextBox>
                            <%--<textarea runat="server" id="Aciklama" class="textarea" placeholder="Enter text ..." TextMode="MultiLine" style="width: 100%; height: 470px"></textarea>--%>
                        </div>
                        <div class="col-12 col-lg-6">
                            <p>&nbsp;</p>
                            <p>&nbsp;</p>
                            <label for="resim2">Açıklama Resmi Ekle</label>
                            <div runat="server" id="resim2">
                                <div class="card">
                                    <div>
                                        <div runat="server" id="Div3" class="number"></div>
                                        <div class="cardName"></div>
                                    </div>
                                    <div class="profile-picture2">
                                        <h1 class="upload-icon2">
                                            <i class="fa fa-plus fa-2x" aria-hidden="true"></i>
                                        </h1>
                                        <input
                                            id="oFile2"
                                            type="file"
                                            runat="server"
                                            class="file-uploader2"
                                            onchange="upload2()"
                                            accept="image/*,application/pdf" />
                                    </div>
                                    <div class="iconBox">
                                        <label id="uploaderName2"></label>
                                    </div>
                                    <asp:Button ID="btnUpload2" type="submit" Text="Yükle" runat="server" OnClick="btnUpload2_Click"></asp:Button>
                                    <asp:Panel ID="frmConfirmation2" Visible="False" runat="server">
                                        <asp:Label ID="lblUploadResult2" runat="server"></asp:Label>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:Button runat="server" ID="Kaydet" CssClass="form-control" Text="Kaydet" OnClick="Kaydet_Click" BackColor="Green" />
                </div>
                <div id="errorDiv" runat="server" class="validationDiv"></div>
            </div>
        </div>
    </div>
    <!-- Modal -->
        <div id="myModal" class="modal" tabindex="-1" role="dialog" aria-labelledby="imageModalLabel" aria-hidden="true">
            <h5>Aranacak Kelime</h5>
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="imageModalLabel">Tam Boyut Resim</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <img runat="server" id="modalImage" src="" class="img-fluid" alt="Resim" />
                    <div class="row gx-3 gy-2 align-items-center form-group text-center">
                        <asp:Button runat="server" ID="Kapat" CssClass="btn btn-outline-danger" Width="100%" Text="Kapat"/>
                    </div>
                </div>
            </div>
        </div>
        <!-- /.modal-dialog -->
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
    <script>
        function upload() {

            const fileUploadInput = document.querySelector('.file-uploader');
            const filetext = document.getElementById('uploaderName1');
            const pdf = document.getElementById("iframe2");
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
        function upload2() {

            const fileUploadInput = document.querySelector('.file-uploader2');
            const filetext = document.getElementById('uploaderName2');

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
                const profilePicture = document.querySelector('.profile-picture2');
                profilePicture.style.backgroundImage = `url('assets/images/pdf.png')`; // Adjust the path as needed
                filetext.textContent = `Seçilen PDF dosyası: ${file.name}`;
            }
            else if (image.type.includes('image')) {
                const fileReader = new FileReader();
                fileReader.readAsDataURL(image);

                fileReader.onload = (fileReaderEvent) => {
                    const profilePicture = document.querySelector('.profile-picture2');
                    profilePicture.style.backgroundImage = `url(${fileReaderEvent.target.result})`;
                    nodes.text = fileUploadInput.value;
                }
                filetext.text = fileUploadInput.files[0];

                // upload image to the server or the cloud
            }
        }
        function upload3() {

            const fileUploadInput = document.querySelector('.file-uploader3');
            const filetext = document.getElementById('uploaderName3');

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
                const profilePicture = document.querySelector('.profile-picture3');
                profilePicture.style.backgroundImage = `url('img/pdf.jpg')`; // Adjust the path as needed
                filetext.textContent = `Seçilen PDF dosyası: ${file.name}`;
            }
            else if (image.type.includes('image')) {
                const fileReader = new FileReader();
                fileReader.readAsDataURL(image);

                fileReader.onload = (fileReaderEvent) => {
                    const profilePicture = document.querySelector('.profile-picture3');
                    profilePicture.style.backgroundImage = `url(${fileReaderEvent.target.result})`;
                    nodes.text = fileUploadInput.value;
                }
                filetext.text = fileUploadInput.files[0];
            }
        }
        function upload4() {

            const fileUploadInput = document.querySelector('.file-uploader4');
            const filetext = document.getElementById('uploaderName4');

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
                const profilePicture = document.querySelector('.profile-picture4');
                profilePicture.style.backgroundImage = `url('img/pdf.jpg')`; // Adjust the path as needed
                filetext.textContent = `Seçilen PDF dosyası: ${file.name}`;
            }
            else if (image.type.includes('image')) {
                const fileReader = new FileReader();
                fileReader.readAsDataURL(image);

                fileReader.onload = (fileReaderEvent) => {
                    const profilePicture = document.querySelector('.profile-picture4');
                    profilePicture.style.backgroundImage = `url(${fileReaderEvent.target.result})`;
                    nodes.text = fileUploadInput.value;
                }
                filetext.text = fileUploadInput.files[0];
            }
            // upload image to the server or the cloud
        }
    </script>
    <script src="/CKEditor5/ckeditor.js"></script>
    <script>
        ClassicEditor
            .create(document.querySelector('.editor'), {

                toolbar: {
                    shouldNotGroupWhenFull: true
                },
                codeBlock: {
                    indentSequence: "    "
                }

            });
    </script>
</asp:Content>

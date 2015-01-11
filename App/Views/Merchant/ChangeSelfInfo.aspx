<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Edit.Master" Inherits="System.Web.Mvc.ViewPage<DAL.Merchant>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
    修改 商家
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Res/jquery.uploadify.customized/FileUploader.css" rel="stylesheet" />
    <style type="text/css">
        .hide {
            display: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>
            <input id="btnSubmit" class="a2 f2" type="button" value="修改" />
        </legend>
        <div class="bigdiv">
            <%: Html.HiddenFor(model => model.Id ) %>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Name) %>：
            </div>
            <div class="editor-field">
                <%: Html.DisplayFor(model => model.Name) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MerchantTypeId) %>：
            </div>
            <div class="editor-field">
                <%:Html.DisplayFor(model=>model.MerchantType.Name) %>
            </div>

             <div class="editor-label">
                <%: Html.LabelFor(model => model.RegisterCode) %>：
            </div>
            <div class="editor-field">
                  <%: Html.DisplayFor(model => model.RegisterCode) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Contacts) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Contacts) %>
                <%: Html.ValidationMessageFor(model => model.Contacts) %>
            </div>
             <div class="editor-label">
                <%: Html.LabelFor(model => model.Cellphone) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Cellphone) %>
                <%: Html.ValidationMessageFor(model => model.Cellphone) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Telephone) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Telephone) %>
                  <%: Html.ValidationMessageFor(model => model.Telephone) %>
            </div>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.SiteUrl) %>：
            </div>
            <div class="editor-field">
                  <%: Html.EditorFor(model => model.SiteUrl) %>
                  <%: Html.ValidationMessageFor(model => model.SiteUrl) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Address) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Address) %>
                <%: Html.ValidationMessageFor(model => model.Address) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.ComprehensiveStar) %>：
            </div>
            <div class="editor-field">
                <%: Html.DisplayFor(model => model.ComprehensiveStar) %>
            </div>
            <br style="clear: both;" />
            <div class="editor-label">
                <label for="fileMerchantLogoUploader">商家Logo：</label>
            </div>
            <div class="editor-field">
                <input type="text" value="" id="txtFileName" class="text-box single-line valid" style="width: 165px;" readonly="readonly" />
                <%:Html.TextBoxFor(m=>m.Logo,
                    new{ @id="fileMerchantLogo", @type="file", @readonly="readonly"}) %>
            </div>
            <div class="<%:ViewBag.HaveLogo?"":"hide" %>" style="display: block; clear: both; text-align: center;">
                <img src="<%:"/Merchant/Logo/"+Model.Id %>" alt="商家Logo" />
            </div>
            <br style="clear: both;" />
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Description) %>：
            </div>
            <div class="textarea-box">
                <%: Html.TextAreaFor(model => model.Description) %>
                <%: Html.ValidationMessageFor(model => model.Description) %>
            </div>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="scriptBlock" ContentPlaceHolderID="ScriptPlace" runat="server">
    <script src="../../Res/jquery.uploadify-v2.1.4/swfobject.js" type="text/javascript"></script>
    <script src="../../Res/jquery.uploadify-v2.1.4/jquery.uploadify.v2.1.4.1.js"></script>
    <script src="../../Scripts/custom/merchant/FileUploader.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.serializeObject.js"></script>
    <script type="text/javascript">
        uploader.init('fileMerchantLogo', '/Merchant/ChangeSelfInfo');
        $('#fileMerchantLogoUploader').addClass('fileUploader');
    </script>
</asp:Content>

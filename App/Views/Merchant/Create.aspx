<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Create.Master" Inherits="System.Web.Mvc.ViewPage<DAL.Merchant>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<%@ Import Namespace="Models" %>

<asp:Content ID="HeadHolder" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Res/jquery.uploadify.customized/FileUploader.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="CurentPlace" runat="server">
    创建 商家
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>
            <input id="btnSubmit" class="a2 f2" type="button" value="创建" />
            <input class="a2 f2" type="button" onclick="BackList('Merchant')" value="返 回" />
        </legend>
        <div class="bigdiv">
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Name) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Name) %>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MerchantTypeId) %>：
            </div>
            <div class="editor-field">
                <%:Html.DropDownList("MerchantTypeId",(SelectList)ViewBag.MerchantTypeList,"请选择") %><span style="color:red; font-size:10px;"><br />(必选)</span>
            </div>
                 <div class="editor-label">
                <%: Html.LabelFor(model => model.RegisterCode) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.RegisterCode) %>
                <%: Html.ValidationMessageFor(model => model.RegisterCode) %>
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
                <%: Html.LabelFor(model => model.QQ) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.QQ) %>
                <%: Html.ValidationMessageFor(model => model.QQ) %>
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
                <%: Html.EditorFor(model => model.ComprehensiveStar) %>
                <%: Html.ValidationMessageFor(model => model.ComprehensiveStar) %>
            </div>
             <div class="editor-label">
                <%: Html.LabelFor(model => model.Balance) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Balance) %>
                <%: Html.ValidationMessageFor(model => model.Balance) %>
            </div>
            <div class="editor-label">
                <label for="fileMerchantLogoUploader">商家Logo：</label>
            </div>
            <div class="editor-field">
                <input type="text" value="" id="txtFileName" class="text-box single-line valid" style="width: 165px;" readonly="readonly" />
                <%:Html.TextBoxFor(m=>m.Logo,
                    new{ @id="fileMerchantLogo", @type="file", @readonly="readonly"}) %>
            </div>
            <br style="clear: both;" />
            <div class="editor-label">
                <a class="anUnderLine" onclick="showModalOnly('SysPersonId','../../SysPerson');">
                    <%: Html.LabelFor(model => model.SysPersonId) %>
                </a>：
            </div>
            <div id="checkSysPersonId">
                <% 
                    if (Model != null && !string.IsNullOrWhiteSpace(Model.SysPersonId))
                    {
                        foreach (var item10 in Model.SysPersonId.Split('^'))
                        {
                            string[] it = item10.Split('&');
                            if (it != null && it.Length == 2 && !string.IsNullOrWhiteSpace(it[0]) && !string.IsNullOrWhiteSpace(it[1]))
                            {                        
                %>
                <table id="<%: item10 %>"
                    class="deleteStyle">
                    <tr>
                        <td>
                            <img alt="删除" title="点击删除" onclick="deleteTable('<%: item10  %>','SysPersonId');" src="../../../Images/deleteimge.png" />
                        </td>
                        <td>
                            <%: it[1] %>
                        </td>
                    </tr>
                </table>
                <%}
                        }
                    }%>
                <%: Html.HiddenFor(model => model.SysPersonId) %>
            </div>
        </div>
          <br style="clear: both;" />
        <div class="editor-label">
                    <%: Html.LabelFor(model => model.IsVisible) %>：
                </div>
        <div class="editor-field">
                <%: Html.DropDownListFor(model => model.IsVisible,new[]{new SelectListItem(){Text="是",Value="1"},
                    new SelectListItem(){Text="否",Value="0"}},"请选择")%>
                <%: Html.ValidationMessageFor(model => model.IsVisible) %>
            </div>
            <br style="clear: both;" />
          <br style="clear: both;" />
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Description) %>：
            </div>
            <div class="textarea-box">
                <%: Html.TextAreaFor(model => model.Description) %>
                <%: Html.ValidationMessageFor(model => model.Description) %>
            </div>
    </fieldset>
</asp:Content>
<asp:Content ID="ScriptHolder" ContentPlaceHolderID="ScriptPlace" runat="server">
    <script src="../../Scripts/jquery.serializeObject.js"></script>
    <script src="../../Res/jquery.uploadify-v2.1.4/swfobject.js" type="text/javascript"></script>
    <script src="../../Res/jquery.uploadify-v2.1.4/jquery.uploadify.v2.1.4.1.js"></script>
    <script src="../../Scripts/custom/merchant/FileUploader.js" type="text/javascript"></script>

    <script type="text/javascript">
        uploader.init('fileMerchantLogo', '/Merchant/Create');
        $('#fileMerchantLogoUploader').addClass('fileUploader');
    </script>
</asp:Content>


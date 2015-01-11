<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Edit.Master" Inherits="System.Web.Mvc.ViewPage<DAL.Merchant>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
    修改 商家
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Res/jquery.uploadify.customized/FileUploader.css" rel="stylesheet" />
    <style type="text/css">
        .hide
        {
            display: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>
            <input id="btnSubmit" class="a2 f2" type="button" value="修改" />
            <%if (ViewBag.Merchant == null){%>
                 <input class="a2 f2" type="button" onclick="BackList('Merchant')" value="返回" />
            <%} %>

        </legend>
        <div class="bigdiv">
            <%: Html.HiddenFor(model => model.Id ) %>
            <%: Html.HiddenFor(model => model.OldRegisterCode ) %>
            <%if (ViewBag.Merchant!=null)
              {%>
             <%: Html.HiddenFor(model => model.RegisterCode ) %>
         
             <%: Html.HiddenFor(model => model.ParentId) %>
            <%: Html.HiddenFor(model => model.MerchantTypeId ) %>
            <%: Html.HiddenFor(model => model.ComprehensiveStar ) %>
            <%: Html.HiddenFor(model => model.Balance ) %>
           <%: Html.HiddenFor(model => model.Name ) %>
            <%: Html.HiddenFor(model => model.SysPersonId ) %>
             <% } %>

           
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Name) %>：
            </div>
            <%if (ViewBag.Merchant != null)
              {%>
                <div class="editor-field">
                    <%:Html.DisplayFor(model=>model.Name) %>
                </div>
            <%} %>
            <%else
              {%>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Name) %>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </div>
            <% } %>

            <div class="editor-label">
                <%: Html.LabelFor(model => model.MerchantTypeId) %>：
            </div>
            <%if (ViewBag.Merchant != null)
              {%>
            <div class="editor-field">
                <%:Html.DisplayFor(model=>model.MerchantType.Name) %>
            </div>
            <%} %>
            <%else
              {%>
            <div class="editor-field">
                <%:Html.DropDownListFor(model=>model.MerchantTypeId,(SelectList)ViewBag.MerchantTypeList,"请选择") %><span style="color:red; font-size:10px;"><br />(必选)</span>
            </div>
        <% } %>
        <div class="editor-label">
            <%: Html.LabelFor(model => model.RegisterCode) %>：
        </div>
        <%if (ViewBag.Merchant != null)
          {%>
        <div class="editor-field">
            <%: Html.DisplayFor(model => model.RegisterCode) %>
        </div>
        <%} %>
        <%else
              {%>
            <%if (!string.IsNullOrEmpty(Model.RegisterCode))
              {%>
                <div class="editor-field">
                    <%: Html.DisplayFor(model => model.RegisterCode) %>
                </div>
            <%} %>
            <%else
              { %>
                <div class="editor-field">
                    <%: Html.EditorFor(model => model.RegisterCode)%>
                    <%: Html.ValidationMessageFor(model => model.RegisterCode) %>
                </div>
            <%} %>
        <%} %>

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
        <%if (ViewBag.Merchant!=null)
          {%>
            <div class="editor-field">
                <%: Html.DisplayFor(model => model.ComprehensiveStar) %>
            </div>
         <% } %>
        <%else
          {%>
           <div class="editor-field">
            <%: Html.EditorFor(model => model.ComprehensiveStar) %>
            <%: Html.ValidationMessageFor(model => model.ComprehensiveStar) %>
        </div>
         <%  }%>
        <div class="editor-label">
            <%: Html.LabelFor(model => model.Balance) %>：
        </div>
        <%if (ViewBag.Merchant!=null)
          {%>
                <div class="editor-field">
                <%: Html.DisplayFor(model => model.Balance) %>
                </div>
         <% } %>
        <%else
          {%>
           <div class="editor-field">
            <%: Html.EditorFor(model => model.Balance) %>
            <%: Html.ValidationMessageFor(model => model.Balance) %>
        </div>
         <%  }%>

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
        <%if(ViewBag.Merchant==null) {%>
                <%if (!string.IsNullOrEmpty(Model.SysPersonId))
          {%>
        <div class="editor-label">
            <%: Html.LabelFor(model => model.SysPersonId) %>
        </div>
        <%: Html.DisplayFor(model => model.SysPerson.SingleOrDefault(o=>o.Id==model.SysPersonId).Name) %>
        <%} %>
        <%else{ %>
                <div class="editor-label">
            <a class="anUnderLine" onclick="showModalOnly('SysPersonId','../../SysPerson');">
                <%: Html.LabelFor(model => model.SysPersonId) %>
            </a>：
        </div>
        <div class="editor-field">
            <div id="checkSysPersonId">
                <% string ids8 = string.Empty;
                   if (Model != null)
                   {
                       foreach (var item8 in Model.SysPerson)
                       {
                           string item81 = string.Empty;
                           item81 += item8.Id;
                           if (ids8.Length > 0)
                           {
                               ids8 += "^" + item81;
                           }
                           else
                           {
                               ids8 += item81;
                           }
                %>
                <table id="<%: item81 %>"
                    class="deleteStyle">
                    <tr>
                        <td>
                            <img alt="删除" title="点击删除" onclick="deleteTable('<%: item81 %>','SysPersonId');" src="../../../Images/deleteimge.png" />
                        </td>
                        <td>
                            <%: item8.Name %>
                        </td>
                    </tr>
                </table>
                <%}
                       }%>
                <input type="hidden" value="<%= ids8 %>" name="SysPersonIdOld" id="SysPersonIdOld" />
                <input type="hidden" value="<%= ids8 %>" name="SysPersonId" id="SysPersonId" />
            </div>
        </div>
        <%} %>
        <%} %>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.IsVisible) %>：
            </div>
<%--            <div class="editor-field">
                <%: Html.EditorFor(model => model.IsVisible)%>
                <%: Html.ValidationMessageFor(model => model.IsVisible)%>
            </div>--%>
             <div class="editor-field">
                <%: Html.DropDownListFor(model => model.IsVisible,new[]{new SelectListItem(){Text="是",Value="1"},
                    new SelectListItem(){Text="否",Value="0"}},"请选择")%>
                <%: Html.ValidationMessageFor(model => model.IsVisible) %>
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
    <%--<script src="../../Res/jquery.uploadify-v2.1.4/jquery.uploadify.v2.1.4.js" type="text/javascript"></script>--%>
    <script src="../../Res/jquery.uploadify-v2.1.4/jquery.uploadify.v2.1.4.1.js"></script>
    <script src="../../Scripts/custom/merchant/FileUploader.js" type="text/javascript"></script>
    <%--<script src="../../Res/easyui/jquery.easyui.loading.1.0.js"></script>--%>
    <script src="../../Scripts/jquery.serializeObject.js"></script>
    <script type="text/javascript">
        uploader.init('fileMerchantLogo', '/Merchant/Edit');
        $('#fileMerchantLogoUploader').addClass('fileUploader');
    </script>
</asp:Content>

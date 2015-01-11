<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Edit.Master" Inherits="System.Web.Mvc.ViewPage<DAL.RegisterCode>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="App.Models" %>
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
    修改 注册码
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>
            <input class="a2 f2" type="submit" value="修改" />
            <input class="a2 f2" type="button" onclick="BackList('MerchantRegisterCode')" value="返回" />
        </legend>
        <div class="bigdiv">
            <%: Html.HiddenFor(model => model.Id ) %>
             <%:Html.HiddenFor(model=>model.IsUsed) %>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Value) %>：
                
            </div>
            <div class="editor-field">
                <%: Html.DisplayFor(model => model.Value) %>
            </div>
             <br style="clear: both;" />
             <div class="editor-label">
                <%: Html.LabelFor(model => model.IsValid) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.IsValid) %>
                <%: Html.ValidationMessageFor(model => model.IsValid) %>
            </div>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

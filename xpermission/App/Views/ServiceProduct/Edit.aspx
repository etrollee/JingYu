<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Edit.Master" Inherits="System.Web.Mvc.ViewPage<DAL.ServiceProduct>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
    修改 服务产品
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>
            <input class="a2 f2" type="submit" value="修改" />
            <input class="a2 f2" type="button" onclick="BackList('ServiceProduct')" value="返回" />
        </legend>
        <div class="bigdiv">
            <%: Html.HiddenFor(model => model.Id ) %>   
            <%:Html.HiddenFor(model=>model.MerchantId) %>  
            <%if(ViewBag.Merchant!=null) {%>
             <%:Html.HiddenFor(model=>model.Worth) %>  
            <%} %>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Name) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Name) %>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </div>
             <div class="editor-label">
                <%: Html.LabelFor(model => model.Star) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Star) %>
                <%: Html.ValidationMessageFor(model => model.Star) %>
            </div>
               <%if(ViewBag.Merchant==null) {%>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Worth) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Worth) %>
                <%: Html.ValidationMessageFor(model => model.Worth) %>
            </div>
            <%} %>
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
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    
   
</asp:Content>


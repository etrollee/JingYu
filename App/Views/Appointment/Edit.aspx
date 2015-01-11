<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Edit.Master" Inherits="System.Web.Mvc.ViewPage<DAL.Appointment>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
    修改 会员预约
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>
            <input class="a2 f2" type="submit" value="修改" />
            <input class="a2 f2" type="button" onclick="BackList('Appointment')" value="返回" />
        </legend>
        <div class="bigdiv">
            <%: Html.HiddenFor(model => model.Id ) %>   
            <%:Html.HiddenFor(model=>model.MemberId) %>  
            <%:Html.HiddenFor(model=>model.CreateTime) %>  
            <%:Html.HiddenFor(model=>model.ServiceProductId) %>  
            <div class="editor-label">
                <%: Html.LabelFor(model => model.MemberName) %>：
            </div>
            <div class="editor-field">
                <%: Html.DisplayFor(model => model.MemberName) %>
            </div>
             <div class="editor-label">
                <%: Html.LabelFor(model => model.ServiceProductId) %>：
            </div>
            <div class="editor-field">
                <%: Html.DisplayFor(model => model.ServiceProduct.Name) %>
            </div>
             <div class="editor-label">
                <%: Html.LabelFor(model => model.CreateTime) %>：
            </div>
            <div class="editor-field">
                <%: Html.DisplayFor(model => model.CreateTime) %>
            </div>
             <div class="editor-label">
                <%: Html.LabelFor(model => model.State) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.State) %>
                <%:Html.ValidationMessageFor(model=>model.State) %>
            </div>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Details.Master" Inherits="System.Web.Mvc.ViewPage<DAL.Member>" %>

<%@ Import Namespace="Common" %>

<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
    详细 会员
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <fieldset>
        <legend>
            <input class="a2 f2" type="button" onclick="window.location.href = 'javascript:history.go(-1)';" value="返回" />
        </legend>
        <div class="bigdiv">

            <div class="display-label">
                <%: Html.LabelFor(model => model.Name) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.Name) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Code) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.Code) %>
            </div>
            <%if (ViewBag.IsVisible == true)
              { %>
            <div class="display-label">
                <%: Html.LabelFor(model => model.RegisterCode)%>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.RegisterCode)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Area)%>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.Area)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Regtime)%>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.Regtime)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.RegisteredCapital)%>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.RegisteredCapital)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Contacts)%>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.Contacts)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Phone)%>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.Phone)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.RegisteredAddress)%>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.RegisteredAddress)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.LegalRepresentative)%>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.LegalRepresentative)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.RegisteredCellPhone)%>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.RegisteredCellPhone)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.SelfDefineOne)%>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.SelfDefineOne)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.SelfDefineTwo)%>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.SelfDefineTwo)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.VIP)%>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.VIP)%>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.IsValid)%>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.IsValid)%>
            </div>
            <br style="clear: both;" />
            <div class="display-label">
                <%: Html.LabelFor(model => model.Remark)%>：
            </div>
            <div class="textarea-box">
                <%: Html.DisplayFor(model => model.Remark)%>
            </div>
            <%} %>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Details.Master" Inherits="System.Web.Mvc.ViewPage<DAL.Merchant>" %>

<%@ Import Namespace="Common" %>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .hide {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
    详细 商家
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
                <%: Html.LabelFor(model => model.Contacts) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.Contacts) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Telephone) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.Telephone) %>
            </div>

            <div class="display-label">
                <%: Html.LabelFor(model => model.Cellphone) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.Cellphone) %>
            </div>
              <div class="display-label">
                <%: Html.LabelFor(model => model.QQ) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.QQ) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.SiteUrl) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.SiteUrl) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.Address) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.Address) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.ComprehensiveStar) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.ComprehensiveStar) %>
            </div>
             <div class="display-label">
                <%: Html.LabelFor(model => model.Balance) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.Balance) %>
            </div>
            <div class="display-label">
                <%: Html.LabelFor(model => model.MerchantTypeId) %>：
            </div>
            <div class="display-field">
                <%: Html.DisplayFor(model => model.MerchantType.Name) %>
            </div>
            <div class="display-label">
                <%:Html.LabelFor(model=>model.SysPersonId) %>:
            </div>
            <div class="display-field">
                <% string ids24 = string.Empty;
                   foreach (var item23 in Model.SysPerson)
                   {
                       ids24 += item23.Name;
                       ids24 += " , ";
                %>
                <%}%>
                <div class="display-field">
                    <%= ids24 %>
                </div>
            </div>
            
            <br style="clear: both;" />
            <div class="display-label">
                <%: Html.LabelFor(model => model.Description) %>：
            </div>
            <div class="textarea-box">
                <%: Html.DisplayFor(model => model.Description) %>
            </div>
            <br style="clear: both;" />
            <div class="display-label">
                <label>商家Logo：</label>
            </div>
            <div class="display-field" style="height:auto;">
                <img src="<%:"/Merchant/Logo/"+Model.Id %>" alt=""
                    class="<%:((bool)ViewBag.HasLogo)?"":"hide" %>" />
            </div>
        </div>
    </fieldset>
</asp:Content>



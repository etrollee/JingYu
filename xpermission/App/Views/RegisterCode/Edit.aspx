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
            <input class="a2 f2" type="button" onclick="BackList('RegisterCode')" value="返回" />
        </legend>
        <div class="bigdiv">
            <%: Html.HiddenFor(model => model.Id ) %>
            <%:Html.HiddenFor(model=>model.MerchantId) %>
            <%:Html.HiddenFor(model=>model.MemberId) %>
             <%:Html.HiddenFor(model=>model.Value) %>
             <%:Html.HiddenFor(model=>model.IsUsed) %>
             <%:Html.HiddenFor(model=>model.CreateTime) %>
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
            <div class="editor-label">
                <%: Html.LabelFor(model => model.IsDistribution) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.IsDistribution) %>
                <%: Html.ValidationMessageFor(model => model.IsDistribution) %>
            </div>
            <div class="editor-label">
                <a class="anUnderLine" onclick="showModalOnly('BelongMerchantId','../../Merchant');">
                    <%: Html.LabelFor(model => model.BelongMerchantId) %>
                </a>：
            </div>
            <div class="editor-field">
                <div id="checkBelongMerchantId">
                    <% if (Model != null)
                       {
                           if (!string.IsNullOrWhiteSpace(Model.BelongMerchantId))
                           {%>
                    <table id="<%: Model.BelongMerchantId %>" class="deleteStyle">
                        <tr>
                            <td>
                                <img alt="删除" title="点击删除" src="../../../Images/deleteimge.png" onclick="deleteTable('<%: Model.BelongMerchantId %>','BelongMerchantId');" />
                            </td>
                            <td>
                                <%: Model.BelongMerchant%>
                            </td>
                        </tr>
                    </table>
                    <%}
                       }%>
                </div>
                <%: Html.HiddenFor(model => model.BelongMerchantId)%>
            </div>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

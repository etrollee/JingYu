<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Create.Master" Inherits="System.Web.Mvc.ViewPage<DAL.ServiceProduct>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CurentPlace" runat="server">
        创建 服务产品
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <fieldset>
        <legend>
            <input class="a2 f2" type="submit" value="创建" />
            <input class="a2 f2" type="button" onclick="BackList('ServiceProduct')" value="返回" />
        </legend>
        <div class="bigdiv">
           
            <%if(ViewBag.Merchant==null) {%>
                        <div class="editor-label">
                <a class="anUnderLine" onclick="showModalOnly('MerchantId','../../Merchant');">
                    <%: Html.LabelFor(model => model.MerchantId) %>
                </a>：
            </div>
            <div id="checkMerchantId">
                <% 
                    if (Model != null && !string.IsNullOrWhiteSpace(Model.MerchantId))
                    {
                        foreach (var item10 in Model.MerchantId.Split('^'))
                        {
                            string[] it = item10.Split('&');
                            if (it != null && it.Length == 2 && !string.IsNullOrWhiteSpace(it[0]) && !string.IsNullOrWhiteSpace(it[1]))
                            {                        
                %>
                <table id="<%: item10 %>"
                    class="deleteStyle">
                    <tr>
                        <td>
                            <img alt="删除" title="点击删除" onclick="deleteTable('<%: item10  %>','MerchantId');" src="../../../Images/deleteimge.png" />
                        </td>
                        <td>
                            <%: it[1] %>
                        </td>
                    </tr>
                </table>
                <%}
                        }
                    }%>
                <%: Html.HiddenFor(model => model.MerchantId) %>
            </div>
             <br style="clear: both;" />
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
                         <br style="clear: both;" />
             <div class="editor-label">
                <%: Html.LabelFor(model => model.Worth) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Worth) %>
                <%: Html.ValidationMessageFor(model => model.Worth) %>
            </div>
            <%}%>
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


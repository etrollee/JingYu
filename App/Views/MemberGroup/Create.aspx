<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Create.Master" Inherits="System.Web.Mvc.ViewPage<DAL.MemberGroup>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CurentPlace" runat="server">
        创建 会员分组
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <fieldset>
        <legend>
            <input class="a2 f2" type="submit" value="创建" />
            <input class="a2 f2" type="button" onclick="BackList('MemberGroup')" value="返回" />
        </legend>
        <div class="bigdiv">
                 
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Name) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Name) %>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </div>
            <br style="clear: both;" />
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Description) %>：
            </div>
            <div class="textarea-box">
                <%: Html.TextAreaFor(model => model.Description) %>
                <%: Html.ValidationMessageFor(model => model.Description) %>
            </div>
               <div class="editor-label">
            <a class="anUnderLine" onclick="showModalMany('MemberId','../../Member');">
                <%: Html.LabelFor(model => model.MemberId) %>
            </a>：
        </div>
        <div class="editor-field">
            <div id="checkMemberId">
                <% 
                    if (Model != null && !string.IsNullOrWhiteSpace(Model.MemberId))
                {
                    foreach (var item10 in Model.MemberId.Split('^'))
                   {
                       string[] it = item10.Split('&');
                        if (it != null && it.Length == 2 && !string.IsNullOrWhiteSpace(it[0]) && !string.IsNullOrWhiteSpace(it[1]))
                        {                        
                %>
                <table id="<%: item10 %>"
                    class="deleteStyle">
                    <tr>
                        <td>
                            <img  alt="删除" title="点击删除" onclick="deleteTable('<%: item10  %>','MemberId');"  src="../../../Images/deleteimge.png" />
                        </td>
                        <td>
                            <%: it[1] %>
                        </td>
                    </tr>
                </table>
                <%}}}%>
               <%: Html.HiddenFor(model => model.MemberId) %>
            </div>
        </div>
        </div>
    </fieldset>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


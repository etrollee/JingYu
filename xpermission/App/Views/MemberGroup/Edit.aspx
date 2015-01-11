<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Edit.Master" Inherits="System.Web.Mvc.ViewPage<DAL.MemberGroup>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
    修改 会员分组
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>
            <input class="a2 f2" type="submit" value="修改" />
            <input class="a2 f2" type="button" onclick="BackList('MemberGroup')" value="返回" />
        </legend>
        <div class="bigdiv">
            <%: Html.HiddenFor(model => model.Id ) %>   
            <%:Html.HiddenFor(model=>model.CreatePersonId) %>  
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
                    <% string ids8 = string.Empty;
                       if (Model != null)
                       {
                           foreach (var item8 in Model.Member)
                           {
                               string item81 = string.Empty;
                               item81 += item8.Id + "&" + item8.Name;
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
                                <img alt="删除" title="点击删除" onclick="deleteTable('<%: item81 %>','MemberId');" src="../../../Images/deleteimge.png" />
                            </td>
                            <td>
                                <%: item8.Name %>
                            </td>
                        </tr>
                    </table>
                    <%}
                }%>
                    <input type="hidden" value="<%= ids8 %>" name="MemberIdOld" id="MemberIdOld" />
                    <input type="hidden" value="<%= ids8 %>" name="MemberId" id="MemberId" />
                </div>
            </div>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    
   
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Details.Master"Inherits="System.Web.Mvc.ViewPage<DAL.Informations>" %>
<%@ Import Namespace="Common" %>
 
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
      详细 信息
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <fieldset>
        <legend>
            <input class="a2 f2" type="button"  onclick="window.location.href = 'javascript:history.go(-1)';"  value="返回" />   
        </legend>
        <div class="bigdiv">
                  
                <div class="display-label">
                      <%: Html.LabelFor(model => model.Title) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Title) %>
                </div>
              <div class="display-label">
                      <%: Html.LabelFor(model => model.Type) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Type) %>
                </div>
                <div class="display-label">
                      <%: Html.LabelFor(model => model.TimeLimit) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.TimeLimit) %>
                </div>
             <div class="display-label">
                      <%: Html.LabelFor(model => model.CreatePersonId) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.SysPerson.Name) %>
                </div>
                <div class="display-label">
                      <%: Html.LabelFor(model => model.CreateTime) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.CreateTime) %>
                </div>
                <br style="clear: both;" />
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Content) %>：
                </div>
                <div class="textarea-box">
                    <%: Html.Raw(Model.Content) %>                  
                </div>   
              <br style="clear: both;" />     
                <div class="display-label">
                      <%: Html.LabelFor(model => model.FeedbackTemplateId) %>：
                </div>
                <div class="display-field">            
                    <% string ids8 = string.Empty;
                       foreach (var item8 in Model.FeedbackTemplate)
                       {
                           ids8 += item8.Name;
                           ids8 += " , ";
                    %>               
                    <%}%>
                
                        <%= ids8 %>   
                     
                </div>
            <div class="display-label">
                      <%: Html.LabelFor(model => model.MemberId) %>：
                </div>
                <div class="display-field">            
                    <% string ids9 = string.Empty;
                       foreach (var item8 in Model.Member)
                       {
                           ids9 += item8.Name;
                           ids9 += " , ";
                    %>               
                    <%}%>
                
                        <%= ids9 %>   
                     
                </div>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
 
</asp:Content>


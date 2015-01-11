<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Details.Master"Inherits="System.Web.Mvc.ViewPage<DAL.FeedbackTemplate>" %>
<%@ Import Namespace="Common" %>
 
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
      详细 反馈模板
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <fieldset>
        <legend>
            <input class="a2 f2" type="button"  onclick="window.location.href = 'javascript:history.go(-1)';"  value="返回" />   
        </legend>
        <div class="bigdiv">
                  
                <div class="display-label">
                      <%: Html.LabelFor(model => model.Name) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Name) %>
                </div>
                <br style="clear: both;" />
                <div class="display-label">
                    <%: Html.LabelFor(model => model.Description) %>：
                </div>
                <div class="textarea-box">
                    <%: Html.TextAreaFor(model => model.Description, new {  @readonly=true}) %>                  
                </div>        
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
 
</asp:Content>


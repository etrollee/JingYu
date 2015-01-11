<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Details.Master" Inherits="System.Web.Mvc.ViewPage<DAL.RegisterCode>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
      详细 注册码
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <fieldset>
        <legend>
            <input class="a2 f2" type="button"  onclick="window.location.href = 'javascript:history.go(-1)';"  value="返回" />   
        </legend>
        <div class="bigdiv">
                  
                <div class="display-label">
                      <%: Html.LabelFor(model => model.Value) %>：
                </div>
                <div class="display-field">
                    <%: Html.DisplayFor(model => model.Value) %>
                </div>
                <div class="display-label">
                    <%: Html.LabelFor(model => model.IsValid) %>：
                </div>
                 <div class="display-field">
                    <%: Html.DisplayFor(model => model.IsValid) %>
                </div>  
                <div class="display-label">
                    <%: Html.LabelFor(model => model.IsUsed) %>：
                </div>
                 <div class="display-field">
                    <%: Html.DisplayFor(model => model.IsUsed) %>
                </div>  
                            <div class="display-label">
                    <%: Html.LabelFor(model => model.IsDistribution) %>：
                </div>
                 <div class="display-field">
                    <%: Html.DisplayFor(model => model.IsDistribution) %>
                </div>  
                                        <div class="display-label">
                    <%: Html.LabelFor(model => model.MerchantId) %>：
                </div>
                 <div class="display-field">
                    <%: Html.DisplayFor(model => model.MerchantName) %>
                </div>  
                                                    <div class="display-label">
                    <%: Html.LabelFor(model => model.MemberId) %>：
                </div>
                 <div class="display-field">
                    <%: Html.DisplayFor(model => model.MemberName) %>
                </div>      
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
 
</asp:Content>

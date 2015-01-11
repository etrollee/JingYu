<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Edit.Master" Inherits="System.Web.Mvc.ViewPage<DAL.Member>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
    修改 会员
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>
            <input class="a2 f2" type="submit" value="修改" />
            <input class="a2 f2" type="button" onclick="BackList('Member')" value="返回" />
        </legend>
        <div class="bigdiv">
            <%: Html.HiddenFor(model => model.Id ) %>
             <%: Html.HiddenFor(model => model.OldRegisterCode ) %>
             <%: Html.HiddenFor(model => model.IsValid ) %>  
            <%: Html.HiddenFor(model => model.VIP ) %>
             <%: Html.HiddenFor(model => model.IsVisible ) %> 
             <%: Html.HiddenFor(model => model.LogOnTimes ) %>
             <%: Html.HiddenFor(model => model.CreatePersonId ) %>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Name) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Name) %>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Code) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Code) %>
                <%: Html.ValidationMessageFor(model => model.Code) %>
            </div>
            <%if (ViewBag.IsVisible == true)
              { %>
            <%if (!string.IsNullOrEmpty(Model.RegisterCode))
              {%>
                   <div class="editor-label">
                <%: Html.LabelFor(model => model.RegisterCode)%>：
            </div>
            <div class="editor-field">
                  <%: Html.DisplayFor(model => model.RegisterCode)%>
            </div>
              <%} %>
            <%else
              { %>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.RegisterCode)%>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.RegisterCode)%>
                <%: Html.ValidationMessageFor(model => model.RegisterCode)%>
            </div>
            <%} %>
           
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Area)%>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Area)%>
                <%: Html.ValidationMessageFor(model => model.Area)%>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Regtime)%>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Regtime)%>
                <%: Html.ValidationMessageFor(model => model.Regtime)%><span style="color:red; font-size:10px;"><br />(成立时间格式如：2013-01-01)</span>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.RegisteredCapital)%>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.RegisteredCapital)%>
                <%: Html.ValidationMessageFor(model => model.RegisteredCapital)%>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Contacts)%>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Contacts)%>
                <%: Html.ValidationMessageFor(model => model.Contacts)%>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Phone)%>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Phone)%>
                <%: Html.ValidationMessageFor(model => model.Phone)%>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.RegisteredAddress)%>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.RegisteredAddress)%>
                <%: Html.ValidationMessageFor(model => model.RegisteredAddress)%>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.LegalRepresentative)%>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.LegalRepresentative)%>
                <%: Html.ValidationMessageFor(model => model.LegalRepresentative)%>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.RegisteredCellPhone)%>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.RegisteredCellPhone)%>
                <%: Html.ValidationMessageFor(model => model.RegisteredCellPhone)%>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.SelfDefineOne)%>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.SelfDefineOne)%>
                <%: Html.ValidationMessageFor(model => model.SelfDefineOne)%>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(model => model.SelfDefineTwo)%>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.SelfDefineTwo)%>
                <%: Html.ValidationMessageFor(model => model.SelfDefineTwo)%>
            </div>
            <br style="clear: both;" />
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Remark)%>：
            </div>
            <div class="textarea-box">
                <%: Html.TextAreaFor(model => model.Remark)%>
                <%: Html.ValidationMessageFor(model => model.Remark)%>
            </div>
            <%} %>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


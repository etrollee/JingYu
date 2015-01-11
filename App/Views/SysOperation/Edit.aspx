﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Edit.Master" Inherits="System.Web.Mvc.ViewPage<DAL.SysOperation>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
    修改 操作
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>
            <input class="a2 f2" type="submit" value="修改" />
            <input class="a2 f2" type="button" onclick="BackList('SysOperation')" value="返回" />
        </legend>
        <div class="bigdiv">
            <%: Html.HiddenFor(model => model.Id ) %>     
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Name) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Name) %>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </div>     
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Function) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Function) %>
                <%: Html.ValidationMessageFor(model => model.Function) %>
            </div>     
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Iconic) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Iconic) %>
                <%: Html.ValidationMessageFor(model => model.Iconic) %>
            </div>     
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Sort) %>：
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Sort, new {  onkeyup = "isInt(this)", @class="text-box single-line" })%>
                <%: Html.ValidationMessageFor(model => model.Sort) %>
            </div>
            <br style="clear: both;" />
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Remark) %>：
            </div>
            <div class="textarea-box">
                <%: Html.TextAreaFor(model => model.Remark) %>
                <%: Html.ValidationMessageFor(model => model.Remark) %>
            </div>        
            <div class="editor-label">
                <%: Html.LabelFor(model => model.State) %>：
            </div>
            <div class="editor-field">
                <%=Html.DropDownListFor(model => model.State,Models.SysFieldModels.GetSysField("SysOperation","State"),"请选择")%>  
            </div><%: Html.HiddenFor(model => model.CreateTime ) %><%: Html.HiddenFor(model => model.CreatePerson ) %>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    
  
</asp:Content>


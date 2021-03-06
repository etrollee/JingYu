﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Create.Master" Inherits="System.Web.Mvc.ViewPage<DAL.FileUploader>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CurentPlace" runat="server">
      创建 附件
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
        <legend>
            <input class="a2 f2" type="submit" value="创建" />
            <input class="a2 f2" type="button" onclick="BackList('FileUploader')" value="返回" />
        </legend>
        <div class="bigdiv">
                 
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Name) %>
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Name) %>
                <%: Html.ValidationMessageFor(model => model.Name) %>
            </div>     
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Path) %>
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Path) %>
                <%: Html.ValidationMessageFor(model => model.Path) %>
            </div>
            <br style="clear: both;" />
            <div class="editor-label">
                <%: Html.LabelFor(model => model.FullPath) %>
            </div>
            <div class="textarea-box">
                <%: Html.TextAreaFor(model => model.FullPath) %>
                <%: Html.ValidationMessageFor(model => model.FullPath) %>
            </div>     
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Suffix) %>
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.Suffix) %>
                <%: Html.ValidationMessageFor(model => model.Suffix) %>
            </div>     
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Size) %>
            </div>
            <div class="editor-field">
                <%: Html.TextBoxFor(model => model.Size, new {  onkeyup = "isInt(this)", @class="text-box single-line" })%>
                <%: Html.ValidationMessageFor(model => model.Size) %>
            </div>
            <br style="clear: both;" />
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Remark) %>
            </div>
            <div class="textarea-box">
                <%: Html.TextAreaFor(model => model.Remark) %>
                <%: Html.ValidationMessageFor(model => model.Remark) %>
            </div>     
            <div class="editor-label">
                <%: Html.LabelFor(model => model.State) %>
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(model => model.State) %>
                <%: Html.ValidationMessageFor(model => model.State) %>
            </div>
        </div>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    
    <script type="text/javascript">

        $(function () {
            

        });
              

    </script>
</asp:Content>


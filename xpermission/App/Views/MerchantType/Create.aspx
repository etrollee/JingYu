﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Create.Master" Inherits="System.Web.Mvc.ViewPage<DAL.MerchantType>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CurentPlace" runat="server">
        创建 商家类型
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <fieldset>
        <legend>
            <input class="a2 f2" type="submit" value="创建" />
            <input class="a2 f2" type="button" onclick="BackList('MerchantType')" value="返回" />
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
        </div>
    </fieldset>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


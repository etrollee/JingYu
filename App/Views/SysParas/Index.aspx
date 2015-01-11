<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<DAL.SysParas>" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>系统参数</title>
    <script src="<%: Url.Content("~/Scripts/jquery.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>"
        type="text/javascript"></script>
        <style type="text/css">
        body
        {
            font-size: 12px;
            font-family: 微软雅黑,新宋体;
        
        }
        fieldset
        {
            margin: 0 auto;
            padding: 9px;
            border: 1px solid #CCC;
            width: 387px;
        }
        
        legend
        {
            font-size: 1.1em;
            font-weight: 600;
            padding: 2px 4px 8px 4px;
        }
        input[type="text"], input[type="password"]
        {
            width: 200px;
            border: 1px solid #CCC;
        }
        .editor-label
        {
            margin: 1em 0 0 0;
        }
        
        .editor-field
        {
            margin: 0.5em 0 0 0;
        }
        
        .field-validation-error, .validation-summary-errors
        {
            color: #ff0000;
        }
        .mbx
        {
            height: 23px;
            font-weight: bold;
            color: #9b6835;
            border-bottom: 1px #999 dotted;
            padding: 12px 19px 3px 19px;
            margin: 4px;
            background: url(../Images/direction.gif) 0 center no-repeat;
        }
        .buttonOn
        {
            background: url(../images/buttonOn_login.gif) left top no-repeat;
            height: 21px;
            width: 109px;
            text-align: center;
            border: 0px;
            margin: 9px,150px,0px,222px;
        }
    </style>
</head>
<body>
    <div class="mbx">
        系统参数
    </div>
    <% using (Html.BeginForm("Edit","SysParas"))
       { %>
    <div style="margin-left:20px;">
         <%: Html.HiddenFor(model => model.Id ) %>
            <div class="editor-label">
                <%: Html.LabelFor(m => m.WelcomeInfo) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(m => m.WelcomeInfo) %>
                <%: Html.ValidationMessageFor(m => m.WelcomeInfo) %>
            </div>
            <div class="editor-label">
                <%: Html.LabelFor(m => m.DeductMoney) %>：
            </div>
            <div class="editor-field">
                <%: Html.EditorFor(m => m.DeductMoney) %>
                <%: Html.ValidationMessageFor(m => m.DeductMoney) %>
            </div>
            <p>
                <input class="buttonOn" type="submit" value="修改" />
            </p>
            <p>
                <%: Html.ValidationSummary(true) %>
            </p>
    </div>
    <% } %>
</body>
</html>


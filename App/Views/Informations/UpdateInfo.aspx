<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Create.Master" Inherits="System.Web.Mvc.ViewPage<DAL.Informations>" %>


<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<%@ Import Namespace="Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CurentPlace" runat="server">
    创建 升级信息
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset style="width: 900px;">
        <legend>
            <input id="btnSubmit" class="a2 f2" type="button" value="创建" />
        </legend>
        <div class="bigdiv">
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Title) %>：
            </div>
            <div class="editor-field" style="width: auto;">
                <%: Html.EditorFor(model => model.Title) %>
                <%: Html.ValidationMessageFor(model => model.Title) %>
            </div>
             <br style="clear: both;" />
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Content) %>：
            </div>
            <div class="textarea-box" style="margin-left: 125px;">
                <%: Html.TextArea("Content", new { Style="width:750px;height:160px;"})%>
                <%: Html.ValidationMessageFor(model => model.Content) %>
            </div>
        </div>
    </fieldset>
      <script charset="utf-8" src="../../Res/kindeditor/kindeditor-min.js"></script>
    <script charset="utf-8" src="../../Res/kindeditor/lang/zh_CN.js"></script>
    <script src="../../Scripts/jquery.serializeObject.js"></script>
        <script>
            var editor;
            KindEditor.ready(function (K) {
                editor = K.create('textarea[name="Content"]', {
                    resizeType: 1,
                    allowPreviewEmoticons: false,
                    allowImageUpload: false,
                    items: [
                        'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline',
                        'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'insertorderedlist',
                        'insertunorderedlist', '|', 'emoticons', 'image', 'link']

                });

            });
    </script>
    <script type="text/javascript">
        function submitCreate() {
            var form = $('form').first();
            var formValid = form.validate().form();
            var data = form.serializeObject();
            data.Content = editor.html();
            if (formValid) {
                $.ajax({
                    type: 'POST',
                    url: '/Informations/UpdateInfo',
                    data: data,
                    success: function (response) {
                        if (response.indexOf('成功') == -1) {
                            $.messager.alert('操作提示', response, 'error');
                        }
                        else {
                            $.messager.defaults.ok = '继续';
                            $.messager.defaults.cancel = '返回';
                            $.messager.confirm('操作提示', response, function (r) {
                                if (!r) {
                                    window.location.href = 'javascript:history.go(-1)';
                                }
                            });
                        }
                    },
                    complete: function (xhr, ts)
                    { },
                    error: function (xhr, ts, err) {

                    }
                });
            }
        }
        $(document).ready(function () {
            $('#btnSubmit').click(function () { submitCreate(); });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
 

</asp:Content>

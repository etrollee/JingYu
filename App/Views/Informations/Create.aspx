<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Create.Master" Inherits="System.Web.Mvc.ViewPage<DAL.Informations>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<%@ Import Namespace="Models" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CurentPlace" runat="server">
    创建 信息
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset style="width: 900px;">
        <legend>
            <input id="btnSubmit" class="a2 f2" type="button" value="创建" />
            <input class="a2 f2" type="button" onclick="BackList('Informations')" value="返 回" />
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
                <%:Html.LabelFor(model=>model.Type) %>
            </div>
              <div class="editor-field" style="width: auto;">
                <%: Html.DropDownListFor(model => model.Type,new[]{new SelectListItem(){Text="普通信息",Value="1"},
                new SelectListItem(){Text="政务信息",Value="2"}}) %>
                <%: Html.ValidationMessageFor(model => model.Type) %>
            </div>
             <br style="clear: both;" />
            <div class="editor-label">
                <%: Html.LabelFor(model => model.Content) %>：
            </div>
            <div class="textarea-box" style="margin-left: 125px;">
                <%: Html.TextArea("Content", new { Style="width:750px;height:160px;"})%>
                <%: Html.ValidationMessageFor(model => model.Content) %>
            </div>
            <br style="clear: both;" />
            <div class="editor-label">
                <%: Html.LabelFor(model => model.TimeLimit) %>：&nbsp;
            </div>
            <div class="textarea-box">
                <%: Html.EditorFor(model => model.TimeLimit)%>
                <%: Html.ValidationMessageFor(model => model.TimeLimit) %>
            </div>
            <br style="clear: both;" />
                <div class="editor-label">
                <a class="anUnderLine" onclick="showModalMany('FeedbackTemplateId','../../FeedbackTemplate');">
                    <%: Html.LabelFor(model => model.FeedbackTemplateId) %>
                </a>：
            </div>
            <div class="editor-field" style="height:auto;">
                <div id="checkFeedbackTemplateId">
                    <% 
                        int count = 0;
                        if (Model != null && !string.IsNullOrWhiteSpace(Model.FeedbackTemplateId))
                        {
                            foreach (var item25 in Model.FeedbackTemplateId.Split('^'))
                            {
                                count++;
                                string[] it = item25.Split('&');
                                if (it != null && it.Length == 2 && !string.IsNullOrWhiteSpace(it[0]) && !string.IsNullOrWhiteSpace(it[1]))
                                {                        
                    %>

                    <table id="<%:item25 %>"
                        class="deleteStyle">
                        <tr>
                            <td>
                                <img alt="删除" title="点击删除" onclick="deleteTable('<%: item25  %>','FeedbackTemplateId');" src="../../../Images/deleteimge.png" />
                            </td>
                            <td>
                                <%: it[1] %>
                            </td>
                        </tr>
                    </table>
                    <%}
                   }
                }%>
                    <%: Html.HiddenFor(model => model.FeedbackTemplateId) %>
                </div>
            </div>

            <div class="editor-label">
                <a class="anUnderLine" onclick="showModalMany('MemberId','../../Member');">
                    <%: Html.LabelFor(model => model.MemberId) %>
                </a>：
            </div>
            <div class="editor-field"  style="height:auto;">
                <div id="checkMemberId">
                    <% 
                        if (Model != null && !string.IsNullOrWhiteSpace(Model.MemberId))
                        {
                            foreach (var item10 in Model.MemberId.Split('^'))
                            {
                                string[] it = item10.Split('&');
                                if (it != null && it.Length == 2 && !string.IsNullOrWhiteSpace(it[0]) && !string.IsNullOrWhiteSpace(it[1]))
                                {
                                                   
                    %>
                    <table id="<%: item10 %>"
                        class="deleteStyle">
                        <tr>
                            <td>
                                <img alt="删除" title="点击删除" onclick="deleteTable('<%: item10  %>','MemberId');" src="../../../Images/deleteimge.png" />
                            </td>
                            <td>
                                <%: it[1] %>
                            </td>
                        </tr>
                    </table>
                    <%}
                   }
                }%>
                    <%: Html.HiddenFor(model => model.MemberId) %>
                </div>
            </div>
            <br style="clear:both;" />
<%--                        <%if (ViewBag.Power != null)
                          {%>--%>
            <div class="editor-label">
                <a class="anUnderLine" onclick="showModalMany('MerchantId','../../Merchant');">
                    <%: Html.LabelFor(model => model.MerchantId)%>
                </a>：
            </div>
            <div class="editor-field"  style="height:auto;">
                <div id="checkMerchantId">
                    <% 
                              if (Model != null && !string.IsNullOrWhiteSpace(Model.MerchantId))
                              {
                                  foreach (var item11 in Model.MerchantId.Split('^'))
                                  {
                                      string[] it = item11.Split('&');
                                      if (it != null && it.Length == 2 && !string.IsNullOrWhiteSpace(it[0]) && !string.IsNullOrWhiteSpace(it[1]))
                                      {
                                                   
                    %>
                    <table id="<%: item11 %>"
                        class="deleteStyle">
                        <tr>
                            <td>
                                <img alt="删除" title="点击删除" onclick="deleteTable('<%: item11  %>','MerchantId');" src="../../../Images/deleteimge.png" />
                            </td>
                            <td>
                                <%: it[1]%>
                            </td>
                        </tr>
                    </table>
                    <%}
                                  }
                              }%>
                    <%: Html.HiddenFor(model => model.MerchantId)%>
                </div>
            </div>
          <%--  <%} %>--%>
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
                    url: '/Informations/Create',
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

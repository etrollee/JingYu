<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Edit.Master" Inherits="System.Web.Mvc.ViewPage<DAL.Informations>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="Models" %>
<%@ Import Namespace="App.Models" %>
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
    修改 信息
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset style="width: 900px;">
        <legend>
            <input id="btnSubmit" class="a2 f2" type="button" value="修改" />
            <input class="a2 f2" type="button" onclick="BackList('Informations')" value="返回" />
        </legend>
        <div class="bigdiv">
            <%: Html.HiddenFor(model => model.Id ) %>
            <%: Html.HiddenFor(model => model.CreatePersonId ) %>
            <%: Html.HiddenFor(model => model.CreateTime ) %>
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
                <%: Html.TextAreaFor(model=>model.Content, new { Style="width:750px;height:160px;"})%>
                <%: Html.ValidationMessageFor(model => model.Content) %>
            </div>
            <br style="clear: both;" />
            <div class="editor-label">
                <%: Html.LabelFor(model => model.TimeLimit) %>：&nbsp;
            </div>
            <div class="textarea-box">
                <%: Html.EditorFor(model => model.TimeLimit) %>
                <%: Html.ValidationMessageFor(model => model.TimeLimit) %>
            </div>
            <br style="clear: both;" />
            <div class="editor-label">
                <a class="anUnderLine" onclick="showModalMany('FeedbackTemplateId','../../FeedbackTemplate');">
                    <%: Html.LabelFor(model => model.FeedbackTemplateId) %>
                </a>：
            </div>
            <div class="editor-field" style="height: auto;">
                <div id="checkFeedbackTemplateId">
                    <% string ids23 = string.Empty;
                       if (Model != null)
                       {
                           foreach (var item23 in Model.FeedbackTemplate)
                           {
                               string item231 = string.Empty;
                               item231 += item23.Id + "&" + item23.Name;
                               if (ids23.Length > 0)
                               {
                                   ids23 += "^" + item231;
                               }
                               else
                               {
                                   ids23 += item231;
                               }
                    %>
                    <table id="<%: item231 %>" class="deleteStyle">
                        <tr>
                            <td>
                                <img alt="删除" title="点击删除" onclick="deleteTable('<%: item231 %>','FeedbackTemplateId');" src="../../../Images/deleteimge.png" />
                            </td>
                            <td>
                                <%: item23.Name %>
                            </td>
                        </tr>
                    </table>
                    <%}
                       }%>
                    <input type="hidden" value="<%= ids23 %>" name="FeedbackTemplateIdOld" id="FeedbackTemplateIdOld" />
                    <input type="hidden" value="<%= ids23 %>" name="FeedbackTemplateId" id="FeedbackTemplateId" />
                </div>
            </div>

            <div class="editor-label">
                <a class="anUnderLine" onclick="showModalMany('MemberId','../../Member');">
                    <%: Html.LabelFor(model => model.MemberId) %>
                </a>：
            </div>
            <div class="editor-field" style="height: auto;">
                <div id="checkMemberId">
                    <% string ids24 = string.Empty;
                       if (Model != null)
                       {
                           foreach (var item23 in Model.Member)
                           {
                               string item232 = string.Empty;
                               item232 += item23.Id + "&" + item23.Name;
                               if (ids23.Length > 0)
                               {
                                   ids24 += "^" + item232;
                               }
                               else
                               {
                                   ids24 += item232;
                               }
                    %>
                    <table id="<%: item232 %>" class="deleteStyle">
                        <tr>
                            <td>
                                <img alt="删除" title="点击删除" onclick="deleteTable('<%: item232 %>','MemberId');" src="../../../Images/deleteimge.png" />
                            </td>
                            <td>
                                <%: item23.Name %>
                            </td>
                        </tr>
                    </table>
                    <%}
                       }%>
                    <input type="hidden" value="<%= ids24 %>" name="MemberIdOld" id="MemberIdOld" />
                    <input type="hidden" value="<%= ids24 %>" name="MemberId" id="MemberId" />
                </div>
            </div>
            <br style="clear: both;" />
<%--            <%if (ViewBag.Power != null)
              {%>--%>
                 <div class="editor-label">
                <a class="anUnderLine" onclick="showModalMany('MerchantId','../../Merchant');">
                    <%: Html.LabelFor(model => model.MerchantId)%>
                </a>：
            </div>
                                                                                                                                                    <div class="editor-field" style="height: auto;">
                <div id="checkMerchantId">
                    <% string ids25 = string.Empty;
                       if (Model != null)
                       {
                           foreach (var item23 in Model.Merchant)
                           {
                               string item233 = string.Empty;
                               item233 += item23.Id + "&" + item23.Name;
                               if (ids25.Length > 0)
                               {
                                   ids25 += "^" + item233;
                               }
                               else
                               {
                                   ids25 += item233;
                               }
                    %>
                    <table id="<%: item233 %>" class="deleteStyle">
                        <tr>
                            <td>
                                <img alt="删除" title="点击删除" onclick="deleteTable('<%: item233 %>','MerchantId');" src="../../../Images/deleteimge.png" />
                            </td>
                            <td>
                                <%: item23.Name%>
                            </td>
                        </tr>
                    </table>
                    <%}
                       }%>
                    <input type="hidden" value="<%= ids25 %>" name="MerchantIdOld" id="MerchantIdOld" />
                    <input type="hidden" value="<%= ids25 %>" name="MerchantId" id="MerchantId" />
                </div>
            </div>
       <%--     <%} %>--%>
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

        function submitEdit() {
            var form = $('form').first();
            var formValid = form.validate().form();//
            var data = form.serializeObject();
            data.Content = editor.html();
            if (formValid) {
                $.ajax({
                    type: 'POST',
                    url: '/Informations/Edit',
                    data: data,
                    success: function (response)
                    {
                        if (response.indexOf('成功') == -1)
                        {
                            $.messager.alert('操作提示', response, 'error');
                        }
                        else
                        {
                            $.messager.defaults.ok = '继续';
                            $.messager.defaults.cancel = '返回';
                            $.messager.confirm('操作提示', response, function (r)
                            {
                                if (!r)
                                {
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
            $('#btnSubmit').click(function () { submitEdit(); });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

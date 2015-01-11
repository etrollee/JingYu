<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Create.Master" Inherits="System.Web.Mvc.ViewPage<DAL.RegisterCode>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="CurentPlace" runat="server">
    批量生成 注册码
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset>
       <%-- <legend>
            <input id="btnSubmit" class="a2 f2" type="button" value="批量生成" />
            <input class="a2 f2" type="button" onclick="BackList('RegisterCode')" value="返 回" />
        </legend>--%>
         <legend>
            <input class="a2 f2" type="submit" value="创建" />
            <input class="a2 f2" type="button" onclick="BackList('RegisterCode')" value="返回" />
        </legend>
        <div class="bigvid">
            <div class="editor-label">
                <%:Html.LabelFor(model=>model.Count) %>
            </div>
            <div class="editor-field" style="width: auto;">
                <%: Html.EditorFor(model => model.Count) %>
                <%: Html.ValidationMessageFor(model => model.Count) %>
            </div>
                       <div class="editor-label">
                <a class="anUnderLine" onclick="showModalOnly('MerchantId','../../Merchant');">
                   <%-- <%: Html.LabelFor(model => model.MerchantId) %>--%>
                    所属商家
                </a>：
            </div>
            <div id="checkMerchantId">
                <% 
                    if (Model != null && !string.IsNullOrWhiteSpace(Model.MerchantId))
                    {
                        foreach (var item10 in Model.MerchantId.Split('^'))
                        {
                            string[] it = item10.Split('&');
                            if (it != null && it.Length == 2 && !string.IsNullOrWhiteSpace(it[0]) && !string.IsNullOrWhiteSpace(it[1]))
                            {                        
                %>
                <table id="<%: item10 %>"
                    class="deleteStyle">
                    <tr>
                        <td>
                            <img alt="删除" title="点击删除" onclick="deleteTable('<%: item10  %>','MerchantId');" src="../../../Images/deleteimge.png" />
                        </td>
                        <td>
                            <%: it[1] %>
                        </td>
                    </tr>
                </table>
                <%}
                        }
                    }%>
                <%: Html.HiddenFor(model => model.MerchantId) %>
            </div>
        </div>
    </fieldset>
     <script src="../../Scripts/jquery.serializeObject.js"></script>
<%--    <script type="text/javascript">
        $(function () {
            $("#btnSubmit").click(function () {
                var form = $('form').first();
                var formValid = form.validate().form();
                var data = form.serializeObject();
                if (formValid) {
                    $.ajax({
                        type: 'POST',
                        url: '/RegisterCode/Create',
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
            });

        });
    </script>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="ScriptPlace" runat="server">
</asp:Content>

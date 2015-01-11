<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Index.Master" Inherits="System.Web.Mvc.ViewPage<DAL.InformationFeedbackReport>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    信息
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset style="width: auto">
        <legend>
            <input class="a2 f2" type="button" onclick="window.location.href = 'javascript:history.go(-1)';" value="返回" />
        </legend>
        <div class="bigdiv">
            <table style="border: solid 0px #000000">
                <tr>
                    <td style="border: solid 0px #000000">
                        <div style="margin-top: 10px; color: blue;">基本信息</div>
                    </td>
                </tr>
                <tr>
                    <td style="border: solid 0px #000000">标题：
                        <span style="color: #79D52E;"><%:Model.Title %></span>
                    </td>
                    <td style="border: solid 0px #000000; margin-left: 30px;">&nbsp;&nbsp;&nbsp;&nbsp;发送时间：
                        <span style="color: #79D52E;"><%:Model.SendTime %></span>
                    </td>
                </tr>
                <tr>
                    <td style="border: solid 0px #000000">发送会员总数：
                        <span style="color: #79D52E;"><%:Model.ReceiveMemberAmount %></span>
                    </td>
                    <td style="border: solid 0px #000000">&nbsp;&nbsp;&nbsp;&nbsp;已反馈会员数量：
                        <span style="color: #79D52E;"><%:Model.FeedbackMemberAmount %></span>
                    </td>
                     <td style="border: solid 0px #000000">未反馈会员数量：
                        <span style="color: #79D52E;"><%:Model.ReceiveMemberAmount-Model.FeedbackMemberAmount %></span>
                    </td>
                </tr>
<%--                <%if (ViewBag.Power != null)
                  { %>--%>
                <tr>
                    <td style="border: solid 0px #000000">
                        发送商家总数： <span style="color: #79D52E;">
                            <%:Model.ReceiveMerchantAmount %></span>
                    </td>
                    <td style="border: solid 0px #000000">
                        &nbsp;&nbsp;&nbsp;&nbsp;已反馈商家数量： <span style="color: #79D52E;">
                            <%:Model.FeedbackMerchantAmount%></span>
                    </td>
                    <td style="border: solid 0px #000000">
                        未反馈商家数量： <span style="color: #79D52E;">
                            <%:Model.ReceiveMerchantAmount - Model.FeedbackMerchantAmount%></span>
                    </td>
                </tr>
          <%--      <%} %>--%>

                <tr>
                    <td style="border: solid 0px #000000">
                        <div style="margin-top: 10px; color: blue;">会员信息统计</div>
                    </td>
                </tr>
                <tr>
                    <%foreach (var item in Model.FeedbackTemplateStatisticsList)
                      {%>
                    <td style="border: solid 0px #000000">
                        <%:item.FeedbackTemplateName %>
                        : <span style="color: #79D52E;">
                            <%:item.FeedbackTemplateFeedbackAmount %></span>
                    </td>
                    <%} %>
                </tr>
<%--                <%if (ViewBag.Power != null)
                  { %>--%>
                <tr>
                    <td style="border: solid 0px #000000">
                        <div style="margin-top: 10px; color: blue;">
                            商家信息统计</div>
                    </td>
                </tr>
                <tr>
                    <%foreach (var item in Model.FeedbackTemplateMerchantStatisticsList)
                      {%>
                    <td style="border: solid 0px #000000">
                        <%:item.FeedbackTemplateName %>
                        : <span style="color: #79D52E;">
                            <%:item.FeedbackTemplateFeedbackAmount %></span>
                    </td>
                    <%} %>
                </tr>
        <%--        <%} %>--%>
            </table>

        </div>
        <div>
            <div id="fackbackMember" style="margin-top: 5px;">
                <div id="fackbackMemberTitle" style="color: blue;">已反馈会员</div>
                <div id="fackbackMemberContent">
                        <table id="FeedbackMemberFlexigridData">
                        </table>
                </div>
            </div>
            <div style="clear: both;"></div>
            <div id="unfackbackMember" style="margin-top: 10px;">
                <div id="unfackbackMemberTitle" style="color: blue;">未反馈会员</div>
                <div id="unfackbackMemberContent">
                        <table id="UnFeedbackMemberFlexigridData">
                        </table>
                </div>
            </div>
<%--            <%if (ViewBag.Power != null)
              {%>--%>
            </div>      
                  <div id="fackbackMerchant" style="margin-top: 5px;">
                <div id="fackbackMerchantTitle" style="color: blue;">
                    已反馈商家</div>
                <div id="fackbackMerchantContent">
                    <table id="FeedbackMerchantFlexigridData">
                    </table>
                </div>
            </div>
             <div style="clear: both;">
            <div id="unfackbackMerchant" style="margin-top: 10px;">
                <div id="unfackbackMerchantTitle" style="color: blue;">
                    未反馈商家</div>
                <div id="unfackbackMerchantContent">
                    <table id="UnFeedbackMerchantFlexigridData">
                    </table>
                </div>
            </div>
       <%--     <%} %>--%>


        </div>
    </fieldset>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .Content
        {
            width: auto;
            border-collapse: collapse;
            border: 1px #000 solid;
            font-size: 14px;
            text-align: center;
            margin-top: 8px;
        }

            .Content th
            {
                background-color: #9DEE9F;
            }

        .Content th, td
        {
            font-size: 14px;
            border: 1px solid #000;
        }
    </style>

     <script type="text/javascript" language="javascript">
         $(function () {
             //未反馈的会员列表
             $('#UnFeedbackMemberFlexigridData').datagrid({
                 title: ' ',
                 iconCls: 'icon-site',
                 width: 'auto',
                 height: '300',
                 nowrap: false,
                 striped: true,
                 collapsible: true,
                 url: '../Informations/GetUnFeedbackMemberData', //获取数据的url
                 sortName: 'Id',
                 sortOrder: 'desc',
                 idField: 'Id',
                 singleSelect: true,/*禁用多选*/
                 toolbar: [
                 ],
                 columns:
                 [[
                     { field: 'MemberName', title: '会员名称', width: 200 },
                     { field: 'Contacts', title: '联系人', width: 100 },
                    { field: 'Phone', title: '手机号码', width: 400 },
                ]],
                pagination: true,
                rownumbers: true
             });
            //反馈的会员列表
             $('#FeedbackMemberFlexigridData').datagrid({
                 title: ' ',
                 iconCls: 'icon-site',
                 width: 'auto',
                 height: '300',
                 nowrap: false,
                 striped: true,
                 collapsible: true,
                 url: '../Informations/GetFeedbackMemberData', //获取数据的url
                 sortName: 'Id',
                 sortOrder: 'desc',
                 idField: 'Id',
                 singleSelect: true,/*禁用多选*/
                 toolbar: [
                 ],
                 columns:
                 [[
                     { field: 'MemberName', title: '会员名称', width: 200 },
                     { field: 'Contacts', title: '联系人', width: 100 },
                    { field: 'Phone', title: '手机号码', width:100 },
                    { field: 'FeedbackTemplateName', title: '反馈选项', width: 200 },
                    { field: 'FeedbackContent', title: '客户反馈信息', width: 200 },
                    { field: 'CreateTime', title: '反馈时间', width: 200 },
                 ]],
                 pagination: true,
                 rownumbers: true
             });

             //未反馈的商家列表
             $('#UnFeedbackMerchantFlexigridData').datagrid({
                 title: ' ',
                 iconCls: 'icon-site',
                 width: 'auto',
                 height: '300',
                 nowrap: false,
                 striped: true,
                 collapsible: true,
                 url: '../Informations/GetUnFeedbackMerchantData', //获取数据的url
                 sortName: 'Id',
                 sortOrder: 'desc',
                 idField: 'Id',
                 singleSelect: true, /*禁用多选*/
                 toolbar: [
                 ],
                 columns:
                 [[
                     { field: 'MemberName', title: '商家名称', width: 200 },
                     { field: 'Contacts', title: '联系人', width: 100 },
                    { field: 'Phone', title: '手机号码', width: 400 },
                ]],
                 pagination: true,
                 rownumbers: true
             });
             //反馈的商家列表
             $('#FeedbackMerchantFlexigridData').datagrid({
                 title: ' ',
                 iconCls: 'icon-site',
                 width: 'auto',
                 height: '300',
                 nowrap: false,
                 striped: true,
                 collapsible: true,
                 url: '../Informations/GetFeedbackMerchantData', //获取数据的url
                 sortName: 'Id',
                 sortOrder: 'desc',
                 idField: 'Id',
                 singleSelect: true, /*禁用多选*/
                 toolbar: [
                 ],
                 columns:
                 [[
                     { field: 'MemberName', title: '商家名称', width: 200 },
                     { field: 'Contacts', title: '联系人', width: 100 },
                    { field: 'Phone', title: '手机号码', width: 100 },
                    { field: 'FeedbackTemplateName', title: '反馈选项', width: 200 },
                    { field: 'FeedbackContent', title: '客户反馈信息', width: 200 },
                    { field: 'CreateTime', title: '反馈时间', width: 200 },
                 ]],
                 pagination: true,
                 rownumbers: true
             });
         });
         </script>
</asp:Content>

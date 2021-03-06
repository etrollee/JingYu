﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/ManyTree.Master" Inherits="System.Web.Mvc.ViewPage<DAL.SysOperation>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    操作
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CurentPlace" runat="server">
     当前 操作 是：<%= ViewData["myname"] %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="dialo" title="操作">
        <fieldset style="margin: 10px 3px 0px 3px">
            <legend>
                <input class="submitcss" type="submit" onclick="Save(this)" value="保存" />
                <input class="submitcss" type="button" onclick="BackList('SysOperation')" value="返回" />
            </legend>
            <ul style="margin-top: 6px;" id="myTree">
            </ul>
        </fieldset>
    </div>
    <div>
        <% string ids11 = string.Empty;
           if (Model != null)
           {
               foreach (var item11 in Model.SysMenu)
               {
                   string item111 = string.Empty;
                   item111 += item11.Id;
                   if (ids11.Length > 0)
                   {
                       ids11 += "," + item111;
                   }
                   else
                   {
                       ids11 += item111;
                   }
        %>
        <%}
           }%>
        <input type="hidden" value="<%= ids11 %>" name="SysMenuIdOld" id="SysMenuIdOld" />
        <input type="hidden" value="" name="SysMenuId" id="SysMenuId" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        fieldset
        {
            width: 710px;
            padding: 1em;
            border: 1px solid #CCC;
        }
        legend
        {
            font-size: 1.1em;
            font-weight: 600;
            padding: 2px 4px 8px 4px;
        }
    </style>
    <script type="text/javascript">
        function Save(btn) {
            var nodes = $('#myTree').tree('getChecked');
            var s = '';
            for (var i = 0; i < nodes.length; i++) {
                if (s != '') s += ',';
                s += nodes[i].id;
            }
            $('#SysMenuId').val(s);
        }
        $(function () {

            $('#SysMenuId').val("");
            easyloader.locale = "zh_CN"; // 本地化设置
            //easyloader.theme = "gray"; // 设置主题
            using(['messager', 'tree'], function () {
                $('#myTree').tree({
                    checkbox: true,
                    url: '../../SysMenuTree/GetTree',
                    onLoadSuccess: function (node, data) {
                        if (data != "") {
                            var oldId = $('#SysMenuIdOld').val();
                            var menuids = oldId.split(',');
                            for (i = 0; i < menuids.length; i++) {
                                var nodeid = $("#myTree").tree("find", menuids[i]);
                                if (nodeid) {
                                    $(this).tree('check', nodeid.target);
                                }
                            }
                             
                        }
                    }


                });
            });
        });
    </script>
</asp:Content>


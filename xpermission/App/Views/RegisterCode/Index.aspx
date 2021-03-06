﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Index.Master" Inherits="System.Web.Mvc.ViewPage<DAL.RegisterCode>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    注册码
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divQuery">
        &nbsp;&nbsp;<%: Html.LabelFor(model => model.Value) %>：
        <input type='text' id='Value' />
         &nbsp;&nbsp; &nbsp;&nbsp;<%: Html.LabelFor(model => model.BelongMerchant) %>： <input type='text' id='BelongMerchant' />
        &nbsp;&nbsp; &nbsp;&nbsp;<input type="button" value="查 询" onclick="flexiQuery()" />
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" language="javascript">
        $(function () {
            $('#flexigridData').datagrid({
                title: '注册码',
                iconCls: 'icon-site',
                width: 'auto',
                height: '375',
                nowrap: false,
                striped: true,
                collapsible: true,
                url: '../RegisterCode/GetData', //获取数据的url
                sortName: 'Id',
                sortOrder: 'desc',
                idField: 'Id',
                singleSelect: true,/*禁用多选*/
                toolbar: [
                ],
                columns:
                [[
                    { field: 'Value', title: '<%:Html.LabelFor(model=>model.Value)%>', width: 100 },
                     { field: 'CreateTime', title: '<%:Html.LabelFor(model=>model.CreateTime)%>', width: 150 },
                    { field: 'IsValid', title: '<%:Html.LabelFor(model=>model.IsValid)%>', width: 100 },
                    { field: 'IsUsed', title: '<%:Html.LabelFor(model=>model.IsUsed)%>', width: 100 },
                    { field: 'IsDistribution', title: '<%:Html.LabelFor(model=>model.IsDistribution)%>', width: 100 },
                    { field: 'BelongMerchant', title: '<%:Html.LabelFor(model=>model.BelongMerchant)%>', width: 200 },
                    { field: 'MerchantName', title: '<%:Html.LabelFor(model=>model.MerchantName)%>', width: 200 },
                    { field: 'MemberName', title: '<%:Html.LabelFor(model=>model.MemberName)%>', width: 200 },
                ]],
                pagination: true,
                rownumbers: true
            });

            //如果列表页出现在弹出框中，则只显示查询和选择按钮 
            var parent = window.dialogArguments; //获取父页面
            //异步获取按钮          
            if (parent == "undefined" || parent == null) {
                //首先获取iframe标签的id值
                var iframeid = window.parent.$('#tabs').tabs('getSelected').find('iframe').attr("id");

                //然后关闭AJAX相应的缓存
                $.ajaxSetup({
                    cache: false
                });

                //获取按钮值
                $.getJSON("../Home/GetToolbar", { id: iframeid }, function (data) {
                    if (data == null) {
                        return;
                    }
                    $('#flexigridData').datagrid("addToolbarItem", data);

                });

            } else {
                //添加选择按钮
                $('#flexigridData').datagrid("addToolbarItem", [{ "text": "选择", "iconCls": "icon-ok", handler: function () { flexiSelect(); } }]);
            }
        });

        //“查询”按钮，弹出查询框
        function flexiQuery() {

            //将查询条件按照分隔符拼接成字符串
            var search = "";
            $('#divQuery').find(":text,:selected,select,textarea,:hidden,:checked,:password").each(function () {
                search = search + this.id + "&" + this.value + "^";
            });
            //执行查询                        
            $('#flexigridData').datagrid('reload', { search: search });

        };

        //“选择”按钮，在其他（与此页面有关联）的页面中，此页面以弹出框的形式出现，选择页面中的数据
        function flexiSelect() {

            var rows = $('#flexigridData').datagrid('getSelections');
            if (rows.length == 0) {
                $.messager.alert('操作提示', '请选择数据!', 'warning');
                return false;
            }

            var arr = [];
            for (var i = 0; i < rows.length; i++) {
                arr.push(rows[i].Id);
            }
            arr.push("^");
            for (var i = 0; i < rows.length; i++) {
                arr.push(rows[i].Name);
            }
            //主键列和显示列之间用 ^ 分割   每一项用 , 分割
            if (arr.length > 0) {//一条数据和多于一条
                returnParent(arr.join("&")); //每一项用 & 分割
            }
        }

        //导航到修改的按钮
        function flexiModify() {

            var arr = $('#flexigridData').datagrid('getSelections');

            if (arr.length == 1) {
                window.location.href = "../RegisterCode/Edit/" + arr[0].Id;

            } else {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
            }
            return false;

        };

        //导航到查看详细的按钮
        function getView() {

            var arr = $('#flexigridData').datagrid('getSelections');

            if (arr.length == 1) {
                window.location.href = "../RegisterCode/Details/" + arr[0].Id;

            } else {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
            }
            return false;
        }

        //导航到创建的按钮
        function flexiCreate() {

            window.location.href = "../RegisterCode/Create";
            return false;
        }
        //删除的按钮
        function flexiDelete() {

            var rows = $('#flexigridData').datagrid('getSelections');
            if (rows.length == 0) {
                $.messager.alert('操作提示', '请选择数据!', 'warning');
                return false;
            }

            var arr = [];
            for (var i = 0; i < rows.length; i++) {
                arr.push(rows[i].Id);
            }

            $.messager.confirm('操作提示', "确认删除这 " + arr.length + " 项吗？", function (r) {
                if (r) {
                    $.post("../RegisterCode/Delete", { query: arr.join(",") }, function (res) {
                        if (res == "OK") {
                            //移除删除的数据
                            $("#flexigridData").datagrid("reload");
                            $("#flexigridData").datagrid("clearSelections");
                            $.messager.alert('操作提示', '删除成功!', 'info');
                        }
                        else {
                            if (res == "") {
                                $.messager.alert('操作提示', '删除失败!请查看该数据与其他模块下的信息的关联，或联系管理员。', 'info');
                            }
                            else {
                                $.messager.alert('操作提示', res, 'info');
                            }
                        }
                    });
                }
            });

        };
    </script>
</asp:Content>

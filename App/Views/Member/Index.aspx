<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Index.Master" Inherits="System.Web.Mvc.ViewPage<DAL.Member>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<%@ Import Namespace="DAL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    会员
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <div id="divQuery">
        &nbsp;&nbsp;<%: Html.LabelFor(model => model.Name) %>：
        <input type='text' id='Name' />
         &nbsp;&nbsp;<%: Html.LabelFor(model => model.MemberGroupId) %>：
          <%:Html.DropDownList("MemberGroupId",(SelectList)ViewBag.MemberGroups,"请选择") %>
        &nbsp;&nbsp; &nbsp;&nbsp;<input type="button" value="查 询" onclick="flexiQuery()" />
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" language="javascript">
        $(function () {
            $('#flexigridData').datagrid({
                title: '会员',
                iconCls: 'icon-site',
                width: 'auto',
                height: '370',
                nowrap: false,
                striped: true,
                collapsible: true,
                url: '../Member/GetData', //获取数据的url
                sortName: 'Id',
                sortOrder: 'desc',
                idField: 'Id',
                toolbar: [
                ],
                columns:
                [[
                    { field: 'Name', title: '<%:Html.LabelFor(model=>model.Name)%>', width: 200 },
                    { field: 'Code', title: '<%:Html.LabelFor(model=>model.Code)%>', width: 100 },
                    { field: 'RegisterCode', title: '<%: Html.LabelFor(model => model.RegisterCode) %>', width: 150 },
                    { field: 'VIP', title: '<%: Html.LabelFor(model => model.VIP) %>', width: 80 },
                    { field: 'IsValid', title: '<%: Html.LabelFor(model => model.IsValid) %>', width: 80 },
                    { field: 'Area', title: '<%: Html.LabelFor(model => model.Area) %>', width: 80 },
                    { field: 'Regtime', title: '<%: Html.LabelFor(model => model.Regtime) %>', width: 150 },
                    { field: 'RegisteredCapital', title: '<%: Html.LabelFor(model => model.RegisteredCapital) %>', width: 150 },
                    { field: 'Contacts', title: '<%: Html.LabelFor(model => model.Contacts) %>', width: 150 },
                    { field: 'Phone', title: '<%: Html.LabelFor(model => model.Phone) %>', width: 100 },
                    { field: 'RegisteredAddress', title: '<%: Html.LabelFor(model => model.RegisteredAddress) %>', width: 150 },
                    { field: 'LegalRepresentative', title: '<%: Html.LabelFor(model => model.LegalRepresentative) %>', width: 150 },
                    { field: 'RegisteredCellPhone', title: '<%: Html.LabelFor(model => model.RegisteredCellPhone) %>', width: 80 },
                    { field: 'SelfDefineOne', title: '<%: Html.LabelFor(model => model.SelfDefineOne) %>', width: 80 },
                    { field: 'SelftDefineTwo', title: '<%: Html.LabelFor(model => model.SelfDefineTwo) %>', width: 80 },
                    { field: 'Remark', title: '<%: Html.LabelFor(model => model.Remark) %>', width: 300 }
                    //{ field: 'MemberGroupId', title: '会员分组', width: 100 }
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

        //导航到查看详细的按钮
        function getView() {

            var arr = $('#flexigridData').datagrid('getSelections');

            if (arr.length == 1) {
                window.location.href = "../Member/Details/" + arr[0].Id;

            } else {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
            }
            return false;
        }

        //导航到创建的按钮
        function flexiCreate() {

            window.location.href = "../Member/Create";
            return false;
        }
        //导航到修改的按钮
        function flexiModify() {

            var arr = $('#flexigridData').datagrid('getSelections');

            if (arr.length == 1) {
                window.location.href = "../Member/Edit/" + arr[0].Id;

            } else {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
            }
            return false;

        };

        //导航到设置按钮
        function flexiSetup() {
            var arr = $('#flexigridData').datagrid('getSelections');
            if (arr.length == 1) {
                window.location.href = "../Member/Setup/" + arr[0].Id;
            }
            else {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
            }
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
                    $.post("../Member/Delete", { query: arr.join(",") }, function (res) {
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

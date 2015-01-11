<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Index.Master" Inherits="System.Web.Mvc.ViewPage<DAL.Merchant>" %>

<%@ Import Namespace="Common" %>
<%@ Import Namespace="App.Models" %>
<%@ Import Namespace="DAL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    商家
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div id="divQuery">
        &nbsp;&nbsp;<%: Html.LabelFor(model => model.Name) %>：
        <input type='text' id='Name' />
        &nbsp;&nbsp; &nbsp;&nbsp;<input type="button" value="查 询" onclick="flexiQuery()" />
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" language="javascript">
        $(function ()
        {
            $('#flexigridData').datagrid({
                title: '商家',
                iconCls: 'icon-site',
                width: 'auto',
                height: '480',
                nowrap: false,
                striped: true,
                collapsible: true,
                url: '../Merchant/GetData', //获取数据的url
                sortName: 'Id',
                sortOrder: 'desc',
                idField: 'Id',
                //singleSelect: true,/*禁用多选*/
                toolbar:
                [
                    //{
                    //    text: '设置商家',
                    //    iconCls: 'icon-search',
                    //    handler: function ()
                    //    {
                    //        setRelationShip();
                    //    }
                    //}
                ],
                columns:
                [[
                    { field: 'Name', title: '<%:Html.LabelFor(model=>model.Name)%>', width: 200 },
                    { field: 'MerchantTypeId', title: '<%: Html.LabelFor(model => model.MerchantTypeId) %>', width: 200 },
                    { field: 'RegisterCode', title: '<%: Html.LabelFor(model => model.RegisterCode) %>', width: 80 },
                    { field: 'Telephone', title: '<%: Html.LabelFor(model => model.Telephone) %>', width: 150 },
                    { field: 'Contacts', title: '<%: Html.LabelFor(model => model.Contacts) %>', width: 80 },
                    { field: 'Cellphone', title: '<%: Html.LabelFor(model => model.Cellphone) %>', width: 150 },
                     { field: 'QQ', title: '<%: Html.LabelFor(model => model.QQ) %>', width: 150 },
                    { field: 'SiteUrl', title: '<%: Html.LabelFor(model => model.SiteUrl) %>', width: 150 },
                    { field: 'Address', title: '<%: Html.LabelFor(model => model.Address) %>', width: 150 },
                    { field: 'ComprehensiveStar', title: '<%: Html.LabelFor(model => model.ComprehensiveStar) %>', width: 80 },
                    { field: 'Balance', title: '<%: Html.LabelFor(model => model.Balance) %>', width: 80 },
                    { field: 'Description', title: '<%: Html.LabelFor(model => model.Description) %>', width: 300 },
                    { field: 'IsVisible', title: '<%: Html.LabelFor(model => model.IsVisible) %>', width: 80 },
                    { field: 'SysPersonId', title: '用户名', width: 220 }
                ]],
                pagination: true,
                rownumbers: true
            });

            //如果列表页出现在弹出框中，则只显示查询和选择按钮 
            var parent = window.dialogArguments; //获取父页面
            //异步获取按钮          
            if (parent == "undefined" || parent == null)
            {
                //首先获取iframe标签的id值
                var iframeid = window.parent.$('#tabs').tabs('getSelected').find('iframe').attr("id");

                //然后关闭AJAX相应的缓存
                $.ajaxSetup({
                    cache: false
                });

                //获取按钮值
                $.getJSON("../Home/GetToolbar", { id: iframeid }, function (data)
                {
                    if (data == null)
                    {
                        return;
                    }
                    $('#flexigridData').datagrid("addToolbarItem", data);

                });

            } else
            {
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
        function flexiSelect()
        {

            var rows = $('#flexigridData').datagrid('getSelections');
            if (rows.length == 0)
            {
                $.messager.alert('操作提示', '请选择数据!', 'warning');
                return false;
            }

            var arr = [];
            for (var i = 0; i < rows.length; i++)
            {
                arr.push(rows[i].Id);
            }
            arr.push("^");
            for (var i = 0; i < rows.length; i++)
            {
                arr.push(rows[i].Name);
            }
            //主键列和显示列之间用 ^ 分割   每一项用 , 分割
            if (arr.length > 0)
            {//一条数据和多于一条
                returnParent(arr.join("&")); //每一项用 & 分割
            }
        }

        //导航到查看详细的按钮
        function getView()
        {

            var arr = $('#flexigridData').datagrid('getSelections');

            if (arr.length == 1)
            {
                window.location.href = "../Merchant/Details/" + arr[0].Id;

            } else
            {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
            }
            return false;
		}

        //导航到创建的按钮
        function flexiCreate()
        {

            window.location.href = "../Merchant/Create";
            return false;
        }
        //导航到修改的按钮
        function flexiModify()
        {

            var arr = $('#flexigridData').datagrid('getSelections');

            if (arr.length == 1)
            {
                window.location.href = "../Merchant/Edit/" + arr[0].Id;

            } else
            {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
            }
            return false;

        };
        //导航到修改的按钮
        function flexiSetup() {

            var arr = $('#flexigridData').datagrid('getSelections');

            if (arr.length == 1) {
                window.location.href = '../Merchant/RelationShip/' + arr[0].Id;

            } else {
                $.messager.alert('操作提示', '请选择一条数据!', 'warning');
            }
            return false;

        };
        //删除的按钮
        function flexiDelete()
        {

            var rows = $('#flexigridData').datagrid('getSelections');
            if (rows.length == 0)
            {
                $.messager.alert('操作提示', '请选择数据!', 'warning');
                return false;
            }

            var arr = [];
            for (var i = 0; i < rows.length; i++)
            {
                arr.push(rows[i].Id);
            }

            $.messager.confirm('操作提示', "确认删除这 " + arr.length + " 项吗？", function (r)
            {
                if (r)
                {
                    $.post("../Merchant/Delete", { query: arr.join(",") }, function (res)
                    {
                        if (res == "OK")
                        {
                            //移除删除的数据
                            $("#flexigridData").datagrid("reload");
                            $("#flexigridData").datagrid("clearSelections");
                            $.messager.alert('操作提示', '删除成功!', 'info');
                        }
                        else
                        {
                            if (res == "")
                            {
                                $.messager.alert('操作提示', '删除失败!请查看该数据与其他模块下的信息的关联，或联系管理员。', 'info');
                            }
                            else
                            {
                                $.messager.alert('操作提示', res, 'info');
                            }
                        }
                    });
                }
            });
        };

        function setRelationShip()
        {
            var rows = $('#flexigridData').datagrid('getSelections');
            if (rows.length == 0)
            {
                $.messager.alert('操作提示', '请选择商家!', 'warning');
                return false;
            }
            window.location.href = '../Merchant/RelationShip/' + rows[0].Id;
        }

    </script>
</asp:Content>

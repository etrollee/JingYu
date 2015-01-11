<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Base.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Res/jquery.qtip.customed/jquery.qtip.min.css" rel="stylesheet" />
    <style type="text/css">
        #merchant {
            clear: both;
            display: block;
            width: 760px;
            height: 400px;
            margin: auto;
        }

        #merchantTable {
            display: table;
            width: 100%;
            height: 160px;
            margin: auto;
        }

            #merchantTable > div {
                display: table-cell;
                width: 50%;
            }

            #merchantTable ul {
                width: 92%;
                padding: 10px;
                border: 1px solid #808080;
            }

                #merchantTable ul li {
                    list-style: none;
                }

                    #merchantTable ul li input {
                        margin-right: 5px;
                        width: 100px;
                    }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CurentPlace" runat="server">
    <div>商家关系设置</div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="padding: 10px;">
        <input type="hidden" name="merchantId" value="<%:ViewBag.Id.ToString()%>" />
        <div id="curMerchant" style="display: inline-block; width: 45%;">
        </div>
        <div style="display: inline-block;">
            <a id="btnSubmit" href="javascript:void(0)" class="l-btn"><span class="l-btn-left"><span class="l-btn-text icon-save l-btn-icon-left">保存</span></span></a>
            <a  href="javascript:void(0)" class="l-btn" onclick="javascript:history.go(-1);"><span class="l-btn-left"><span class="l-btn-text icon-back l-btn-icon-left">返回</span></span></a>
        </div>
    </div>
    <div id="merchant" class="">
        <div id="merchantTable">
            <div>
                <label>商家列表：</label>
                <ul id="merchantSource" style="height: 344px; overflow-y: auto;">
                </ul>
            </div>
            <div>
                <label>上级商家：</label>
                <ul id="parentMerchant" style="height: 26px;">
                </ul>
                <label>下级商家：</label>
                <ul id="subMerchant" style="height: 248px; overflow-y: auto;">
                </ul>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="scriptPlace" ContentPlaceHolderID="ScriptPlace" runat="server">
    <script src="../../Res/jquery.qtip.customed/jquery.qtip.min.js"></script>
    <script src="../../Scripts/jquery.serializeObject.js"></script>
    <%--<script src="../../Res/jquery.qtip.customed/imagesloaded.min.js"></script>--%>
    <script src="../../Scripts/custom/merchant/SetMerchantRelation.js" type="text/javascript"></script>
    <script type="text/javascript">
        var merchant = {
            self: {
                id: '<%:ViewBag.Id.ToString()%>'
            },
            parent: null,
            oldParent: null,
            children: [],
            oldChildren: []
        };
        getMerchants(merchant.self.id);
    </script>
</asp:Content>

/// <reference path="../../../Res/jquery.uploadify-v2.1.4/jquery.uploadify.v2.1.4.js" />
/// <reference path="../../jquery.1.8.3.js" />
/// <reference path="../../jquery.serializeObject.js" />
/// <reference path="../../../Res/easyui/jquery.easyui.loading.1.0.js" />


var uploader = uploader || function () { };
uploader.fileCount = 0;
uploader.fileSizeLimit = 1 * 1024 * 1024;//1 MB
uploader.init = function (uploaderId, uploadUrl)
{
    var urlBase = '../../../';
    $('#' + uploaderId).uploadify({
        uploader: urlBase + 'Res/jquery.uploadify-v2.1.4/uploadify.allglyphs.swf', //Flash上传插件
        expressInstall: urlBase + '/Content/FileUploader/flashplayer11.6-activeX.exe',//flash插件下载地址
        script: uploadUrl,// '/Merchant/Create', //服务器处理页面（url）
        fileDataName: 'image',//后台根据此名称获取文件
        scriptData: {},
        cancelImg: urlBase + '/Content/FileUploader/FileUploader/jquery.uploadify-v2.1.4/cancel.gif', //关闭按钮的图片
        scriptAccess: 'samedomain',
        folder: 'Files/', //保存文件的文件夹
        queueID: 'fileQueue',
        fileDesc: '*.jpg;*.png;*.bmp;*.jpeg', //描述（和fileExt一起使用）
        fileExt: '*.jpg;*.png;*.bmp;*.jpeg', //允许浏览上传的文件扩展名（必须和fileDesc一起使用）
        sizeLimit: uploader.fileSizeLimit, //文件大小限制1MB（1024*1024B）（按byte计算， 注意，在ASP.NET中Web.Config也要配置）
        auto: false,//自动上传
        wmode: 'transparent',
        hideButton: true,
        multi: false, //是否支持多文件上传
        buttonText: '浏览…', //按钮上的文本
        files: 0,
        width: 69,
        height: 24,
        onError: function (a, b, c, d)
        {
            if (d.status == 404)
            {
                alert('找不到上传脚本！')
            }
            else if (d.type === "HTTP")
            {
                alert('error ' + d.type + ": " + d.status);
            }
            else if (d.type === "File Size")
            {
                alert("文件“ " + c.name + ' '
                    + ' ”已超出文件大小 ' + (uploader.fileSizeLimit / (1024 * 1024)).toFixed(2) + 'MB 的限制！');
                uploader.fileCount = 0;
                $('#txtFileName').val('');
            }
            else
            {
                alert('error ' + d.type + ": " + d.info);
            }
        },
        onUploadSuccess: function (a, b, c)
        {
        },
        onComplete: function (event, id, fileObj, response, data)
        {    // 完成一个上传后执行
            $('#txtFileName').val('');
            uploader.fileCount = 0;
            if ($.messager)
            {
                if (response.indexOf('成功') == -1)
                {
                    $.messager.alert('操作提示', '保存失败！', 'error');
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
                        $('img').each(function ()
                        {
                            var $img = $(this);
                            $img.parent().removeClass('hide');
                            var src = $img.attr('src');
                            $img.attr('src', "").attr('src', src);
                        });
                    });
                }
            }
        },
        onAllComplete: function (a, b)
        {   // 完成所有上传后执行
        },
        uploadifySelect: function ()
        {
        },
        onSelectOnce: function (a, b)
        {   // 浏览一次本机文件后执行
            uploader.fileCount = b.fileCount;
            if (b.fileCount < 1)
            {
                $('#txtFileName').val('');
            }
        },
        afterSelect: function (args, randomId, fileObj)
        {
            $('#txtFileName').val(fileObj.name || '');
        },
        onCancel: function (a, b, c, d)
        { // 取消一个将要上传的文件后执行
        }
    });
};


function submitForm(edit)
{
    var form = $('form').first();
    var formValid = form.validate().form();
    var valid = formValid;
    if (!valid)
    {
        return false;
    }
    var data = form.serializeObject();
    if (uploader.fileCount > 0)
    {
        data.MerchantTypeId = $('#MerchantTypeId').get(0).value || $('select#MerchantTypeId').val();
        $('#fileMerchantLogo').uploadifySettings('scriptData', data, true);
        $('#fileMerchantLogo').uploadifyUpload();
    }
    else
    {
        data.MerchantTypeId = $('select#MerchantTypeId').val() || $('#MerchantTypeId').get(0).value;
        $.ajax({
            url: form.attr('action'),
            type: "Post",
            data: data,
            dataType: "json",
            success: function (data)
            {
                if ($.messager)
                {
                    if (data.indexOf('成功') == -1)
                    {
                        $.messager.alert('操作提示', data, 'error');
                    }
                    else
                    {
                        $.messager.defaults.ok = '继续';
                        $.messager.defaults.cancel = '返回';
                        $.messager.confirm('操作提示', data, function (r)
                        {
                            if (!r)
                            {
                                window.location.href = 'javascript:history.go(-1)';
                            }
                        });
                    }
                }
            },
            complete: function (xhr, ts) { },
            error: function (xhr, ts, err) { alert(ts + '\n' + err); }
        });
    }
}


$(document).ready(function ()
{
    $('#btnSubmit').click(function ()
    {
        submitForm();
    });
});
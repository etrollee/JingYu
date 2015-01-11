function dragDropMerchant()
{
    $('.drag').draggable({
        proxy: 'clone',
        revert: true,
        cursor: 'auto',
        onStartDrag: function ()
        {
            $(this).draggable('options').cursor = 'not-allowed';
            $(this).draggable('proxy').addClass('dp');
            var oldPos = $(this).position();
            $('.dp').first().css({ left: oldPos.left, top: oldPos.top });
        },
        onStopDrag: function ()
        {
            $(this).draggable('options').cursor = 'auto';
            var pwdbox = $(this).find('input[type="password"]').first();
            pwdbox && pwdbox.focus();
        }
    });
    $('#merchantSource,#parentMerchant,#subMerchant').droppable({
        onDragEnter: function (e, source)
        {
            $(source).draggable('options').cursor = 'auto';
            //$(source).draggable('proxy').css('border', '1px solid red');
        },
        onDragLeave: function (e, source)
        {
            $(source).draggable('options').cursor = 'not-allowed';
            //$(source).draggable('proxy').css('border', '1px solid #ccc');
            $(this).removeClass('over');
        },
        onDrop: function (e, source)
        {
            var id = $(this).get(0).id;
            var pid = $(source).attr('pid');
            if ($(this).find('#' + source.id).length < 1)
            {
                var added = $(source).find("input").length > 0;
                var merchantId = $(source).attr('id');
                var code = $(source).data('code');
                var name = $(source).attr('name');
                var merchantId = $(source).attr('id');
                var codeBox = '<input type="password" required="true" title="" onblur="javascript:validateCode(this);" />';
                if (id == 'parentMerchant')
                {
                    if ($(this).find('li').length == 0)
                    {
                        if ($('#subMerchant').find('li#' + pid).length > 0)
                        {
                            var parentName = $('#subMerchant')
                                            .find('li#' + pid)
                                            .first().attr('name');
                            $.messager.alert('系统提示', '“' + name + '”为“' + parentName
                                        + '”的下级商家，不能为当前商家的上级商家！', 'error');
                            return;
                        }
                        else if ($('#subMerchant').find('li[pid="' + merchantId + '"]').length > 0)
                        {
                            var childName = $('#subMerchant').find('li[pid="' + merchantId + '"]')
                                                                .first().attr('name');
                            $.messager.alert('系统提示', '“' + name + '”为“' + childName
                                        + '”的上级商家，不能为当前商家的上级商家！', 'error');
                            return;
                        }

                        if (!added && id != 'merchantSource')
                        {
                            var $codeBox = $(codeBox);
                            $(source).prepend($('<input type="hidden" name="newParentId" value="' + merchantId + '" />'))
                                .prepend($codeBox);

                            //$codeBox.qtip();
                        }
                        $(this).append(source);
                        $(source).find('input[type="hidden"]')
                                        .first().attr('name', 'newParentId');
                    }
                }
                else
                {
                    var hasParent = $('#parentMerchant,#subMerchant').find('li#' + pid).length > 0;
                    var hasChild = $('#parentMerchant,#subMerchant').find('li[pid="'
                                                                    + merchantId + '"]').length > 0;
                    if ((hasParent || hasChild) && id != 'merchantSource')
                    {
                        //$(source).blur();
                        if (hasParent)
                        {
                            var parentName = $('#parentMerchant,#subMerchant')
                                                .find('#' + pid)
                                                .first().attr('name');
                            $.messager.alert('系统提示', '“' + name + '”为“' + parentName
                                        + '”的下级商家，不能做为当前商家的下级商家！', 'error');
                        }
                        else if (hasChild)
                        {
                            var childName = $('#parentMerchant,#subMerchant')
                                                .find('li[pid="' + merchantId + '"]')
                                                .first().attr('name');
                            $.messager.alert('系统提示', '“' + name + '”为“' + childName
                                            + '”的上级商家，不能做为当前商家的下级商家！', 'error');
                        }
                        return;
                    }
                    if (id == 'merchantSource')
                    {
                        $(source).find('input').remove();
                    }
                    if (!added && id != 'merchantSource')
                    {
                        var $codeBox = $(codeBox);
                        $(source).prepend($('<input type="hidden" name="newChildIds" value="' + merchantId + '" />'))
                            .prepend($codeBox);
                    }
                    $(this).append(source);
                    if (id != 'merchantSource')
                    {
                        $(source).find('input[type="hidden"]')
                                           .first().attr('name', 'newChildIds');
                    }
                }
            }
        }
    });
}


function getMerchants(merchantId)
{
    $.ajax({
        type: 'POST',
        url: '/Merchant/RelationShip/',
        async: true,
        data: {
            id: merchantId,
            getMerchant: true
        },
        success: function (data)
        {
            var $source = $('#merchantSource');
            var self = data.self;//object
            var parent = data.parent;//null or object
            var children = data.children;//array
            var others = data.others;//array

            $('#curMerchant').text('当前商家：[ ' + self.name + ' ]');
            var codeBox = '<input type="password" required="true" title="" onblur="javascript:validateCode(this);" />';

            var $form = $('form').first();
            if (parent)
            {
                merchant.oldParent = parent;
                $form.append($('<input />')
                                .attr({
                                    type: 'hidden',
                                    name: 'oldParentId',
                                    value: parent.id
                                })
                            );

                var $li = $('<li></li>')
                            .attr({
                                id: parent.id,
                                pid: parent.parentId,
                                name: parent.name,
                                class: 'drag'
                            })
                            .text(parent.name)
                            .data('code', parent.code);
                $('#parentMerchant').append($li);
                $li.prepend($('<input />')
                            .attr({
                                type: 'hidden',
                                name: 'newParentId',
                                value: parent.id
                            }))
                   .prepend($(codeBox).attr({ value: parent.code }));
            }

            merchant.oldChildren = children;
            for (var i = 0; i < children.length; i++)
            {
                var c = children[i];
                $form.append($('<input />')
                                .attr({
                                    type: 'hidden',
                                    name: 'oldChildIds',
                                    value: c.id
                                })
                            );
                var $li = $('<li></li>')
                            .attr({
                                id: c.id,
                                pid: c.parentId,
                                name: c.name,
                                class: 'drag'
                            })
                            .text(c.name)
                            .data('code', c.code);
                $('#subMerchant').append($li);
                $li.prepend($('<input />')
                          .attr({
                              type: 'hidden',
                              name: 'newChildIds',
                              value: c.id
                          }))
                  .prepend($(codeBox).attr({ value: c.code }));
            }

            for (var i = 0; i < others.length; i++)
            {
                var m = others[i];
                var $merchant = $('<li></li>')
                                .attr({
                                    id: m.id,
                                    pid: m.parentId,
                                    name: m.name,
                                    class: 'drag'
                                })
                                .text(m.name)
                                .data({
                                    id: m.id,
                                    code: m.code
                                });
                $source.append($merchant);
            }
            dragDropMerchant();
        },
        complete: function (xhr, ts) { },
        error: function (xhr, ts, err) { }
    });
}

function checkMerchantCode()
{
    var codeboxes = $('#parentMerchant>li>input[type="password"],#subMerchant>li>input[type="password"]');
    var valid = true;
    for (var i = 0; i < codeboxes.length; i++)
    {
        var box = codeboxes[i];
        var isEmpty = $.trim(box.value) == '';
        if (isEmpty)
        {
            valid = false;
            $(box).addClass('input-validation-error');
        }
    }
    return valid;
}

function validateCode(box)
{
    var $self = $(box);
    var code = $self.parent().data('code');
    var pwd = $.trim($self.val());
    if (pwd != '' && pwd != code)
    {
        $self.addClass('input-validation-error').val('');
        $self.qtip({ content: '注册码不匹配！' }).show();
    }
    else
    {
        $self.removeClass('input-validation-error');
    }
}

function submit()
{
    var relative = $('#parentMerchant>li,#subMerchant>li').length > 0;
    var hasOld = merchant.oldParent != null || merchant.oldChildren.length > 0;

    if (!relative && !hasOld)
    {
        $.messager.alert('系统提示', '请给商家设置上级商家或下级商家！', 'warning');
        return false;
    }
    if (relative)
    {
        var $boxes = $('#parentMerchant>li>input[type="password"],#subMerchant>li>input[type="password"]');
        var valid = true;
        var invalid = [];
        $boxes.each(function ()
        {
            var $self = $(this);
            if ($self.val() == '')
            {
                $self.addClass('input-validation-error');
                $self.qtip({ content: '请输入商家注册码！' }).show();
                invalid.push($self);
                valid = false;
            }
        });
        invalid[0] && invalid[0].focus();
        if (!valid)
        {
            return false;
        }
    }
    var data = $('form').first().serializeObject();

    if (data.oldChildIds)
    {
        var isArray = typeof (data.oldChildIds) != 'string';
        data.oldChildIds = isArray ? data.oldChildIds.join(',') : data.oldChildIds;
    }
    if (data.newChildIds)
    {
        var isArray = typeof (data.newChildIds) != 'string';
        data.newChildIds = isArray ? data.newChildIds.join(',') : data.newChildIds;
    }

    $.ajax({
        type: 'POST',
        url: '/Merchant/SetRelationShip',
        async: true,
        data: data,
        success: function (response)
        {
            $.messager.alert('系统提示', response.msg, response.success ? 'info' : 'warning');
        },
        complete: function (xhr, ts) { },
        error: function (xhr, ts, err)
        {
            $.messager.alert('系统提示', err, 'error');
        }
    });
}


$(document).ready(function ()
{
    $('#btnSubmit').first().click(submit);
});
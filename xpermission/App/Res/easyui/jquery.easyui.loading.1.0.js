(function ($)
{
    $.extend($.fn.messager,
    {
        loadingId: null,
        loading: function (msg)
        {
            this.loadingId = 'loading' + Math.random();
            var height = window.screen.height - 250;
            var width = window.screen.width;
            var leftW = 300;
            if (width > 1200)
            {
                leftW = 500;
            }
            else if (width > 1000)
            {
                leftW = 350;
            }
            else
            {
                leftW = 100;
            }
            var style1 = "position:absolute; width:100%;height:" + height
                                    + "px;top:0;background:#E0ECFF;opacity:0.8;filter:alpha(opacity=80);";
            var style2 = "position:absolute; cursor:wait; left:" + leftW
                        + "px;top:200px;width:auto;height:16px;padding:12px 5px 10px 30px; "
                        + "background:#fff url(themes/default/images/pagination_loading.gif) no-repeat scroll 5px 10px; border:2px solid #ccc; color:#000;";
            var loadingHtml = "<div id='loading' style='" + style1 + "'>"
                        + "<div style='" + style2 + "'>"
                        + (msg ? msg : '正在处理，请等待...') + "</div></div>";
            $(body).append($(loadingHtml));
        },
        clearLoading: function ()
        {
            var id = this.loadingId;
            $('#' + id).remove();
        }
    })
})(jQuery);
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxeducation.aspx.cs" Inherits="MobileCms.wxeducation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="WxCss/wxcommon.css" rel="stylesheet" type="text/css" />
    <link href="WxCss/wxeducation.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            
        });

        function menuItemClick(obj) {
            $("#menu ul li span").attr("style", "color:#000000");

            var checkId = $(obj).attr("id");

            $("#menu ul li a").each(function (i, j) {
                var itemId = $(this).attr("id");

                if (itemId != checkId) {
                    var src = "/WxImages/icon_" + itemId + ".png";

                    $(this).find("img").attr("src", src);
                } else {
                    $(this).parents("li").find("span").attr("style", "color:#f677aa");
                }
            });

            var checkSrc = "/WxImages/icon_" + checkId + "hover.png";
            $(obj).find("img").attr("src", checkSrc);

            $(".education_message").text("建设中.........");
        }
    </script>
</head>
<body>
    <div class="wrapper">
        <div class="top">
            <div class="back">
                <a href="javascript:;"></a>
            </div>
            <span class="title">教育</span>
        </div>

        <div id="menu">
            <ul>
                <li>
                    <a id="homework" class="" href="javascript:;" onclick="menuItemClick(this)">
                        <img alt="" src="/WxImages/icon_homework.png" />
                    </a>
                    <span>今日作业</span>
                </li>
                <li>
                    <a id="teacherbbs" href="javascript:;" onclick="menuItemClick(this)">
                        <img alt="" src="/WxImages/icon_teacherbbs.png" />
                    </a>
                    <span>老师评语</span>
                </li>
                <li>
                    <a id="classtable" href="javascript:;" onclick="menuItemClick(this)">
                        <img alt="" src="/WxImages/icon_classtable.png" />
                    </a>
                    <span>课程表</span>
                </li>
                <li>
                    <a id="bbs" href="javascript:;" onclick="menuItemClick(this)">
                        <img alt="" src="/WxImages/icon_bbs.png" />
                    </a>
                    <span>留言</span>
                </li>
                <li>
                    <a id="sign" href="javascript:;" onclick="menuItemClick(this)">
                        <img alt="" src="/WxImages/icon_sign.png" />
                    </a>
                    <span>签到</span>
                </li>
            </ul>

            <div class="education_message"></div>
        </div>

    </div>
</body>
</html>

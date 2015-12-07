<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxguardian.aspx.cs" Inherits="MobileCms.wxguardian" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Css/wxcommon.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $(".education_message").text("");

            hideAllTable();

            $("#guardian").click();
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

            hideAllTable();

            $("#" + checkId + "_table").show();
        }

        function hideAllTable() {
            $("#guardian_table").hide();
            $("#undisturb_table").hide();
            $("#help_table").hide();
        }
    </script>
</head>
<body>
    <div class="wrapper">
        <div class="top">
            <div class="back">
                <a href="javascript:;"></a>
            </div>
            <span class="title">监护人设置</span>
        </div>

        <div id="menu">
            <ul>
                <li>
                    <a id="guardian" class="" href="javascript:;" onclick="menuItemClick(this)">
                        <img alt="" src="/WxImages/icon_guardian.png" />
                    </a>
                    <span>监护人</span>
                </li>
                <li>
                    <a id="undisturb" href="javascript:;" onclick="menuItemClick(this)">
                        <img alt="" src="/WxImages/icon_undisturb.png" />
                    </a>
                    <span>免打扰</span>
                </li>
                <li>
                    <a id="help" href="javascript:;" onclick="menuItemClick(this)">
                        <img alt="" src="/WxImages/icon_help.png" />
                    </a>
                    <span>帮助</span>
                </li>
            </ul>
        </div>

        <div id="tablecontent">
            <div id="guardian_table">
                <ul class="foldingedit">
                    <li>
                        <span>昵称</span>
                        <input type="text" id="displayName1" name="displayName1" class="inputtext2" />

                        <span>手机号码</span>
                        <input type="text" id="mobilePhone1" name="mobilePhone1" class="inputtext2" />

                        <span>短号码</span>
                        <input type="text" id="shortNum1" name="shortNum1" class="inputtext2" />
                    </li>
                </ul>

                <div class="tablecontent_button">
                    <a href="javascript:;" class="buttonnext">
                        <img alt="" src="/WxImages/loginbutton.png" />
                    </a>
                </div>
                
            </div>
            <div id="undisturb_table">
                <p>建设中.....</p>
            </div>
            <div id="help_table">
                <p>建设中.....</p>
            </div>
        </div>

    </div>
</body>
</html>

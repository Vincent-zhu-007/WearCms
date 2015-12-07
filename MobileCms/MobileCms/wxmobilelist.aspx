<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxmobilelist.aspx.cs" Inherits="MobileCms.wxmobilelist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="WxCss/wxcommon.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="wrapper">
        <div class="top">
            <div class="back">
                <a href="javascript:;"></a>
            </div>
            <span class="title">切换宝贝</span>
        </div>

        <div class="content">
            <%=mobileListHtml%>

            <%--<ul>
                <li>
                    123456323158
                </li>
            </ul>--%>
        </div>
    </div>
</body>
</html>

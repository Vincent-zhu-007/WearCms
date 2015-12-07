<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxbindmobile.aspx.cs" Inherits="MobileCms.wxbindmobile" %>

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
            <span class="title">添加宝贝失败</span>
        </div>

        <div class="content">
            <p>很遗憾，您的宝贝添加失败，<font>失败原因：<%=errorMessage%></font>，您可以与客服取得联系，帮助您查找问题原因。</p>
        </div>
    </div>
</body>
</html>

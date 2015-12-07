<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="MobileCms.index" %>

<%@ Register Src="~/Controls/primaryleft.ascx" TagName="primaryLeft" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="Css/common.css" rel="stylesheet" type="text/css" />
    <link href="Css/innertoolbar.css" rel="stylesheet" type="text/css" />
    <link href="Css/detialjia.css" rel="stylesheet" type="text/css" />
    <link href="Css/page.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        /* Alignment */
        .alignleft {
            display: inline;
            float: left;
        }
        .alignright {
            display: inline;
            float: right;
        }
        .aligncenter {
            clear: both;
            display: block;
            margin:auto;
        } 
    </style>

    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
</head>
<body>
    <div class="wrapper">
        <uc1:primaryLeft ID="PrimaryLeft1" runat="server" />

        <div class="primary_right">
            <div class="inner">
                <div class="position ps">
                    <span></span>
                    <div class="brad">
                        
                    </div>
                </div>

                <div class="content ps">
                    <div class="title">
                        
                    </div>

                    <div class="xx_nr">
                        <img class="aligncenter" alt="" title="" src="/Images/welcome.png" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>

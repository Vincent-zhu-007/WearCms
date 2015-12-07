<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mobilecontrol.aspx.cs" Inherits="MobileCms.mobilecontrol" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

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
    <link href="Scripts/dist/themes/default/style1.css" rel="stylesheet" type="text/css" />
    <link href="Scripts/jqueryui/jquery-ui1.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .table tr:hover 
        {
            background-color:#FFECEC;
        }
    </style>

    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/dist/libs/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jqueryui/jquery-ui.js" type="text/javascript"></script>

    <script src="Scripts/formValidator.js" type="text/javascript"></script>
    <script src="Scripts/jquery.form.js" type="text/javascript"></script>
    <script src="Scripts/formValidatorRegex.js" type="text/javascript"></script>

    <script src="CustScripts/common.js" type="text/javascript"></script>
</head>
<body>
    <div class="wrapper">
        <uc1:primaryLeft ID="PrimaryLeft1" runat="server" />

        <div class="primary_right">
            <div class="inner">
                <div class="position ps">
                    <span>用户管理</span>
                    <div class="brad">
                        <a href="#">系统管理</a>
                        >
                        <a href="#">用户管理</a>
                    </div>
                </div>

                <div class="content ps">
                    <div class="title">
                        <form runat="server" id="queryForm" name="queryForm" method="post" action="">
                            &nbsp;&nbsp;&nbsp;&nbsp;用户名：
                            <asp:TextBox ID="userName" runat="server"></asp:TextBox>
                            <div class="queryform_btn btn">
                                <label>
                                    <asp:Button ID="butSearch" CssClass="inputbtn" runat="server" Text="查询" onclick="butSearch_Click" />    
                                </label>
                            </div>
                        </form>
                    </div>

                    <div class="public_jia_video">
                        <table class="table" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <th width="8%">用户名</th>
                                <th width="10%">昵称</th>
                                <th width="15%">操作</th>
                            </tr>

                            <asp:Repeater ID="rptList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <%--<td>
                                            <input type="checkbox" value='<%#Eval("Id")%>' name="checkbox" />
                                        </td>--%>
                                        <td><%#Eval("UserName")%></td>
                                        <td><%#Eval("DisplayName")%></td>
                                        <td>
                                            <a class="" href="javascript:;" onclick="openProcessConfirmDialog('<%#Eval("Id")%>', 'poweroff');">远程关机</a>
                                            &nbsp;&nbsp;<a class="" href="javascript:;" onclick="openProcessConfirmDialog('<%#Eval("Id")%>', 'reportlocation');">位置上报</a>
                                            &nbsp;&nbsp;<a class="" href="javascript:;" onclick="openProcessConfirmDialog('<%#Eval("Id")%>', 'listening');">监听</a>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>

                        </table>
                        
                        <div class="pagecontain">
                            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" CssClass="fanye" CurrentPageButtonClass="cursor"
                                CustomInfoHTML = "总数：<font>%RecordCount%</font> 每页：<font>%PageSize%</font> 当前页：<font>%CurrentPageIndex%</font> 共<font>%PageCount%</font>页"
                                ShowCustomInfoSection="Left"
                                PageSize="10" ShowInputBox="Never" CustomInfoTextAlign="Left" AlwaysShow="false"
                                LayoutType="Table" UrlPageIndexName="page1">
                            </webdiyer:AspNetPager>
                        </div>
                    </div>

                </div>

            </div>
        </div>
    </div>

    <div id="warningDialog" class="index_dialog" title="警告">
        <p id="warningDialog_Message"></p>
    </div>

    <div id="messageDialog" class="index_dialog" title="提醒">
        <p id="messageDialog_Message"></p>
    </div>

    <div id="loadingDialog" class="index_dialog">
        <img alt="" src="/Images/loading.gif" style="margin-top:20px; margin-left:120px;" />
    </div>

    <div id="confirmDialog" class="index_dialog" title="确认">
        <p id="confirmDialog_Message">您确认执行操作？</p>
    </div>

    <script type="text/javascript">
        function openProcessConfirmDialog(id, op) {
            openConfirmDialog(op, id);
        }

        function lockById(id) {
            openLoadingDialog();

            $.ajax({
                type: "post",
                url: "/Ajax/lockmobile.ashx",
                data: "id=" + id + "",
                success: function (data) {
                    if (data != null && data != "" && data.message == "200") {
                        closeLoadingDialog();
                        location.href = "mobilecontrol.aspx";
                    } else {
                        closeLoadingDialog();
                        openMessageDialog(data.message);
                        return false;
                    }
                },
                error: function (XMLHttpRequest, data, errorThrown) {
                    closeLoadingDialog();
                    openMessageDialog(errorThrown);
                }
            });
        }

        function clearById(id) {
            openLoadingDialog();

            $.ajax({
                type: "post",
                url: "/Ajax/clearmobile.ashx",
                data: "id=" + id + "",
                success: function (data) {
                    if (data != null && data != "" && data.message == "200") {
                        closeLoadingDialog();
                        location.href = "mobilecontrol.aspx";
                    } else {
                        closeLoadingDialog();
                        openMessageDialog(data.message);
                        return false;
                    }
                },
                error: function (XMLHttpRequest, data, errorThrown) {
                    closeLoadingDialog();
                    openMessageDialog(errorThrown);
                }
            });
        }

        function powerOffById(id) {
            openLoadingDialog();

            $.ajax({
                type: "post",
                url: "/Ajax/poweroffmobile.ashx",
                data: "id=" + id + "",
                success: function (data) {
                    if (data != null && data != "" && data.message == "200") {
                        closeLoadingDialog();
                        location.href = "mobilecontrol.aspx";
                    } else {
                        closeLoadingDialog();
                        openMessageDialog(data.message);
                        return false;
                    }
                },
                error: function (XMLHttpRequest, data, errorThrown) {
                    closeLoadingDialog();
                    openMessageDialog(errorThrown);
                }
            });
        }

        function reportLocationById(id) {
            openLoadingDialog();

            $.ajax({
                type: "post",
                url: "/Ajax/reportlocation.ashx",
                data: "id=" + id + "",
                success: function (data) {
                    if (data != null && data != "" && data.message == "200") {
                        closeLoadingDialog();
                        location.href = "mobilecontrol.aspx";
                    } else {
                        closeLoadingDialog();
                        openMessageDialog(data.message);
                        return false;
                    }
                },
                error: function (XMLHttpRequest, data, errorThrown) {
                    closeLoadingDialog();
                    openMessageDialog(errorThrown);
                }
            });
        }

        function listeningById(id) {
            openLoadingDialog();

            $.ajax({
                type: "post",
                url: "/Ajax/listeningmobile.ashx",
                data: "id=" + id + "",
                success: function (data) {
                    if (data != null && data != "" && data.message == "200") {
                        closeLoadingDialog();
                        location.href = "mobilecontrol.aspx";
                    } else {
                        closeLoadingDialog();
                        openMessageDialog(data.message);
                        return false;
                    }
                },
                error: function (XMLHttpRequest, data, errorThrown) {
                    closeLoadingDialog();
                    openMessageDialog(errorThrown);
                }
            });
        }
    </script>
</body>
</html>

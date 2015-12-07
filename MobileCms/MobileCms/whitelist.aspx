<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="whitelist.aspx.cs" Inherits="MobileCms.whitelist" %>

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
                    <span>亲情号码</span>
                    <div class="brad">
                        <a href="#">通讯录管理</a>
                        >
                        <a href="#">亲情号码</a>
                    </div>
                </div>

                <div class="content ps">
                    <div class="title">
                        <form runat="server" id="queryForm" name="queryForm" method="post" action="">
                            &nbsp;&nbsp;&nbsp;&nbsp;微信用户名：
                            <asp:TextBox ID="userName" runat="server"></asp:TextBox>
                            <div class="queryform_btn btn">
                                <label>
                                    <asp:Button ID="butSearch" CssClass="inputbtn" runat="server" Text="查询" onclick="butSearch_Click" />    
                                </label>
                                <%--<a href="javascript:;" onclick="batchDelete()">
                                    <span>批量删除</span>
                                </a>--%>
                            </div>
                        </form>
                    </div>

                    <div class="public_jia_video">
                        <table class="table" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <%--<th width="6%">
                                    <a onclick="SelectAll()" href="#">全/反选</a>
                                </th>--%>

                                <th width="12%">微信用户名</th>
                                <th width="10%">微信昵称</th>
                                <th width="10%">设备用户名</th>
                                <th width="10%">设备昵称</th>
                                <th width="5%">状态</th>
                                <th width="10%">创建日期</th>
                                <th width="10%">操作</th>
                            </tr>

                            <asp:Repeater ID="rptList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <%--<td>
                                            <input type="checkbox" value='<%#Eval("Id")%>' name="checkbox" />
                                        </td>--%>
                                        <td><%#Eval("WeiXinUserName")%></td>
                                        <td><%#Eval("WeiXinDisplayName")%></td>
                                        <td><%#Eval("MobileUserName")%></td>
                                        <td><%#Eval("MobileDisplayName")%></td>
                                        <td><%#Eval("Status")%></td>
                                        <td><%#Eval("CreateTime")%></td>
                                        <td>
                                            <a class="" href="javascript:;" onclick="openWhiteListMember('<%#Eval("MobileContactId")%>');">查看亲情号码</a>
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

    <script type="text/javascript">
        function openWhiteListMember(id) {
            var timestamp = new Date().getTime();
            location.href = "whitelistmember.aspx?id=" + id + "&t=" + timestamp;
        }
    </script>
</body>
</html>

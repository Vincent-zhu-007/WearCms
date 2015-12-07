<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mobileappwhitelistitem.aspx.cs" Inherits="MobileCms.mobileappwhitelistitem" %>

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
                    <span>查看应用</span>
                    <div class="brad">
                        <a href="javascript:;">应用管理</a>
                        >
                        <a href="mobileappwhitelist.aspx">应用白名单</a>
                        >
                        <a href="javascript:;">查看应用</a>
                    </div>
                </div>

                <div class="content ps">
                    <div class="title">
                        <form runat="server" id="queryForm" name="queryForm" method="post" action="">
                            &nbsp;&nbsp;&nbsp;&nbsp;应用名称：
                            <asp:TextBox ID="description" runat="server"></asp:TextBox>
                            <div class="queryform_btn btn">
                                <label>
                                    <asp:Button ID="butSearch" CssClass="inputbtn" runat="server" Text="查询" onclick="butSearch_Click" />    
                                </label>
                                <a href="/createmobileappwhitelistitem.aspx?id=<%=mobileAppId%>">
                                    <span>增加</span>
                                </a>
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
                                <th width="10%">用户名</th>
                                <th width="10%">昵称</th>
                                <th width="10%">应用编码</th>
                                <th width="10%">应用名称</th>
                                <th width="10%">应用包名</th>
                                <th width="10%">操作</th>
                            </tr>

                            <asp:Repeater ID="rptList" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <%--<td>
                                            <input type="checkbox" value='<%#Eval("Id")%>' name="checkbox" />
                                        </td>--%>
                                        <td><%#Eval("UserName")%></td>
                                        <td><%#Eval("DisplayName")%></td>
                                        <td><%#Eval("AppCodeName")%></td>
                                        <td><%#Eval("Description")%></td>
                                        <td><%#Eval("PackageName")%></td>
                                        <td>
                                            <a class="" href="javascript:;" onclick="openDeleteConfirmDialog('<%#Eval("Id")%>');">删除</a>
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
        <p id="confirmDialog_Message">您确认删除数据？</p>
    </div>

    <script type="text/javascript">
        function openDeleteConfirmDialog(id) {
            openConfirmDialog("delete", id);
        }

        function deleteById(id) {
            openLoadingDialog();

            $.ajax({
                type: "post",
                url: "/Ajax/deletemobileappwhitelistitem.ashx",
                data: "id=" + id + "",
                success: function (data) {
                    if (data != null && data != "" && data.message == "200") {
                        closeLoadingDialog();
                        var timestamp = new Date().getTime();
                        location.href = "mobileappwhitelistitem.aspx?id=<%=mobileAppId%>&t=" + timestamp;
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

        function batchDelete() {
            var ids = "";

            $('input:checkbox[name=checkbox]:checked').each(function (i) {
                if (0 == i) {
                    ids = $(this).val();
                } else {
                    ids += ("," + $(this).val());
                }
            });

            if (ids != null && ids != "") {
                $.ajax({
                    type: "post",
                    url: "/Ajax/batchdeletemobileuser.ashx",
                    data: {
                        "ids": ids
                    },
                    dataType: "json",
                    timeout: 50000,
                    success: function (data) {
                        if (data.message == "200") {
                            alert("删除成功");
                            var timestamp = new Date().getTime();
                            location.href = "mobileappwhitelistitem.aspx?id=<%=mobileAppId %>&t=" + timestamp;
                        } else {
                            alert(data.message);
                            return false;
                        }
                    },
                    error: function (XMLHttpRequest, data, errorThrown) {

                    }
                });
            } else {
                alert("请选择要删除的用户");
                return false;
            }
        }
    </script>
</body>
</html>

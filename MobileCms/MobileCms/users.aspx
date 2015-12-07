<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="MobileCms.users" %>

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
                                <a href="/createuser.aspx">
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
                                <th width="10%">手机号码</th>
                                <th width="5%">性别</th>
                                <th width="8%">生日</th>
                                <th width="5%">状态</th>
                                <th width="12%">创建日期</th>
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
                                        <td><%#Eval("MobilePhone")%></td>
                                        <td><%#Eval("Gender")%></td>
                                        <td><%#Eval("Birthday")%></td>
                                        <td><%#Eval("Status")%></td>
                                        <td><%#Eval("CreateTime")%></td>
                                        <td>
                                            <a class="" href="javascript:;" onclick="mobileUserEdit('<%#Eval("Id")%>');">修改</a>
                                            <a class="" href="javascript:;" onclick="mobileUserDisable('<%#Eval("Id")%>', '<%#Eval("StatusCode")%>');"><%#Eval("DisableOption")%></a>
                                            <a class="" href="javascript:;" onclick="openDeleteConfirmDialog('<%#Eval("Id")%>');">删除</a>
                                            <a class="" href="javascript:;" onclick="openResetPasswordConfirmDialog('<%#Eval("Id")%>');">重置密码</a>
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

    <div id="confirmDialog_ResetPassword" class="index_dialog" title="确认">
        <p id="confirmDialog_ResetPassword_Message">您确认重置密码？</p>
    </div>

    <script type="text/javascript">
        function openDeleteConfirmDialog(id) {
            openConfirmDialog("delete", id);
        }

        function openResetPasswordConfirmDialog(id) {
            openConfirmDialogByResetPassword("resetpassword", id);
        }

        function mobileUserEdit(id) {
            location.href = "edituser.aspx?id=" + id;
        }

        function mobileUserDisable(id, status) {
            $.ajax({
                type: "post",
                url: "/Ajax/disablemobileuser.ashx",
                data: "id=" + id + "&status=" + status + "",
                success: function (data) {
                    if (data != null && data != "" && data.message == "200") {
                        location.href = "users.aspx";
                    } else {
                        alert(data.message);
                        return false;
                    }
                },
                error: function (XMLHttpRequest, data, errorThrown) {
                    alert(errorThrown);
                }
            });
        }

        function deleteById(id) {
            openLoadingDialog();

            $.ajax({
                type: "post",
                url: "/Ajax/deletemobileuser.ashx",
                data: "id=" + id + "",
                success: function (data) {
                    if (data != null && data != "" && data.message == "200") {
                        closeLoadingDialog();
                        location.href = "users.aspx";
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

        function resetPassword(id) {
            openLoadingDialog();

            $.ajax({
                type: "post",
                url: "/Ajax/resetpassword.ashx",
                data: "id=" + id + "",
                success: function (data) {
                    if (data != null && data != "" && data.message == "200") {
                        closeLoadingDialog();
                        location.href = "users.aspx";
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
                            location.href = "users.aspx";
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

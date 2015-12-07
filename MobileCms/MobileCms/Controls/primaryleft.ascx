<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="primaryleft.ascx.cs" Inherits="MobileCms.Controls.primaryleft" %>
<link href="../Css/primaryleft.css" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    function exitSystem() {
        $.ajax({
            type: "post",
            url: "/Ajax/logout.ashx",
            dataType: "json",
            timeout: 50000,
            success: function (data) {
                if (data.message == "200") {
                    location.href = "login.aspx";
                }
            },
            error: function (XMLHttpRequest, data, errorThrown) {
                location.href = "login.aspx";
            }
        });
    }
</script>

<div class="primary_left">
    <div class="logo">
        <div class="logo_message"></div>
        <div class="logo_user">欢迎您，<%=userName%></div>
        <div class="logo_close">
            <a href="javascript:exitSystem()">退出系统</a>
        </div>
    </div>

    <dl>
        <%=webSiteMenuTreeHtml %>

        <%--<dt class="dt1 png_bg">通讯录管理</dt>
        <dd>
            <ul>
                <li>
                    <a href="../whitelist.aspx">
                        <span class="png_bg"></span>
                        <b class="">通讯录白名单</b>
                    </a>
                </li>
            </ul>
        </dd>

        <dt class="dt2 png_bg">应用管理</dt>
        <dd>
            <ul>
                <li>
                    <a href="../mobileappwhitelist.aspx">
                        <span class="png_bg"></span>
                        <b class="">应用白名单</b>
                    </a>
                </li>
                <li>
                    <a href="../mobileappconfig.aspx">
                        <span class="png_bg"></span>
                        <b class="">应用配置</b>
                    </a>
                </li>
            </ul>
        </dd>

        <dt class="dt7 png_bg">系统管理</dt>
        <dd>
            <ul>
                <li>
                    <a href="../users.aspx">
                        <span class="png_bg"></span>
                        <b class="">用户管理</b>
                    </a>
                </li>
                <li>
                    <a href="../modifypassword.aspx">
                        <span class="png_bg"></span>
                        <b class="">修改密码</b>
                    </a>
                </li>
            </ul>
        </dd>--%>

    </dl>
</div>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="modifypassword.aspx.cs" Inherits="MobileCms.modifypassword" %>

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
    <link href="Css/validator2.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .table td 
        {
            border-bottom: 1px solid #dedede;
        }
    </style>

    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/dist/libs/jquery.js" type="text/javascript"></script>

    <script src="Scripts/formValidator.js" type="text/javascript"></script>
    <script src="Scripts/jquery.form1.js" type="text/javascript"></script>
    <script src="Scripts/formValidatorRegex.js" type="text/javascript"></script>

    <script src="Scripts/jqueryui/jquery-ui.js" type="text/javascript"></script>
    <script src="CustScripts/datehelp.js" type="text/javascript"></script>
</head>
<body>
    <div class="wrapper">
        <uc1:primaryLeft ID="PrimaryLeft1" runat="server" />

        <div class="primary_right">
            <div class="inner">
                <div class="position ps">
                    <span>修改密码</span>
                    <div class="brad">
                        <a href="#">管理账户</a>
                        >
                        <a href="#">修改密码</a>
                    </div>
                </div>

                <div class="content ps">
                    <h2 class="h2">
                        修改密码
                        <span>*标有星号的为必填项</span>
                    </h2>

                    <form id="modifyPasswordForm" name="modifyPasswordForm" method="post" action="">
                        <table class="table add" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td class="key" width="28%">原密码</td>
                                <td width="72%">
                                    <div class="out">
                                        <input id="oldPassword" name="oldPassword" class="left" type="password" value="" />
                                        <span class="left">*</span>
                                        <label id="oldPasswordTip"></label>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td class="key" width="28%">新密码</td>
                                <td width="72%">
                                    <div class="out">
                                        <input id="newPassword" name="newPassword" class="left" type="password" value="" />
                                        <span class="left">*</span>
                                        <label id="newPasswordTip"></label>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td class="key" width="28%">确认密码</td>
                                <td width="72%">
                                    <div class="out">
                                        <input id="rePassword" name="rePassword" class="left" type="password" value="" />
                                        <span class="left">*</span>
                                        <label id="rePasswordTip"></label>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td class="28%"></td>
                                <td class="72%">
                                    <div class="tj_btn bt" style="display: block;">
                                        <a href="javascript:;" onclick="save();">
                                            <span class="a">保存</span>
                                        </a>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </form>
                </div>

            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(function () {
            $("#modifyPasswordForm").resetForm();

            $.formValidator.initConfig({
                formid: "modifyPasswordForm",
                submitbuttonid: "modifyPasswordFormSub",
                validatorgroup: "1",
                errorfocus: true,
                onerror: function () {
                    return false;
                }
            });

            checkOldPassword();
            checkNewPassword();
            checkRePassword();
        })

        function checkOldPassword() {
            $("#oldPassword").formValidator({
                validatorgroup: "1",
                onempty: "请输入原密码",
                onshow: "请输入原密码",
                onfocus: "请输入原密码"
            }).inputValidator({
                min: 6,
                max: 12,
                onerror: "6-12个字符"
            }).regexValidator({
                regexp: "username",
                datatype: "enum",
                onerror: "数字或字母或下划线组成"
            });
        };

        function checkNewPassword() {
            $("#newPassword").formValidator({
                validatorgroup: "1",
                onempty: "请输入新密码",
                onshow: "请输入新密码",
                onfocus: "请输入新密码"
            }).inputValidator({
                min: 6,
                max: 12,
                onerror: "6-12个字符"
            }).regexValidator({
                regexp: "username",
                datatype: "enum",
                onerror: "数字或字母或下划线组成"
            });
        };

        function checkRePassword() {
            $("#rePassword").formValidator({
                validatorgroup: "1",
                onempty: "请输入确认密码",
                onshow: "请输入确认密码",
                onfocus: "请输入确认密码"
            }).inputValidator({
                min: 6,
                max: 12,
                onerror: "6-12个字符"
            }).regexValidator({
                regexp: "username",
                datatype: "enum",
                onerror: "数字或字母或下划线组成"
            });
        };

        function showResponse(responseText, statusText, xhr, $form) {
            if (responseText.message == "200") {
                alert("保存成功");
                $("#modifyPasswordForm").resetForm();
            } else {
                alert(responseText.message);
                return false;
            }
        }

        function validate() {
            checkOldPassword();
            checkNewPassword();
            checkRePassword();

            if ($.formValidator.pageIsValid("1")) {
                return true;
            } else {
                $(".onFocus").removeAttr("style");
                return false;
            }
        }

        function save() {
            $("#modifyPasswordForm").ajaxSubmit({
                url: "/Ajax/modifypassword.ashx",
                success: showResponse,
                beforeSubmit: validate,
                error: function (XmlHttpRequest, textStatus, errorThrown) {
                    alert("操作失败，请稍后再试");
                }
            });

            return false;
        }
    </script>
</body>
</html>

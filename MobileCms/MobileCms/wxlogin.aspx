<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxlogin.aspx.cs" Inherits="MobileCms.wxlogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=0, minimum-scale=0.5, maximum-scale=2.0, user-scalable=no" /> 
    <title><%=pageTitle%></title>
    <link href="WxCss/wxcommon.css" rel="stylesheet" type="text/css" />
    <link href="WxCss/wxvalidator_center.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/formValidator.js" type="text/javascript"></script>
    <script src="Scripts/jquery.form.js" type="text/javascript"></script>
    <script src="Scripts/formValidatorRegex.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#form").resetForm();

            $.formValidator.initConfig({
                formid: "form",
                submitbuttonid: "loginsub",
                validatorgroup: "1",
                errorfocus: false,
                onerror: function () {
                    return false;
                }
            });

            checkMobilePhone();
            checkPassword();

            $("#loginsub").click(function () {
                $("#form").ajaxSubmit({ success: showResponse, beforeSubmit: validate });
            });
        });

        function checkMobilePhone() {
            $("#mobilePhone").formValidator({
                validatorgroup: "1",
                onempty: "请输入手机号码",
                onfocus: "请输入手机号码",
                onshow: "请输入手机号码",
                oncorrect: ""
            }).inputValidator({
                min: 11,
                max: 11,
                onerror: "手机号码格式不正确"
            }).regexValidator({
                regexp: "mobile",
                datatype: "enum",
                onerror: "手机号码格式不正确"
            });
        }

        function checkPassword() {
            $("#password").formValidator({
                validatorgroup: "1",
                onempty: "请输入密码",
                onfocus: "请输入密码",
                onshow: "请输入密码",
                oncorrect: ""
            }).inputValidator({
                min: 6,
                max: 15,
                onerror: "6-15位"
            }).regexValidator({
                regexp: "username",
                datatype: "enum",
                onerror: "字母或数字或下划线"
            });
        }

        function showResponse(responseText, statusText, xhr, $form) {
            if (responseText.result == "1") {
                location.href = "wxeducation.aspx";
            } else {
                alert(responseText.message);
                return false;
            }
        }

        function validate() {
            checkMobilePhone();
            checkPassword();
            
            if ($.formValidator.pageIsValid("1")) {
                return true;
            } else {
                return false;
            }
        }

        function goRegister() {
            location.href = "wxregister.aspx";
        }
    </script>
</head>
<body>
    <div class="wrapper">
        <div class="top">
            <div class="back">
                <a href="javascript:;"></a>
            </div>
            <span class="title">诺维小天使</span>
        </div>

        <div class="content">
            <form id="form" name="form" method="post" defaultbutton="loginsub" action="/Ajax/wxlogin.ashx">
                <input type="hidden" id="openId" name="openId" value="<%=openId%>" />

                <div class="forminput">
                    <div class="forminputicon">
                        <img alt="" src="/WxImages/login_usericon.png" />
                    </div>
                    <input type="text" id="mobilePhone" name="mobilePhone" class="forminputtext" style="text-align:center;" value="" />
                    <span id="mobilePhoneTip"></span>
                </div>

                <div class="forminput">
                    <div class="forminputicon">
                        <img alt="" src="/WxImages/login_passwordicon.png" />
                    </div>
                    <input type="password" id="password" name="password" class="forminputtext" style="text-align:center;" value="" />
                    <span id="passwordTip"></span>  
                </div>
            
                <div class="formbutton">
                    <div class="button">
                        <input type="button" id="loginsub" name="loginsub" class="formbuttonsub" value="登录" />
                    </div>

                    <div class="button">
                        <a href="javascript:;" class="formbuttonlink" style="background-image:url('/WxImages/formregisterbutton.png'); color:#6d7d8a;" onclick="goRegister()">注册</a>    
                    </div>

                    <div class="button">
                        <a href="javascript:;" class="formbuttonlink" style="color:#6d7d8a;">忘记密码?</a>    
                    </div>
                </div>

            </form>
        </div>
    </div>
</body>
</html>

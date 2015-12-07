<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxregister.aspx.cs" Inherits="MobileCms.wxregister" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=0, minimum-scale=0.5, maximum-scale=2.0, user-scalable=no" />
    <title><%=pageTitle%></title>
    <link href="WxCss/wxcommon.css" rel="stylesheet" type="text/css" />
    <link href="WxCss/wxvalidator.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/formValidator.js" type="text/javascript"></script>
    <script src="Scripts/jquery.form.js" type="text/javascript"></script>
    <script src="Scripts/formValidatorRegex.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#form").resetForm();

            $.formValidator.initConfig({
                formid: "form",
                submitbuttonid: "registersub",
                validatorgroup: "1",
                errorfocus: false,
                onerror: function () {
                    return false;
                }
            });

            checkMobilePhone();
            checkDisplayName();
            checkImageCode();
            checkPassword();
            checkRePassword();
            checkSmsCode();

            $("#registersub").click(function () {
                $("#form").ajaxSubmit({ success: showResponse, beforeSubmit: validate });
            });
        })
        
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

        function checkDisplayName() {
            $("#displayName").formValidator({
                validatorgroup: "1",
                onempty: "请输入昵称",
                onfocus: "请输入昵称",
                onshow: "请输入昵称",
                oncorrect: ""
            }).inputValidator({
                min: 1,
                max: 20,
                onerror: "1-20个字符"
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

        function checkRePassword() {
            $("#rePassword").formValidator({
                validatorgroup: "1",
                onempty: "请输入确认密码",
                onfocus: "请输入确认密码",
                onshow: "请输入确认密码",
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

        function checkImageCode() {
            $("#imageCode").formValidator({
                validatorgroup: "1",
                onempty: "请输入图验证码",
                onfocus: "请输入图验证码",
                onshow: "请输入图验证码",
                oncorrect: ""
            }).inputValidator({
                min: 1,
                onerror: "请输入图验证码"
            });
        }

        function checkSmsCode() {
            $("#smsCode").formValidator({
                validatorgroup: "1",
                onempty: "请输入手机验证码",
                onfocus: "请输入手机验证码",
                onshow: "请输入手机验证码",
                oncorrect: ""
            }).inputValidator({
                min: 6,
                max: 6,
                onerror: "6位数字"
            }).regexValidator({
                regexp: "username",
                datatype: "enum",
                onerror: "6位数字"
            });
        }

        function sendSmsCode() {
            var openId = '<%=openId%>';
            var mobilePhone = $("#mobilePhone").val();
            var imageCode = $("#imageCode").val();

            $.ajax({
                type: "post",
                url: "/Ajax/wxsendregistersms.ashx",
                data: {
                    "openId": openId,
                    "mobilePhone": mobilePhone,
                    "imageCode": imageCode
                },
                dataType: "json",
                timeout: 50000,
                success: function (data) {
                    if (data.result == "1") {
                        alert("验证码发送成功.")
                    } else {
                        alert(data.message);
                    }
                },
                error: function (XMLHttpRequest, data, errorThrown) {

                }
            });
        }

        function showResponse(responseText, statusText, xhr, $form) {
            if (responseText.result == "1") {
                location.href = "wxbindmobilehelp.aspx";
            } else {
                alert(responseText.message);
                return false;
            }
        }

        function validate() {
            checkMobilePhone();
            checkDisplayName();
            checkImageCode();
            checkPassword();
            checkRePassword();
            checkSmsCode();

            if ($.formValidator.pageIsValid("1")) {
                return true;
            } else {
                return false;
            }
        }
    </script>
</head>
<body>
    <div class="wrapper">
        <div class="top">
            <div class="back">
                <a href="javascript:;"></a>
            </div>
            <span class="title">注册</span>
        </div>
        <div class="content">
            <form id="form" name="form" method="post" defaultbutton="registersub" action="/Ajax/wxregister.ashx">
                <div class="forminput">
                    <input type="text" id="mobilePhone" name="mobilePhone" class="forminputtext" value="" />
                    <span id="mobilePhoneTip"></span>
                </div>

                <div class="forminput">
                    <input type="text" id="displayName" name="displayName" class="forminputtext" value="" />
                    <span id="displayNameTip"></span>
                </div>

                <div class="forminput">
                    <input type="text" id="smsCode" name="smsCode" class="forminputtext" style="width:60%" value="" />
                    <a href="javascript:;" class="formbuttonscode" onclick="sendSmsCode()">获取验证码</a>
                    <span id="smsCodeTip"></span>
                </div>

                <div class="forminput">
                    <input type="text" name="imageCode" id="imageCode" class="forminputtext" style="width:60%" />

                    <span class="formimagecode">
                        <img style="cursor:pointer; vertical-align:middle; margin-left:10px; width:90%; height:90%;" onclick="this.src='PlugIn/checkcode.aspx?t='+Math.random();" src="PlugIn/checkcode.aspx" class="img_01" alt="点击换一张" />    
                    </span>
                    
                    <span id="imageCodeTip"></span>
                </div>

                <div class="forminput">
                    <input type="password" id="password" name="password" class="forminputtext" value="" />
                    <span id="passwordTip"></span>
                </div>

                <div class="forminput">
                    <input type="password" id="rePassword" name="rePassword" class="forminputtext" value="" />
                    <span id="rePasswordTip"></span>
                </div>
            
                <div class="formbutton">
                    <div class="button">
                        <input type="button" id="registersub" name="registersub" class="formbuttonsub" value="注册" />
                    </div>
                </div>
            </form>
        </div>
    </div>
</body>
</html>

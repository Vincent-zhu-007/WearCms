<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="MobileCms.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>登录</title>
    <link href="Css/common.css" rel="stylesheet" type="text/css" />
    <link href="Css/login.css" rel="stylesheet" type="text/css" />
    <link href="Css/register.css" rel="stylesheet" type="text/css" />
    <link href="Css/retrievepassword.css" rel="stylesheet" type="text/css" />
    <link href="Css/validator.css" rel="stylesheet" type="text/css" />
    <link href="Css/custwindow.css" rel="stylesheet" type="text/css" />

    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/formValidator.js" type="text/javascript"></script>
    <script src="Scripts/jquery.form.js" type="text/javascript"></script>
    <script src="Scripts/formValidatorRegex.js" type="text/javascript"></script>
</head>
<body style="background-color: #f0f0f0; background-image:none;" onkeypress="bindEnter(event)">
    <div id="retrievepassword_window_div" class="window_div">
        <div class="window_div_top">
            <a class="close" href="javascript:;" onclick="closeRetrievePasswordWin()">×</a>
            <h3>Mobile-找回密码</h3>    
        </div>
        <div class="window_div_body">
            <form id="retrievePasswordForm" name="retrievePasswordForm" method="post" defaultbutton="loginmain_btn_btnregsub" action="/Ajax/retrievepassword.ashx">
	  		    <div id="retrievePasswordFormContent">
		    	    <ul>
		    		    <li>
                            <span class="inputlab">手机号码：</span>
						    <input type="text" name="retrievePasswordUserName" id="retrievePasswordUserName" />
                            <span id="retrievePasswordUserNameTip"></span>
		    		    </li>
                        <li>
                            <span class="inputlab">手机验证码：</span>
						    <input type="text" name="retrievePasswordSmsCode" id="retrievePasswordSmsCode" style="width:178px;" />
                            <a class="btn_sms" href="javascript:;" onclick="sendRetrievePasswordSms()">获取验证码</a>
                            <span id="retrievePasswordSmsCodeTip"></span>
		    		    </li>

                        <li>
                            <span class="inputlab">验证码：</span>
						    <input type="text" name="retrievePasswordImageCode" id="retrievePasswordImageCode" style="width:178px;" />
                            <img alt="" style="cursor:pointer; vertical-align:middle; margin-left:10px;" onclick="this.src='PlugIn/checkcode.aspx?t='+Math.random();" src="PlugIn/checkcode.aspx" class="img_01" alt="点击换一张" />
                            <span id="retrievePasswordImageCodeTip"></span>
		    		    </li>

		    	    </ul>
                    <span id="retrievePasswordSmsMessage" class="smsmessage"></span>
                    <input type="button" id="loginmain_btn_btnretrievepasswordsub" name="loginmain_btn_btnretrievepasswordsub" class="loginmain_btn_btnretrievepasswordsub" value="" />
	    	    </div>
    	    </form>
        </div>
    </div>

    <div id="loginmain">
  		<form id="loginForm" name="loginForm" method="post" defaultbutton="loginmain_btn_btnlogin" action="/Ajax/login.ashx">
	  		<span class="loginmain_title">诺维可穿戴设备管理平台</span>
	  		<div id="loginFormContent">
		    	<ul>
		    		<li class="usernameli">
						<input type="text" name="userName" id="userName" />
                        <span id="userNameTip"></span>
		    		</li>
		    		<li class="passwordli">
						<input type="password" name="password" id="password" />
                        <span id="passwordTip"></span>
		    		</li>
		    	</ul>
	    	</div>
	    	
	    	<span class="loginmain_btn">
                <input type="button" id="loginmain_btn_btnlogin" name="loginmain_btn_btnlogin" class="loginmain_btn_btnlogin" value="登录" />
                <%--<input type="button" id="loginmain_btn_btnreg" name="loginmain_btn_btnreg" class="loginmain_btn_btnreg" value="立即注册 >>" />
                <input type="button" id="loginmain_btn_btnretrievepassword" name="loginmain_btn_btnretrievepassword" class="loginmain_btn_btnretrievepassword" value="找回密码 >>" />--%>
            </span>
    	</form>
  	</div>
  	
  	<div id="loginfoot"></div>

    <div class="overlay" style="z-index:100;"></div>
</body>

<script type="text/javascript">
    $(function () {
        $(".overlay").hide();

        var date = new Date();
        $("#loginfoot").html('诺维    ©' + date.getFullYear() + '  NuoWei All Rights Reserved');

        $("#loginForm").submit(function () {
            $("#loginForm").ajaxSubmit({ success: showResponse, beforeSubmit: validate });
            return false;
        });

        $("#regForm").submit(function () {
            $("#regForm").ajaxSubmit({ success: showRegResponse, beforeSubmit: regValidate });
            return false;
        });

        $.formValidator.initConfig({
            formid: "loginForm",
            submitbuttonid: "loginmain_btn_btnlogin",
            validatorgroup: "1",
            errorfocus: false,
            onerror: function () {
                return false;
            }
        });

        $.formValidator.initConfig({
            formid: "regForm",
            submitbuttonid: "loginmain_btn_btnregsub",
            validatorgroup: "2",
            errorfocus: false,
            onerror: function () {
                return false;
            }
        });

        /*$("#userName").hover(
        function () {
        var classValue = $("#userNameTip").attr("class");

        if (classValue == null || classValue == "") {
        checkUserName();
        }
        },
        function () {
        var classValue = $("#userNameTip").attr("class");

        if (classValue != null && classValue != "" && classValue == "onShow") {
        $("#userNameTip").html("");
        $("#userNameTip").removeClass();
        }
        }
        );*/

        /*$("#password").hover(
        function () {
        var classValue = $("#passwordTip").attr("class");

        if (classValue == null || classValue == "") {
        checkPassword();
        }
        },
        function () {
        var classValue = $("#passwordTip").attr("class");

        if (classValue != null && classValue != "" && classValue == "onShow") {
        $("#passwordTip").html("");
        $("#passwordTip").removeClass();
        }
        }
        );*/

        $("#loginmain_btn_btnlogin").click(function () {
            $("#loginForm").submit();
        });

        $("#loginmain_btn_btnregsub").click(function () {
            $("#regForm").submit();
        });

        $("#loginmain_btn_btnlogin").keyup(function (event) {
            if (event.keyCode == 13) {
                doLogin();
                return false;
            }
        });

        $("#loginmain_btn_btnreg").click(function () {
            openRegWin();
        });

        /*$("#regUserName").hover(
        function () {
        var classValue = $("#regUserNameTip").attr("class");

        if (classValue == null || classValue == "") {
        checkRegUserName();
        }
        },
        function () {
        var classValue = $("#regUserNameTip").attr("class");

        if (classValue != null && classValue != "" && classValue == "onShow") {
        $("#regUserNameTip").html("");
        $("#regUserNameTip").removeClass();
        }
        }
        );*/

        /*$("#regPassword").hover(
        function () {
        var classValue = $("#regPasswordTip").attr("class");

        if (classValue == null || classValue == "") {
        checkRegPassword();
        }
        },
        function () {
        var classValue = $("#regPasswordTip").attr("class");

        if (classValue != null && classValue != "" && classValue == "onShow") {
        $("#regPasswordTip").html("");
        $("#regPasswordTip").removeClass();
        }
        }
        );*/

        /*$("#regRePassword").hover(
        function () {
        var classValue = $("#regRePasswordTip").attr("class");

        if (classValue == null || classValue == "") {
        checkRegRePassword();
        }
        },
        function () {
        var classValue = $("#regRePasswordTip").attr("class");

        if (classValue != null && classValue != "" && classValue == "onShow") {
        $("#regRePasswordTip").html("");
        $("#regRePasswordTip").removeClass();
        }
        }
        );*/
    });

    function checkUserName() {
        $("#userName").formValidator({
            validatorgroup: "1",
            onempty: "请输入用户名",
            onfocus: "请输入用户名",
            oncorrect: ""
        }).regexValidator({
            regexp: "weixinuser",
            datatype: "enum",
            onerror: "用户名格式错误"
        });
    };

    function checkPassword() {
        $("#password").formValidator({
            validatorgroup: "1",
            onempty: "请输入密码",
            onfocus: "请输入密码",
            oncorrect: ""
        }).inputValidator({
            min: 6,
            max: 15,
            onerror: "必须6-15位"
        }).regexValidator({
            regexp: "username",
            datatype: "enum",
            onerror: "字母或数字或下划线"
        });
    }

    function checkRegUserName() {
        $("#regUserName").formValidator({
            validatorgroup: "2",
            onempty: "请输入手机号码",
            onfocus: "请输入手机号码",
            oncorrect: ""
        }).inputValidator({
            min: 11,
            max: 11,
            onerror: "手机号码有误"
        }).regexValidator({
            regexp: "mobile",
            datatype: "enum",
            onerror: "手机号码格式不正确"
        });
    }

    function checkRegDisplayName() {
        $("#regDisplayName").formValidator({
            validatorgroup: "2",
            onempty: "请输入昵称",
            onfocus: "请输入昵称",
            oncorrect: ""
        }).regexValidator({
            regexp: "chinese",
            datatype: "enum",
            onerror: "昵称必须是中文"
        });
    };

    function checkRegPassword() {
        $("#regPassword").formValidator({
            validatorgroup: "2",
            onempty: "请输入密码",
            onfocus: "请输入密码",
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

    function checkRegRePassword() {
        $("#regRePassword").formValidator({
            validatorgroup: "2",
            onempty: "请输入确认密码",
            onfocus: "请输入确认密码",
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

    function checkSmsCode() {
        $("#smsCode").formValidator({
            validatorgroup: "2",
            onempty: "请输入手机验证码",
            onfocus: "请输入手机验证码",
            oncorrect: ""
        }).inputValidator({
            min: 6,
            max: 6,
            onerror: "6位"
        }).regexValidator({
            regexp: "username",
            datatype: "enum",
            onerror: "字母或数字"
        });
    }

    function checkImageCode() {
        $("#imageCode").formValidator({
            validatorgroup: "2",
            onempty: "请输入验证码",
            onfocus: "请输入验证码",
            oncorrect: ""
        }).inputValidator({
            min: 1,
            onerror: "请输入验证码"
        });
    }

    function showResponse(responseText, statusText, xhr, $form) {
        if (responseText.message == "200") {
            location.href = "index.aspx";
        } else {
            alert(responseText.message);
            return false;
        }
    }

    function showRegResponse(responseText, statusText, xhr, $form) {
        $("#smsMessage").text("");

        if (responseText.message == "200") {
            location.href = "videohistory.aspx";
        } else {
            $("#smsMessage").text(responseText.message);
            return false;
        }
    }

    function validate() {
        checkUserName();

        checkPassword();

        if ($.formValidator.pageIsValid("1")) {
            return true;
        } else {
            $(".onFocus").removeAttr("style");
            return false;
        }
    }

    function regValidate() {
        checkRegUserName();

        checkRegDisplayName();

        checkRegPassword();

        checkRegRePassword();

        checkSmsCode();

        checkImageCode();

        if ($.formValidator.pageIsValid("2")) {
            return true;
        } else {
            $(".onFocus").removeAttr("style");
            return false;
        }
    }

    function bindEnter(obj) {
        var loginBtn = $("#loginmain_btn_btnlogin");
        if (obj.keyCode == 13) {
            loginBtn.click();
            obj.returnValue = false;
        }
    }

    function openRegWin() {
        $(".overlay").show();

        $("#loginForm").get(0).reset();
        $("#regForm").get(0).reset();
        $("#retrievePasswordForm").get(0).reset();

        var clientWidth = document.documentElement.clientWidth;
        var windowWidth = $("#reg_window_div").width();

        var left = (clientWidth - windowWidth) / 2;

        if ($("#reg_window_div").css("display") == "none") {
            $("#reg_window_div").attr("style", "display:block; margin-left:" + left + "px;");
            return false;
        } else {
            $("#reg_window_div").attr("style", "display:none;");
            return false;
        }
    }

    function closeRegWin() {
        $("#reg_window_div").attr("style", "display:none;");
        $(".overlay").hide();
        return false;
    }

    function sendSms() {
        $("#smsMessage").text("");

        var regUserName = $("#regUserName").val();

        $.ajax({
            type: "post",
            url: "/Ajax/sendsms.ashx",
            data: {
                "regUserName": regUserName
            },
            dataType: "json",
            timeout: 50000,
            success: function (data) {
                if (data.message == "200") {
                    $("#smsMessage").text("验证码发送成功");
                } else {
                    $("#smsMessage").text(data.message);
                }
            },
            error: function (XMLHttpRequest, data, errorThrown) {

            }
        });
    }

    /*retrieve password*/
    $("#loginmain_btn_btnretrievepassword").click(function () {
        openRetrievePasswordWin();
    });

    function openRetrievePasswordWin() {
        $(".overlay").show();

        $("#loginForm").get(0).reset();
        $("#regForm").get(0).reset();
        $("#retrievePasswordForm").get(0).reset();

        var clientWidth = document.documentElement.clientWidth;
        var windowWidth = $("#retrievepassword_window_div").width();

        var left = (clientWidth - windowWidth) / 2;

        var top = 32;

        if ($("#retrievepassword_window_div").css("display") == "none") {
            $("#retrievepassword_window_div").attr("style", "display:block; margin-left:" + left + "px; margin-top:" + top + "px;");
            return false;
        } else {
            $("#retrievepassword_window_div").attr("style", "display:none;");
            return false;
        }
    }

    function closeRetrievePasswordWin() {
        $("#retrievepassword_window_div").attr("style", "display:none;");
        $(".overlay").hide();
        return false;
    }

    $("#retrievePasswordForm").submit(function () {
        $("#retrievePasswordForm").ajaxSubmit({ success: showRetrievePasswordResponse, beforeSubmit: retrievePasswordValidate });
        return false;
    });

    $("#loginmain_btn_btnretrievepasswordsub").click(function () {
        $("#retrievePasswordForm").submit();
    });

    $.formValidator.initConfig({
        formid: "retrievePasswordForm",
        submitbuttonid: "loginmain_btn_btnretrievepasswordsub",
        validatorgroup: "3",
        errorfocus: false,
        onerror: function () {
            return false;
        }
    });

    function checkRetrievePasswordUserName() {
        $("#retrievePasswordUserName").formValidator({
            validatorgroup: "3",
            onempty: "请输入手机号码",
            onfocus: "请输入手机号码",
            oncorrect: ""
        }).inputValidator({
            min: 11,
            max: 11,
            onerror: "手机号码有误"
        }).regexValidator({
            regexp: "mobile",
            datatype: "enum",
            onerror: "手机号码格式不正确"
        });
    }

    function checkRetrievePasswordSmsCode() {
        $("#retrievePasswordSmsCode").formValidator({
            validatorgroup: "3",
            onempty: "请输入手机验证码",
            onfocus: "请输入手机验证码",
            oncorrect: ""
        }).inputValidator({
            min: 6,
            max: 6,
            onerror: "6位"
        }).regexValidator({
            regexp: "username",
            datatype: "enum",
            onerror: "字母或数字"
        });
    }

    function checkRetrievePasswordImageCode() {
        $("#retrievePasswordImageCode").formValidator({
            validatorgroup: "3",
            onempty: "请输入验证码",
            onfocus: "请输入验证码",
            oncorrect: ""
        }).inputValidator({
            min: 1,
            onerror: "请输入验证码"
        });
    }

    function showRetrievePasswordResponse(responseText, statusText, xhr, $form) {
        $("#retrievePasswordSmsMessage").text("");

        if (responseText.message == "200") {
            closeRetrievePasswordWin();
            alert("密码找回成功，请查看短信");
        } else {
            $("#retrievePasswordSmsMessage").text(responseText.message);
            return false;
        }
    }

    function retrievePasswordValidate() {

        checkRetrievePasswordUserName();

        checkRetrievePasswordSmsCode();

        checkRetrievePasswordImageCode();

        if ($.formValidator.pageIsValid("3")) {
            return true;
        } else {
            $(".onFocus").removeAttr("style");
            return false;
        }
    }

    function sendRetrievePasswordSms() {
        $("#retrievePasswordSmsMessage").text("");

        var retrievePasswordUserName = $("#retrievePasswordUserName").val();

        $.ajax({
            type: "post",
            url: "/Ajax/sendsmsforretrievepassword.ashx",
            data: {
                "retrievePasswordUserName": retrievePasswordUserName
            },
            dataType: "json",
            timeout: 50000,
            success: function (data) {
                if (data.message == "200") {
                    $("#retrievePasswordSmsMessage").text("验证码发送成功");
                } else {
                    $("#retrievePasswordSmsMessage").text(data.message);
                }
            },
            error: function (XMLHttpRequest, data, errorThrown) {

            }
        });
    }

</script>

</html>

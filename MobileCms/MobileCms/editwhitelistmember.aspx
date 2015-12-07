<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="editwhitelistmember.aspx.cs" Inherits="MobileCms.editwhitelistmember" %>

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
    <link href="Scripts/jqueryui/jquery-ui.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .table td 
        {
            border-bottom: 1px solid #dedede;
        }
        
        #inputData img
        {
            position:absolute;
            margin-top:1px;
            margin-left:5px;
        }
        
        #inputData img:hover
        {
            cursor:pointer;
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
                    <span>编辑</span>
                    <div class="brad">
                        <a href="javascript:;">通讯录管理</a>
                        >
                        <a href="whitelist.aspx">亲情号码</a>
                        >
                        <a href="whitelistmember.aspx?id=<%=mobileContactId%>">查看亲情号码</a>
                        >
                        <a href="javascript:;">编辑</a>
                    </div>
                </div>

                <div class="content ps">
                    <h2 class="h2">
                        编辑
                        <span>*标有星号的为必填项</span>
                    </h2>

                    <form id="editMobileContactMemberForm" name="editMobileContactMemberForm" method="post" action="">
                        <table class="table add" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td class="key" width="28%">姓名</td>
                                <td width="72%">
                                    <div class="out">
                                        <input type="hidden" id="mobileContactId" name="mobileContactId" value="<%=mobileContactId%>" />
                                        <input type="hidden" id="mobileContactMemberId" name="mobileContactMemberId" value="<%=mobileContactMemberId%>" />
                                        <input type="text" id="displayName" name="displayName" class="left" value="<%=displayName%>" />
                                        <span class="left">*</span>
                                        <label id="displayNameTip"></label>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td class="key" width="28%">手机号码</td>
                                <td width="72%">
                                    <div class="out">
                                        <input type="text" id="mobilePhone" name="mobilePhone" class="left" value="<%=mobilePhone%>" />
                                        <span class="left">*</span>
                                        <label id="mobilePhoneTip"></label>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td class="key" width="28%">短号码</td>
                                <td width="72%">
                                    <div class="out">
                                        <input type="text" id="shortNum" name="shortNum" class="left" value="<%=shortNum%>" />
                                        <label id="shortNumTip"></label>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td class="key" width="28%">按键</td>
                                <td width="50%">
                                    <div class="out">
                                        <select id="numButton" name="numButton">
                                            <option value="">请选择</option>
                                            <option value="1">按键1</option>
                                            <option value="2">按键2</option>
                                            <option value="3">按键3</option>
                                            <option value="4">按键4</option>
                                        </select>            
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
                                        <a href="javascript:;" onclick="back();">
                                            <span class="a">返回</span>
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
        var mobileContactId = '<%=mobileContactId%>';

        $(function () {
            $("#editMobileContactMemberForm").resetForm();

            $.formValidator.initConfig({
                formid: "editMobileContactMemberForm",
                submitbuttonid: "editMobileContactMemberFormSub",
                validatorgroup: "1",
                errorfocus: true,
                onerror: function () {
                    return false;
                }
            });

            checkDisplayName();
            checkMobilePhone();

            setSelectChecked("numButton", '<%=numButton%>');
        });

        function checkDisplayName() {
            $("#displayName").formValidator({
                validatorgroup: "1",
                onempty: "请输入昵称",
                onshow: "请输入昵称",
                onfocus: "请输入昵称"
            }).inputValidator({
                min: 1,
                max: 20,
                onerror: "1-20个字符"
            });
        };

        function checkMobilePhone() {
            $("#mobilePhone").formValidator({
                validatorgroup: "1",
                onempty: "请输入手机号码",
                onshow: "请输入手机号码",
                onfocus: "请输入手机号码"
            }).inputValidator({
                min: 11,
                max: 11,
                onerror: "11位手机号码"
            }).regexValidator({
                regexp: "mobile",
                datatype: "enum",
                onerror: "11位手机号码"
            }); ;
        };

        function showResponse(responseText, statusText, xhr, $form) {
            if (responseText.message == "200") {
                alert("保存成功");
                location.href = "whitelistmember.aspx?id=" + mobileContactId;
            } else {
                alert(responseText.message);
                return false;
            }
        }

        function validate() {
            checkDisplayName();
            checkMobilePhone();

            if ($.formValidator.pageIsValid("1")) {
                return true;
            } else {
                $(".onFocus").removeAttr("style");
                return false;
            }
        }

        function save() {
            $("#editMobileContactMemberForm").ajaxSubmit({
                url: "/Ajax/editwhitelistmember.ashx",
                success: showResponse,
                beforeSubmit: validate,
                error: function (XmlHttpRequest, textStatus, errorThrown) {
                    alert("操作失败，请稍后再试");
                }
            });

            return false;
        }

        function back() {
            location.href = "whitelistmember.aspx?id=" + mobileContactId;
        }

        function setSelectChecked(selectId, checkValue) {
            var select = document.getElementById(selectId);
            for (var i = 0; i < select.options.length; i++) {
                if (select.options[i].value == checkValue) {
                    select.options[i].selected = true;
                    break;
                }
            }
        }
    </script>
</body>
</html>
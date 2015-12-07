<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="createmobileappconfig.aspx.cs" Inherits="MobileCms.createmobileappconfig" %>

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
                    <span>增加配置</span>
                    <div class="brad">
                        <a href="#">应用管理</a>
                        >
                        <a href="#">应用配置</a>
                        >
                        <a href="#">增加配置</a>
                    </div>
                </div>

                <div class="content ps">
                    <h2 class="h2">
                        增加配置
                        <span>*标有星号的为必填项</span>
                    </h2>

                    <form id="createMobileAppConfigForm" name="createMobileAppConfigForm" method="post" action="">
                        <table class="table add" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td class="key" width="28%">编码</td>
                                <td width="72%">
                                    <div class="out">
                                        <input type="text" id="codeName" name="codeName" class="left" value="" />
                                        <span class="left">*</span>
                                        <label id="codeNameTip"></label>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td class="key" width="28%">名称</td>
                                <td width="72%">
                                    <div class="out">
                                        <input type="text" id="description" name="description" class="left" value="" />
                                        <span class="left">*</span>
                                        <label id="descriptionTip"></label>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td class="key" width="28%">包名</td>
                                <td width="72%">
                                    <div class="out">
                                        <input type="text" id="packageName" name="packageName" class="left" value="" />
                                        <span class="left">*</span>
                                        <label id="packageNameTip"></label>
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
        $(function () {
            $("#createMobileAppConfigForm").resetForm();

            $.formValidator.initConfig({
                formid: "createMobileAppConfigForm",
                submitbuttonid: "createMobileAppConfigFormSub",
                validatorgroup: "1",
                errorfocus: true,
                onerror: function () {
                    return false;
                }
            });

            checkCodeName();
            checkDescription();
            checkPackageName();
        });

        function checkCodeName() {
            $("#codeName").formValidator({
                validatorgroup: "1",
                onempty: "请输入编码",
                onshow: "请输入编码",
                onfocus: "请输入编码"
            }).inputValidator({
                min: 1,
                max: 20,
                onerror: "1-20个字符"
            }).regexValidator({
                regexp: "letter",
                datatype: "enum",
                onerror: "必须是字母"
            });
        };

        function checkDescription() {
            $("#description").formValidator({
                validatorgroup: "1",
                onempty: "请输入名称",
                onshow: "请输入名称",
                onfocus: "请输入名称"
            }).inputValidator({
                min: 1,
                max: 20,
                onerror: "1-20个字符"
            });
        };

        function checkPackageName() {
            $("#packageName").formValidator({
                validatorgroup: "1",
                onempty: "请输入包名",
                onshow: "请输入包名",
                onfocus: "请输入包名"
            }).inputValidator({
                min: 1,
                max: 40,
                onerror: "1-40个字符"
            });
        };

        function showResponse(responseText, statusText, xhr, $form) {
            if (responseText.message == "200") {
                alert("保存成功");
                location.href = "mobileappconfig.aspx";
            } else {
                alert(responseText.message);
                return false;
            }
        }

        function validate() {
            checkCodeName();
            checkDescription();
            checkPackageName();

            if ($.formValidator.pageIsValid("1")) {
                return true;
            } else {
                $(".onFocus").removeAttr("style");
                return false;
            }
        }

        function save() {
            $("#createMobileAppConfigForm").ajaxSubmit({
                url: "/Ajax/createmobileappconfig.ashx",
                success: showResponse,
                beforeSubmit: validate,
                error: function (XmlHttpRequest, textStatus, errorThrown) {
                    alert("操作失败，请稍后再试");
                }
            });

            return false;
        }

        function back() {
            location.href = "mobileappconfig.aspx";
        }
    </script>
</body>
</html>

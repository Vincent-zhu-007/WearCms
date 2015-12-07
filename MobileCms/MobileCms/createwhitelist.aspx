<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="createwhitelist.aspx.cs" Inherits="MobileCms.createwhitelist" %>

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

    <link href="Scripts/dist/themes/default/style1.css" rel="stylesheet" type="text/css" />

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

    <script src="Scripts/dist/jstree.min.js" type="text/javascript"></script>
    <script src="Scripts/jqueryui/jquery-ui.js" type="text/javascript"></script>
    <script src="CustScripts/datehelp.js" type="text/javascript"></script>
</head>
<body>
    <div class="wrapper">
        <uc1:primaryLeft ID="PrimaryLeft1" runat="server" />

        <div class="primary_right">
            <div class="inner">
                <div class="position ps">
                    <span>增加成员</span>
                    <div class="brad">
                        <a href="javascript:;">通讯录管理</a>
                        >
                        <a href="whitelist.aspx">通讯录白名单</a>
                        >
                        <a href="whitelistmember.aspx?id=<%=mobileContactId%>">查看成员</a>
                        >
                        <a href="javascript:;">增加成员</a>
                    </div>
                </div>

                <div class="content ps">
                    <h2 class="h2">
                        增加成员
                    </h2>

                    <form id="createMobileContactMemberForm" name="createMobileContactMemberForm" method="post" action="">
                        <table class="table add" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td class="key" width="1%"></td>
                                <td width="99%">
                                    <input type="hidden" id="mobileContactId" name="mobileContactId" value="<%=mobileContactId%>" />
                                    <input type="hidden" id="contactMemberUris" name="contactMemberUris" value="" />
                                    <div id="createMobileContactMemberTree" style="background:#fcfcfc none repeat scroll 0 0;"></div>
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
            $("#createMobileContactMemberForm").resetForm();

            initCreateMobileContactMemberTree();

            $.formValidator.initConfig({
                formid: "createMobileContactMemberForm",
                submitbuttonid: "createMobileContactMemberFormSub",
                validatorgroup: "1",
                errorfocus: true,
                onerror: function () {
                    return false;
                }
            });
        });

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
            if ($.formValidator.pageIsValid("1")) {
                var contactMemberUris = $("#contactMemberUris").val();

                if (contactMemberUris != null && contactMemberUris != "") {
                    return true;
                } else {
                    alert("请选择要添加的用户");
                    return false;
                }

                return true;
            } else {
                $(".onFocus").removeAttr("style");
                return false;
            }
        }

        function save() {
            var contactMemberUris = $("#createMobileContactMemberTree").jstree("get_checked");

            $("#contactMemberUris").val(contactMemberUris)

            $("#createMobileContactMemberForm").ajaxSubmit({
                url: "/Ajax/createwhitelistmember.ashx",
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

        function initCreateMobileContactMemberTree() {
            var listType = "WhiteList";
            $("#createMobileContactMemberTree").jstree("destroy");

            $('#createMobileContactMemberTree').jstree({
                "core": {
                    "animation": 0,
                    "check_callback": true,
                    "themes": { "stripes": true },
                    'data': {
                        'url': '/Ajax/initcreatemobilecontactmembertree.ashx?mobileContactId=' + mobileContactId,
                        'data': function (node) {
                            return { 'id': node.id };
                        }
                    }
                },
                "checkbox": {
                    "keep_selected_style": false
                },
                "plugins": ["checkbox"]
            }).bind("loaded.jstree", function (event, data) {
                $("#createMobileContactMemberTree").jstree("open_all");
            });
        }
    </script>
</body>
</html>

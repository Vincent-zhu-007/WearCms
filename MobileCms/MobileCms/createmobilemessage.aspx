<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="createmobilemessage.aspx.cs" Inherits="MobileCms.createmobilemessage" %>

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
        
        .custbottom_left{
		    float: left;
		    width: 200px;
		    height: 200px;
		    padding-top:10px;
		    padding-bottom:10px;
	    }
	
	    .custbottom_middle{
		    float: left;
		    width: 27px;
		    height: 187px;
		    padding-left: 10px;
		    padding-right: 10px;
		    padding-top: 13px;
	    }
	
	    .custbottom_right{
		    float: left;
		    width: 200px;
		    height: 200px;
		    padding-top:10px;
		    padding-bottom:10px;
	    }
	
	    .multiple{
		    width: 198px;
		    height: 198px;
		    border: 1px solid #95b8e7;
	    }
	
	    .multiple option{
		    font:12px/18px "微软雅黑","宋体","黑体",Arial;
	    }
	
	    .btn_multiple{
		    width: 27px;
		    height: 27px;
		    margin-top: 13px;
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
    <script src="CustScripts/common.js" type="text/javascript"></script>
</head>
<body>
    <div class="wrapper">
        <uc1:primaryLeft ID="PrimaryLeft1" runat="server" />

        <div class="primary_right">
            <div class="inner">
                <div class="position ps">
                    <span>增加消息</span>
                    <div class="brad">
                        <a href="javascript:;">系统管理</a>
                        >
                        <a href="mobilemessage.aspx">消息管理</a>
                        >
                        <a href="javascript:;">增加消息</a>
                    </div>
                </div>

                <div class="content ps">
                    <h2 class="h2">
                        增加消息
                        <span>*标有星号的为必填项</span>
                    </h2>

                    <form id="createMobileMessageForm" name="createMobileMessageForm" method="post" action="">
                        <table class="table add" width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td class="key" width="10%">内容</td>
                                <td width="90%">
                                    <div class="out">
                                        <input type="text" id="content" name="content" class="left" value="" />
                                        <span class="left">*</span>
                                        <label id="contentTip"></label>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td class="key" width="10%">用户</td>
                                <td width="90%">
                                    <input type="hidden" id="targetUris" name="targetUris" value="" />

                                    <div class="custbottom_left">
					                    <select id="listboxRemove" multiple="multiple" name="listboxRemove" class="multiple"></select>
				                    </div>
				                    <div class="custbottom_middle">
					                    <input type="button" id="multiple_del" value=">" onclick="multipleDelClick();" class="btn_multiple" style="width: 27px; height: 27px; margin-top: 13px;" />
                                        <input type="button" id="multiple_del_all" value=">>" onclick="multipleDelAllClick();" class="btn_multiple" style="width: 27px; height: 27px; margin-top: 13px;" />
					                    <input type="button" id="multiple_add" value="<" onclick="multipleAddClick();" class="btn_multiple" style="width: 27px; height: 27px; margin-top: 13px;" />
                                        <input type="button" id="multiple_add_all" value="<<" onclick="multipleAddAllClick();" class="btn_multiple" style="width: 27px; height: 27px; margin-top: 13px;" />
				                    </div>
				                    <div class="custbottom_right">
					                    <select id="listboxAdd" multiple="multiple" name="listboxAdd" class="multiple"></select>
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
            $("#createMobileMessageForm").resetForm();

            $.formValidator.initConfig({
                formid: "createMobileMessageForm",
                submitbuttonid: "createMobileMessageFormSub",
                validatorgroup: "1",
                errorfocus: true,
                onerror: function () {
                    return false;
                }
            });

            initListboxRemoveAndListboxAdd();

            /* 添加项[向右移单项移动] */
            multipleAddClick = function () {
                var $options = $('#listboxAdd option:selected');
                var $remove = $options.remove();
                $remove.appendTo("#listboxRemove");
            };

            /* 添加项[向右移全部移动] */
            multipleAddAllClick = function () {
                var $options = $('#listboxAdd option');
                var $remove = $options.remove();
                $remove.appendTo("#listboxRemove");
            };

            /* 移除项[向左移单项移动] */
            multipleDelClick = function () {
                $('#listboxRemove option:selected').appendTo('#listboxAdd');
            };

            /* 移除项[向左移单项移动] */
            multipleDelAllClick = function () {
                $('#listboxRemove option').appendTo('#listboxAdd');
            };

            /* 双击选项 */
            $('#listboxAdd').dblclick(function () { /* 绑定双击事件 */
                /* 获取全部的选项,删除并追加给对方 */
                var $options = $('#listboxAdd option:selected');
                var $remove = $options.remove();
                $remove.appendTo("#listboxRemove");
            });

            /* 双击选项 */
            $('#listboxRemove').dblclick(function () {
                var $options = $('#listboxRemove option:selected');
                var $remove = $options.remove();
                $remove.appendTo("#listboxAdd");
            });

            checkContent();
        });

        function checkContent() {
            $("#content").formValidator({
                validatorgroup: "1",
                onempty: "请输入内容",
                onshow: "请输入内容",
                onfocus: "请输入内容"
            }).inputValidator({
                min: 1,
                max: 60,
                onerror: "1-60个字符"
            });
        };

        function initListboxRemoveAndListboxAdd() {
            $.ajax({
                url: "/Ajax/getremoveandaddmobilemessagehtmloptions.ashx",
                type: "POST",
                async: false,
                dataType: "text",
                success: function (data) {
                    if (data != null && data != "") {
                        var listboxAddOptions = data.split("&")[0];
                        var listboxRemoveOptions = data.split("&")[1];
                        window.parent.$("#listboxAdd").html(listboxAddOptions);
                        window.parent.$("#listboxRemove").html(listboxRemoveOptions);
                    }
                }
            });
        }

        function showResponse(responseText, statusText, xhr, $form) {
            if (responseText.message == "200") {
                alert("保存成功");
                location.href = "mobilemessage.aspx";
            } else {
                alert(responseText.message);
                return false;
            }
        }

        function validate() {
            if ($.formValidator.pageIsValid("1")) {
                var targetUris = $("#targetUris").val();

                if (targetUris != null && targetUris != "") {
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
            var targetUris = "";
            var size = $("#listboxAdd option").size();
            if (size > 0) {
                $.each($("#listboxAdd option"), function (i, own) {

                    var text = $(own).val();
                    if (size - 1 == i) {
                        targetUris += text;
                    } else {
                        targetUris += text + ",";
                    }
                });
            }

            $("#targetUris").val(targetUris);

            $("#createMobileMessageForm").ajaxSubmit({
                url: "/Ajax/createmobilemessage.ashx",
                success: showResponse,
                beforeSubmit: validate,
                error: function (XmlHttpRequest, textStatus, errorThrown) {
                    alert("操作失败，请稍后再试");
                }
            });

            return false;
        }

        function back() {
            location.href = "mobilemessage.aspx";
        }
    </script>
</body>
</html>

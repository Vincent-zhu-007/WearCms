<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="daysummary.aspx.cs" Inherits="MobileCms.daysummary" %>

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
    <link href="Scripts/jqueryui/jquery-ui.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .table tr:hover 
        {
            background-color:#FFECEC;
        }
        
        .ui-datepicker-trigger
        {
            position:absolute;
            margin-top:5px;
            margin-left:5px;
        }
        
        .ui-datepicker-trigger:hover
        {
            cursor:pointer;
        }
    </style>

    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/highcharts.js" type="text/javascript"></script>
    <script src="Scripts/dist/libs/jquery.js" type="text/javascript"></script>
    <script src="Scripts/jqueryui/jquery-ui.js" type="text/javascript"></script>

    <script src="Scripts/formValidator.js" type="text/javascript"></script>
    <script src="Scripts/jquery.form.js" type="text/javascript"></script>
    <script src="Scripts/formValidatorRegex.js" type="text/javascript"></script>

    <script src="CustScripts/common.js" type="text/javascript"></script>
    <script src="CustScripts/datehelp.js" type="text/javascript"></script>
</head>
<body>
    <div class="wrapper">
        <uc1:primaryLeft ID="PrimaryLeft1" runat="server" />

        <div class="primary_right">
            <div class="inner">
                <div class="position ps">
                    <span>24小时汇总</span>
                    <div class="brad">
                        <a href="#">系统管理</a>
                        >
                        <a href="mobilecardreaderreport.aspx">用户行为分析</a>
                        >
                        <a href="#">24小时汇总</a>
                    </div>
                </div>

                <div class="content ps">
                    <div class="title">
                        <form runat="server" id="queryForm" name="queryForm" method="post" action="">
                            &nbsp;&nbsp;&nbsp;&nbsp;日期：
                            <asp:TextBox ID="searchDate" runat="server"></asp:TextBox>
                            <div class="queryform_btn btn">
                                <label>
                                    <asp:Button ID="butSearch" CssClass="inputbtn" runat="server" Text="查询" onclick="butSearch_Click" />    
                                </label>
                            </div>
                        </form>
                    </div>

                    <div class="public_jia_video">
                        <div id="chartBar" style="width:75%; height:400px; float:left"></div>
                    </div>

                </div>

            </div>
        </div>
    </div>
    
    <script type="text/javascript">
        var chart;
        $(function () {
            $("#searchDate").val("");

            setDatePickerZh();

            $("#searchDate").datepicker({
                changeMonth: true,
                changeYear: true,
                showOn: "button",
                buttonImage: "/Images/calendar.png",
                buttonImageOnly: true,
                buttonText: "选择日期"
            });

            /*var line1 = [0, 0, 0, 0, 0, 60, 60, 20, 60, 60, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            var line2 = [60, 60, 60, 60, 60, 0, 0, 40, 0, 0, 50, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60, 60];*/

            var withinData = <%=withinData%>;
            var outsideData = <%=outsideData%>;

            chart = new Highcharts.Chart({
                chart: {
                    renderTo: 'chartBar',
                    type: 'column'
                },
                title: {
                    text: '<%=displayName%>-24小时汇总'
                },
                subtitle: {
                    text: ''
                },
                credits: {
                    enabled: false //移除官网的链接
                },
                xAxis: {
                    categories: ['1:00', '2:00', '3:00', '4:00', '5:00', '6:00', '7:00', '8:00', '9:00', '10:00', '11:00', '12:00', '13:00', '14:00', '15:00', '16:00', '17:00', '18:00', '19:00', '20:00', '21:00', '22:00', '23:00', '24:00']
                },
                yAxis: {
                    min: 0,
                    title: {
                        text: '时长 (分钟)'
                    }
                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.series.name + ':</b>' + this.y + ' 分钟';
                    }
                },
                plotOptions: {
                    column: {
                        pointPadding: 0.2,
                        borderWidth: 0,
                        shadow: false
                    }
                },
                series: [
                {
                    name: '停留时长（分钟）',
                    data: withinData
                },
                {
                    name: '外出时长（分钟）',
                    data: outsideData
                }
                ]
            });
        })
    </script>
</body>
</html>

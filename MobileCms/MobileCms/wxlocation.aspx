<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wxlocation.aspx.cs" Inherits="MobileCms.wxlocation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=0, minimum-scale=0.5, maximum-scale=2.0, user-scalable=no" /> 
    <title></title>
    <link href="WxCss/wxcommon.css" rel="stylesheet" type="text/css" />
    <link href="WxCss/wxlocation.css" rel="stylesheet" type="text/css" />

    <script src="http://api.map.baidu.com/api?v=2.0&ak=hGEz4NNxGy04E1rBuNVbf4kp" type="text/javascript"></script>
    <script src="http://api.map.baidu.com/library/CurveLine/1.5/src/CurveLine.min.js" type="text/javascript"></script>

    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="CustScripts/datecommon.js" type="text/javascript"></script>
</head>
<body>
    <div class="wrapper">
        <div class="top">
            <div class="back">
                <a href="javascript:;"></a>
            </div>
            <span class="title">位置</span>
        </div>

        <div id="allmap"></div>
    </div>

    <script type="text/jscript">
        $(function () {
            var clientHeight = document.documentElement.clientHeight;
            var mapHeight = clientHeight - 172;

            $("#allmap").attr("style", "height:" + mapHeight + "px;");

            initMap();
        });

        function showLocations() {
            var now = new Date();
            var date = formatterDate(now);
            var startTime = date + " 00:00:00";
            var endTime = date + " 23:59:59";

            viewMobileUserLocationTrajectory(startTime, endTime);
        }

        var pts = new Array();

        function viewMobileUserLocationTrajectory(startTime, endTime) {
            map.clearOverlays();

            $.ajax({
                type: "post",
                url: "/Ajax/getmobileuserlocationintime.ashx",
                data: "startTime=" + startTime + "&endTime=" + endTime + "",
                async: false,
                timeout: 50000,
                success: function (data) {
                    if (data != null && data != "") {

                        var array = new Array();

                        for (var i = 0; i < data.length; i++) {
                            var json = data[i];

                            var startLongitude = json.startLongitude;
                            var startLatitude = json.startLatitude;
                            var endLongitude = json.endLongitude;
                            var endLatitude = json.endLatitude;

                            var startPoint = new BMap.Point(startLongitude, startLatitude);
                            var endPoint = new BMap.Point(endLongitude, endLatitude);

                            var points = [startPoint, endPoint];

                            if (i == 0) {
                                map.centerAndZoom(startPoint, 12);

                                var startIcon = new BMap.Icon("/WxImages/startpoint.png", new BMap.Size(32, 32));
                                var startMarker = new BMap.Marker(startPoint, { icon: startIcon });
                                map.addOverlay(startMarker);
                            }

                            var polygon = new BMap.Polygon(points, { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 });
                            map.addOverlay(polygon);

                            pts.push(startPoint);

                            if (i == data.length - 1) {
                                pts.push(endPoint);

                                var endIcon = new BMap.Icon("/WxImages/endpoint.png", new BMap.Size(32, 32));
                                var endMarker = new BMap.Marker(endPoint, { icon: endIcon });
                                map.addOverlay(endMarker);
                            }
                        }
                    } else {
                        alert("暂无位置信息");
                    }
                },
                error: function (XMLHttpRequest, data, errorThrown) {
                    alert("服务器异常，请稍后再试.");
                }
            });
        }
    </script>
    
    <script type="text/javascript">
        //创建和初始化地图函数
        function initMap() {
            createMap(); //创建地图
            setMapEvent(); //设置地图事件
            addMapControl(); //向地图添加控件

            showLocations();
        }

        //创建地图函数：
        function createMap() {
            var map = new BMap.Map("allmap"); //在百度地图容器中创建一个地图
            //var point = new BMap.Point(116.331398, 39.897445); //定义一个中心点坐标
            map.centerAndZoom("西安", 12); //设定地图的中心点和坐标并将地图显示在地图容器中
            map.enableScrollWheelZoom(true);

            if (navigator.userAgent.indexOf("MSIE") > 0) {
                map.panBy(305, 165);
            }

            window.map = map; //将map变量存储在全局
        }

        //地图事件设置函数：
        function setMapEvent() {
            map.enableDragging(); //启用地图拖拽事件，默认启用(可不写)
            map.enableScrollWheelZoom(); //启用地图滚轮放大缩小
            map.enableDoubleClickZoom(); //启用鼠标双击放大，默认启用(可不写)
            map.enableKeyboard(); //启用键盘上下左右键移动地图
        }

        //地图控件添加函数：
        function addMapControl() {
            //向地图中添加缩放控件
            var ctrl_nav = new BMap.NavigationControl({ anchor: BMAP_ANCHOR_TOP_LEFT, type: BMAP_NAVIGATION_CONTROL_LARGE });
            map.addControl(ctrl_nav);
            //向地图中添加缩略图控件
            var ctrl_ove = new BMap.OverviewMapControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT, isOpen: 1 });
            map.addControl(ctrl_ove);
            //向地图中添加比例尺控件
            var ctrl_sca = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_BOTTOM_LEFT });
            map.addControl(ctrl_sca);
        }

        /*function map_load() {
            var load = document.createElement("script");
            load.src = "http://api.map.baidu.com/api?v=2.0&ak=hGEz4NNxGy04E1rBuNVbf4kp&callback=initMap";
            document.body.appendChild(load);
        }*/

        
    </script>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="locations.aspx.cs" Inherits="MobileCms.locations" %>

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

    <style type="text/css">
        #allmap
        {
            width:100%;
            height:100%;
        }
    </style>

    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="http://api.map.baidu.com/api?v=2.0&ak=hGEz4NNxGy04E1rBuNVbf4kp" type="text/javascript"></script>
    <script src="http://api.map.baidu.com/library/CurveLine/1.5/src/CurveLine.min.js" type="text/javascript"></script>
</head>
<body>
    <div class="wrapper">
        <uc1:primaryLeft ID="PrimaryLeft1" runat="server" />

        <div class="primary_right">
            <div class="inner">
                <div class="position ps">
                    <span>位置信息</span>
                    <div class="brad">
                        <a href="#">位置管理</a>
                        >
                        <a href="#">位置信息</a>
                    </div>
                </div>

                <div class="content ps" style="height:100%;">
                    <div id="allmap"></div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>

<script type="text/javascript">
    //创建和初始化地图函数
    function initMap() {
        createMap(); //创建地图
        setMapEvent(); //设置地图事件
        addMapControl(); //向地图添加控件

        theLocation();
    }

    //创建地图函数：
    function createMap() {
        var map = new BMap.Map("allmap"); //在百度地图容器中创建一个地图
        //var point = new BMap.Point(116.331398, 39.897445); //定义一个中心点坐标
        map.centerAndZoom("西安", 5); //设定地图的中心点和坐标并将地图显示在地图容器中
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

    function theLocation() {
        $.ajax({
            type: "post",
            url: "/Ajax/getmobileuserlocation.ashx",
            dataType: "json",
            async: false,
            timeout: 50000,
            success: function (data) {
                map.clearOverlays();
                if (data != null && data != "") {
                    loadLocation(data);
                }
            },
            error: function (XMLHttpRequest, data, errorThrown) {
                alert(errorThrown);
            }
        });
    }

    function loadLocation(markerArray) {
        var markers = new Array();

        map.clearOverlays();
        if (markerArray != null && markerArray != "" && markerArray != "undefined" && markerArray != "[]") {
            for (var i = 0; i < markerArray.length; i++) {
                var json = markerArray[i];

                var ownerUri = json.ownerUri;
                var displayName = json.displayName;
                var longitude = json.longitude;
                var latitude = json.latitude;
                var address = json.address;
                var createTime = json.createTime;

                if (longitude != null && longitude != "" && latitude != null && latitude != "") {

                    var position = new BMap.Point(longitude, latitude);

                    //创建标注
                    markers[i] = new BMap.Marker(position);

                    var content = "<p style=’font-size:12px;line-height:2em;’>用户：" + displayName + "</br> 地址：" + address + "</br> 经度：" + longitude + "</br>纬度：" + latitude + "</br>上报时间：" + createTime + "</p>";

                    //将标注添加到地图中
                    map.addOverlay(markers[i]);

                    addClickHandler(content, markers[i]);
                }
            }
        }
    }

    function addClickHandler(content, marker) {
        marker.addEventListener("click", function (e) {
            openInfo(content, e)
        });
    }

    function openInfo(content, e) {
        var p = e.target;
        var point = new BMap.Point(p.getPosition().lng, p.getPosition().lat);
        var infoWindow = new BMap.InfoWindow(content);  // 创建信息窗口对象 
        map.openInfoWindow(infoWindow, point); //开启信息窗口
    }

    function map_load() {
        var load = document.createElement("script");
        load.src = "http://api.map.baidu.com/api?v=1.4&callback=initMap";
        document.body.appendChild(load);
    }

    window.onload = map_load;
</script>

<%--<script type="text/javascript">
    $(function () {
        window.setInterval(theLocation, 5 * 1000);
    })
</script>--%>

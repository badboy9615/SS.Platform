<%@ Page Language="C#"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>jQuery ligerUI Demos 导航页</title>
    <link href="lib/ligerUI/skins/Aqua/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    </style>
    <script src="lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>    
    <script src="lib/ligerUI/js/core/base.js" type="text/javascript"></script> 
    <script src="lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerResizable.js" type="text/javascript"></script>
        <script src="lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerLayout.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerTab.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerAccordion.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    
    <script src="lib/ligerUI/js/plugins/ligerWindow.js" type="text/javascript"></script>

    <script src="lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
        <script type="text/javascript">
            var tab = null;
            var accordion = null;
            var tree = null;
            $(function ()
            { 

                //布局
                $("#layout1").ligerLayout({ leftWidth: 190, height: '100%', onHeightChanged: f_heightChanged });

                var height = $(".l-layout-center").height();

                //Tab
                $("#framecenter").ligerTab({ height: height });

                //面板
                $("#accordion1").ligerAccordion({ height: height - 24, speed: null });

                $(".l-link").hover(function ()
                {
                    $(this).addClass("l-link-over");
                }, function ()
                {
                    $(this).removeClass("l-link-over");
                });
                //树
                $("#tree1").ligerTree({
                    checkbox: false,
                    nodeWidth: 120,
                    slide: false,
                    attribute: ['nodename', 'url'],
                    onSelect: function (node)
                    {
                        if (!node.data.url) return;
                        var tabid = $(node.target).attr("tabid");
                        if (!tabid)
                        {
                            tabid = new Date().getTime();
                            $(node.target).attr("tabid", tabid)
                        } 
                        f_addTab(tabid, node.data.text, node.data.url);
                    }
                });

                tab = $("#framecenter").ligerGetTabManager();
                accordion = $("#accordion1").ligerGetAccordionManager();
                tree = $("#tree1").ligerGetTreeManager();
                $("#pageloading").hide();
            });

            function f_heightChanged(options)
            {
                if (tab)
                    tab.addHeight(options.diff);
                if (accordion && options.middleHeight - 24 > 0)
                    accordion.setHeight(options.middleHeight - 24);
            }
            function f_addTab(tabid,text, url)
            { 
                tab.addTabItem({ tabid : tabid,text: text, url: url });
            } 
             
            
     </script> 
<style type="text/css"> 
    body,html{height:100%;}
    body{ padding:0px; margin:0;   overflow:hidden;}  
    .l-link{ display:block; height:26px; line-height:26px; padding-left:10px; text-decoration:underline; color:#333;}
    .l-link2{text-decoration:underline; color:white;}
    .l-layout-top{background:#102A49; color:White;}
    .l-layout-bottom{ background:#E5EDEF; text-align:center;}
    #pageloading{position:absolute; left:0px; top:0px; background:white url('loading.gif') no-repeat center; width:100%; height:100%; z-index:99999;}
    .l-link{ display:block; line-height:22px; height:22px; padding-left:20px;border:1px solid white; margin:4px;}
    .l-link-over{ background:#FFEEAC; border:1px solid #DB9F00;}
 
 </style>
</head>
<body style="padding:0px;">  
<div id="pageloading"></div> 
  <div id="layout1" style="width:100%">
        <div position="top" style="background:#102A49; color:White; ">
            <div style="margin-top:10px; margin-left:10px">
            <span>jQuery ligerUI 中文官方网站</span> 
             <a href="index.htm" class="l-link2">本地版本</a>
            <a href="index.aspx" class="l-link2">服务器版本</a>
            </div>
        </div>
        <div position="left"  title="主要菜单" id="accordion1"> 
                     <div title="功能列表" class="l-scroll">
                         <ul id="tree1" style="margin-top:3px;"> 
                            <li isexpand="false"><span>下拉框</span><ul> 

                            </ul></li>
                            <li isexpand="false"><span>树</span><ul> 

                            </ul></li>
                            <li isexpand="false"><span>表格</span><ul> 
                                 <li isexpand="false"><span>可排序</span>
                                    <ul> 
                                        <li url="dotnetdemos/grid/sortable/client.aspx"><span>客户端</span></li> 
                                        <li url="dotnetdemos/grid/sortable/server.aspx"><span>服务器</span></li>     
                                    </ul>
                                </li>
                                <li isexpand="false"><span>分页</span>
                                    <ul> 
                                        <li url="dotnetdemos/grid/pager/client.aspx"><span>客户端</span></li> 
                                        <li url="dotnetdemos/grid/pager/server.aspx"><span>服务器</span></li>     
                                    </ul>
                                </li>
                                <li isexpand="false"><span>树表格</span>
                                    <ul> 
                                        <li url="dotnetdemos/grid/treegrid/tree.aspx"><span>树表格</span></li>  
                                    </ul>
                                </li>
                            </ul> 
                            </li>
                         </ul>
                    </div>  
        </div>
        <div position="center" id="framecenter"> 
            <div tabid="home" title="我的主页" style="height:300px" >
                <iframe frameborder="0" name="home" src="welcome.htm"></iframe>
            </div> 
        </div> 
        <div position="bottom">
            Copyright © 2011 www.ligerui.com
        </div>
    </div> 
        <div style="display:none">
    <!-- 流量统计代码 --> 
    </div>
</body>
</html>

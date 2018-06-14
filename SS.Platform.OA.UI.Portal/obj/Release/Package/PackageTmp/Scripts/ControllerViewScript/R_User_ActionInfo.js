var $gridAction;
var $gridUserList;
var $selectedUser = null;
var $selectedAction = null;
$(function () {
    $("#layout1").ligerLayout({ leftWidth: 230, allowLeftResize: false });
    $("#addDiv").css("display", "none");
    $("#editDiv").css("display", "none");
    $("#setModuleDiv").css("display", "none");
    $("#setControlDiv").css("display", "none"); 
    $("#setActionDiv").css("display", "none");
    //初始化用户列表
    initUserGrid();
    //初始化权限列表
    initActionGrid();

    //选择模块按钮（添加）
    $("#SModule").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。
        var queryData = { SName: $("#Module").val() };
        $("#Module").attr("value", "");
        showModuleDialog(queryData);
        //由于input type image 控件会触发提交事件，所以“return false 取消触发提交事件”
        return false;
    });

    //选择模块窗口查找按钮
    $("#linkSearchModule").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。
        var queryData = { SName: $("#searchModuleName").val() };
        $("#searchModuleName").attr("value", "");
        showModuleDialog(queryData);
    });

    //选择控制器按钮（添加）
    $("#SControl").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。

        if ($("#ModuleID").val() == "") {
            $.messager.alert('警告', '请先选择模块!', '错误');
            return false;
        }

        var queryData = { SName: $("#ControlName").val(), SModuleID: $("#ModuleID").val() };
        $("#ControlName").attr("value", "");
        showControlDialog(queryData);
        //由于input type image 控件会触发提交事件，所以“return false 取消触发提交事件”
        return false;
    });

    //选择控制器窗口查找按钮
    $("#linkSearchControl").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。
        var queryData = { SName: $("#searchControlName").val(), SModuleID: $("#ModuleID").val() };
        $("#searchControlName").attr("value", "");
        showControlDialog(queryData);
    });

    //选择权限按钮（添加）
    $("#SAction").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。

        if ($("#ModuleID").val() == "") {
            $.messager.alert('警告', '请先选择模块!', '错误');
            return false;
        }

        if ($("#ControlID").val() == "") {
            $.messager.alert('警告', '请先选择控制器!', '错误');
            return false;
        }

        var queryData = { SControl: $("#ControlID").val(), SModule: $("#ModuleID").val(), SName: $("#ActionInfo").val(), };
        $("#ActionInfo").attr("value", "");
        showActionDialog(queryData);
        //由于input type image 控件会触发提交事件，所以“return false 取消触发提交事件”
        return false;
    });

    //选择权限窗口查找按钮
    $("#linkSearchAction").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。
        var queryData = { SName: $("#searchActionName").val(), SControl: $("#ControlID").val(), SModule: $("#ModuleID").val() };
        $("#searchActionName").attr("value", "");
        showActionDialog(queryData);
    });
    //是否允许点击事件（添加）
    $("#IsPass").click(function () {
        if (this.checked) {
            $("#IsPass").attr("value", "true");
        } else {

            $("#IsPass").attr("value", "false"); 
        }
    });
    //是否允许点击事件（添加）
    $("#EdIsPass").click(function () {
        if (this.checked) {
            $("#EdIsPass").attr("value", "true");
        } else {

            $("#EdIsPass").attr("value", "false"); 
        }
    });
});

//初始化用户列表
function initUserGrid() {
    $gridUserList = $("#usergrid").ligerGrid({
        columns: [
        { display: '主键', name: 'ID', hide: 'hide', width: 1 }
        ,{ display: '名称', name: 'Name', width: 220 }
        ], width: '100%', pkName: 'ID', pageSize: 20, pageSizeOptions: [5, 10, 15, 20], height: '100%'
        , onSelectRow: userGridClick
    });
    $.getJSON("/R_User_ActionInfo/GetAllUserInfo", {}, function (data) {
        $gridUserList.set({ data: data });
    });
}
//初始化权限列表
function initActionGrid() {
    $gridAction = $("#maingrid").ligerGrid({
        columns: [
        { display: '权限', name: 'Action', width: 200 }
        , { display: '是否允许', name: 'IsPass', width: 200 }
        ], width: '100%', pkName: 'ID', pageSize: 20, pageSizeOptions: [5, 10, 15, 20], height: '100%'
        , toolbar: {
            items: [
            { text: '设置权限', click: showAddDialog, icon: 'memeber' },
            { text: '修改权限', click: ShoweditDialog, img: '../../lib/ligerUI/skins/icons/edit.gif' },
            { text: '删除权限', click: doDelete, img: '../../lib/ligerUI/skins/icons/delete.gif' }
            ]
        }
        , onSelectRow: actionGridClick
        , onDblClickRow: ShoweditDialog 
    });
}

//用户列表单击事件
function userGridClick(note) {
    $.getJSON("/R_User_ActionInfo/GetUserAction", { SUser: note.ID }, function (data) {
        $gridAction.set({ data: data });
    });
    $selectedUser = note.ID;
}
//权限列表单击事件
actionGridClick
function actionGridClick(note) {
    $selectedAction = note.ID;
}


//弹出一个显示添加的对话框
function showAddDialog() {
    $("#addDiv").css("display", "block");
    clearControlData();
    $("#addDiv").dialog({
        title: "添加用户权限",
        width: 260,
        height: 220,
        collapsible: true,  //卷帘
        //minimizable: true,  //最小化
        //maximizable: true,  //最大化
        //resizable: true,    //是否可调整大小
        modal: true,
        buttons: [{
            text: '添加',
            iconCls: 'icon-ok',
            handler: function () {
                //触发  添加表单提交
                $("#addDiv form").submit();
            }
        }
        , {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#addDiv').dialog('close');
                }
            }
        ]
    });
}

//添加成功之后，会自动调用此方法
function afterAdd(data) {
    if (data == "ok")
    {
        //添加成功过了
        //关闭对话框
        $('#addDiv').dialog('close');
    }
    else
    {
        $.messager.alert("错误消息", data);
    }
    $.getJSON("/R_User_ActionInfo/GetUserAction", { SUser: $selectedUser }, function (data1) {
        $gridAction.set({ data: data1 });
    });
    //$selectedUser = note.ID;
}


//弹出一个显示修改权限的对话框
function ShoweditDialog() {
    //启动异步请求到后台，查询修改数据
    //$gridAction
    if ($selectedAction==null) 
    {
        $.messager.alert("错误提示", "请选择要修改的用户权限？");
        return;
    }
    $.post("/R_User_ActionInfo/Edit", { ids: $selectedAction }, function (data) {
        if (data.model.IsPass) {
            $("#EdIsPass").attr("checked", "checked");
        }
        else {
            $("#EdIsPass").attr("checked", false);
        }
        $("#EdIsPass").attr("value", data.model.IsPass);
        $("#EdModule").attr("value", data.model.ActionInfo.Control.Module.Name);
        $("#EdModuleID").attr("value", data.model.ActionInfo.Control.Module.ID);
        $("#EdControl").attr("value", data.model.ActionInfo.Control.Name);
        $("#EdControlID").attr("value", data.model.ActionInfo.Control.ID);
        $("#EdActionName").attr("value", data.model.ActionInfo.Name);
        $("#EdActionInfoID").attr("value", data.model.ActionInfo.ID);
        $("#EdActionInfoCode").attr("value", data.model.ActionInfo.Code);
        $("#EdUserInfoID").attr("value", data.model.UserInfoID);
        $("#EdCode").attr("value", data.model.Code);
        $("#EdName").attr("value", data.model.Name);

        $("#EdID").attr("value", data.model.ID);
    });

    $("#editDiv").css("display", "block");
    //clearEdControlData();
    $("#editDiv").dialog({
        title: "修改用户权限信息",
        width: 260,
        height: 220,
        collapsible: true,
        //minimizable: true,
        maximizable: true,
        //resizable: true,
        modal: true,
        buttons: [{
            text: '保存',
            iconCls: 'icon-ok',
            handler: function () {
                //触发修改表单提交
                //在父容器中拿到子容器的window submitEditFrm();
                $("#EdCode").removeAttr("disabled");
                $("#EdName").removeAttr("disabled");
                $("#editDiv form").submit();
                $("#EdCode").attr("disabled");
                $("#EdName").attr("disabled");
            }
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#editDiv').dialog('close');
            }
        }]
    });
}

//当修改成功之后，由子容器来调用的方法：关闭修改对话框，刷新表格
function afterEdit(data) {
    if (data == "ok") {
        //修改成功了
        $('#editDiv').dialog('close');
        //关闭对话框
    }
    else {
        $.messager.alert("错误消息", data);
    }
    $.getJSON("/R_User_ActionInfo/GetUserAction", { SUser: $selectedUser }, function (data1) {
        $gridAction.set({ data: data1 });
    });
}

//设置添加对话框控件默认值
function clearControlData() {
    $("#ActionName").attr("value", "");
    $("#ActionInfoID").attr("value", "");
    $("#ActionInfoCode").attr("value", "");
    $("#UserInfoID").attr("value", $selectedUser);
    $("#Code").attr("value", "自动生成");
    $("#Name").attr("value", "自动生成");
    $("input[name='IsPass'][value='0']").attr("checked", true);
    $("#Module").attr("value", "");
    $("#ModuleID").attr("value", "");
    $("#ControlName").attr("value", "");
    $("#ControlID").attr("value", "");
}

//选择模块对话框
function showModuleDialog(sarcheParam) {
    //弹出一个选择模块的对话框出来。
    $("#setModuleDiv").css("display", "block");
    $('#tbModule').datagrid({
        url: '/ActionInfo/GetAllModule',//rows=10  page=1
        title: '模块列表',
        width: 380,
        height: 300,
        fitColumns: true,
        idField: 'ID',
        loadMsg: '正在加载模块信息...',
        pagination: true,
        singleSelect: false,
        pageSize: 5,
        pageNumber: 1,
        pageList: [5],
        queryParams: sarcheParam,//表格初始化往后台发送异步请求后台的json数据时候额外发送的请求参数。
        columns: [[//u.ID, u.UserName, u.DelFlag, u.Mail, u.Phone, u.SubTime, u.SubBy
            //{ field: 'ck', checkbox: true, align: 'left', width: 50 },
            { field: 'Code', title: '编码', width: 80 },
            { field: 'Name', title: '名称', width: 120 }
        ]],
        onHeaderContextMenu: function (e, field) {

        }
        , onDblClickRow: function (rowIndex) {
            $('#tbModule').datagrid('unselectAll');
            $('#tbModule').datagrid('selectRow', rowIndex);
            var rows = $("#tbModule").datagrid('getSelected');
            $("#Module").attr("value", rows.Name);
            $("#ModuleID").attr("value", rows.ID);
            $('#setModuleDiv').dialog('close');
        }
    });
    $("#setModuleDiv").dialog({
        title: "选择模块",
        width: 400,
        height: 380,
        //collapsible: true,  //卷帘
        //minimizable: true,  //最小化
        //maximizable: true,  //最大化
        //resizable: true,    //是否可调整大小
        modal: true
    });
}

//选择控制器对话框
function showControlDialog(sarcheParam) {
    //弹出一个选择控制器的对话框出来。
    $("#setControlDiv").css("display", "block");
    $('#tbControl').datagrid({
        url: '/ActionInfo/GetAllControl',//rows=10  page=1
        title: '控制器列表',
        width: 380,
        height: 300,
        fitColumns: true,
        idField: 'ID',
        loadMsg: '正在加载控制器信息...',
        pagination: true,
        singleSelect: false,
        pageSize: 5,
        pageNumber: 1,
        pageList: [5],
        queryParams: sarcheParam,//表格初始化往后台发送异步请求后台的json数据时候额外发送的请求参数。
        columns: [[//u.ID, u.UserName, u.DelFlag, u.Mail, u.Phone, u.SubTime, u.SubBy
            //{ field: 'ck', checkbox: true, align: 'left', width: 50 },
            { field: 'Module', title: '模块', width: 80 },
            { field: 'Code', title: '编码', width: 80 },
            { field: 'Name', title: '名称', width: 120 }
        ]],
        onHeaderContextMenu: function (e, field) {

        }
        , onDblClickRow: function (rowIndex) {
            $('#tbControl').datagrid('unselectAll');
            $('#tbControl').datagrid('selectRow', rowIndex);
            var rows = $("#tbControl").datagrid('getSelected');
            $("#ControlName").attr("value", rows.Name);
            $("#ControlID").attr("value", rows.ID);
            $('#setControlDiv').dialog('close');
        }
    });
    $("#setControlDiv").dialog({
        title: "选择控制器",
        width: 400,
        height: 380,
        //collapsible: true,  //卷帘
        //minimizable: true,  //最小化
        //maximizable: true,  //最大化
        //resizable: true,    //是否可调整大小
        modal: true
    });
}

//选择权限对话框
function showActionDialog(sarcheParam) {
    //弹出一个选择控制器的对话框出来。
    $("#setActionDiv").css("display", "block");
    $('#tbAction').datagrid({
        url: '/ActionInfo/GetAllActionByControl',//rows=10  page=1
        title: '权限列表',
        width: 380,
        height: 300,
        fitColumns: true,
        idField: 'ID',
        loadMsg: '正在加载权限信息...',
        pagination: true,
        singleSelect: false,
        pageSize: 5,
        pageNumber: 1,
        pageList: [5],
        queryParams: sarcheParam,//表格初始化往后台发送异步请求后台的json数据时候额外发送的请求参数。
        columns: [[//u.ID, u.UserName, u.DelFlag, u.Mail, u.Phone, u.SubTime, u.SubBy
            //{ field: 'ck', checkbox: true, align: 'left', width: 50 },
            { field: 'Module', title: '模块', width: 80 },
            { field: 'Control', title: '控制器', width: 80 },
            { field: 'Code', title: '编码', width: 80 },
            { field: 'Name', title: '名称', width: 120 }
        ]],
        onHeaderContextMenu: function (e, field) {

        }
        , onDblClickRow: function (rowIndex) {
            $('#tbAction').datagrid('unselectAll');
            $('#tbAction').datagrid('selectRow', rowIndex);
            var rows = $("#tbAction").datagrid('getSelected');
            $("#ActionName").attr("value", rows.Name);
            $("#ActionInfoID").attr("value", rows.ID);
            $('#setActionDiv').dialog('close');
        }
    });
    $("#setActionDiv").dialog({
        title: "选择权限",
        width: 400,
        height: 380,
        //collapsible: true,  //卷帘
        //minimizable: true,  //最小化
        //maximizable: true,  //最大化
        //resizable: true,    //是否可调整大小
        modal: true
    });
}

//删除用户信息
function doDelete()
{
    if ($selectedAction == null) {
        $.messager.alert("错误提示", "请选择要删除的用户权限？");
        return;
    }

    $.messager.confirm("提示消息", "选定用户权限将删除？", function (r) {
        if (r) {
            //启动异步请到后台，批量删除数据
            $.post("/R_User_ActionInfo/DeleteIds", { ids: $selectedAction }, function (data) {
                if (data == "ok") {
                    //重新加载数据。
                    $.getJSON("/R_User_ActionInfo/GetUserAction", { SUser: $selectedUser }, function (data1) {
                        $gridAction.set({ data: data1 });
                    });
                } else {
                    $.messager.alert("错误提示", "删除出现错误！");
                }
            });
        }
    });
}
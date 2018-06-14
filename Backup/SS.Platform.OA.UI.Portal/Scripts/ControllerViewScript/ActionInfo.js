$(function () {
    initTable();

    $("#addDiv").css("display", "none");
    $("#editDiv").css("display", "none");
    $("#setModuleDiv").css("display", "none");
    $("#setControlDiv").css("display", "none");

    //绑定一个 搜索的点击事件。
    $("#linkSearch").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。
        var queryData = { SModule: $("#searchModule").val(), SControl: $("#searchControl").val(), SName: $("#searchName").val() };
        initTable(queryData);
    });

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
            $.messager.alert('警告','请先选择模块!','错误');
            return false;
        }

        var queryData = { SName: $("#Control").val(), SModuleID: $("#ModuleID").val() };
        $("#Control").attr("value", "");
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

    //生效点击事件（添加）
    $("#TakeEffect").click(function () {
        if (this.checked) {
            $("#TakeEffect").attr("value", "true");
            $("#TakeEffectTime").datebox("enable");
            $("#LoseEffectTime").datebox("enable");
        } else {

            $("#TakeEffect").attr("value", "false");
            $("#TakeEffectTime").datebox("disable");
            $("#LoseEffectTime").datebox("disable");
        }
    });

    //生效点击事件(修改)
    $("#EdTakeEffect").click(function () {
        if (this.checked) {
            $("#EdTakeEffect").attr("value", "true");
            $("#EdTakeEffectTime").datebox("enable");
            $("#EdLoseEffectTime").datebox("enable");
        } else {

            $("#EdTakeEffect").attr("value", "false");
            $("#EdTakeEffectTime").datebox("disable");
            $("#EdLoseEffectTime").datebox("disable");
            var time = $("#EdTakeEffectTime").datebox("getValue");
        }
    });
});

//初始化表格
function initTable(sarcheParam) {
    $('#tt').datagrid({
        url: '/ActionInfo/GetAllActionInfos',//rows=10  page=1
        title: '权限列表',
        width: 900,
        height: 490,
        fitColumns: true,
        idField: 'ID',
        loadMsg: '正在加载权限信息...',
        pagination: true,
        singleSelect: false,
        pageSize: 15,
        pageNumber: 1,
        pageList: [10, 15, 30],
        queryParams: sarcheParam,//表格初始化往后台发送异步请求后台的json数据时候额外发送的请求参数。
        columns: [[//u.ID, u.UserName, u.DelFlag, u.Mail, u.Phone, u.SubTime, u.SubBy
            { field: 'ck', checkbox: true, align: 'left', width: 50 },
            { field: 'Code', title: '编码', width: 80 },
            { field: 'Name', title: '名称', width: 120 },
            { field: 'TakeEffect', title: '生效', width: 60 },
            { field: 'Module', title: '模块', width: 80 },
            { field: 'Control', title: '控制器', width: 80 },
            { field: 'HttpMethod', title: '请求类型', width: 60 },
            { field: 'ActionMethod', title: '请求方法', width: 60 },
            { field: 'Url', title: '请求地址', width: 120 },
            { field: 'SubBy', title: '创建人', width: 60 },
            {
                field: 'SubTime', title: '创建时间', width: 80, align: 'right'
                , formatter: function (value, row, index) {
                    return (eval(value.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d");
                }
            }
        ]],
        toolbar: [{
            id: 'btnAdd',
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                //alert("ok add");
                //弹出一个div
                showAddDialog();
                $("#Code").focus();
            }
        }
        , {
            id: 'btnEdit',
            text: '修改',
            iconCls: 'icon-edit',
            handler: function () {
                ShoweditDialog();
                $("#EdCode").focus();
            }
        }
        , {
            id: 'btnDelete',
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                doDelete();
            }
        }
        ],
        onHeaderContextMenu: function (e, field) {

        }
        , onDblClickRow: function (rowIndex) {
            $('#tt').datagrid('unselectAll');
            $('#tt').datagrid('selectRow', rowIndex);
            ShoweditDialog();
        }
    });
}

//弹出一个显示添加的对话框
function showAddDialog() {
    $("#addDiv").css("display", "block");
    clearControlData();
    $("#addDiv").dialog({
        title: "添加权限信息",
        width: 510,
        height: 320,
        collapsible: true,  //卷帘
        //minimizable: true,  //最小化
        maximizable: true,  //最大化
        //resizable: true,    //是否可调整大小
        modal: true,
        buttons: [{
            text: '保存',
            iconCls: 'icon-ok',
            handler: function () {
                //触发  添加表单提交
                $("#addDiv form").submit();
            }
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#addDiv').dialog('close');
            }
        }]
    });
}

//选择模块对话框
function showModuleDialog(sarcheParam) {
    //弹出一个选择模块的对话框出来。
    $("#setModuleDiv").css("display", "block");
    $('#tbModule').datagrid({
        url: '/ActionInfo/GetAllModule',//rows=10  page=1
        title: '模块列表',
        width: 380,
        height: 425,
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
        height: 500,
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
        height: 425,
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
        height: 500,
        //collapsible: true,  //卷帘
        //minimizable: true,  //最小化
        //maximizable: true,  //最大化
        //resizable: true,    //是否可调整大小
        modal: true
    });
}

//添加成功之后，会自动调用此方法
function afterAdd(data) {
    if (data == "ok") {
        //添加成功过了
        $('#addDiv').dialog('close');
        //关闭对话框
        initTable();
    } else {
        $.messager.alert("错误消息", data);
    }
}

//修改
function ShoweditDialog() {
    //先拿到  你选中的所有的的  rows
    var rows = $("#tt").datagrid('getSelections');
    //rows是选中行的数据的json对象的集合
    if (rows.length < 1) {
        $.messager.alert("错误消息", "请选中要修改的权限信息？");
        return;
    }
    if (rows.length > 1) {
        $.messager.alert("错误消息", "一次只能修改一个权限信息！！");
        return;
    }
    var ids = "";
    ids += rows[0].ID;
    //启动异步请求到后台，查询修改数据
    $.post("/ActionInfo/Edit", { ids: ids }, function (data) {
        if (data.model.TakeEffect) {
            $("#EdTakeEffect").attr("checked", "checked");
            $("#EdTakeEffectTime").datebox("enable");
            $("#EdLoseEffectTime").datebox("enable");
        }
        else {
            $("#EdTakeEffect").attr("checked", false);
            $("#EdTakeEffectTime").datebox("disable");
            $("#EdLoseEffectTime").datebox("disable");
        }
        $("#EdTakeEffect").attr("value", data.model.TakeEffect);
        $("#EdTakeEffectTime").datebox("setValue", (eval(data.model.TakeEffectTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d"));
        $("#EdLoseEffectTime").datebox("setValue", (eval(data.model.LoseEffectTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d"));
        $("#EdModule").attr("value", data.model.Control.Module.Name);
        $("#EdModuleID").attr("value", data.model.Control.Module.ID);
        $("#EdControl").attr("value", data.model.Control.Name);
        $("#EdControlID").attr("value", data.model.Control.ID);
        if (data.model.HttpMethod==0) {
            $("input[name='HttpMethod'][value='0']").attr("checked", true);
        } else {
            $("input[name='HttpMethod'][value='1']").attr("checked", true);
        }
        $("#EdCode").attr("value", data.model.Code);
        $("#EdName").attr("value", data.model.Name);
        $("#EdActionMethod").attr("value", data.model.ActionMethod);
        $("#EdUrl").attr("value", data.model.Url);
        $("#EdRemark").attr("value", data.model.Remark);

        $("#EdID").attr("value", data.model.ID);
        $("#EdIn_Id").attr("value", data.model.In_Id);
        $("#EdSubBy").attr("value", data.model.SubBy);
        $("#EdSubTime").attr("value", (eval(data.model.SubTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d"));
    });
    //弹出一个当前选中实体修改的对话框出来。
    $("#editDiv").css("display", "block");
    clearEdControlData();
    $("#editDiv").dialog({
        title: "修改权限信息",
        width: 510,
        height: 320,
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
                if ($("#EdTakeEffect").attr("checked")) {
                    $("#EdCode").removeAttr("disabled");
                    $("#EdUrl").removeAttr("disabled");
                    $("#EdActionMethod").removeAttr("disabled");
                    $("#editDiv form").submit();
                    $("#EdCode").attr("disabled", "disabled");
                    $("#EdUrl").attr("disabled", "disabled");
                    $("#EdActionMethod").attr("disabled", "disabled");
                }else {
                    $("#EdTakeEffectTime").datebox("enable");
                    $("#EdLoseEffectTime").datebox("enable");
                    $("#EdCode").removeAttr("disabled");
                    $("#EdUrl").removeAttr("disabled");
                    $("#EdActionMethod").removeAttr("disabled");
                    $("#editDiv form").submit();
                    $("#EdCode").attr("disabled", "disabled");
                    $("#EdUrl").attr("disabled", "disabled");
                    $("#EdActionMethod").attr("disabled", "disabled");
                    $("#EdTakeEffectTime").datebox("disable");
                    $("#EdLoseEffectTime").datebox("disable");
                }
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
        initTable();
    }
    else {
        $.messager.alert("错误消息", data);
    }
}
//设置添加对话框控件默认值
function clearControlData() {
    var currTime = new Date();
    var strDate = currTime.getFullYear() + "-";
    strDate += currTime.getMonth() + 1 + "-";
    strDate += currTime.getDate();
    $("#TakeEffect").attr("checked", "checked");
    $("#TakeEffect").attr("value", "true");
    $("#TakeEffectTime").datebox("enable");
    $("#LoseEffectTime").datebox("enable");
    $("#TakeEffectTime").datebox("setValue", strDate);
    $("#LoseEffectTime").datebox("setValue", "9999-12-31");

    $("input[name='HttpMethod'][value='0']").attr("checked", true);

    $("#Module").attr("value", "");
    $("#ModuleID").attr("value", "");
    $("#ControlName").attr("value", "");
    $("#ControlID").attr("value", "");
    $("#Code").attr("value", "自动生成");
    $("#Name").attr("value", "");
    $("#ActionMethod").attr("value", "");
    $("#Url").attr("value", "自动生成");
    $("#Remark").attr("value", "");
}

//设置修改对话框控件默认值
function clearEdControlData() {

    var currTime = new Date();
    var strDate = currTime.getFullYear() + "-";
    strDate += currTime.getMonth() + 1 + "-";
    strDate += currTime.getDate();
    $("#TakeEffect").attr("checked", "checked");
    $("#TakeEffect").attr("value", "true");
    $("#TakeEffectTime").datebox("enable");
    $("#LoseEffectTime").datebox("enable");
    $("#TakeEffectTime").datebox("setValue", strDate);
    $("#LoseEffectTime").datebox("setValue", "9999-12-31");

    $("input[name='HttpMethod'][value='0']").attr("checked", true);

    $("#Module").attr("value", "");
    $("#ModuleID").attr("value", "");
    $("#Control").attr("value", "");
    $("#ControlID").attr("value", "");
    $("#Code").attr("value", "自动生成");
    $("#Name").attr("value", "");
    $("#ActionMethod").attr("value", "");
    $("#Url").attr("value", "自动生成");
    $("#Remark").attr("value", "");
}

//删除用户组信息
function doDelete() {
    //先拿到  你选中的所有的的  rows
    var rows = $("#tt").datagrid('getSelections');
    //rows是选中行的数据的json对象的集合
    if (rows.length <= 0) {
        $.messager.alert("错误消息", "删除前请选中要删除的权限？");
        return;
    }

    $.messager.confirm("提示消息", "确认删除选中的权限吗？", function (r) {
        if (r) {
            //把所有行的id拿到
            var ids = "";
            for (var i = 0; i < rows.length; i++) {
                ids += rows[i].ID + ",";
            }
            ids = ids.substr(0, ids.length - 1);
            //启动异步请到后台，批量删除数据
            $.post("/ActionInfo/DeleteIds", { ids: ids }, function (data) {
                if (data == "ok") {
                    //清除选中数据。
                    $("#tt").datagrid("clearSelections");
                    initTable();
                } else {
                    $.messager.alert("错误提示", "对不起，删除失败！！");
                }
            });
        }
    });

}
$(function () {
    initTable();

    $("#addDiv").css("display", "none");
    $("#editDiv").css("display", "none");
    $("#setModuleDiv").css("display", "none");

    //控制器查找按钮
    $("#linkSearch").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。

        var queryData = { SModu: $("#searchModu").val(), SName: $("#searchName").val() };

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
});

//初始化表格
function initTable(sarcheParam) {
    $('#tt').datagrid({
        url: '/Control/GetAllControls',//rows=10  page=1
        title: '控制器列表',
        width: 700,
        height: 400,
        fitColumns: true,
        idField: 'ID',
        loadMsg: '正在加载控制器信息...',
        pagination: true,
        singleSelect: false,
        pageSize: 10,
        pageNumber: 1,
        pageList: [10, 20, 30],
        queryParams: sarcheParam,//表格初始化往后台发送异步请求后台的json数据时候额外发送的请求参数。
        columns: [[//u.ID, u.UserName, u.DelFlag, u.Mail, u.Phone, u.SubTime, u.SubBy
            { field: 'ck', checkbox: true, align: 'left', width: 50 },
            { field: 'module', title: '模块', width: 80 },
            { field: 'Code', title: '编码', width: 60 },
            { field: 'Name', title: '名称', width: 60 },
            { field: 'Url', title: 'Url', width: 60 },
            { field: 'SubBy', title: '提交人', width: 60 },
            {
                field: 'SubTime', title: '提交时间', width: 80, align: 'right'
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
                $("#UserGroup").focus();
            }
        }
        , {
            id: 'btnEdit',
            text: '修改',
            iconCls: 'icon-edit',
            handler: function () {
                ShoweditDialog();
                $("#EdUserGroup").focus();
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

//显示模块对话框
function showModuleDialog(sarcheParam) {
    //弹出一个选择模块的对话框出来。
    $("#setModuleDiv").css("display", "block");
    $('#tbModule').datagrid({
        url: '/Control/GetAllModule',//rows=10  page=1
        title: '模块列表',
        width: 290,
        height: 230,
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
            { field: 'Code', title: '模块编码', width: 80 },
            { field: 'Name', title: '模块名称', width: 120 }
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
        width: 305,
        height: 303,
        //collapsible: true,  //卷帘
        //minimizable: true,  //最小化
        //maximizable: true,  //最大化
        //resizable: true,    //是否可调整大小
        modal: true
    });
}

//弹出一个显示添加的对话框
function showAddDialog() {
    $("#addDiv").css("display", "block");
    clearControlData();
    $("#addDiv").dialog({
        title: "添加控制器",
        width: 260,
        height: 290,
        collapsible: true,  //卷帘
        //minimizable: true,  //最小化
        maximizable: true,  //最大化
        //resizable: true,    //是否可调整大小
        modal: true,
        buttons: [{
            text: '添加',
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
        $.messager.alert("错误消息", "请选中要修改的控制器信息？");
        return;
    }
    if (rows.length > 1) {
        $.messager.alert("错误消息", "一次只能修改一个控制器信息！！");
        return;
    }
    var ids = "";
    ids += rows[0].ID;
    //启动异步请求到后台，查询修改数据
    $.post("/Control/Edit", { ids: ids }, function (data) {
        $("#EdModule").attr("value", data.model.Module.Name);
        $("#EdModuleID").attr("value", data.model.Module.ID);
        $("#EdCode").attr("value", data.model.Code);
        $("#EdName").attr("value", data.model.Name);
        $("#EdTakeEffect").attr("value", data.model.TakeEffect);
        $("#EdTakeEffectTime").attr("value", (eval(data.model.TakeEffectTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d"));
        $("#EdLoseEffectTime").attr("value", (eval(data.model.LoseEffectTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d"));
        $("#EdID").attr("value", data.model.ID);
        $("#EdIn_Id").attr("value", data.model.In_Id);
        $("#EdSubBy").attr("value", data.model.SubBy);
        $("#EdSubTime").attr("value", (eval(data.model.SubTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d"));
        $("#EdUrl").attr("value", data.model.Url);
        $("#EdRemark").attr("value", data.model.Remark);
    });
    //弹出一个当前选中实体修改的对话框出来。
    $("#editDiv").css("display", "block");
    clearEdControlData();
    $("#editDiv").dialog({
        title: "修改用户组信息",
        width: 260,
        height: 290,
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
                $("#EdUrl").removeAttr("disabled");
                $("#editDiv form").submit();
                $("#EdCode").attr("disabled", "disabled");
                $("#EdUrl").attr("disabled", "disabled");
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

//删除用户信息
function doDelete() {
    //先拿到  你选中的所有的的  rows
    var rows = $("#tt").datagrid('getSelections');
    //rows是选中行的数据的json对象的集合
    if (rows.length <= 0) {
        $.messager.alert("请选择要删除的控制器？");
        return;
    }

    $.messager.confirm("提示消息", "选定控制器信息将删除？", function (r) {
        if (r) {
            //把所有行的id拿到。
            var ids = "";
            for (var i = 0; i < rows.length; i++) {
                ids += rows[i].ID + ",";
            }
            ids = ids.substr(0, ids.length - 1);
            //启动异步请到后台，批量删除数据
            $.post("/Control/DeleteIds", { ids: ids }, function (data) {
                if (data == "ok") {
                    //清除选中数据。
                    $("#tt").datagrid("clearSelections");
                    initTable();
                } else {
                    $.messager.alert("错误提示", "删除出现错误！");
                }
            });
        }
    });

}

//设置添加对话框控件默认值
function clearControlData() {
    $("#Module").attr("value", "");
    $("#ModuleID").attr("value", "");
    $("#Code").attr("value", "自动生成");
    $("#Name").attr("value", "");
    $("#Url").attr("value", "");
    $("#Remark").attr("value", "");
}

//设置修改对话框控件默认值
function clearEdControlData() {
    $("#EdModule").attr("value", "");
    $("#EdModuleID").attr("value", "");
    $("#EdCode").attr("value", "");
    $("#EdName").attr("value", "");
    $("#EdUrl").attr("value", "");
    $("#EdRemark").attr("value", "");
    $("#EdID").attr("value", "");
    $("#EdIn_Id").attr("value", "");
    $("#EdSubBy").attr("value", "");
    $("#EdSubTime").attr("value", "");
    $("#EdTakeEffect").attr("value", "");
    $("#EdTakeEffectTime").attr("value", "");
    $("#EdLoseEffectTime").attr("value", "");
}
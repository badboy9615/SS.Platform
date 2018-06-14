$(function () {
    initTable();

    $("#addDiv").css("display", "none");
    $("#editDiv").css("display", "none");

    //绑定一个 搜索的点击事件。
    $("#linkSearch").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。
        var queryData = { SName: $("#searchName").val() };
        initTable(queryData);
    });
});

//初始化表格
function initTable(sarcheParam) {
    $('#tt').datagrid({
        url: '/Module/GetAllModule',//rows=10  page=1
        title: '模块列表',
        width: 680,
        height: 400,
        fitColumns: true,
        idField: 'ID',
        loadMsg: '正在加载模块信息...',
        pagination: true,
        singleSelect: false,
        pageSize: 15,
        pageNumber: 1,
        pageList: [10, 15, 30],
        queryParams: sarcheParam,//表格初始化往后台发送异步请求后台的json数据时候额外发送的请求参数。
        columns: [[//u.ID, u.UserName, u.DelFlag, u.Mail, u.Phone, u.SubTime, u.SubBy
            { field: 'ck', checkbox: true, align: 'left', width: 50 },
            { field: 'Code', title: '模块编码', width: 80 },
            { field: 'Name', title: '模块名称', width: 120 },
            { field: 'Url', title: 'Url', width: 80 },
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
        title: "添加角色信息",
        width: 270,
        height: 270,
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
        $.messager.alert("错误消息", "请选中要修改的模块信息？");
        return;
    }
    if (rows.length > 1) {
        $.messager.alert("错误消息", "一次只能修改一个模块信息！！");
        return;
    }
    var ids = "";
    ids += rows[0].ID;
    //启动异步请求到后台，查询修改数据
    $.post("/Module/Edit", { ids: ids }, function (data) {
        $("#EdCode").attr("value", data.model.Code);
        $("#EdName").attr("value", data.model.Name);
        $("#EdTakeEffect").attr("value", data.model.TakeEffect);
        $("#EdTakeEffectTime").attr("value", (eval(data.model.TakeEffectTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d"));
        $("#EdLoseEffectTime").attr("value", (eval(data.model.LoseEffectTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d"));
        $("#EdID").attr("value", data.model.ID);
        $("#EdIn_Id").attr("value", data.model.In_Id);
        $("#EdRemark").attr("value", data.model.Remark);
        $("#EdSubBy").attr("value", data.model.SubBy);
        $("#EdSubTime").attr("value", (eval(data.model.SubTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d"));
        $("#EdUrl").attr("value", data.model.Url);
    });
    //弹出一个当前选中实体修改的对话框出来。
    $("#editDiv").css("display", "block");
    clearEdControlData();
    $("#editDiv").dialog({
        title: "修改模块信息",
        width: 270,
        height: 270,
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
//设置添加对话框控件默认值
function clearControlData() {
    $("#Code").attr("value", "自动生成");
    $("#Name").attr("value", "");
    $("#Url").attr("value", "");
    $("#Remark").attr("value", "");
}

//设置修改对话框控件默认值
function clearEdControlData() {

    $("#EdCode").attr("value", "");
    $("#EdName").attr("value", "");
    $("#EdID").attr("value", "");
    $("#EdIn_Id").attr("value", "");
    $("#EdSubBy").attr("value", "");
    $("#EdSubTime").attr("value", "");
    $("#EdTakeEffect").attr("value", "");
    $("#EdTakeEffectTime").attr("value", "");
    $("#EdLoseEffectTime").attr("value", "");
    $("#EdUrl").attr("value", "");
    $("#EdRemark").attr("value", "");
}

//删除用户组信息
function doDelete() {
    //先拿到  你选中的所有的的  rows
    var rows = $("#tt").datagrid('getSelections');
    //rows是选中行的数据的json对象的集合
    if (rows.length <= 0) {
        $.messager.alert("错误消息", "删除前请选中要删除的模块？");
        return;
    }

    $.messager.confirm("提示消息", "确认删除选中的模块吗？", function (r) {
        if (r) {
            //把所有行的id拿到
            var ids = "";
            for (var i = 0; i < rows.length; i++) {
                ids += rows[i].ID + ",";
            }
            ids = ids.substr(0, ids.length - 1);
            //启动异步请到后台，批量删除数据
            $.post("/Module/DeleteIds", { ids: ids }, function (data) {
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
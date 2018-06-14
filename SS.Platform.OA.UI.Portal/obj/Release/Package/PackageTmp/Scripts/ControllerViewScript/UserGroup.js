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
        url: '/UserGroup/GetAllUserGroupInfos',//rows=10  page=1
        title: '用户组列表',
        width: 680,
        height: 400,
        fitColumns: true,
        idField: 'ID',
        loadMsg: '正在加载用户组的信息...',
        pagination: true,
        singleSelect: false,
        pageSize: 10,
        pageNumber: 1,
        pageList: [10, 20, 30],
        queryParams: sarcheParam,//表格初始化往后台发送异步请求后台的json数据时候额外发送的请求参数。
        columns: [[
            { field: 'ck', checkbox: true, align: 'left', width: 50 },
            { field: 'Code', title: '用户组编码', width: 80 },
            { field: 'Name', title: '用户组名称', width: 120 }
            //{ field: 'SubBy', title: '创建人', width: 120 },
            //{
            //    field: 'SubTime', title: '创建时间', width: 80, align: 'right',
            //    formatter: function (value, row, index) {
            //        //return value.toString().replace("T", " ").substring(0, 11);
            //        return (eval(value.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d");
            //    }
            //}
        ]],
        toolbar: [{
            id: 'btnAdd',
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                //显示添加对话框
                showAddDialog();
            }
        }
        , {
            id: 'btnEdit',
            text: '修改',
            iconCls: 'icon-edit',
            handler: function () {
                ShoweditDialog();
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
    $("#addDiv").dialog({
        title: "添加用户组信息",
        width: 280,
        height: 240,
        collapsible: true,  //卷帘
        //minimizable: true,  //最小化
        maximizable: true,  //最大化
        //resizable: true,    //是否可调整大小
        modal: true,
        buttons: [{
            text: '保存',
            iconCls: 'icon-ok',
            handler: function () {
                //触发添加表单提交
                $("#addDiv form").submit();
            }
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                clearControlData();
                $('#addDiv').dialog('close');
            }
        }]
    });
}

//清空控件数据
function clearControlData() {
    $("#Code").attr("value", "");
    $("#Name").attr("value", "");
    $("#Remark").attr("value", "");

}

//添加成功之后，会自动调用此方法
function afterAdd(data) {
    if (data == "ok") {
        //添加成功过了
        //清空控件数据
        clearControlData();
        $('#addDiv').dialog('close');
        //关闭对话框
        initTable();
    }
    else {
        $.messager.alert("错误消息", data);
    }
}

//修改
function ShoweditDialog() {
    //先拿到  你选中的所有的的  rows
    var rows = $("#tt").datagrid('getSelections');
    //rows是选中行的数据的json对象的集合
    if (rows.length < 1) {
        $.messager.alert("错误消息", "请选中要修改的用户组信息？");
        return;
    }
    if (rows.length > 1) {
        $.messager.alert("错误消息", "一次只能修改一个用户组的信息！！");
        return;
    }
    var ids = "";
    ids += rows[0].ID;
    //启动异步请到后台，查询修改数据
    $.post("/UserGroup/Edit", { ids: ids }, function (data) {
        $("#EdCode").attr("value", data.model.Code);
        $("#EdName").attr("value", data.model.Name);
        $("#EdRemark").attr("value", data.model.Remark);
        $("#EdID").attr("value", data.model.ID); 
        $("#EdIn_Id").attr("value", data.model.In_Id);
        $("#EdSubBy").attr("value", data.model.SubBy);
        $("#EdSubTime").attr("value", data.model.SubTime).toString().replace("T", " ").substring(0, 11);
        $("#EdDelFlag").attr("value", data.model.DelFlag);
    });
    //弹出一个当前选中实体修改的对话框出来。
    $("#editDiv").css("display", "block");
    $("#editDiv").dialog({
        title: "修改用户组信息",
        width: 280,
        height: 240,
        collapsible: true,
        minimizable: true,
        maximizable: true,
        resizable: true,
        modal: true,
        buttons: [{
            text: '保存',
            iconCls: 'icon-ok',
            handler: function () {
                //触发修改表单提交
                //在父容器中拿到子容器的window submitEditFrm();
                $("#EdCode").removeAttr("disabled");
                $("#editDiv form").submit();
                $("#EdCode").attr("disabled", "disabled");
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
//格式化日期
function formatDate(v) {
    var date = new Date(parseInt(v.replace("/Date(", "").replace(")/", ""), 10));
    if (date instanceof Date) {
        var y = date.getFullYear();
        var m = date.getMonth() + 1;
        var d = date.getDate();
        var h = date.getHours();
        var i = date.getMinutes();
        var s = date.getSeconds();
        var ms = date.getMilliseconds();
        if (ms > 0)
            return y + '-' + m + '-' + d + ' ' + h + ':' + i + ':' + s
                    + '.' + ms;
        if (h > 0 || i > 0 || s > 0)
            return y + '-' + m + '-' + d + ' ' + h + ':' + i + ':' + s;
        return y + '-' + m + '-' + d;
    }
    return '';
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

//删除用户组信息
function doDelete() {
    //先拿到  你选中的所有的的  rows
    var rows = $("#tt").datagrid('getSelections');
    //rows是选中行的数据的json对象的集合
    if (rows.length <= 0) {
        $.messager.alert("错误消息", "删除前请选中要删除的用户组？");
        return;
    }

    $.messager.confirm("提示消息", "确认删除选中用户组吗？", function (r) {
        if (r) {
            //把所有行的id拿到
            var ids = "";
            for (var i = 0; i < rows.length; i++) {
                ids += rows[i].ID + ",";
            }
            ids = ids.substr(0, ids.length - 1);
            //启动异步请到后台，批量删除数据
            $.post("/UserGroup/DeleteIds", { ids: ids }, function (data) {
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
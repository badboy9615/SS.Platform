var $selectedUser = null;
var $gridAddRole = null;
var $dlgSetRole = null;
var $checkedAddRole = [];
$(function () {
    initTable();

    $("#addDiv").css("display", "none");
    $("#editDiv").css("display", "none");
    $("#setActionDiv").css("display", "none");
    $("#setUserGroupDiv").css("display", "none");

    //用户查找按钮
    $("#linkSearch").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。

        var queryData = { SLoginName: $("#searchLoginName").val(), SShowName: $("#searchShowName").val() };

        initTable(queryData);
    });

    //选择用户组按钮（添加）
    $("#SUserGroup").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。
        var queryData = { SName: $("#UserGroup").val() };
        $("#UserGroup").attr("value", "");
        $("#AddOrEdit").attr("value", "Add");
        showGroupDialog(queryData);
        //由于input type image 控件会触发提交事件，所以“return false 取消触发提交事件”
        return false;
    });

    //选择用户组按钮（修改）
    $("#EdSUserGroup").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。
        var queryData = { SName: $("#EdUserGroup").val() };
        $("#EdUserGroup").attr("value", "");
        $("#AddOrEdit").attr("value", "Edit");
        showGroupDialog(queryData);
        //由于input type image 控件会触发提交事件，所以“return false 取消触发提交事件”
        return false;
    });

    //选择联系对象
    $("#SContactObject").click(function () {
        //选择联系对象。
        //由于input type image 控件会触发提交事件，所以“return false 取消触发提交事件”
        return false;
    });

    //选择用户组窗口查找按钮
    $("#linkSearchUserGroup").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。
        var queryData = { SName: $("#searchGroupName").val() };
        $("#searchGroupName").attr("value", "");
        showGroupDialog(queryData);
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
    //初始化设置用户角色窗口
    initSetRole();
});

//初始化表格
function initTable(sarcheParam) {
    $('#tt').datagrid({
        url: '/UserInfo/GetAllUserInfos', //rows=10  page=1
        title: '用户列表',
        width: 700,
        height: 400,
        fitColumns: true,
        idField: 'ID',
        loadMsg: '正在加载用户的信息...',
        pagination: true,
        singleSelect: false,
        pageSize: 10,
        pageNumber: 1,
        pageList: [10, 20, 30],
        queryParams: sarcheParam, //表格初始化往后台发送异步请求后台的json数据时候额外发送的请求参数。
        columns: [
            [
//u.ID, u.UserName, u.DelFlag, u.Mail, u.Phone, u.SubTime, u.SubBy
                { field: 'ck', checkbox: true, align: 'left', width: 50 },
                //{ field: 'Name', title: '用户组', width: 80 },
                { field: 'Code', title: '登录名', width: 60 },
                { field: 'Name', title: '显示名', width: 60 },
                { field: 'TakeEffect', title: '生效', width: 60 },
                { field: 'SubBy', title: '提交人', width: 60 },
                {
                    field: 'SubTime',
                    title: '提交时间',
                    width: 80,
                    align: 'right',
                    formatter: function(value, row, index) {
                        //return (eval(value.replace(/T/, " "))).pattern("yyyy-M-d h:m:s.S");
                        return (eval(value.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d");
                        //var subTime = new Date(value);
                        //return value.toString().replace("T", " ").substring(0, 11); //subTime.toLocaleDateString();
                    }
                }
            ]
        ],
        toolbar: [
        {
            id: 'btnAdd',
            text: '添加',
            iconCls: 'icon-add',
            handler: function() {
                //alert("ok add");
                //弹出一个div
                showAddDialog();
                $("#UserGroup").focus();
            }
        }, {
            id: 'btnEdit',
            text: '修改',
            iconCls: 'icon-edit',
            handler: function() {
                ShoweditDialog();
                $("#EdUserGroup").focus();
            }
        }, {
            id: 'btnDelete',
            text: '删除',
            iconCls: 'icon-remove',
            handler: function() {
                doDelete();
            }
        }
        //, {
        //    id: 'btnSetRole',
        //    text: '添加角色',
        //    iconCls: 'icon-redo'
        //    ,handler: function () {
        //        ShowSetRoleDialog();
        //    }
        //}
        , {
            id: 'btnSetAction',
            text: '设置特殊权限',
            iconCls: 'icon-redo',
            handler: function () {
                //edit();
                //alert("设置角色");
                //先拿到  你选中的所有的的  rows
                var rows = $("#tt").datagrid('getSelections');
                //rows是选中行的数据的json对象的集合
                if (rows.length != 1) {
                    $.messager.alert("错误消息", "请选中要设置角色的用户信息？");
                    return;
                }
                setAction(rows[0].ID);
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

//显示用户组对话框
function showGroupDialog(sarcheParam) {
    //弹出一个选择用户组的对话框出来。
    $("#setUserGroupDiv").css("display", "block");
    $('#tbUserGroup').datagrid({
        url: '/UserInfo/GetAllUserGroup',//rows=10  page=1
        title: '用户组列表',
        width: 360,
        height: 260,
        fitColumns: true,
        idField: 'ID',
        loadMsg: '正在加载用户组信息...',
        pagination: true,
        singleSelect: false,
        pageSize: 5,
        pageNumber: 1,
        pageList: [5],
        queryParams: sarcheParam,//表格初始化往后台发送异步请求后台的json数据时候额外发送的请求参数。
        columns: [[//u.ID, u.UserName, u.DelFlag, u.Mail, u.Phone, u.SubTime, u.SubBy
            //{ field: 'ck', checkbox: true, align: 'left', width: 50 },
            { field: 'Code', title: '用户组编码', width: 60 },
            { field: 'Name', title: '用户组名称', width: 80 }
        ]],
        onHeaderContextMenu: function (e, field) {

        }
        , onDblClickRow: function (rowIndex) {
            if ($("#AddOrEdit").attr("value") == "Add") {
                $('#tbUserGroup').datagrid('unselectAll');
                $('#tbUserGroup').datagrid('selectRow', rowIndex);
                var rows = $("#tbUserGroup").datagrid('getSelected');
                $("#UserGroup").attr("value", rows.Name);
                $("#UserGroupID").attr("value", rows.ID);
                $("#UserGroupCode").attr("value", rows.Code);
                $('#setUserGroupDiv').dialog('close');
            }
            if ($("#AddOrEdit").attr("value") == "Edit") {
                $('#tbUserGroup').datagrid('unselectAll');
                $('#tbUserGroup').datagrid('selectRow', rowIndex);
                var rowsEdit = $("#tbUserGroup").datagrid('getSelected');
                $("#EdUserGroup").attr("value", rowsEdit.Name);
                $("#EdUserGroupID").attr("value", rowsEdit.ID);
                $("#EdUserGroupCode").attr("value", rowsEdit.Code);
                $('#setUserGroupDiv').dialog('close');
            }
        }
    });
    $("#setUserGroupDiv").dialog({
        title: "选择用户组",
        width: 380,
        height: 345,
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
        title: "添加用户信息",
        width: 428,
        height: 248,
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
                if ($("#TakeEffect").attr("checked")) {

                    $("#addDiv form").submit();
                } else {
                    $("#TakeEffectTime").datebox("enable");
                    $("#LoseEffectTime").datebox("enable");
                    $("#addDiv form").submit();
                    $("#TakeEffectTime").datebox("disable");
                    $("#LoseEffectTime").datebox("disable");
                }
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
        $.messager.alert("错误消息", "请选中要修改的用户信息？");
        return;
    }
    if (rows.length > 1) {
        $.messager.alert("错误消息", "一次只能修改一个用户的信息！！");
        return;
    }
    var ids = "";
    ids += rows[0].ID;
    //启动异步请求到后台，查询修改数据
    $.post("/UserInfo/Edit", { ids: ids }, function (data) {
        $("#EdUserGroup").attr("value", data.model.UserGroup.Name);
        $("#EdUserGroupID").attr("value", data.model.UserGroup.ID);
        $("#EdUserGroupCode").attr("value", data.model.UserGroup.Code);
        $("#EdTakeEffect").attr("value", data.model.TakeEffect);
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
        //$("#EdContactObject").attr("value", data.model);
        //$("#EdContactObjectID").attr("value", data.model.TakeEffect);
        $("#EdTakeEffectTime").datebox("setValue", (eval(data.model.TakeEffectTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d"));
        $("#EdLoginName").attr("value", data.model.Code);
        $("#EdPwd").attr("value", data.model.Pwd);
        $("#EdID").attr("value", data.model.ID);
        //$("#EdIn_Id").attr("value", data.model.In_Id);
        $("#EdLoseEffectTime").datebox("setValue", (eval(data.model.LoseEffectTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d"));
        $("#EdShowName").attr("value", data.model.Name);
        $("#EdRemark").attr("value", data.model.Remark);
        $("#EdSubBy").attr("value", data.model.SubBy);
        $("#EdSubTime").attr("value", (eval(data.model.SubTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d"));
        $("#EdMail").attr("value", data.model.Mail);
        $("#EdPhone").attr("value", data.model.Phone);
        $("#EdQQNum").attr("value", data.model.QQNum);
    });
    //弹出一个当前选中实体修改的对话框出来。
    $("#editDiv").css("display", "block");
    clearEdControlData();
    $("#editDiv").dialog({
        title: "修改用户组信息",
        width: 428,
        height: 248,
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
                //var time = $("#EdTakeEffectTime").datebox("getValue");
                if ($("#EdTakeEffect").attr("checked")) {
                    $("#EdLoginName").removeAttr("disabled");
                    $("#editDiv form").submit();
                    $("#EdLoginName").attr("disabled");
                } else {
                    $("#EdTakeEffectTime").datebox("enable");
                    $("#EdLoseEffectTime").datebox("enable");
                    $("#EdLoginName").removeAttr("disabled");
                    $("#editDiv form").submit();
                    $("#EdTakeEffectTime").datebox("disable");
                    $("#EdLoseEffectTime").datebox("disable");
                    $("#EdLoginName").attr("disable", "disable");
                }
                //$("#EdTakeEffectTime").datebox("enable");
                //$("#EdTakeEffectTime").datebox("disable");
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
        $.messager.alert("请选择要删除的用户？");
        return;
    }

    $.messager.confirm("提示消息", "选定用户信息将删除？", function (r) {
        if (r) {
            //把所有行的id拿到。
            var ids = "";
            for (var i = 0; i < rows.length; i++) {
                ids += rows[i].ID + ",";
            }
            ids = ids.substr(0, ids.length - 1);
            //启动异步请到后台，批量删除数据
            $.post("/UserInfo/DeleteIds", { ids: ids }, function (data) {
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

//初始化设置用户角色窗口
function initSetRole() {
    //设置用户角色grid
    $gridAddRole = $("#setRole").ligerGrid({
        checkbox: true,
        columns: [
          { display: '主键', name: 'ID', hide: 'hide', width: 1 }
        , { display: '登录名', name: 'Name', width: 300 }
        ]
        , width: 'auto', pkName: 'ID', pageSize: 15, pageSizeOptions: [5, 10, 15, 20], height: '85%'
        , isChecked: f_isChecked, onCheckRow: f_onCheckRow, onCheckAllRow: f_onCheckAllRow
        , toolbar: {
            items: [
            { text: '保存', click: setSave, icon: 'ok' }
            ]
        }
    });
}


//设置角色，弹出设置角色对话框
function ShowSetRoleDialog() {
    //先拿到  你选中的所有的的  rows
    var rows = $("#tt").datagrid('getSelections');
    //rows是选中行的数据的json对象的集合
    if (rows.length < 1) {
        $.messager.alert("错误消息", "请选中要设置角色的用户信息？");
        return;
    }
    if (rows.length > 1) {
        $.messager.alert("错误消息", "一次只能设置一个用户的角色！！");
        return;
    }
    var ids = "";
    ids += rows[0].ID;
    $selectedUser = rows[0].ID;

    $.getJSON("/UserInfo/GetRoleInfo", { SUser: ids }, function (data) {
        $gridAddRole.set({ data: data });
        for (var i = 0; i < data.Rows.length; i++) {
            if (data.Rows[i].selected) {
                addCheckedRole(data.Rows[i].ID);
                $gridAddRole.reload();
            }
        }
    });
    $dlgSetRole = $.ligerDialog.open({
        target: $("#setRole"),
        title: '添加用户角色',
        width: 410,
        height: 600
        //,isResize :true
    });
    $checkedAddRole = [];
}

//设置用户的特殊权限
function setAction(id) {
    //弹出一个设置权限的对话框
    var url = "/UserInfo/SetAction/" + id;
    $("#frameSetAction").attr("src", url);

    //弹出一个当前选中实体修改的对话框出来。
    $("#setActionDiv").css("display", "block");
    $("#setActionDiv").dialog({
        title: "设置用户角色信息",
        width: 580,
        height: 400,
        collapsible: true,
        minimizable: true,
        maximizable: true,
        resizable: true,
        modal: true
    });
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

    $("#UserGroup").attr("value", "");
    $("#UserGroupID").attr("value", "");
    $("#UserGroupCode").attr("value", "");
    $("#ContactObject").attr("value", "");
    $("#ContactObjectID").attr("value", "");
    $("#Code").attr("value", "");
    $("#Name").attr("value", "");
    $("#Remark").attr("value", "");
    $("#Mail").attr("value", "");
    $("#Phone").attr("value", "");
    $("#QQNum").attr("value", "");

}

//设置修改对话框控件默认值
function clearEdControlData() {

    var currTime = new Date();
    var strDate = currTime.getFullYear() + "-";
    strDate += currTime.getMonth() + 1 + "-";
    strDate += currTime.getDate();
    $("#EdUserGroup").attr("value", "");
    $("#EdUserGroupID").attr("value", "");
    $("#EdUserGroupCode").attr("value", "");
    $("#EdTakeEffect").attr("value", "false");
    $("#EdTakeEffectTime").datebox("setValue", strDate);
    $("#EdLoginName").attr("value", "");
    $("#EdPwd").attr("value", "");
    $("#EdID").attr("value", "");
    $("#EdLoseEffectTime").datebox("setValue", "9999-12-31");
    $("#EdShowName").attr("value", "");
    $("#EdRemark").attr("value", "");
    $("#EdSubBy").attr("value", "");
    $("#EdSubTime").attr("value", "");
    $("#EdLoginName").attr("disabled", "disabled");
    $("#EdMail").attr("value", "");
    $("#EdPhone").attr("value", "");
    $("#EdQQNum").attr("value", "");

} 
/************************表单分页多选功能开始***************添加******************/
/*即利用onCheckRow将选中的行记忆下来，并利用isChecked将记忆下来的行初始化选中*/
function f_onCheckAllRow(checked) {
    for (var rowid in this.records) {
        if (checked)
            addCheckedRole(this.records[rowid]['ID']);
        else
            removeCheckedRole(this.records[rowid]['ID']);
    }
}

function findCheckedRole(roleId) {
    for (var i = 0; i < $checkedAddRole.length; i++) {
        if ($checkedAddRole[i] == roleId) return i;
    }
    return -1;
}
function addCheckedRole(roleId) {
    if (findCheckedRole(roleId) == -1)
        $checkedAddRole.push(roleId);
}
function removeCheckedRole(roleId) {
    var i = findCheckedRole(roleId);
    if (i == -1) return;
    $checkedAddRole.splice(i, 1);
}
function f_isChecked(rowdata) {
    if (findCheckedRole(rowdata.ID) == -1)
        return false;
    return true;
}
function f_onCheckRow(checked, data) {
    if (checked) {
        addCheckedRole(data.ID);
    } else {
        removeCheckedRole(data.ID);
    }
}
function f_getChecked() {
    alert($checkedAddRole.join(','));
}
/************************表单分页多选功能结束*****************添加****************/
//增加用户角色窗口保存按钮
function setSave() {
    var ids = "";
    for (var i = 0; i < $checkedAddRole.length; i++) {
        ids += $checkedAddRole[i] + ",";
    }
    ids = ids.substr(0, ids.length - 1);
    //启动异步请求到后台，查询修改数据
    $.post("/UserInfo/SetUserRoleSave", { UserId: $selectedUser, RolesId: ids }, function (data) {
        if (data == "ok") {
            //添加成功过了
            $checkedAddRole = [];
            $dlgSetRole.hidden();
        } else {
            //添加失败了
            $checkedAddRole = [];
            $dlgSetRole.hidden();
        }
    });
}
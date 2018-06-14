var t;
var $grid;
var $gridAddUser;
var $gridDelUser;
var $dlgAdd = null;
var $dlgDel = null;
var $selectedDept = null;
var $checkedAddUser = [];
var $checkedDelUser = [];
$.ligerui.controls.Tree.prototype.alert = function () {
    var data = this.getChecked();
    for (var i = 0; i < data.length; i++) {
        alert(data[i]['data']['text']);
    }
};
function f_click() {
    t.alert();
}
$(function () {
    //初始化树控件
    $("#layout1").ligerLayout({ leftWidth: 200 });
    $.getJSON("/DepartmentUserinfo/GetTreeDepNodes", {}, function (data) {
        var tree = $("#tree1").ligerTree({
            data: data,
            idFieldName: 'CategoryId',
            textFieldName: 'Name',
            slide: true,
            parentIDFieldName: 'ParentId'
            , onClick: treeClick
        });
        var treeManager = $("#tree1").ligerGetTreeManager();
        treeManager.expandAll();
    });

    t = $("#tree1").ligerTree({ checkbox: false });
    //用户列表grid
    $grid = $("#maingrid").ligerGrid({
        columns: [
        //{ display: '主键', name: 'ID', width: 200},
        { display: '登录名', name: 'Code', width: 200 },
        { display: '显示名', name: 'Name', width: 200 }
        ], width: '100%', pkName: 'ID', pageSize: 20, pageSizeOptions: [5, 10, 15, 20], height: '100%'
        , toolbar: {
            items: [
            { text: '设置部门用户', click: setDepartmentUser, icon: 'memeber' },
            { text: '删除部门用户', click: DeleteDepartmentUser, img: '../../lib/ligerUI/skins/icons/delete.gif' }
            ]
        }
    });
    //增加用户grid
    $gridAddUser = $("#adduser").ligerGrid({
        checkbox: true,
        columns: [
          { display: '主键', name: 'ID', hide: 'hide', width: 1 }
        , { display: '登录名', name: 'Code', width: 200 },
        { display: '显示名', name: 'Name', width: 200 }
        ]
        , width: '98%', pkName: 'ID', pageSize: 10, pageSizeOptions: [5, 10, 15, 20], height: '80%'
        , isChecked: f_isChecked, onCheckRow: f_onCheckRow, onCheckAllRow: f_onCheckAllRow
        , toolbar: {
            items: [
            { text: '保存', click: addSave, icon: 'ok' },
            { text: '关闭', click: addExit, img: '../../lib/ligerUI/skins/icons/candle.gif' }
            ]
        }
    });
    //删除用户grid
    $gridDelUser = $("#deluser").ligerGrid({
        checkbox: true,
        columns: [
          { display: '主键', name: 'ID', hide: 'hide', width: 1 }
        , { display: '登录名', name: 'Code', width: 200 },
        { display: '显示名', name: 'Name', width: 200 }
        ]
        , width: 'auto', pkName: 'ID', pageSize: 20, pageSizeOptions: [5, 10, 15, 20], height: '93%'
        , isChecked: delf_isChecked, onCheckRow: delf_onCheckRow, onCheckAllRow: f_delonCheckAllRow
        , toolbar: {
            items: [
            { text: '删除', click: delSave, icon: 'ok' },
            { text: '关闭', click: delExit, img: '../../lib/ligerUI/skins/icons/cancel.gif' }
            ]
        }
    });
});
//增加用户窗口保存按钮
function addSave(item) {
    var ids = "";
    for (var i = 0; i < $checkedAddUser.length; i++) {
        ids += $checkedAddUser[i] + ",";
    }
    ids = ids.substr(0, ids.length - 1);
    //启动异步请求到后台，查询修改数据
    $.post("/DepartmentUserinfo/AddUser", { DepId: $selectedDept, UsersId: ids }, function (data) {
        if (data == "ok") {
            //添加成功过了
            $checkedAddUser = [];
            $dlgAdd.hidden();
            $.getJSON("/DepartmentUserinfo/GetDepartmentUser", { SDep: $selectedDept }, function (data1) {
                $grid.set({ data: data1 });
            });
        } else {
            //添加失败了
            $checkedAddUser = [];
            $dlgAdd.hidden();
            $.getJSON("/DepartmentUserinfo/GetDepartmentUser", { SDep: $selectedDept }, function (data1) {
                $grid.set({ data: data1 });
            });
        }
    });
}
//增加用户窗口关闭按钮
function addExit(item) {
    $checkedAddUser = [];
    $dlgAdd.hidden();
}
//删除用户窗口保存按钮
function delSave(item) {
    var ids = "";
    for (var i = 0; i < $checkedDelUser.length; i++) {
        ids += $checkedDelUser[i] + ",";
    }
    ids = ids.substr(0, ids.length - 1);
    //启动异步请求到后台，查询修改数据
    $.post("/DepartmentUserinfo/DelUser", { DepId: $selectedDept, UsersId: ids }, function (data) {
        if (data == "ok") {
            //添加成功过了
            $checkedDelUser = [];
            $dlgDel.hidden();
            $.getJSON("/DepartmentUserinfo/GetDepartmentUser", { SDep: $selectedDept }, function (data1) {
                $grid.set({ data: data1 });
            });
        } else {
            //添加失败了
            $checkedDelUser = [];
            $dlgDel.hidden();
            $.getJSON("/DepartmentUserinfo/GetDepartmentUser", { SDep: $selectedDept }, function (data1) {
                $grid.set({ data: data1 });
            });
        }
    });
}
//删除用户窗口关闭按钮
function delExit(item) {
    $checkedDelUser = [];
}
//树节点单击事件
function treeClick(note) {
    $.getJSON("/DepartmentUserinfo/GetDepartmentUser", { SDep: note.data.CategoryId }, function (data) {
        $grid.set({ data: data });
    });
    $selectedDept = note.data.CategoryId;
}
//设置部门用户按钮单击事件
function setDepartmentUser(item) {
    //alert(item.text);
    if ($selectedDept == null) {
        alert("请选择要添加用户的部门！");
        return;
    }
    $.getJSON("/DepartmentUserinfo/GetAllUserInfo", {}, function (data) {
        $gridAddUser.set({ data: data });
    });
    $dlgAdd = $.ligerDialog.open({
        target: $("#adduser")
        , title: '添加部门用户'
        , width: 500
        , height: 400
        //,isResize :true
    });
}
//删除部门用户按钮单击事件
function DeleteDepartmentUser(item) {
    if ($selectedDept == null) {
        alert("请选择要删除用户的部门！");
        return;
    }
    $.getJSON("/DepartmentUserinfo/GetDepartmentUser", { SDep: $selectedDept }, function (data) {
        $gridDelUser.set({ data: data });
    });
    $dlgDel = $.ligerDialog.open({
        target: $("#deluser")
        , title: '删除部门用户'
        , width: 500
        , height: 700
        //,isResize :true
    });
}

/************************表单分页多选功能开始***************添加******************/
/*即利用onCheckRow将选中的行记忆下来，并利用isChecked将记忆下来的行初始化选中*/
function f_onCheckAllRow(checked) {
    for (var rowid in this.records) {
        if (checked)
            addCheckedUser(this.records[rowid]['ID']);
        else
            removeCheckedUser(this.records[rowid]['ID']);
    }
}

function findCheckedUser(userId) {
    for (var i = 0; i < $checkedAddUser.length; i++) {
        if ($checkedAddUser[i] == userId) return i;
    }
    return -1;
}
function addCheckedUser(userId) {
    if (findCheckedUser(userId) == -1)
        $checkedAddUser.push(userId);
}
function removeCheckedUser(userId) {
    var i = findCheckedUser(userId);
    if (i == -1) return;
    $checkedAddUser.splice(i, 1);
}
function f_isChecked(rowdata) {
    if (findCheckedUser(rowdata.ID) == -1)
        return false;
    return true;
}
function f_onCheckRow(checked, data) {
    if (checked) {
        addCheckedUser(data.ID);
    } else {
        removeCheckedUser(data.ID);
    }
}
function f_getChecked() {
    alert($checkedAddUser.join(','));
}
/************************表单分页多选功能结束*****************添加****************/
/************************表单分页多选功能开始*****************删除****************/
/*即利用onCheckRow将选中的行记忆下来，并利用isChecked将记忆下来的行初始化选中*/
function f_delonCheckAllRow(checked) {
    for (var rowid in this.records) {
        if (checked)
            deladdCheckedUser(this.records[rowid]['ID']);
        else
            delremoveCheckedUser(this.records[rowid]['ID']);
    }
}

function delfindCheckedUser(userId) {
    for (var i = 0; i < $checkedDelUser.length; i++) {
        if ($checkedDelUser[i] == userId) return i;
    }
    return -1;
}
function deladdCheckedUser(userId) {
    if (delfindCheckedUser(userId) == -1)
        $checkedDelUser.push(userId);
}
function delremoveCheckedUser(userId) {
    var i = delfindCheckedUser(userId);
    if (i == -1) return;
    $checkedDelUser.splice(i, 1);
}
function delf_isChecked(rowdata) {
    if (delfindCheckedUser(rowdata.ID) == -1)
        return false;
    return true;
}
function delf_onCheckRow(checked, data) {
    if (checked) {
        deladdCheckedUser(data.ID);
    } else {
        delremoveCheckedUser(data.ID);
    }
}
function delf_getChecked() {
    alert($checkedDelUser.join(','));
}
/************************表单分页多选功能结束************删除*********************/
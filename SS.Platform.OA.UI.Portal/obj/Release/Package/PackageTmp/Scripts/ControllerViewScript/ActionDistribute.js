var $gridRoleList;
var $selectedRole = null;
$(function () {
    $("#layout1").ligerLayout({ leftWidth: 230, allowLeftResize: false });
    //初始化角色列表
    initRoleGrid();
    //初始化工具条
    initToolbar();
    //初始化权限列表框
    initListBox();
});

//初始化角色列表
function initRoleGrid() {
    $gridRoleList = $("#rolegrid").ligerGrid({
        columns: [
        { display: '主键', name: 'ID', hide: 'hide', width: 1 }
        , { display: '角色', name: 'Name', width: 220 }
        ], width: '100%', pkName: 'ID', pageSize: 20, pageSizeOptions: [5, 10, 15, 20], height: '100%'
        , onSelectRow: roleGridClick
    });
    $.getJSON("/ActionDistribute/GetAllRoleInfo", {}, function (data) {
        $gridRoleList.set({ data: data });
    });
}

//角色列表单击事件
function roleGridClick(note) {
    $selectedRole = note.ID;
    liger.get("listbox1").setValue("");
    liger.get("listbox2").setValue("");
    liger.get("listbox2").removeItems(liger.get("listbox2").data);
    liger.get("listbox1").removeItems(liger.get("listbox1").data);
    $.getJSON("/ActionDistribute/GetRoleAction", { SRole: note.ID }, function (data) {
        liger.get("listbox2").setData(data.Rows);
        liger.get("listbox1").setData(data.RowsExcept);
    });
}
//初始化工具条
function initToolbar() {
    $("#toptoolbar").ligerToolBar({
        items: [
            {
                text: '保存', icon: 'save', click: editSave
            }
        ]
    });
}
//保存修改
function editSave() {
    if ($selectedRole != null) {
        //角色权限保存
        var ids = "";
        for (var j = 0; j < liger.get("listbox2").data.length; j++) {
            ids += liger.get("listbox2").data[j].id + ",";
        }
        ids = ids.substr(0, ids.length - 1);
        //启动异步请求到后台，修改数据
        $.post("/ActionDistribute/RoleActionSave", { RoleId: $selectedRole, ActionsId: ids }, function (data) {
            if (data == "ok") {
                //添加成功过了
                //重新绑定
            } else {
                alert("系统运行出错！");
            }
        });
    } else {
        alert("系统运行出错");
    }
}
/***********************列表框操作区开始************************/
//初始化列表框
function initListBox() {
    $("#listbox1,#listbox2").ligerListBox({
        textField: 'Name'
        , isShowCheckBox: true
        , isMultiSelect: true,
        height: 500
    });
}
function moveToLeft() {
    var box1 = liger.get("listbox1"), box2 = liger.get("listbox2");
    var selecteds = box2.getSelectedItems();
    if (!selecteds || !selecteds.length) return;
    box2.removeItems(selecteds);
    box1.addItems(selecteds);
}
function moveToRight() {
    var box1 = liger.get("listbox1"), box2 = liger.get("listbox2");
    var selecteds = box1.getSelectedItems();
    if (!selecteds || !selecteds.length) return;
    box1.removeItems(selecteds);
    box2.addItems(selecteds);
}
function moveAllToLeft() {
    var box1 = liger.get("listbox1"), box2 = liger.get("listbox2");
    var selecteds = box2.data;
    if (!selecteds || !selecteds.length) return;
    box1.addItems(selecteds);
    box2.removeItems(selecteds);
}
function moveAllToRight() {
    var box1 = liger.get("listbox1"), box2 = liger.get("listbox2");
    var selecteds = box1.data;
    if (!selecteds || !selecteds.length) return;
    box2.addItems(selecteds);
    box1.removeItems(selecteds);

}
/***********************列表框操作区结束************************/
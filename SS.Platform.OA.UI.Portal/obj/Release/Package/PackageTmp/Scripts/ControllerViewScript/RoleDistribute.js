var $gridUserList;
var $selectedUser = null;
var $treeDept;
var $selectedDept = null;
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
    $("#layout1").ligerLayout({ leftWidth: 230, allowLeftResize: false });
    //默认是部门角色
    $("#rdDep").attr("checked", "checked");
    //初始化单选按钮
    $("#rdCat input:radio").ligerRadio();
    //注册单选按钮单击事件
    $(":radio").click = rdClick;
    //初始化部门树
    initDepTree();
    //初始化用户列表
    initUserGrid();
    //初始化工具条
    initToolbar();
    //初始化列表框
    initListBox();
});
//单选按钮单击事件
function rdClick() {
    $selectedDept = null;
    $selectedUser = null;
    if (liger.get("listbox2").data != null)
    {
        liger.get("listbox2").removeItems(liger.get("listbox2").data);
    }
    if (liger.get("listbox1").data != null)
    {
        liger.get("listbox1").removeItems(liger.get("listbox1").data);
    }
    var rdCheck = $(":radio");
    for (var i = 0; i < rdCheck.length; i++) {
        if (rdCheck[i].checked == true) {
            if (rdCheck[i].value == "部门角色") {
                $(".l-layout-header-inner").html("部门列表");
                $.getJSON("/RoleDistribute/GetTreeDepNodes", {}, function (data) {
                    $treeDept.set({ data: data });
                });
                $("#userList").attr("style", "display:none;");
                $("#deptTree").attr("style", "display:block");
            } else if (rdCheck[i].value == "用户角色") {
                $(".l-layout-header-inner").html("用户列表");
                $("#deptTree").attr("style", "display:none;");
                $.getJSON("/RoleDistribute/GetAllUserInfo", {}, function (data) {
                    $gridUserList.set({ data: data });
                });
                $("#userList").attr("style", "display:block;");
            }
        }
    }
}
//初始化部门树
function initDepTree() {
    $.getJSON("/RoleDistribute/GetTreeDepNodes", {}, function (data) {
        var $treeDept = $("#tree1").ligerTree({
            checkbox: false
            , data: data,
            idFieldName: 'CategoryId',
            textFieldName: 'Name',
            slide: true,
            parentIDFieldName: 'ParentId'
            , onClick: treeClick
        });
        var treeManager = $("#tree1").ligerGetTreeManager();
        treeManager.expandAll();
    });
}
//树节点单击事件
function treeClick(note) {
    $selectedDept = note.data.Name;
    liger.get("listbox1").setValue("");
    liger.get("listbox2").setValue("");
    liger.get("listbox2").removeItems(liger.get("listbox2").data);
    liger.get("listbox1").removeItems(liger.get("listbox1").data);
    $.getJSON("/RoleDistribute/GetDepartmentRole", { SDep: note.data.CategoryId }, function (data) {
        liger.get("listbox2").setData(data.Rows);
        liger.get("listbox1").setData(data.RowsExcept);
    });
    $selectedDept = note.data.CategoryId;
}
//初始化用户列表框
function initUserGrid() {
    $gridUserList = $("#usergrid").ligerGrid({
        columns: [
        { display: '主键', name: 'ID', hide: 'hide', width: 1 }
        , { display: '显示名', name: 'Name', width: 220 }
        ], width: '100%', pkName: 'ID', pageSize: 20, pageSizeOptions: [5, 10, 15, 20], height: '100%'
        , onSelectRow :userGridClick
    });
}
//用户列表单击事件
function userGridClick(note) {
    $selectedUser = note.ID;
    liger.get("listbox2").removeItems(liger.get("listbox2").data);
    liger.get("listbox1").removeItems(liger.get("listbox1").data);
    $.getJSON("/RoleDistribute/GetUserRole", { SUser: note.ID }, function (data) {
        liger.get("listbox2").setData(data.Rows);
        liger.get("listbox1").setData(data.RowsExcept);
    });
    $selectedDept = note.data.CategoryId;
}
/**********************用户名过滤区代码开始**********************************/
function userSearch() {
    $.getJSON("/RoleDistribute/GetAllUserInfo", {}, function (data) {
        $gridUserList.options.data = data;
        $gridUserList.loadData(f_getWhere());
    });
}
function f_getWhere() {
    if (!$gridUserList) return null;
    var clause = function (rowdata, rowindex) {
        var key = $("#keyName").val();
        return rowdata.ShowName.indexOf(key) > -1;
    };
    return clause;
}
/**********************用户名过滤区代码结束**********************************/
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
    if ($selectedDept != null) {
        if ($selectedUser==null) {
            //部门角色保存
            var ids = "";
            for (var i = 0; i < liger.get("listbox2").data.length; i++) {
                ids += liger.get("listbox2").data[i].id + ",";
            }
            ids = ids.substr(0, ids.length - 1);
            //启动异步请求到后台，修改数据
            $.post("/RoleDistribute/DeptRoleSave", { DepId: $selectedDept, RolesId: ids }, function (data) {
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
    } else {
        if ($selectedUser != null) {
            //用户角色保存
            var ids2 = "";
            for (var j = 0; j < liger.get("listbox2").data.length; j++) {
                ids2 += liger.get("listbox2").data[j].id + ",";
            }
            ids2 = ids2.substr(0, ids2.length - 1);
            //启动异步请求到后台，修改数据
            $.post("/RoleDistribute/UserRoleSave", { UserId: $selectedUser, RolesId: ids2 }, function (data) {
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
}
/***********************列表框操作区开始************************/
//初始化列表框
function initListBox() {
    $("#listbox1,#listbox2").ligerListBox({
        isShowCheckBox: true
        ,isMultiSelect: true,
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
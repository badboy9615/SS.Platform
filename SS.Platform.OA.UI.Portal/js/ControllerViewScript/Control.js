var manager;
$(function () {
    var grid = manager=$("#maingrid").ligerGrid({
        height: '94%',
        url: '/Control/GetAllControl',
        //data: CustomersData, 

        columns: [
        { display: '模块', name: 'moduleName', align: 'left', width: '13%', minWidth: 30 },
        { display: '编码', name: 'Code', align: 'left', width: '13%', minWidth: 30 },
        { display: '名称', name: 'Name', align: 'left', width: '14%', minWidth: 30 },
        { display: 'URL', name: 'Url', align: 'left', width: '14%', minWidth: 30 },
        { display: '备注', name: 'Remark', align: 'left', width: '14%', minWidth: 30 },
        { display: '创建人', name: 'SubBy', align: 'left', width: '14%', minWidth: 30 },
        { display: '创建时间', name: 'SubTime', align: 'left', width: '14%', minWidth: 30, type: 'date' }
        ], pageSize: 10, rownumbers: false,
        autoFilter: false, rowHeight: 30,
        checkbox: true
        , isChecked: f_isChecked, onCheckRow: f_onCheckRow, onCheckAllRow: f_onCheckAllRow
        , onDblClickRow: function (data, rowindex, rowobj) {
            //$.ligerDialog.alert('选择的是' + data.ID);
            showEdit(data.ID);
        }
    });

    $("#toptoolbar").ligerToolBar({
        items: [
                    {
                        text: '增加', click: 
                            function (item) {
                                $.ligerDialog.open({
                                    height: 340,
                                    width: 345,
                                    title: '增加控制器',
                                    url: '/Control/test',
                                    //isDrag:false,
                                    showMax: true,
                                    showToggle: true,
                                    showMin: false,
                                    isResize: false,
                                    slide: false
                                });
                            }, icon: 'add'
                    },
                    { line: true },
                    {
                        text: '修改', click: function (item) {
                            var row = manager.getSelectedRow();
                            if (row == null) {
                                $.ligerDialog.warn('请选中要修改的控制器信息!');
                                return;
                            }
                            var id = row.ID;

                            showEdit(id);
                        }, icon: 'edit'
                    },
                    { line: true },
                    {
                        text: '删除', click: function (item) {
                            var row = manager.getSelectedRow();
                            if (row == null) {
                                $.ligerDialog.warn('请选中要删除的控制器信息!');
                                return;
                            }
                            var ids = checkedRow.join(',');
                            //启动异步请到后台，批量删除数据
                            $.post("/Control/DeleteIds", { ids: ids }, function (data) {
                                if (data == "ok") {
                                    //清除选中数据。
                                    manager.reload();
                                } else {
                                    $.messager.alert("错误提示", "删除出现错误！");
                                }
                            });
                        }, icon: 'delete'
                    }
        ]
    });

    $("#pageloading").hide();

    function f_validate() {
        var form = liger.get('form1');
        if (form.valid()) {
            alert('验证通过');
        }
        else {
            form.showInvalid();
        }
    }
});


//以下为多选处理
function f_onCheckAllRow(checked) {
    for (var rowid in this.records) {
        if (checked)
            addCheckedRow(this.records[rowid]['ID']);
        else
            removeCheckedRow(this.records[rowid]['ID']);
    }
}

/*
该例子实现 表单分页多选
即利用onCheckRow将选中的行记忆下来，并利用isChecked将记忆下来的行初始化选中
*/
var checkedRow = [];
function findCheckedRow(ID) {
    for (var i = 0; i < checkedRow.length; i++) {
        if (checkedRow[i] == ID) return i;
    }
    return -1;
}
//选择后加入到已选变量
function addCheckedRow(ID) {
    if (findCheckedRow(ID) == -1)
        checkedRow.push(ID);
}
//取消选择后从已选变量移除
function removeCheckedRow(ID) {
    var i = findCheckedRow(ID);
    if (i == -1) return;
    checkedRow.splice(i, 1);
}
function f_isChecked(rowdata) {
    if (findCheckedRow(rowdata.ID) == -1)
        return false;
    return true;
}
function f_onCheckRow(checked, data) {
    if (checked) addCheckedRow(data.ID);
    else removeCheckedRow(data.ID);
}
function f_getChecked() {
    alert(checkedRow.join(','));
}

function showEdit(id) {

    $.ligerDialog.open({
        height: 320,
        width: 345,
        title: '修改模块信息',
        url: '/Control/Edit',
        //isDrag:false,
        showMax: true,
        showToggle: true,
        showMin: false,
        isResize: false,
        slide: false,
        data: {
            id: id
        }
    });
}




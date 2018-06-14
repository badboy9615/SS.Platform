var manager;
$(function () {
    var grid = manager=$("#maingrid").ligerGrid({
        height: '94%',
        url: '/UserInfo/GetAllUser',
        //data: CustomersData, 

        columns: [
        { display: '登录名', name: 'Code', align: 'left', width: '16%', minWidth: 60 },
        { display: '显示名', name: 'Name', align: 'left', width: '17%', minWidth: 60 },
        { display: '邮件', name: 'Mail', align: 'left', width: '17%', minWidth: 60 },
        { display: '电话', name: 'Phone', align: 'left', width: '17%', minWidth: 60 },
        { display: 'QQ号码', name: 'QQNum', align: 'left', width: '17%', minWidth: 60 },
        { display: '备注', name: 'Remark', align: 'left', width: '15%', minWidth: 60 }
        ], pageSize: 10, rownumbers: false,
        autoFilter: false, rowHeight: 30,
        checkbox: true
        , isChecked: f_isChecked, onCheckRow: f_onCheckRow, onCheckAllRow: f_onCheckAllRow
        ,onDblClickRow : function (data, rowindex, rowobj)
        {
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
                                    height: 350,
                                    width: 650,
                                    title: '增加用户',
                                    url: '/UserInfo/Add',
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
                                $.ligerDialog.warn('请选中要修改的用户信息!');
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
                                $.ligerDialog.warn('请选中要删除的用户信息!');
                                return;
                            }
                            var ids = checkedCustomer.join(',');
                            //启动异步请到后台，批量删除数据
                            $.post("/UserInfo/DeleteIds", { ids: ids }, function (data) {
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
            addCheckedCustomer(this.records[rowid]['ID']);
        else
            removeCheckedCustomer(this.records[rowid]['ID']);
    }
}

/*
该例子实现 表单分页多选
即利用onCheckRow将选中的行记忆下来，并利用isChecked将记忆下来的行初始化选中
*/
var checkedCustomer = [];
function findCheckedCustomer(ID) {
    for (var i = 0; i < checkedCustomer.length; i++) {
        if (checkedCustomer[i] == ID) return i;
    }
    return -1;
}
//选择后加入到已选变量
function addCheckedCustomer(ID) {
    if (findCheckedCustomer(ID) == -1)
        checkedCustomer.push(ID);
}
//取消选择后从已选变量移除
function removeCheckedCustomer(ID) {
    var i = findCheckedCustomer(ID);
    if (i == -1) return;
    checkedCustomer.splice(i, 1);
}
function f_isChecked(rowdata) {
    if (findCheckedCustomer(rowdata.ID) == -1)
        return false;
    return true;
}
function f_onCheckRow(checked, data) {
    if (checked) addCheckedCustomer(data.ID);
    else removeCheckedCustomer(data.ID);
}
function f_getChecked() {
    alert(checkedCustomer.join(','));
}

function showEdit(id) {
   
    $.ligerDialog.open({
        height: 350,
        width: 650,
        title: '修改用户信息',
        url: '/UserInfo/Edit',
        //isDrag:false,
        showMax: true,
        showToggle: true,
        showMin: false,
        isResize: false,
        slide: false,
        data: {
            id:id
        }
    });
}




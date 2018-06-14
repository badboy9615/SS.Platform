//树状控件使用
var zTree;//对象指向咱们树
var treeNodes;//所有的树的节点
//zTree基本设置  
var setting = {
    showLine: true, //是否显示节点间的连线  
    checkable: false,
    //checkStyle: "radio",
    checkRadioType: "all",
    //async : true, //需要采用异步方式获取子节点数据,默认false  
    //asyncUrl : url, //当 async = true 时，设置异步获取节点的 URL 地址 ,允许接收 function 的引用  
    //asyncParam : ["CategoryId"], //提交的与节点数据相关的必需参数  
    isSimpleData: true, //数据是否采用简单 Array 格式，默认false  
    treeNodeKey: "CategoryId", //在isSimpleData格式下，当前节点id属性  
    treeNodeParentKey: "ParentId", //在isSimpleData格式下，当前节点的父节点id属性  
    nameCol: "Name",            //在isSimpleData格式下，当前节点名称  
    expandSpeed: "normal", //设置 zTree节点展开、折叠时的动画速度或取消动画(三种默认定义："slow", "normal", "fast")或 表示动画时长的毫秒数值(如：1000)          
    checkType: { "Y": "ps", "N": "ps" },
    callback: { //回调函数                              
        dblclick: zTreeOnDblclick
    }
};
//部门树双击事件
function zTreeOnDblclick(event, treeId, treeNode) {
    //第一件事：把vname和id拿到放到页面里面去
    var name = treeNode.Name;
    $("#ParDeptName").val(name);
    var id = treeNode.CategoryId;
    $("#ParDept").val(id);

    //关闭咱们树的对话框
    $("#treeParentDepartment").dialog("close");
}
$(function () {
    initTable();

    $("#addDiv").css("display", "none");
    $("#editDiv").css("display", "none");
    $("#setDepMasterDiv").css("display", "none");

    //部门查找按钮
    $("#linkSearch").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。

        var queryData = { SName: $("#searchName").val(), SDepMaster: $("#searchDepMaster").val() };

        initTable(queryData);
    });
    //选择上级部门点击事件
    $("#ParDeptName").click(function () {
        $("#treeParentDepartment").css("display", "block");
        $("#treeParentDepartment").dialog({
            title: "请选择上级部门",
            width: 380,
            height: 500,
            collapsible: true,
            minimizable: true,
            maximizable: true,
            resizable: true,
            modal: false
        });
    });

    //选择负责人按钮（添加）
    $("#SDepMaster").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。
        var queryData = { SName: $("#MasterName").val() };
        $("#MasterName").attr("value", "");
        $("#AddOrEdit").attr("value", "Add");
        showDepMasterDialog(queryData);
        //由于input type image 控件会触发提交事件，所以“return false 取消触发提交事件”
        return false;
    });

    //选择负责人按钮（修改）
    $("#SEdDepMaster").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。
        var queryData = { SName: $("#EdMasterName").val() };
        $("#EdMasterName").attr("value", "");
        $("#AddOrEdit").attr("value", "Edit");
        showDepMasterDialog(queryData);
        //由于input type image 控件会触发提交事件，所以“return false 取消触发提交事件”
        return false;
    });

    //选择负责人窗口查找按钮
    $("#linkSearchDepMaster").click(function () {
        //把要搜索的条件异步发送到后台。让后台返回新的json对象。再弄到表格上去。
        var queryData = { SName: $("#searchDepMasterName").val() };
        $("#searchDepMasterName").attr("value", "");
        showDepMasterDialog(queryData);
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
    initTree();
    $('#tt').datagrid({
        url: '/Department/GetAllDepartmentInfos',//rows=10  page=1
        title: '部门列表',
        width: 900,
        height: 490,
        fitColumns: true,
        idField: 'ID',
        loadMsg: '正在加载部门的信息...',
        pagination: true,
        singleSelect: false,
        pageSize: 15,
        pageNumber: 1,
        pageList: [10, 15, 30],
        queryParams: sarcheParam,//表格初始化往后台发送异步请求后台的json数据时候额外发送的请求参数。
        columns: [[//u.ID, u.UserName, u.DelFlag, u.Mail, u.Phone, u.SubTime, u.SubBy
            { field: 'ck', checkbox: true, align: 'left', width: 50 },
            { field: 'Code', title: '编码', width: 50 },
            { field: 'Name', title: '名称', width: 80 },
            //{ field: 'DepMaster', title: '负责人', width: 60 },
            { field: 'DepLevel', title: '级别', width: 60 },
            { field: 'ParentDep', title: '上级部门', width: 60 },
            { field: 'TakeEffect', title: '生效', width: 60 },
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

//初始化树
function initTree() {
    $("#treeParentDepartment").css("display", "none");

    $.getJSON("/Department/GetTreeDepNodes", {}, function (data) {
        zTree = $("#tree").zTree(setting, data);

        //使部门树全部展开
        zTree.expandAll(true);
    });
}

//选择负责人对话框
function showDepMasterDialog(sarcheParam) {
    //弹出一个选择用户组的对话框出来。
    $("#setDepMasterDiv").css("display", "block");
    $('#tbDepMaster').datagrid({
        url: '/Department/GetAllUsers',//rows=10  page=1
        title: '用户列表',
        width: 380,
        height: 425,
        fitColumns: true,
        idField: 'ID',
        loadMsg: '正在加载用户信息...',
        pagination: true,
        singleSelect:true,
        pageSize: 10,
        pageNumber: 1,
        pageList: [5,10,20],
        queryParams: sarcheParam,//表格初始化往后台发送异步请求后台的json数据时候额外发送的请求参数。
        columns: [[//u.ID, u.UserName, u.DelFlag, u.Mail, u.Phone, u.SubTime, u.SubBy
            //{ field: 'ck', checkbox: true, align: 'left', width: 50 },
            { field: 'Code', title: '登陆名', width: 80 },
            { field: 'Name', title: '显示名', width: 120 }
        ]],
        onHeaderContextMenu: function (e, field) {

        }
        , onDblClickRow: function (rowIndex) {
            if ($("#AddOrEdit").attr("value") == "Add") {
                $('#tbDepMaster').datagrid('unselectAll');
                $('#tbDepMaster').datagrid('selectRow', rowIndex);
                var rows = $("#tbDepMaster").datagrid('getSelected');
                $("#MasterName").attr("value", rows.Name);
                $("#Master").attr("value", rows.ID);
                $('#setDepMasterDiv').dialog('close');
            }
            if ($("#AddOrEdit").attr("value") == "Edit") {
                $('#tbDepMaster').datagrid('unselectAll');
                $('#tbDepMaster').datagrid('selectRow', rowIndex);
                var rowsEdit = $("#tbDepMaster").datagrid('getSelected');
                $("#EdMasterName").attr("value", rowsEdit.Name);
                $("#EdMaster").attr("value", rowsEdit.ID);
                $('#setDepMasterDiv').dialog('close');
            }
        }
    });
    $("#setDepMasterDiv").dialog({
        title: "选择负责人",
        width: 400,
        height: 500,
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
        title: "添加部门信息",
        width: 490,
        height: 300,
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
        $.messager.alert("错误消息", "请选中要修改的部门信息？");
        return;
    }
    if (rows.length > 1) {
        $.messager.alert("错误消息", "一次只能修改一个部门的信息！！");
        return;
    }
    var ids = "";
    ids += rows[0].ID;
    //启动异步请求到后台，查询修改数据
    $.post("/Department/Edit", { ids: ids }, function (data) {
        if (data.model.ParentDep != null) {
            $("#EdParDeptName").attr("value", data.model.ParentDep.Name);
            $("#EdParDept").attr("value", data.model.ParentDep.ID);
        }
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
        $("#EdCode").attr("value", data.model.Code);
        $("#EdName").attr("value", data.model.Name);
        //$("#EdMasterName").attr("value", data.model.DepMaster.Name);
        //$("#EdMaster").attr("value", data.model.DepMaster.ID);
        $("#EdTakeEffectTime").datebox("setValue", (eval(data.model.TakeEffectTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d"));
        $("#EdID").attr("value", data.model.ID);
        $("#EdIn_Id").attr("value", data.model.In_Id);
        $("#EdLoseEffectTime").datebox("setValue", (eval(data.model.LoseEffectTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d"));
        $("#EdRemark").attr("value", data.model.Remark);
        $("#EdSubBy").attr("value", data.model.SubBy);
        $("#EdSubTime").attr("value", (eval(data.model.SubTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d"));
        $("#EdDepLevel").attr("value", data.model.DepLevel);
    });
    //弹出一个当前选中实体修改的对话框出来。
    $("#editDiv").css("display", "block");
    clearEdControlData();
    $("#editDiv").dialog({
        title: "修改部门信息",
        width: 490,
        height: 300,
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
                    $("#EdParDeptName").removeAttr("disabled");
                    $("#EdCode").removeAttr("disabled");
                    $("#editDiv form").submit();
                    $("#EdParDeptName").attr("disabled", "disabled");
                    $("#EdCode").attr("disabled", "disabled");
                } else {
                    $("#EdTakeEffectTime").datebox("enable");
                    $("#EdLoseEffectTime").datebox("enable");
                    $("#EdParDeptName").removeAttr("disabled");
                    $("#EdCode").removeAttr("disabled");
                    $("#editDiv form").submit();
                    $("#EdTakeEffectTime").datebox("disable");
                    $("#EdLoseEffectTime").datebox("disable");
                    $("#EdParDeptName").attr("disabled", "disabled");
                    $("#EdCode").attr("disabled", "disabled");
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

    $("#Code").attr("value", "自动生成");
    $("#Name").attr("value", "");
    $("#MasterName").attr("value", "");
    $("#Master").attr("value", "");
    $("#ParDeptName").attr("value", "--请选择上级部门--");
    $("#ParDept").attr("value", "");
    $("#Remark").attr("value", "");
}

//设置修改对话框控件默认值
function clearEdControlData() {

    var currTime = new Date();
    var strDate = currTime.getFullYear() + "-";
    strDate += currTime.getMonth() + 1 + "-";
    strDate += currTime.getDate();
    $("#EdParDeptName").attr("value", "");
    $("#EdParDept").attr("value", "");
    $("#EdCode").attr("value", "");
    $("#EdName").attr("value", "");
    $("#EdTakeEffect").attr("value", "false");
    $("#EdTakeEffectTime").datebox("setValue", strDate);
    $("#EdMasterName").attr("value", "");
    $("#EdMaster").attr("value", "");
    $("#EdID").attr("value", "");
    $("#EdLoseEffectTime").datebox("setValue", "9999-12-31");
    $("#EdRemark").attr("value", "");
    $("#EdSubBy").attr("value", "");
    $("#EdSubTime").attr("value", "");
    $("#EdParDeptName").attr("disabled", "disabled");
    $("#EdCode").attr("disabled", "disabled");
}
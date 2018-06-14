$(function () {
    liger.get("Code").setDisabled();
});
function doSave() {
    var options = {
        success: function (data) {
            if (data == "ok") {
                window.parent.$("#maingrid").ligerGrid().reload();
                parent.$.ligerDialog.hide();
            } else {
                $.ligerDialog.warn(data);
            }
        }
    };
    $("#Add").ajaxSubmit(options);
}
function doClose() {
    window.parent.$("#maingrid").ligerGrid().reload();
    parent.$.ligerDialog.hide();
}



$("#popTxt").ligerPopupEdit({
    condition: {
        prefixID: 'condtion_',
        fields: [
            { name: 'CompanyName', type: 'text', label: '客户' }
        ]
    },
    grid: getGridOptions(true),
    searchClick: function (e) {

        //e.grid.set('parms', { userName: 'zzq' });
        //e.grid.reload();

        //grid = e.grid 
        alert("这里可以根据搜索规则来搜索（下面的代码已经本地搜索),搜索规则:" + liger.toJSON(e.rules));
        e.grid.loadData($.ligerFilter.getFilterFunction(e.rules));
    },
    valueField: 'CustomerID',
    textField: 'CustomerID',
    width: 600
});

function getGridOptions(checkbox) {
    var options = {
        columns: [
        { display: '顾客', name: 'CustomerID', align: 'left', width: 100, minWidth: 60 },
        { display: '公司名', name: 'CompanyName', minWidth: 120, width: 100 },
        { display: '联系名', name: 'ContactName', minWidth: 140, width: 100 },
        { display: '电话', name: 'Phone', width: 100 },
        { display: '城市', name: 'City', width: 100 },
        { display: '国家', name: 'Country', width: 100 }
        ], switchPageSizeApplyComboBox: false,
        data: $.extend({}, CustomersData),
        pageSize: 10,
        checkbox: checkbox
    };
    return options;
}
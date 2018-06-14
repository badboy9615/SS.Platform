var form;
$(function () {
    form = liger.get("edit");
    dialog = frameElement.dialog; //调用页面的dialog对象(ligerui对象)
    id = dialog.get('data').id;//获取id参数

    liger.get("Code").setDisabled();

    //启动异步请求到后台，查询修改数据
    $.post("/Module/GetModuleByID", { id: id }, function (data) {
        var subTime = eval('new ' + data.model.SubTime.substr(1, data.model.SubTime.length - 2));
        var takeTime = eval('new ' + data.model.TakeEffectTime.substr(1, data.model.TakeEffectTime.length - 2));
        form.setData({
            ID: data.model.ID
            , SubBy: data.model.SubBy
            , SubTime: subTime.getFullYear() + '-' + (subTime.getMonth() + 1).toString() + '-' + subTime.getDate()
            , TakeEffectTime: takeTime.getFullYear() + '-' + (takeTime.getMonth() + 1).toString() + '-' + takeTime.getDate()

            , Code: data.model.Code
            , Name: data.model.Name
            , Url: data.model.Url
            , Remark: data.model.Remark
        });
    });
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
    $("#edit").ajaxSubmit(options);
}
function doClose() {
    window.parent.$("#maingrid").ligerGrid().reload();
    parent.$.ligerDialog.hide();
}
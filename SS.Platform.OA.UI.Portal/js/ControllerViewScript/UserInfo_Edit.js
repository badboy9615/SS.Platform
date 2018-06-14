var form;
$(function () {
    form = liger.get("userEdit");
    dialog = frameElement.dialog; //调用页面的dialog对象(ligerui对象)
    id = dialog.get('data').id;//获取id参数

    liger.get("Code").setDisabled();
    liger.get("TakeEffectTime").setDisabled();
    liger.get("LoseEffectTime").setDisabled();

    //启动异步请求到后台，查询修改数据
    $.post("/UserInfo/GetUserInfoByID", { id: id }, function (data) {
        var subTime = eval('new ' + data.model.SubTime.substr(1, data.model.SubTime.length - 2));
        //var time = (eval(data.model.SubTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d");
        form.setData({
            ID: data.model.ID
            , Pwd: data.model.Pwd
            , SubBy: data.model.SubBy
            , SubTime: subTime.getFullYear() + '-' + (subTime.getMonth() + 1).toString() + '-' + subTime.getDate()
            , Code: data.model.Code
            , Name: data.model.Name
            , QQNum: data.model.QQNum
            , TakeEffect: data.model.TakeEffect
            , TakeEffect1: data.model.TakeEffect
            , Mail: data.model.Mail
            , TakeEffectTime: data.model.TakeEffectTime
            , Phone: data.model.Phone
            , LoseEffectTime: data.model.LoseEffectTime
            , Remark: data.model.Remark
        });
    });
});
function doSave() {
    $("#TakeEffect").val(liger.get("TakeEffect1").getValue().toString());
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
    $("#userEdit").ajaxSubmit(options);
}
function doClose() {
    window.parent.$("#maingrid").ligerGrid().reload();
    parent.$.ligerDialog.hide();
}
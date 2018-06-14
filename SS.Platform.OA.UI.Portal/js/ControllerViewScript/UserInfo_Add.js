$(function () {

    var form = liger.get("userAdd");

    form.setData({
        TakeEffect1: true,

        TakeEffectTime: new Date(),
        LoseEffectTime: new Date(9999, 11, 31)
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
    $("#userAdd").ajaxSubmit(options);
}
function doClose() {
    window.parent.$("#maingrid").ligerGrid().reload();
    parent.$.ligerDialog.hide();
}
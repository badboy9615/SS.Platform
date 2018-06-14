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
    $("#moduleAdd").ajaxSubmit(options);
}
function doClose() {
    window.parent.$("#maingrid").ligerGrid().reload();
    parent.$.ligerDialog.hide();
}
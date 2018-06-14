$(function () {
    //$("#editDiv").css("display", "none");
});

function afterEditPass(data) {
    if (data == "ok") {
        //修改成功了
        //this.close();
        //win.hide();
        //$.ligerDialog.close();
        //parent.win.hide();
    }
    else {
        alert(data);
    }
}
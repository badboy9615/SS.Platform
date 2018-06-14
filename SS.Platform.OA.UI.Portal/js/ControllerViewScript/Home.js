var win;
$(function () {
    //启动异步请求到后台，查询修改数据
    $.post("/Home/GetCurrentUser", {}, function (data) {
        $("#UserName").html(data.model.Name);
    });
    //修改密码
    $("#EditPass").click(function () {
        //f_open('/EditPass/Index', '修改密码', '../../Lib/Content/Images/3DSMAX.png', 300, 200);
        win = $.ligerDialog.open(
        {
            target: $("#editDiv"), height: 210, url: '/EditPass/Index', width: 280, showMax: true, showToggle: true, showMin: true, isResize: true, modal: false, title: '修改密码', slide: false, buttons: [
              {
                  text: '保存', onclick: function (item, Dialog, index) {
                      $("#editDiv form").submit();
                  }
              } ,{
                  text: '返回', onclick: function (item, Dialog, index) {
                      win.hide();
                  }
              }
            ]
        });
        var task = jQuery.ligerui.win.tasks[win.id];
        if (task) {
            $(".l-taskbar-task-icon:first", task).html('<img src="../../Lib/Content/Images/3DSMAX.png" />');
        }
    });
    //注销
    $("#LogOut").click(function () {
        $.post("/Home/Logout", {}, function (data) {
            if (data = "ok") {
                window.location.href = '/Login';
            }
        });
    });
});
function clearControlData() {
    $("#OldPass").attr("value", "");
    $("#NewPass").attr("value", "");
    $("#ConfPass").attr("value", "");   
}

//当修改成功之后，由子容器来调用的方法：关闭修改对话框，刷新表格
function afterEditPass(data) {
    if (data == "ok") {
        win.hide();
        //没有登陆，
        //filterContext.HttpContext.Response.Redirect("/Login");

        //启动异步请求到后台，查询修改数据
        $.post("/Home/Logout", {}, function (data) {
            if (data="ok") {
                window.location.href= '/Login';
            }
        });
    }
    else {
        $.ligerDialog.question(data);
    }
}

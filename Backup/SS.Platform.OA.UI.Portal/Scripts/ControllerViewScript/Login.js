$(function () {
    $("#userName").keypress(function (event) {
        if(event.keyCode==13)
        {
            $("#passWord")[0].focus();
        }
    });

    $("#passWord").keypress(function (event) {
        if (event.keyCode == 13) {
            $("#authcode")[0].focus();
        }
    });

    $("#authcode").keypress(function (event) {
        if (event.keyCode == 13) {
            $("#form form").submit();
        }
    });
});
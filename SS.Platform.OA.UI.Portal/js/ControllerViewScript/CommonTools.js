
//格式化日期
function formatDate(v) {
    var date = new Date(parseInt(v.replace("/Date(", "").replace(")/", ""), 10));
    if (date instanceof Date) {
        var y = date.getFullYear();
        var m = date.getMonth() + 1;
        var d = date.getDate();
        var h = date.getHours();
        var i = date.getMinutes();
        var s = date.getSeconds();
        var ms = date.getMilliseconds();
        if (ms > 0)
            return y + '-' + m + '-' + d + ' ' + h + ':' + i + ':' + s
                    + '.' + ms;
        if (h > 0 || i > 0 || s > 0)
            return y + '-' + m + '-' + d + ' ' + h + ':' + i + ':' + s;
        return y + '-' + m + '-' + d;
    }
    return '';
}
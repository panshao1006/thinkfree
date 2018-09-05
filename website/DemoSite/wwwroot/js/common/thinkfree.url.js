var UrlWarpper = {
    getUrlParam: function (key) {
        var reg = new RegExp("(^|&)" + key + "=([^&]*)(&|$)");
        //匹配目标参数
        var r = window.location.search.substr(1).match(reg);
        //返回参数值
        if (r != null) {
            return decodeURI(r[2]);
        }
        return null;
    }
}
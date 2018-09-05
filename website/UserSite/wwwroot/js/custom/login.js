var Login = (function () {
    function Login() {
        this.loginRequestUri = "/Home/Login/";
    };

    Login.prototype.init = function () {
        var _this = this;
        _this.initAction();
    }

    Login.prototype.initAction = function () {
        var _this = this;

        $("#btnLogin").off("click").on("click", function () {
            _this.login();
        });
    }

    Login.prototype.login = function () {
        var _this = this;

        var user = {};
        user.Name = $("#username").val();
        user.Password = $("#password").val();
        user.ReturnUri = UrlWarpper.getUrlParam("returnuri");
        
        if (!user.ReturnUri) {
            alert("连接参数错误：returnuri是必填的");
        }

        $.ajax({
            dataType: "json",
            contentType: 'application/json;charset=utf-8',
            url: _this.loginRequestUri,
            data: JSON.stringify(user),
            async: false,
            type: "POST",
            success: function (msg) {
                if (msg.isSuccess) {
                    result = msg.data;

                    var isChecked = $('#remember').is(":checked");

                    if (isChecked) {
                        $.cookie("Token", result.token, { expires: 7, domain: "thinkfree.top" });
                    } else {
                        $.cookie("Token", result.token, { domain: "thinkfree.top" });
                    }

                    
                    if (result.returnUri) {
                        window.location.href = result.returnUri;
                    }
                }
            },
            error: function () {
                alert("登录失败");
            }
        });
    }

    return Login;
}());
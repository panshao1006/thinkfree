var News = (function () {
    function News() {
        this.newsId = '';
        this.requestUri = "/News/GetNews";
        this.titleElement = ".title";
        this.authorElement = ".author";

        this.contentElement = ".news-content";
    }

    News.prototype.init = function () {
        var _this = this;

        _this.getNewsId();
        var news = _this.GetNews();
        _this.initUI(news);
    }

    News.prototype.getNewsId = function () {
        var _this = this;

        if (_this.newsId) {
            return _this.newsId;
        }

        _this.newsId = UrlWarpper.getUrlParam("id");

        return _this.newsId;

    }

    News.prototype.GetNews = function () {
        var _this = this;

        var result;

        $.ajax({
            dataType: "json",
            url: _this.requestUri + "?id=" + _this.newsId,
            async: false,
            type: "GET",
            success: function (msg) {
                if (msg.isSuccess) {
                    result = msg.data;
                }
            },
            error: function () {
                alert("获取新闻内容失败");
            }
        });

        return result;
    }

    News.prototype.initUI = function (news) {
        var _this = this;

        $(_this.titleElement).append(news.title);

        var authorHtml = "";

        if (news.authorName) {
            authorHtml += "作者：" + news.authorName + ".";
        };
        authorHtml += news.createDate;

        $(_this.authorElement).append(authorHtml);

        $(_this.contentElement).append(news.content);
    }

    return News;
}())
var NewsAbstract = (function () {
    function NewsAbstract() {
        this.RequestUri = "/NewsAbstract/Get"; 
        this.listElement = ".news-list";
        this.btnReadNewsElement = ".btn-read-news";
        this.newsUri = "/News/Index/";
        //this._this = this;
    }

    NewsAbstract.prototype.init = function () {

        var _this = this;

        var result = _this.GetList();

        _this.bindData(result);

        _this.initAction();
    };

    NewsAbstract.prototype.bindData = function (list) {

        var _this = this;

        if (!list || list.length == 0) {
            return;
        }

        var newsHtml = "";
        for (var i = 0; i < list.length; i++) {
            var news = list[i];
            newsHtml += '<div class="news-abstract" news-index="'+i+'">';
            newsHtml += '<div class="news-abstract-title">';
            newsHtml += news.title;
            newsHtml += '</div>';
            newsHtml += '<div class="news-abstract-author">';
            if (news.authorName) {
                newsHtml += "作者：" + news.authorName+".";
            };
            newsHtml += news.createDate;
            newsHtml += '</div>';
            //newsHtml += '<div class="news-abstract-image">';
            //newsHtml += '    <a href = "javascript:void(0);">';
            //newsHtml += '       <img src="~/images/news/1166362-20170528144914578-1303493743.png"/>';
            //newsHtml += '    </a>';
            //newsHtml += '</div>';
            newsHtml += '<div class="news-abstract-text">' + news.abstract + '</div >';
            newsHtml += '<div><a class="btn-read-news" href="javascript:void(0);" authid="'+news.id+'"> 阅读全文</a></div>';
            newsHtml += '</div>';
        }

        $(_this.listElement).append(newsHtml);
    }

    NewsAbstract.prototype.initAction = function () {
        var _this = this;

        $(_this.btnReadNewsElement).off("click").on("click", function () {
            var newsId = $(this).attr("authid");

            window.location.href = _this.newsUri + "?id=" + newsId;

        });

    }

    NewsAbstract.prototype.GetList = function () {

        var _this = this;

        var result;

        $.ajax({
            dataType: "json",
            url: _this.RequestUri,
            async:false,
            type:"GET",
            success: function (msg) {
                if (msg.isSuccess) {
                    result = msg.data;
                }
            },
            error: function () {
                alert("获取列表失败");
            }
        });

        return result;
    };

    return NewsAbstract;
}())


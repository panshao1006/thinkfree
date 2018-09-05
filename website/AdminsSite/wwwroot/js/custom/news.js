var News = (function () {
    function News() {
        this.createRequestUri = "/News/Create/";
        this.getNewsAbstractUri = "/News/GetAbstracts/";
        this.getNewsUri = "/News/GetNews";
        this.deleteRequestUri = "/News/Delete";
        this.updateRequestUri = "/News/Update";
        this.btnSaveElement = "#btnSave";
    }

    News.prototype.initEditUI = function () {
        var _this = this;

        var news = _this.getNewsEditModel();

        _this.bindEditData(news);
        
        _this.bindAction();
    }

    News.prototype.getNewsEditModel = function () {
        var id = UrlWarpper.getUrlParam("id");
        //如果没有id标识新增
        if (!id) {
            return;
        }

        var _this = this;
        
        var result;

        $.ajax({
            dataType: "json",
            contentType: 'application/json;charset=utf-8',
            url: _this.getNewsUri+"?id="+ id,
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

    News.prototype.bindEditData = function (news) {
        if (!news) {
            return;
        }

        var _this = this;

        _this.setNewsModel(news);
    }

    News.prototype.initAbstractList = function () {
        var _this = this;

        var abstracts = _this.getAbstract();

        _this.bindData(abstracts);

        _this.bindAction();
    }


    News.prototype.bindAction = function () {
        var _this = this;
        $(_this.btnSaveElement).off("click").on("click", function () {

            var id = UrlWarpper.getUrlParam("id");

            if (id) {
                _this.update(id);
            } else {
                _this.create();
            }
        });

        $(".btn-danger").off("click").on("click", function () {

            var id = $(this).attr("keyid");

            if (id) {
                _this.delete(id);
            } else {
                
            }
        });
    }


    News.prototype.getAbstract = function () {

        var _this = this;

        var result;

        $.ajax({
            dataType: "json",
            contentType: 'application/json;charset=utf-8',
            url: _this.getNewsAbstractUri,
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


    News.prototype.bindData = function (list) {

        var _this = this;

        if (!list || list.length == 0) {
            return;
        }

        var newsHtml = "";
        for (var i = 0; i < list.length; i++) {
            var news = list[i];
            newsHtml += '<tr>';
            newsHtml += '<td>' + (i + 1) + '</td>';
            newsHtml += '<td class="center">' + news.title + '</td>';
            newsHtml += '<td class="center">' + news.authorName + '</td>';
            newsHtml += '<td class="center"></td>';
            newsHtml += '<td class="center">';
            newsHtml += '<span class="label label-success">Active</span>';
            newsHtml += '</td>';
            newsHtml += '<td class="center">';
            newsHtml += '<a class="btn btn-success" href="/News/Edit/">';
            newsHtml += '<i class="icon-zoom-in "></i>';
            newsHtml += '</a>';
            newsHtml += '<a class="btn btn-info" href="/News/Edit/?id=' + news.id + '">';
            newsHtml += '<i class="icon-edit "></i>';
            newsHtml += '</a>';
            newsHtml += '<a class="btn btn-danger" href="javacript:void(0);" keyid="' + news.id + '">';
            newsHtml += '<i class="icon-trash "></i>';
            newsHtml += '</a>';
            newsHtml += '</td>';
            newsHtml += '</tr>';
        }

        $("tbody",".box-content").append(newsHtml);
    }


    News.prototype.Create = function () {

        var _this = this;

        var news = _this.getNewsModel();

        $.ajax({
            dataType: "json",
            contentType: 'application/json;charset=utf-8',
            url: _this.createRequestUri,
            data: JSON.stringify(news),
            async: false,
            type: "POST",
            success: function (msg) {
                if (msg.isSuccess) {
                    result = msg.data;
                    alert("保存成功");
                    location.href = _this.getNewsUri + "?id=" + resize.id;
                }
            },
            error: function () {
                alert("获取新闻内容失败");
            }
        });
    }

    News.prototype.update = function (id) {

        var _this = this;

        var news = _this.getNewsModel();

        news.id = id;

        $.ajax({
            dataType: "json",
            contentType: 'application/json;charset=utf-8',
            url: _this.updateRequestUri,
            data: JSON.stringify(news),
            async: false,
            type: "POST",
            success: function (msg) {
                if (msg.isSuccess) {
                    result = msg.data;
                    alert("保存成功");
                    location.href.reload(true);
                }
            },
            error: function () {
                alert("获取新闻内容失败");
            }
        });
    }

    News.prototype.delete = function (id) {

        var _this = this;

        $.ajax({
            dataType: "json",
            contentType: 'application/json;charset=utf-8',
            url: _this.deleteRequestUri+"?id="+id,
            async: false,
            type: "get",
            success: function (msg) {
                if (msg.isSuccess) {
                    alert("删除成功");

                    window.location.reload(true);
                }
            },
            error: function () {
                alert("获取新闻内容失败");
            }
        });
    }


    News.prototype.getNewsModel = function () {
        var news = {
            Title: $("#tbxTitle").val(),
            CreateDate: $("#tbxCreateDate").val(),
            Abstract: $("#tbxAbstract").val(),
            Content: $("#tbxContent").val()
        }

        return news;
    }

    News.prototype.setNewsModel = function (news) {
        $("#tbxTitle").val(news.title);
        $("#tbxCreateDate").val(news.createDate);

        var abstractCleditor = $("#tbxAbstract").cleditor()[0];

        abstractCleditor.$area.val(news.abstract);
        abstractCleditor.updateFrame();
        
        var contentEditor = $("#tbxContent").cleditor()[0];

        contentEditor.$area.val(news.content);
        contentEditor.updateFrame();
    }

    return News;
}())
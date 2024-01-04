var JBlog = (function(window, $) {
    var ins = {};
    ins.initList = function () {
        var $form = $("#JBlog_list_form");
        $form.find('*[id="ListOptions_PageSize"]').val();
        var pageSize = ("#ListOptions_PageSize").val();
        $("#page_size_select").val(pageSize);
    };

    ins.submitListForm = function (page, event) {
        event.preventDefault();
        var $form = $("#JBlog_list_form");
        //$form.find('*[name="ListOptions.PageSize"]').val(localStorage.getItem('pageSize'));
        // 1. cập nhật lại pageIndex
        $form.find('*[name="ListOptions.PageIndex"]').val(page);
        $form.submit();
    };

    ins.submitForm = function (event) {
        var error = 0;
        event.preventDefault();
        if ($('#Title').val() == "") {
            $('.error_Title').text('Vui lòng nhập thông tin');
            error++;
        } else {
            $('.error_Title').text('');
        }

        if ($('#IntroText').val() == "") {
            $('.error_IntroText').text('Vui lòng nhập thông tin');
            error++;
        } else {
            $('.error_IntroText').text('');
        }

        if (error==0) {
            $('#form_Blog').submit();
        }
    }

    ins.changePageSize = function (event) {
        event.preventDefault();
        var $form = $("#JBlog_list_form");
        // 1. cập nhật lại pageIndex
        $form.find('*[name="ListOptions.PageIndex"]').val(1);
        // 1. cập nhật lại pageSize
        var pageSize = $("#page_size_select").val();
        $form.find('"*[name="ListOptions.PageSize"').val(pageSize);
        //localStorage.setItem('pageSize', pageSize)
        $form.submit();
    };

    return ins;
})(window,jQuery);
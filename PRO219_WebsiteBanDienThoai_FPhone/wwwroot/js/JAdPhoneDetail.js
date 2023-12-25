var JAdPhoneDetail = (function (window, $) {
    var ins = {};
    ins.initList = function () {
        var $form = $("#JAdPhoneDetail_list_form");
        $form.find('*[id="ListOptions_PageSize"]').val();
        var pageSize = $("#ListOptions_PageSize").val();
        $("#page_size_select").val(pageSize);
    };
    ins.submitListForm = function (page, event) {
        event.preventDefault();
        var $form = $("#JAdPhoneDetail_list_form");
        //$form.find('*[name="ListOptions.PageSize"]').val(localStorage.getItem('pageSize'));
        // 1. cập nhật lại pageIndex
        $form.find('*[name="ListOptions.PageIndex"]').val(page);
        $form.submit();
    };

    ins.changePageSize = function (event) {
        event.preventDefault();
        var sizeOptions = $("#ListOptions_PageSize");

        var $form = $("#JAdPhoneDetail_list_form");
        // 1. cập nhật lại pageIndex
        $form.find('*[name="ListOptions.PageIndex"]').val(1);
        // 1. cập nhật lại pageSize
        var pageSize = $("#page_size_select").val();
        sizeOptions.val(pageSize);
        //localStorage.setItem('pageSize', pageSize)
        $form.submit();
    };


    return ins;
})(window, jQuery);
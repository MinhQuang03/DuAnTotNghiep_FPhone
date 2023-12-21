var JAdAccount = (function(window, $) {
    var ins = {};
    ins.initList = function() {
        const $form = $("#JAdAccount_list_form");
        $form.find('*[id="Options_PageSize"]').val();
        const pageSize = ("#Options_PageSize").val();
        $("#page_size_select").val(pageSize);
    };
    ins.submitListForm = function(page, event) {
        event.preventDefault();
        const $form = $("#JAdAccount_list_form");
        //$form.find('*[name="ListOptions.PageSize"]').val(localStorage.getItem('pageSize'));
        // 1. cập nhật lại pageIndex
        $form.find('*[name="Options.PageIndex"]').val(page);
        $form.submit();
    };

    ins.changePageSize = function(event) {
        event.preventDefault();
        const $form = $("#JAdAccount_list_form");
        // 1. cập nhật lại pageIndex
        $form.find('*[name="Options.PageIndex"]').val(1);
        // 1. cập nhật lại pageSize
        const pageSize = $("#page_size_select").val();
        $form.find('"*[name="Options.PageSize"').val(pageSize);
        //localStorage.setItem('pageSize', pageSize)
        $form.submit();
    };


    return ins;
})(window, jQuery);
var JAdPhone = (function (window, $) {
    var ins = {};
    ins.initList = function () {
        var $form = $("#JAdPhone_list_form");
        $form.find('*[id="ListOptions_PageSize"]').val();
        var pageSize = $("#ListOptions_PageSize").val();
        $("#page_size_select").val(pageSize);
    };
    ins.submitListForm = function (page, event) {
        event.preventDefault();
        var $form = $("#JAdPhone_list_form");
        //$form.find('*[name="ListOptions.PageSize"]').val(localStorage.getItem('pageSize'));
        // 1. cập nhật lại pageIndex
        $form.find('*[name="ListOptions.PageIndex"]').val(page);
        $form.submit();
    };

    ins.changePageSize = function (event) {
        event.preventDefault();
        var sizeOptions = $("#ListOptions_PageSize");   

        var $form = $("#JAdPhone_list_form");
        // 1. cập nhật lại pageIndex
        $form.find('*[name="ListOptions.PageIndex"]').val(1);
        // 1. cập nhật lại pageSize
        var pageSize = $("#page_size_select").val();
        sizeOptions.val(pageSize);
        //localStorage.setItem('pageSize', pageSize)
        $form.submit();
    };


    ins.previewImages = function() {
        var preview = $("#imagePreview");
        var files = $("#fileInput").prop("files");
        if (files.length > 0) {
            var reader = new FileReader();
            reader.onload = function(e) {
                preview.attr("src", e.target.result);
            };

            reader.readAsDataURL(files[0]); // Đọc chỉ tệp đầu tiên trong danh sách
        } else {
            alert("Vui lòng chọn một tệp ảnh.");
        }
    };


    return ins;
})(window, jQuery);
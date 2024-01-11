var JAdListImage = (function(window, $) {
    var ins = {};
    const newValue = Math.random();
    ins.initList = function() {
        const $form = $("#JAdListImage_list_form");
        $("#page_size_select").val($form.find('*[name="Options.PageSize"]').val());
    };
    ins.submitListForm = function(page, event) {
        event.preventDefault();
        const $form = $("#JAdListImage_list_form");
        //$form.find('*[name="ListOptions.PageSize"]').val(localStorage.getItem('pageSize'));
        // 1. cập nhật lại pageIndex
        $form.find('*[name="Options.PageIndex"]').val(page);
        $form.submit();
    };

    ins.changePageSize = function(event) {
        event.preventDefault();
        const $form = $("#JAdListImage_list_form");
        // 1. cập nhật lại pageIndex
        $form.find('*[name="Options.PageIndex"]').val(1);
        // 1. cập nhật lại pageSize
        const pageSize = $("#page_size_select").val();
        $form.find('*[name="Options.PageSize"]').val(pageSize);
        //localStorage.setItem('pageSize', pageSize)
        $form.submit();
    };

    ins.previewImages = function() {
        var preview = $("#imagePreview");
        var  files = $("#fileInput")[0].files;
        /*  preview.empty(); // Xóa hết các ảnh đã hiển thị trước đó*/
        if (files.length > 5) {
            alert("Tối đa 5 ảnh");
            return ins.ClearImage();
        }
        for (let i = 0; i < files.length; i++) {
         
            var file = files[i];
            var reader = new FileReader();
            reader.onload = (function (file, index) {
                if (preview.find('img').length >=5) {
                    alert("Tối đa 5 ảnh");
                    return ins.ClearImage();
                }
                return function(e) {
                    var img = $("<img>").attr({
                        'src': e.target.result,
                        'id': `Images[${index}]`,
                        'style': "width:150px;height:150px;margin-left:10px"
                    });
                    preview.append(img);
                };
            })(file, i);

            reader.readAsDataURL(file);
        }
    };
    ins.ClearImage = function() {
        var preview = $("#imagePreview");
        var  files = $("#fileInput");
        preview.empty();
        files.val('');
    };

    return ins;
})(window, jQuery);
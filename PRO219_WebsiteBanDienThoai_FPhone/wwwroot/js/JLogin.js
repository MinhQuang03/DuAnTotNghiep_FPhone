var JLogin = (function(window,$) {
    var ins = {};
    ins.submitForm = function(e) {
        e.preventDefault();
        var error = 0;
        if ($('#UserName').val() == "") {
            error++;
            $('.error_UserName').text("Vui lòng nhập thông tin");
        } else {
            $('.error_UserName').text("");
        }

        if ($('#Password').val() == "") {
            error++;
            $('.error_Password').text("Vui lòng nhập thông tin");
        } else {
            $('.error_Password').text("");
        }

        if (error == 0) {

            $('#form_Login').submit();
        }

    }

    return ins;
})(window,jQuery);
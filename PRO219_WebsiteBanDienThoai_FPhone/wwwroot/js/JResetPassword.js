var JResetPassword = (function(window,$) {
    var ins = {};
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    ins.submitForm = function (event) {
        var error = 0;
        event.preventDefault();
        $('#email_check').text('');
        if ($('#Email').val() == "") {
            $('.error_Email').text("Vui lòng nhập thông tin");
            error++;
        } else {
            if (emailRegex.test($('#Email').val())) {
                $('.error_Email').text("");
            } else {
                $('.error_Email').text("Email sai định dạng");
                error++;
            }
        }

        if (error==0) {
            $('#form_Reset').submit();
        }
    }

    return ins;
})(window,jQuery)
var JContact = (function(window,jQuery) {
    var ins = {};

    ins.submitForm = function (event) {
        var error = 0;
        event.preventDefault();
        if ($('#FullName').val()=="") {
            $('.error_FullName').text("Vui lòng nhập thông tin");
            error++;
        } else {
            $('.error_FullName').text("");
        }

        if ($('#PhoneNumber').val() =="") {
            $('.error_PhoneNumber').text("Vui lòng nhập thông tin");
            error++;
        } else {
            $('.error_PhoneNumber').text("");
        }

        if ($('#Email').val()=="") {
            $('.error_Email').text("Vui lòng nhập thông tin");
            error++;
        } else {
            $('.error_Email').text("");
        }

        if (error ==0) {
            $('#form_Contact').submit();
        }
    }

    return ins;
})(window,jQuery);
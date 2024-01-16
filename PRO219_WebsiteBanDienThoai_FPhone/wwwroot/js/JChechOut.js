var JCheckOut = (function(window, $) {
    const ins = {};
    const fromDistrict = "1542"; // Quận huyện người gửi
    const shopId = "4189088";
    const tienMat = 0;
   /* const phanTram = 1;*/
    const freeShip = 2;
    ins.init = function () {
        //$('input[type="radio"]').change(function () {

        //    if ($('#paymentInStore').is(':checked')) {
        //        var insurance = $("#TotalPhone").val(); // tổng tiền sản phẩm
        //        $("#TotalMoney").val(parseFloat(insurance));
        //        $("#TotalPayment").text(parseFloat(insurance));
        //        $("#TotalShip").val(0);
        //    }
        //});
    };

    ins.maVoucher = function () {
        $('#maVoucher').keypress(function (event) {
            if (event.which === 13) {
                $('#voucher').text("0");
                    $.ajax({
                        method: 'GET',
                        url: '/CheckOut/GetVoucherByCode/' + $('#maVoucher').val(),
                        success: (data) => {
                            console.log(data);
                            $('#mucVoucher').val(data.mucUuDai);
                            if (data.typeVoucher == freeShip && $('#paymentInStore').is(':checked')) {
                                alert("Không thể áp dụng Voucher này");
                                return false;
                            }

                            if ($('#paymentInStore').is(':checked') == false && $('#AvailableService').val() =='') {
                                alert("Vui lòng nhập đầy đủ thông tin trước khi dùng voucher");
                                return false;
                            }

                            if (data != '') {
                                switch (data.typeVoucher) {
                                case tienMat:
                                    var insurance = $("#TotalPhone").val(); // tổng tiền sản phẩm
                                    $('#voucher').text(parseFloat(data.mucUuDai)
                                        .toLocaleString('vi', { style: 'currency', currency: 'VND' })); //Giảm giá
                                     /*   $("#TotalMoney").val(parseFloat(insurance) - parseFloat(data.mucUuDai));*/

                                    if ($('#paymentInStore').is(':checked')) {

                                        $("#TotalPayment")
                                            .text((parseFloat(insurance) - parseFloat(data.mucUuDai))
                                                .toLocaleString('vi',
                                                    { style: 'currency', currency: 'VND' })); //Tổng tiền thanh toán
                                        $("#TotalMoney").val(parseFloat(insurance) - parseFloat(data.mucUuDai));
                                    } else {
                                        $("#TotalMoney").val(parseFloat(insurance) +
                                            parseFloat($("#ToTalShipHide").val()) -
                                            parseFloat(data.mucUuDai));;
                                        $("#TotalPayment").text((parseFloat(insurance) +
                                            parseFloat($("#ToTalShipHide").val()) -
                                            parseFloat(data.mucUuDai)).toLocaleString('vi',
                                            { style: 'currency', currency: 'VND' }));


                                    }
                                    break;
                                case freeShip:
                                    var insurance = $("#TotalPhone").val(); // tổng tiền sản phẩm
                                    $("#TotalMoney").val(parseFloat(insurance));
                                    $("#TotalPayment").text(parseFloat(insurance)
                                        .toLocaleString('vi', { style: 'currency', currency: 'VND' }));
                                    $("#TotalShip").val("0");
                                    $("#TotalShip").text("0");
                                    $('#voucher').text("freeship");
                                    break;
                                }
                            }
                        }
                    });
                
                
                event.preventDefault();
            }
        });
    };

    ins.ChangePay = function () {
        $('#voucher').text(0);
        if ($('#paymentInStore').is(':checked')) {
            $('.displayAddress').hide();
            var insurance = $("#TotalPhone").val(); // tổng tiền sản phẩm
            $("#TotalMoney").val(parseFloat(insurance));
            $("#TotalPayment").text(parseFloat(insurance).toLocaleString('vi', { style: 'currency', currency: 'VND' }));
            $("#TotalShip").val("0");
            $("#TotalShip").text("0");
        } else {
            $('.displayAddress').show();
            ins.TotalShip();
        }
        
    };

    ins.GetProvince = function() {
        const stringHtml = "";
    };

    ins.ChangeProvince = function() {
        let stringHtml = "";
        var value = $("#Province").val();
        if (value.length > 1) {
            var url = "https://online-gateway.ghn.vn/shiip/public-api/master-data/district";
            var headers = {
                token: "a799ced2-febc-11ed-a967-deea53ba3605"
            };
            data = {
                province_id: value
            };
            $.ajax({
                method: "GET",
                url: url,
                headers: headers,
                data: data,
                success: (data) => {
                    stringHtml = '<option value=" " > --Lựa chọn-- </option>';
                    for (let i = 0; i < data.data.length; i++) {
                        stringHtml += `<option value="${data.data[i].DistrictID}" >${data.data[i].DistrictName
                            }</option>`;
                    }
                    $("#District").html(stringHtml);
                    $("#ProvinceName").val($("#Province option:selected").text());  
                }
            });
        } else {
            $("#District").empty();
            $("#District").prepend("<option value=''> --- Lựa chọn --- </option>");
            $("#Ward").empty();
            $("#Ward").prepend("<option value=''> --- Lựa chọn --- </option>");

        }
    };

    ins.ChangeDistrict = function() {
        let stringHtml = "";
        var value = $("#District").val();
        if (value.length > 1) {
            var url = "https://online-gateway.ghn.vn/shiip/public-api/master-data/ward";
            var headers = {
                token: "a799ced2-febc-11ed-a967-deea53ba3605"
            };
           var data = {
                district_id: value
            };
            $.ajax({
                method: "GET",
                url: url,
                headers: headers,
                data: data,
                success: (data) => {
                  
                    stringHtml = '<option value=" " > --Lựa chọn-- </option>';
                    for (let i = 0; i < data.data.length; i++) {
                        stringHtml += `<option value="${data.data[i].WardCode}" >${data.data[i].WardName}</option>`;
                    }
                    $("#Ward").html(stringHtml);
                    $("#DistrictName").val($("#District option:selected").text());
                    ins.AvailableService($("#District option:selected").val());
                }
            });
        } else {
            $("#Ward").empty();
            $("#Ward").prepend("<option value=''> --- Lựa chọn --- </option>");
        }

    };

    ins.ChangeWard = function ()
    {
        var value = $("#Ward").val();
        if (value.length>1){
            $("#WardName").val($("#Ward option:selected").text());
        } else {
            $("#WardName").empty();
        }
    };
    //Lấy gói dịch vụ
    ins.AvailableService = function (toDistrict) {
        var url = "https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/available-services";
        let stringHtml = '';
        var header = {
            token: "a799ced2-febc-11ed-a967-deea53ba3605"
        };
        var data = {
            shop_id: shopId,
            from_district: fromDistrict,
            to_district: toDistrict
        };
        $.ajax({
            method: "GET",
            url: url,
            headers: header,
            data: data,
            success: (data) => {
                stringHtml = '<option value=" " > --Lựa chọn-- </option>';
                for (let i = 0; i < data.data.length; i++) {
                    stringHtml += `<option value="${data.data[i].service_id}" >${data.data[i].short_name}</option>`;
                }
              
                $("#AvailableService").html(stringHtml);
            }
        });
    };

    //Tính phí ship
    ins.TotalShip = function() {
        var wardValue = $("#Ward").val(); // lấy code xã/phường
        var districtValue = $("#District").val(); // lấy code quận/huyện
        var sumPhone = $("#SumPhone").val(); //số lượng sảnphẩm
        var insurance = $("#TotalPhone").val(); // tổng tiền sản phẩm
        var serviceValue = $("#AvailableService").val(); //Thông tin gói dịch vụ
        var weight = 100; // trọng lượng  (gram)
        var length = 20; // chiều dài
        var width = 10; //chiều rộng
        var height = 3; // chiều cao
       var voucher = $('#mucVoucher').val();
        if (parseInt(sumPhone)>1) {
            weight *= parseInt(sumPhone);
            height *= parseInt(sumPhone);
            width *= parseInt(sumPhone);
            length *= parseInt(sumPhone);
        }
        var url = "https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee";
        const header = {
            token: "a799ced2-febc-11ed-a967-deea53ba3605",
            shop_id: shopId
        };
       var data = {
           service_id: serviceValue, 
           insurance_value: insurance,
           coupon: "",
           to_ward_code: wardValue,
           to_district_id: districtValue,
           from_district_id: fromDistrict, 
           weight: weight,
           length: length,
           width: width,
           height: height
       };
        if (wardValue, districtValue, sumPhone, insurance, serviceValue) {
            $.ajax({
                method: "GET",
                url: url,
                headers: header,
                data: data,
                success: (data) => {
                    $("#TotalShip").text((data.data.total).toLocaleString('vi', { style: 'currency', currency: 'VND' }));
                    $("#ToTalShipHide").val(data.data.total);
                    $("#TotalPayment").text((parseFloat(data.data.total) + parseFloat(insurance)).toLocaleString('vi', { style: 'currency', currency: 'VND' }));
                    $("#TotalMoney").val((data.data.total) + parseFloat(insurance));
                   
                },
                error: (error) => {
                    $("#TotalShip").empty();
                    Swal.fire({
                        icon: 'error',
                        title: 'Có lỗi xảy ra!',
                        text: 'Vui lòng chọn phương thức vận chuyển khác',
                        allowOutsideClick: false,
                    });
                }
            });
        }
       
    };

    //khi chọn gói dịch vụ sẽ tính phí ship
    ins.ChangeService = function () {
        if ($('#paymentInStore').is(':checked')) {
            var insurance = $("#TotalPhone").val(); // tổng tiền sản phẩm
            $("#TotalMoney").val(parseFloat(insurance));
            $("#TotalPayment").text(parseFloat(insurance).toLocaleString('vi', { style: 'currency', currency: 'VND' }));
            $("#TotalShip").val("0");
            $("#TotalShip").text("0");
        }
        else {
            var value = $("#AvailableService").val();
            if (value.length > 1) {
                ins.TotalShip();
            } else {
                $("#TotalShip").empty();
            }
        }
       
    };

    return ins;
})(window, jQuery);
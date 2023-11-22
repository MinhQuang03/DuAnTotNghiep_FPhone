var JCheckOut = (function(window, $) {
    const ins = {};
    const fromDistrict = "1542"; // Quận huyện người gửi
    const shopId = "4189088";
    ins.init = function() {

    };

    ins.GetProvince = function() {
        const stringHtml = "";

    };

    ins.ChangeProvince = function() {
        let stringHtml = "";
        const value = $("#Province").val();
        if (value.length > 1) {
            const url = "https://online-gateway.ghn.vn/shiip/public-api/master-data/district";
            const headers = {
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
        const value = $("#District").val();
        if (value.length > 1) {
            const url = "https://online-gateway.ghn.vn/shiip/public-api/master-data/ward";
            const headers = {
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
        const value = $("#Ward").val();
        if (value.length>1){
            $("#WardName").val($("#Ward option:selected").text());
        } else {
            $("#WardName").empty();
        }
    }
    //Lấy gói dịch vụ
    ins.AvailableService = function (toDistrict) {
        var url = "https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/available-services";
        let stringHtml = '';
        const header = {
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
    }

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

        if (parseInt(sumPhone)>1) {
            weight *= parseInt(sumPhone);
            height *= parseInt(sumPhone);
            width *= parseInt(sumPhone);
            length *= parseInt(sumPhone);
        }
        const url = "https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee";
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
       
    }

    //khi chọn gói dịch vụ sẽ tính phí ship
    ins.ChangeService = function () {
       var value= $("#AvailableService").val();
        if (value.length>1) {
            ins.TotalShip();
        } else {
            $("#TotalShip").empty();
        }
    }

    return ins;
})(window, jQuery);
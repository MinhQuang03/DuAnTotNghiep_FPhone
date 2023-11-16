﻿var JCheckOut = (function(window, $) {
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
            const data = {
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

    ins.ChangeWard = function() {
        const value = $("#Ward").val();
        if (value.length > 1) {
            $("#WardName").val($("#Ward option:selected").text());
        } else {
            $("#WardName").empty();
        }
    };
    //Lấy gói dịch vụ
    ins.AvailableService = function(toDistrict) {
        const url = "https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/available-services";
        let stringHtml = "";
        const header = {
            token: "a799ced2-febc-11ed-a967-deea53ba3605"
        };
        const data = {
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
        const wardValue = $("#Ward").val(); // lấy code xã/phường
        const districtValue = $("#District").val(); // lấy code quận/huyện
        const sumPhone = $("#SumPhone").val(); //số lượng sảnphẩm
        const insurance = $("#TotalPhone").val(); // tổng tiền sản phẩm
        const serviceValue = $("#AvailableService").val(); //Thông tin gói dịch vụ
        const weight = 100; // trọng lượng  (gram)
        const length = 20; // chiều dài
        const width = 10; //chiều rộng
        const height = 3; // chiều cao

        const header = {
            token: "a799ced2-febc-11ed-a967-deea53ba3605",
            shop_id: shopId
        };
        const data = {
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
            const url = "https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee";
            $.ajax({
                method: "GET",
                url: url,
                headers: header,
                data: data,
                success: (data) => {
                    $("#TotalShip").text(data.data.total);
                },
                error: (error) => {
                    $("#TotalShip").empty();
                    Swal.fire({
                        icon: "error",
                        title: "Có lỗi xảy ra!",
                        text: "Vui lòng chọn phương thức vận chuyển khác",
                        allowOutsideClick: false,
                    });
                }
            });
        }

    };

    //khi chọn gói dịch vụ sẽ tính phí ship
    ins.ChangeService = function() {
        const value = $("#AvailableService").val();
        if (value.length > 1) {
            ins.TotalShip();
        } else {
            $("#TotalShip").empty();
        }
    };

    return ins;
})(window, jQuery);
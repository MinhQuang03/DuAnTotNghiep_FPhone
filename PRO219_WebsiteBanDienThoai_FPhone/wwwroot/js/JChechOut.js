var JCheckOut = (function(window, $) {
    const ins = {};
    ins.init = function() {

    };

    ins.GetProvince = function() {
        const stringHtml = "";

    };

    ins.ChangeProvince = function() {
        let stringHtml = "";
        const value = $("#ProvinceName").val();
        if (value.length>1) {
            url = "https://online-gateway.ghn.vn/shiip/public-api/master-data/district";
            headers = {
                token: "a799ced2-febc-11ed-a967-deea53ba3605"
            };
            data = {
                province_id: value
            };
        }
        else {
            $("#District").empty();
            $("#District").prepend("<option value=''> --- Lựa chọn --- </option>");
            $("#Ward").empty();
            $("#Ward").prepend("<option value=''> --- Lựa chọn --- </option>");

        }
        $.ajax({
            method: "GET",
            url: url,
            headers: headers,
            data: data,
            success: (data) => {
                console.log(data.data);
                stringHtml = '<option value=" " > --Lựa chọn-- </option>';
                for (let i = 0; i < data.data.length; i++) {
                    stringHtml += `<option value="${data.data[i].DistrictID}" >${data.data[i].DistrictName}</option>`;
                }
                $("#District").html(stringHtml);

            }
        });
    };

    ins.ChangeDistrict = function() {
        let stringHtml = "";
        const value = $("#District").val();
        if (value.length >1) {
            url = "https://online-gateway.ghn.vn/shiip/public-api/master-data/ward";
            headers = {
                token: "a799ced2-febc-11ed-a967-deea53ba3605"
            };
            data = {
                district_id: value
            };
        }
        else {
            $("#Ward").empty();
            $("#Ward").prepend("<option value=''> --- Lựa chọn --- </option>");
        }
        $.ajax({
            method: "GET",
            url: url,
            headers: headers,
            data: data,
            success: (data) => {
                console.log(data.data);
                stringHtml = '<option value=" " > --Lựa chọn-- </option>';
                for (let i = 0; i < data.data.length; i++) {
                    stringHtml += `<option value="${data.data[i].WardCode}" >${data.data[i].WardName}</option>`;
                }
                $("#Ward").html(stringHtml);
            }
        });
    };

    return ins;
})(window, jQuery);
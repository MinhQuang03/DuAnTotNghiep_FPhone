var JPhoneDetail = (function(window,$) {
    var ins = {};
    ins.changeColor = function (idPhoneDetail) {
        $('.btnPhoneDetail').removeClass("selected");
        $('#' + idPhoneDetail).addClass("selected");
     
        var phoneName = "";
        $.ajax({
            method: "GET",
            url: '/PhoneDetail/getPhoneDetailById/' + idPhoneDetail,
            success: (data) => {
                $('#IdPhoneDetail').val(idPhoneDetail);
                phoneName = data.phoneName + ' - ' + data.colorName;
                $('#phoneName').text(data.phoneName + " " + data.colorName);
                console.log(data.price);
                $('#phonePrice').text(parseFloat(data.price)
                    .toLocaleString('vi', { style: 'currency', currency: 'VND' }));
            }
        });


        $.ajax({
            method: "GET",
            url: '/PhoneDetail/checkExitImei/' + idPhoneDetail,
            success:(data) => {
                if (data == 0) {
                    $('#phoneName').text(phoneName);
                    $('#notExits').show();
                    $('#addToCart').hide();
                    $('.in-stock').text(data + ' ' + 'sản phẩm');
                } else {
                    $('.in-stock').text(data + ' ' + 'sản phẩm');
                }
            }
        });
    }
    ins.AddToCard = function (e) {
        e.preventDefault();
        var idphoneDetail = $('#IdPhoneDetail').val();
        if (idphoneDetail) {
            window.location.href = "/Accounts/AddToCard?id=" + idphoneDetail;
        } else {
            alert("Vui lòng chọn phiên bản bên dưới trước khi thêm vào giỏ hàng");
        }
    }

ins.selectPhoneDetail = function (idRam, idPhone) {
        $('#addToCart').show();
        $('#notExits').hide();
        var stringHtml = "";
        $('#colorList').empty();
        $('#IdPhoneDetail').val(null);
        $('.btnRam').removeClass('selected');
       /* console.log(idRam);*/
        $('#' + idRam).addClass("selected");
        $.ajax({
            method: "GET",
            url: `/PhoneDetail/getListPhoneDetailByIdPhone/` + idPhone +'/'+idRam,
            success: (data) => {
                console.log(data);
                for (var i = 0; i < data.length; i++) {
                    stringHtml += `<a id="${data[i].idPhoneDetail}" onclick="JPhoneDetail.changeColor('${data[i].idPhoneDetail}')" class="btn col-4 border rounded phone-hover mt-2 btnPhoneDetail" >
                                    <div>
                                        <strong>${data[i].colorName}</strong>
                                    </div>
                                    <span>${data[i].price} đ</span>
                                </a>`;
                    $('#phoneName').text(data[0].phoneName);
                }
               
                $('#colorList').append(stringHtml);
            }
        });
    }

    return ins;
})(window,jQuery);
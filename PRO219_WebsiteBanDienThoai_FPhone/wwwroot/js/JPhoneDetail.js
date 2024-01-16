$(document).ready(function () {
    $("#notification").modal("hide");

    document.getElementById('submit').addEventListener('click', function () {
        var comment = $('#message')[0].value;

        if (!idAccount) {
            notification('Vui lòng đăng nhập để sử dụng tính năng này');
            return;
        }
        $.ajax({
            url: '/PhoneDetail/CreateComment/',
            data: {
                comment: comment,
                idAccount: idAccount,
                idPhone: idPhone
            },
            method: 'POST',
            success: function (res) {
                if (res > 0) {
                    
                    setTimeout(function () {
                        window.location.reload();
                    }, 2000);

                }
                else {
                    notification('Thất bại');

                }
            },
            error: function (xhr, status, error) {
                console.error('Error: ' + status, error);
            }
        });
    });
});

var JPhoneDetail = (function (window, $) {
    var ins = {};
    ins.changeColor = function (idPhoneDetail) {
        $('.btnPhoneDetail').removeClass("selected");
        $('#' + idPhoneDetail).addClass("selected");
        $('#ShowDetaild').show();
        $('.showChiTiet').empty();
        var phoneName = "";
        $.ajax({
            method: "GET",
            url: '/PhoneDetail/getPhoneDetailById/' + idPhoneDetail,
            success: (data) => {
               console.log(data);
                $('#IdPhoneDetail').val(idPhoneDetail);
                phoneName = data.phoneName + ' - ' + data.colorName;
                $('#phoneName').text(data.phoneName + " " + data.colorName);
                console.log(data.price);
                $('#phonePrice').text(parseFloat(data.price)
                    .toLocaleString('vi', { style: 'currency', currency: 'VND' }));
                $('.showChiTiet').append(`<div class="modal-dialog modal-dialog-centered modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 class="modal-title fs-5" id="exampleModalLabel">Thông tin chi tiết</h1>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <p class="col-sm-12 text-center highlighted-name">${data.phoneName}</p>
                    </div>
                    <div class="row">
                        <label class="col-form-label col-sm-5">Chất liệu</label>
                        <p class="col-sm-7">${data.materialName}</p>
                    </div>
                    <div class="row">
                        <label class="col-form-label col-sm-5">Bộ nhớ trong (ROM)</label>
                        <p class="col-sm-7">${data.romName}</p>
                    </div>
                    <div class="row">
                        <label class="col-form-label col-sm-5">RAM</label>
                        <p class="col-sm-7">${data.ramName}</p>
                    </div>
                    <div class="row">
                        <label class="col-form-label col-sm-5">Hệ điều hành</label>
                        <p class="col-sm-7">${data.operatingSystemName}</p>
                    </div>
                    <div class="row">
                        <label class="col-form-label col-sm-5">Dung lượng pin</label>
                        <p class="col-sm-7">${data.batteryName}</p>
                    </div>
                    <div class="row">
                        <label class="col-form-label col-sm-5">Loại SIM</label>
                        <p class="col-sm-7">${data.simName}</p>
                    </div>
                    <div class="row">
                        <label class="col-form-label col-sm-5">Chip CPU</label>
                        <p class="col-sm-7">${data.chipCPUName}</p>
                    </div>
                    <div class="row">
                        <label class="col-form-label col-sm-5">Chip GPU</label>
                        <p class="col-sm-7">${data.chipGPUName}</p>
                    </div>
                    <div class="row">
                        <label class="col-form-label col-sm-5">Màu sắc</label>
                        <p class="col-sm-7">${data.colorName}</p>
                    </div>
                    <div class="row">
                        <label class="col-form-label col-sm-5">Loại cổng sạc</label>
                        <p class="col-sm-7">${data.chargingportTypeName}</p>
                    </div>
                    <div class="row">
                        <label class="col-form-label col-sm-5">Trọng lượng</label>
                        <p class="col-sm-7">${data.weight}</p>
                    </div>
                    <div class="row">
                        <label class="col-form-label col-sm-5">Camera trước</label>
                        <p class="col-sm-7">${data.frontCamera}</p>
                    </div>
                    <div class="row">
                        <label class="col-form-label col-sm-5">Độ phân giải</label>
                        <p class="col-sm-7">${data.resolution}</p>
                    </div>
                    <div class="row">
                        <label class="col-form-label col-sm-5">Kích thước</label>
                        <p class="col-sm-7">${data.size}</p>
                    </div>
                    <div class="row">
                        <label class="col-form-label col-sm-5">Tên công ty sản xuất</label>
                        <p class="col-sm-7">${data.productionCompanyName}</p>
                    </div>
                    <!-- Thêm các dòng tương ứng với các thuộc tính còn lại của VW_PhoneDetail -->
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-bs-dismiss="modal">Đóng</button>
                </div>
            </div>
        </div>`);
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
        $('#ShowDetaild').hide();
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
})(window, jQuery);
const notification = function (message) {
    $("#notification").modal("show");
    $('#notification-content')[0].innerHTML = message;
    setTimeout(function () {
        $("#notification").modal("hide");
    }, 5000);
}
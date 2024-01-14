var JAdCheckOut = (function () {

    var ins = {};
    const ChuyenKhoan = "BANKING";
    const TienMat = "TIENMAT";
    const TienMat_ChuyenKhoan = "TIENMAT_CHUYENKHOAN";
    const phoneRegex = /^(0\d{9,10})$/;
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    ins.searchClick = function() {
        var dataSearch = $('#dataSearch').val();
            $(`.ShowProduct`).each(function () {
                var phoneName = $(this).attr("id");
                var code = $(this).attr("name");
                if (phoneName.includes(dataSearch) || code == dataSearch || dataSearch=="") {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            });
    }
    ins.submitFormCreateAccount = function (e) {
        e.preventDefault();
        var error = 0;
        var form = $('#createAccountUser');
        var formData = form.serialize();
        //validate họ tên
        if ($('#Account_Name').val() =="") {
            $('.error_Name').text("Vui lòng nhập thông tin");
            error++;
        } else {
            $('.error_Name').text("");
        }
        //validate tên đăng nhập
        if ($('#Account_Username').val()=="") {
            $('.error_Username').text("Vui lòng nhập thông tin");
            error++;
        } else {
            $('.error_Username').text("");
        }
        //validate số điện thoại
        if ($('#Account_PhoneNumber').val() == "") {
            $('.error_SDT').text("Vui lòng nhập thông tin");
            error++;
        } else {
            if (!phoneRegex.test($('#Account_PhoneNumber').val())) {
                $('.error_SDT').text('Số điện thoại sai định dạng');
                error++
            } else {
                $('.error_SDT').text('');
            }
        }
        //validate email
        if ($('#Account_Email').val() == "") {
            $('.error_Email').text("Vui lòng nhập thông tin");
            error++;
        } else {
            if (!emailRegex.test($('#Account_Email').val())) {
                $('.error_Email').text('Email sai định dạng');
                error++
            } else {
                $('.error_Email').text('');
            }
        }

        if (error == 0) {
            $.ajax({
                type: 'POST',
                url: '/AdCheckOut/CreateAccountUser',
                data: formData,
                success: function (response) {
                    alert(response);
                    $("#createAccount").show();
                }
            });
        }
    }

    ins.clickAddProp = function () {
        var value = parseInt($('#NumberSilde').val());
        var new_value = value + 1;
        $('#NumberSilde').val(new_value);
        var id = 'propertity' + new_value;
        var id_tab = id + 'tag';

        $('#mytab').append(`<li class="nav-item ${id_tab} border">
                            <a onclick="JAdCheckOut.ChangeBill(${new_value})" class="nav-link" id="${id_tab}" data-toggle="tab" href="#${id}" role="tab" aria-controls="${id}" aria-selected="false">HD${new_value}</a>
                        </li>`);

        $('#myTabContent').append(` <div class="tab-pane fade show" id="${id}" role="tabpanel" aria-labelledby="${id_tab}">
                                     <div class="card">
                                         <div class="card-body">
                                             <div class="form-group">
                                                <table class="table table-bordered">
                                                  <thead class="thead-light">
                                                    <tr class="text-center">
                                                      <th widht="20%">Mã sản phẩm</th>
                                                      <th widht="20%">Tên sản phẩm</th>
                                                      <th>Giá bán</th>
                                                      <th>Xóa</th>
                                                    </tr>
                                                  </thead>
                                                  <tbody id="spct_${new_value}">
                                                    
                                                  </tbody>
                                                </table>
                                             </div>
                                         </div>
                                     </div>
                                 </div>`);
        ins.initList();
    };

    ins.removeProp = function() {
        var value = parseInt($('#NumberSilde').val());
        if (value>0) {
            var active_tabs = $('#mytab li a.active');
            var id_full = active_tabs.attr("id");
            var id = id_full.replace('propertity', '').replace('tag', '');
            $('.' + id_full).remove();
            $('.row_' + id).remove();
        }
        ins.initList();
    };

    var stt = 0;
    ins.clickButtonAdd = function ( index) {
        var quantity = $('#quantity_hide_' + index).val();
        var active_tabs = $('#mytab li a.active');
        var id_full = active_tabs.attr("id");
        var id = id_full.replace('propertity', '').replace('tag', '');

        if (quantity==0) {
            alert("Đã hết hàng");
        } else {
            var idPhoneDetail = $('#IdPhoneDetail_' + index).val();
            var stringHtml = '';
            $.ajax({
                method: 'GET',
                url: '/AdCheckOut/GetPhoneDetail/' + idPhoneDetail,
                success: (data) => {
                    console.log(data);
                    stringHtml += `<tr class="row_${id} ${stt}">
                            <td hidden class="idsp_${id}" id="idsp_${id} ${stt}">${data.idPhoneDetail}</td>
                            <td id="msp_${id}">${data.code}</td>
                            <td id="tensp_${id}">${data.phoneName}</td>
                            <td class="giaban_${id}" id="giaban_${id} ${stt}">${data.price}</td>
                            <td id="action_${id}">
                                <button onclick="JAdCheckOut.removeCtsp('${stt}','${id}','${index}')" >X</button>
                            </td>
                        </tr>`;
                    $('#spct_' + id).append(stringHtml);
                    stt++;
                    $('#quantity_hide_' + index).val(parseInt(quantity) - 1);
                    $('#quantity_' + index).text('Số lượng: ' + (parseInt(quantity) - 1));

                    var sumPhone = 0;
                    $(`.giaban_${id}`).each(function() {
                        sumPhone += parseInt($(this).text());
                    });
                    $('#SumPhone').val(sumPhone);
                    $('#SumPhone').text(sumPhone);
                }
            });
        }
        ins.initList();
        
    };
    ins.submitForm = function(e) {
        e.preventDefault();
        var error = 0;
        var form = $('#form_ThanhToan');
        var active_tabs = $('#mytab li a.active');
        var id_full = active_tabs.attr("id");
        var id = id_full.replace('propertity', '').replace('tag', '');
        var ind = 0;

        if ($('#paymentMethod').val() == '') {
            error++;
            alert("Vui lòng chọn đầy đủ thông tin trước khi thanh toán");
            return false;
        }
        $(`.idsp_${id}`).each(function() {
            var idPhoneDetailValue = $(this).text();
            // Kiểm tra xem phần tử có giá trị không trước khi gán vào danh sách
            if (idPhoneDetailValue.trim() !== '') {
                form.append(
                    `<input hidden name="ListBillDetail[${ind}].IdPhoneDetail" value="${idPhoneDetailValue}"/>`);
                ind++;
            }
        });

        //mã code bill
        $('#Bill_BillCode').val($('#maHoaDon').val());
        //Ngày tạo
        $('#Bill_CreatedTime').val($('#ngayTao').val());
        //Tổng tiền
        $('#Bill_TotalMoney').val($('#SumPhone').val());
        //Tên khách hàng

        if ($('#FullName').val() == "") {
            alert("Chưa có thông tin khách hàng");
            error++;
            return false;
        } else {
            $('#Bill_Name').val($('#FullName').val());
        }

        if ($('#PhoneNumber').val() == "") {
            alert("Chưa có thông tin khách hàng");
            error++;
            return false;
        } else {
            $('#Bill_Phone').val($('#PhoneNumber').val());
        }

        if ($('#SumPhone').val() == 0) {
            alert("Bạn chưa chọn sản phẩm nào");
            error++;
            return false;
        } else {
            $('#SumPhone').val(0);
        }

        $('#FullName').val('');
        $('#PhoneNumber').val('');
        $('#searchUser').val('');

        //Tiến hành thanh toán
        if (error == 0) {
            var formData = form.serialize();
            $.ajax({
                type: 'POST',
                url: '/AdCheckOut/CheckOutJSon',
                data: formData,
                success: function(response) {
                    //xóa dữ liệu form 
                    $('#Bill_BillCode').val('');
                    $('#Bill_CreatedTime').val('');
                    $('#Bill_TotalMoney').val('');
                    $('#Bill_Name').val('');
                    $('#Bill_Phone').val('');
                    $(`.row_${id}`).remove();
                    $(`#${id_full}`).remove();
                    alert(response);

                }
            });
        }
    };

    //stt: số thứ tự từng row
    //Id: id của từng sp
    //IndexSp: số thứ tự của sản phẩm 
    ins.removeCtsp = function (stt, id, indexSp) { 
        var quantity = $('#quantity_hide_' + indexSp).val();
        var classrow = `.row_${id}.${stt}`;
        $(classrow).remove();

        $('#quantity_hide_' + indexSp).val(parseInt(quantity) + 1);
        $('#quantity_' + indexSp).text('Số lượng: ' + (parseInt(quantity) + 1));

        var sumPhone = 0;
        $(`.giaban_${id}`).each(function () {
            sumPhone += parseInt($(this).text());
        });
        $('#SumPhone').val(sumPhone);
        $('#SumPhone').text(sumPhone);
        ins.initList();
    };

    ins.ChangeBill = function(id) {
        var sumPhone = 0;
        $(`.giaban_${id}`).each(function () {
            sumPhone += parseInt($(this).text());
        });
        $('#SumPhone').val(sumPhone);
        $('#SumPhone').text(sumPhone);
        ins.initList();
    };

    ins.selectPaymentMethod = function() {
        var value = $('#paymentMethod').val();
        $('.banking').hide();
        $('.tienMat').hide();
        if (value == ChuyenKhoan) {
            $('.banking').show();
        } else if (value == TienMat) {
            $('.tienMat').show();
        } else if (value == TienMat_ChuyenKhoan) {
            $('.banking').show();
            $('.tienMat').show();
        }
        ins.initList();
    };

    ins.randomString = function(length) {
        var kyTu = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        var doDaiChuoi = length || 8;
        var chuoiNgauNhien = '';

        for (var i = 0; i < doDaiChuoi; i++) {
            var viTriKyTuNgauNhien = Math.floor(Math.random() * kyTu.length);
            chuoiNgauNhien += kyTu.charAt(viTriKyTuNgauNhien);
        }

        return chuoiNgauNhien;
    };

    ins.initList = function () {

        $('#maHoaDon').val(ins.randomString(6));
      
        var paymentMethod = $('#paymentMethod').val();

        // Hàm xử lý sự kiện input chung
        function xuLySuKienInput(element, giaTriInput) {
            element.on('input', function () {
                giaTriInput = ($(this).val() >= 0) ? $(this).val() : 0;
                tinhTienThua();
            });
        }

        // Hàm tính tiền thừa
        function tinhTienThua() {
            var sumPhone = $('#SumPhone').val();
            var tienThua = 0;

            switch (paymentMethod) {
            case TienMat:
                tienThua = parseInt($('#TienMat').val()) - sumPhone;
                break;
            case ChuyenKhoan:
                tienThua = parseInt($('#ChuyenKhoan').val()) - sumPhone;
                break;
            case TienMat_ChuyenKhoan:
                tienThua = parseInt($('#TienMat').val()) + parseInt($('#ChuyenKhoan').val()) - sumPhone;
                break;
            default:
                break;
            }

            $('#TienThua').val(tienThua || 0);
        }

        // Gọi hàm xử lý sự kiện input cho từng phương thức thanh toán
        switch (paymentMethod) {
        case TienMat:
            xuLySuKienInput($('#TienMat'));
            break;
        case ChuyenKhoan:
            xuLySuKienInput($('#ChuyenKhoan'));
            break;
        case TienMat_ChuyenKhoan:
            xuLySuKienInput($('#TienMat'));
            xuLySuKienInput($('#ChuyenKhoan'));
            break;
        default:
            $('#TienThua').val(0);
        }

        // Tính toán tiền thừa khi trang web được tải
        tinhTienThua();
    };


    ins.searchUser = function() {
        $('#searchUser').keypress(function (event) {
            
            if (event.which === 13) {
                $.ajax({
                    method: 'GET',
                    url: '/AdCheckOut/GetUserByPhone/' + $('#searchUser').val(),
                    success: (data) => {
                        if (data != '') {
                            console.log(data);
                            $('#FullName').val(data.name);
                            $('#PhoneNumber').val(data.phoneNumber);
                            $('#Bill_IdAccount').val(data.id);
                        } else {
                            $('#FullName').val('');
                            $('#PhoneNumber').val('');
                            $("#confirmAC").show();
                        }
                    }
                });
                event.preventDefault();
            }
        });
    };
    ins.submitFormPop = function() {
        var fullName = $('#fullNamePop').val();
        var phoneNumber = $('#phoneNumberPop').val();
        $('#FullName').val(fullName);
        $('#PhoneNumber').val(phoneNumber);
        $('#Bill_IdAccount').val('');
        $('#Bill_IdAccount').val('');
        $('#closePopup').click();
    }
    return ins;
})(window,jQuery);
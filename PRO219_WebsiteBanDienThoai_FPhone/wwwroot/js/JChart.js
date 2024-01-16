var JChart = (function(window,$) {
    var ins = {};

    ins.initList = function() {
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(function () {
            // AJAX request để lấy dữ liệu từ Action
            $.ajax({
                url: '/ThongKe/Chart',
                type: 'GET',
                success: function (data) {
                    console.log(data);
                    ins.drawCharta(data);
                },
                error: function () {
                    console.error('Lỗi tải dữ liệu');
                }
            });

        });
    }

    ins.drawCharta = function (data) {
        var jsonData = data;

        var tb = new google.visualization.DataTable();
        tb.addColumn("string", "billStatus");
        tb.addColumn("number", "coutBill");

        for (var i = 0; i < jsonData.length; i++) {
            tb.addRow([jsonData[i].billStatus, jsonData[i].coutBill]);
        }

        var chart = new google.visualization.PieChart(document.getElementById('detailContent'));
        chart.draw(tb,
            {
                title: "Báo cáo tỉ lệ đơn hàng",
                pieSliceText: 'value'
            });
    }
    return ins;
})(window,jQuery)
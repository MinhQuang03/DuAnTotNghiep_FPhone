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
})(window, jQuery)

/*------------------------------*/
var JChart2 = (function (window, $) {
    var ins = {};

    ins.char1 = function () {
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(function () {
            // AJAX request để lấy dữ liệu từ Action
            $.ajax({
                url: '/ThongKePhone/Chart1',
                type: 'GET',
                success: function (data) {
                    console.log(data);
                    ins.drawCharta(data,'detailContent2','Thống kê phân khúc giá');
                },
                error: function () {
                    console.error('Lỗi tải dữ liệu');
                }
            });

        });
    }
    ins.char2 = function () {
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(function () {
            // AJAX request để lấy dữ liệu từ Action
            $.ajax({
                url: '/ThongKePhone/Chart2',
                type: 'GET',
                success: function (data) {
                    console.log(data);
                    ins.drawCharta(data, 'detailContent3', "Thống kê thời gian mua hàng");
                },
                error: function () {
                    console.error('Lỗi tải dữ liệu');
                }
            });

        });
    }

    ins.drawCharta = function (data,idChart,title) {
        var jsonData = data;

        var tb = new google.visualization.DataTable();
        tb.addColumn("string", "billStatus");
        tb.addColumn("number", "coutBill");

        for (var i = 0; i < jsonData.length; i++) {
            tb.addRow([jsonData[i].billStatus, jsonData[i].coutBill]);
        }

        var chart = new google.visualization.PieChart(document.getElementById(idChart));
        chart.draw(tb,
            {
                title: title,
               /* pieSliceText: 'value'*/
            });
    }
    return ins;
})(window, jQuery)




﻿/* chart.js chart examples */
// chart colors
var colors = ['#007bff', '#28a745', '#333333', '#c3e6cb', '#dc3545', '#6c757d'];
/* bar chart */

$("#selection_month").change(function () {
  
    $.ajax({
        method:'GET',
        url: "https://localhost:44373/api/SellDaillys/getMonthId/1",
        success: function (result) {
            console.log(result);
            var chBar = document.getElementById("chBar");
            if (chBar) {
                new Chart(chBar, {
                    type: 'bar',
                    data: {
                        labels: result.date,
                        datasets: [{
                            data: result.dataQuantiy,
                            backgroundColor: colors[0],
                            label: "Bán ra"
                        }]
                    },
                    options: {
                        legend: {
                            display: true
                        },
                        scales: {
                            xAxes: [{
                                barPercentage: 0.4,
                                categoryPercentage: 0.5
                            }]
                        }
                    }
                });
            }
            /* bar chart */
            var chBar2 = document.getElementById("chBar1");
            if (chBar2) {
                new Chart(chBar2, {
                    type: 'bar',
                    data: {
                        labels: result.date,
                        datasets: [{
                            data: result.dataMoney,
                            backgroundColor: colors[1],
                            label: "Doanh Thu"
                        }]
                    },
                    options: {
                        legend: {
                            display: true
                        },
                        scales: {
                            xAxes: [{
                                barPercentage: 0.4,
                                categoryPercentage: 0.5
                            }]
                        }
                    }
                });
            }
        }
    });
});

$.ajax({
    url: "https://localhost:44373/api/SellDaillys/getMonthId/1", success: function (result) {
        console.log(result);
        var chBar = document.getElementById("chBar");
        if (chBar) {
            new Chart(chBar, {
                type: 'bar',
                data: {
                    labels: result.date,
                    datasets: [{
                        data: result.dataQuantiy,
                        backgroundColor: colors[0],
                        label: "Bán ra"
                    }]
                },
                options: {
                    legend: {
                        display: true
                    },
                    scales: {
                        xAxes: [{
                            barPercentage: 0.4,
                            categoryPercentage: 0.5
                        }]
                    }
                }
            });
        }
        /* bar chart */
        var chBar2 = document.getElementById("chBar1");
        if (chBar2) {
            new Chart(chBar2, {
                type: 'bar',
                data: {
                    labels: result.date,
                    datasets: [{
                        data: result.dataMoney,
                        backgroundColor: colors[1],
                        label: "Doanh Thu"
                    }]
                },
                options: {
                    legend: {
                        display: true
                    },
                    scales: {
                        xAxes: [{
                            barPercentage: 0.4,
                            categoryPercentage: 0.5
                        }]
                    }
                }
            });
        }
    }
});


//-----------------------------------------------------------------------------------------------------

$("#select_year").change(function () {
    var month = $("#select_year").val();
    var current = new Date();
    $.ajax({
        url: "https://localhost:44373/api/SellMonthlys/getYearId/2024" + month, success: function (result) {
            console.log(result);
            var chBar = document.getElementById("chBarmonth");
            if (chBar) {
                new Chart(chBar, {
                    type: 'bar',
                    data: {
                        labels: result.date,
                        datasets: [{
                            data: result.dataQuantiy,
                            backgroundColor: colors[0],
                            label: "Bán ra"
                        }]
                    },
                    options: {
                        legend: {
                            display: true
                        },
                        scales: {
                            xAxes: [{
                                barPercentage: 0.4,
                                categoryPercentage: 0.5
                            }]
                        }
                    }
                });
            }
            /* bar chart */
            var chBar2 = document.getElementById("chBar2month");
            if (chBar2) {
                new Chart(chBar2, {
                    type: 'bar',
                    data: {
                        labels: result.date,
                        datasets: [{
                            data: result.dataMoney,
                            backgroundColor: colors[1],
                            label: "Doanh Thu"
                        }]
                    },
                    options: {
                        legend: {
                            display: true
                        },
                        scales: {
                            xAxes: [{
                                barPercentage: 0.4,
                                categoryPercentage: 0.5
                            }]
                        }
                    }
                });
            }
        }
    });
});

$.ajax({
    url: "https://localhost:44373/api/SellMonthlys/getYearId/2024", success: function (result) {
        console.log(result);
        var chBar = document.getElementById("chBarmonth");
        if (chBar) {
            new Chart(chBar, {
                type: 'bar',
                data: {
                    labels: result.date,
                    datasets: [{
                        data: result.dataQuantiy,
                        backgroundColor: colors[0],
                        label: "Bán ra"
                    }]
                },
                options: {
                    legend: {
                        display: true
                    },
                    scales: {
                        xAxes: [{
                            barPercentage: 0.4,
                            categoryPercentage: 0.5
                        }]
                    }
                }
            });
        }
        /* bar chart */
        var chBar2 = document.getElementById("chBar2month");
        if (chBar2) {
            new Chart(chBar2, {
                type: 'bar',
                data: {
                    labels: result.date,
                    datasets: [{
                        data: result.dataMoney,
                        backgroundColor: colors[1],
                        label: "Doanh Thu"
                    }]
                },
                options: {
                    legend: {
                        display: true
                    },
                    scales: {
                        xAxes: [{
                            barPercentage: 0.4,
                            categoryPercentage: 0.5
                        }]
                    }
                }
            });
        }
    }
});


/* 3 donut charts */
var donutOptions = {
    cutoutPercentage: 85,
    legend: { position: 'bottom', padding: 5, labels: { pointStyle: 'circle', usePointStyle: true } }
};

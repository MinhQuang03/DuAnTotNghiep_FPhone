
/* chart.js chart examples */

// chart colors
var colors = ['#007bff', '#28a745', '#333333', '#c3e6cb', '#dc3545', '#6c757d'];
/* bar chart */
var chBar = document.getElementById("chBar");
if (chBar) {
    new Chart(chBar, {
        type: 'bar',
        data: {
            labels: ["Quang", "Minh", "Vũ"],
            datasets: [{
                data: [600, 3000, 483],
                backgroundColor: colors[1],
                label: "TOP POINT",
                'style': "width:150px;height:150px;margin-left:10px"
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

var colors = ['#007bff', '#28a745', '#333333', '#c3e6cb', '#dc3545', '#6c757d'];
/* bar chart */
var chBar2 = document.getElementById("chBar1");
if (chBar2) {
    new Chart(chBar2, {
        type: 'bar',
        data: {
            labels: ["1/1/2023", "1/2/2023", "1/3/2023", "1/4/2023", "1/5/2023", "1/6/2023"],
            datasets: [{
                data: [600, 645, 483, 503, 689, 692, 634],
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
                    categoryPercentage: 0.5,
                    'style': "width:150px;height:150px;margin-left:10px"
                }]
            }
        }
    });
}

/* 3 donut charts */
var donutOptions = {
    cutoutPercentage: 85,
    legend: { position: 'bottom', padding: 5, labels: { pointStyle: 'circle', usePointStyle: true } }
};

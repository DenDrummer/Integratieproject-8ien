//function areaChart (elementId,result) {

//    Morris.Area({
//        // ID of the element in which to draw the chart.
//        element: elementId,
//        // Chart data records -- each entry in this array corresponds to a point on
//        // the chart.
//        data: result,
//        // The name of the data record attribute that contains x-values.
//        xkey: 'label',
//        ykeys: ['value'],
//        labels: ['aantal tweets Van Verhofstad'],
//        pointSize: 2,
//        hideHover: 'auto',
//        resize: true
//    });
//}

$(document).ready(function () {
    //barChart();
    lineChart();
    areaChart();
    donutChart();

    $(window).resize(function () {
        window.barChart.redraw();
        window.lineChart.redraw();
        window.areaChart.redraw();
        window.donutChart.redraw();
    });
});

function barChart() {
    window.barChart = Morris.Bar({
        element: 'bar-chart',
        data: [
            { y: '2006', a: 100, b: 90 },
            { y: '2007', a: 75, b: 65 },
            { y: '2008', a: 50, b: 40 },
            { y: '2009', a: 75, b: 65 },
            { y: '2010', a: 50, b: 40 },
            { y: '2011', a: 75, b: 65 },
            { y: '2012', a: 100, b: 90 }
        ],
        xkey: 'y',
        ykeys: ['a', 'b'],
        labels: ['Series A', 'Series B'],
        lineColors: ['#1e88e5', '#ff3321'],
        lineWidth: '3px',
        resize: true,
        redraw: true
    });
}

function lineChart() {
    window.lineChart = Morris.Line({
        element: 'line-chart',
        data: [
            { y: '2006', a: 100, b: 90 },
            { y: '2007', a: 75, b: 65 },
            { y: '2008', a: 50, b: 40 },
            { y: '2009', a: 75, b: 65 },
            { y: '2010', a: 50, b: 40 },
            { y: '2011', a: 75, b: 65 },
            { y: '2012', a: 100, b: 90 }
        ],
        xkey: 'y',
        ykeys: ['a', 'b'],
        labels: ['Series A', 'Series B'],
        lineColors: ['#1e88e5', '#ff3321'],
        lineWidth: '3px',
        resize: true,
        redraw: true
    });
}

function areaChart() {
    window.areaChart = Morris.Area({
        element: 'area-chart',
        data: [
            { y: '2006', a: 100, b: 90 },
            { y: '2007', a: 75, b: 65 },
            { y: '2008', a: 50, b: 40 },
            { y: '2009', a: 75, b: 65 },
            { y: '2010', a: 50, b: 40 },
            { y: '2011', a: 75, b: 65 },
            { y: '2012', a: 100, b: 90 }
        ],
        xkey: 'y',
        ykeys: ['a', 'b'],
        labels: ['Series A', 'Series B'],
        lineColors: ['#1e88e5', '#ff3321'],
        lineWidth: '3px',
        resize: true,
        redraw: true
    });
}

function donutChart() {
    window.donutChart = Morris.Donut({
        element: 'donut-chart',
        data: [
            { label: "Download Sales", value: 50 },
            { label: "In-Store Sales", value: 25 },
            { label: "Mail-Order Sales", value: 5 },
            { label: "Uploaded Sales", value: 10 },
            { label: "Video Sales", value: 10 }
        ],
        resize: true,
        redraw: true
    });
}

function pieChart() {
    var paper = Raphael("pie-chart");
    paper.piechart(
        100, // pie center x coordinate
        100, // pie center y coordinate
        90,  // pie radius
        [18.373, 18.686, 2.867, 23.991, 9.592, 0.213], // values
        {
            legend: ["Windows/Windows Live", "Server/Tools", "Online Services", "Business", "Entertainment/Devices", "Unallocated/Other"]
        }
    );
}

function customChart(id,data) {
    Morris.Area({
        // ID of the element in which to draw the chart.
        element: id,
        // Chart data records -- each entry in this array corresponds to a point on
        // the chart.
        data: data,
        // The name of the data record attribute that contains x-values.
        xkey: 'label',
        ykeys: ['value'],
        labels: ['aantal tweets Van Verhofstad'],
        pointSize: 2,
        hideHover: 'auto',
        resize: true
    });
}


//jQuery(function ($) {

//    $('button').on('click', function (e) {
//        $('#line-chart').empty();
//        var type = $(this).data('type')

//            , month = { 18.373, 18.686, 2.867, 23.991, 9.592, 0.213}
    

//            , week = { 5, 4, 5, 2, 1, 7}

//            , day = { 9, 20, 90, 70, 5, 1}

//        , data = {
//        month: month,
//        week: week,
//        day: day
//    }

//    , line = new Morris.Line({
//        element: 'line-chart',
//        resize: true,
//        data: data[type],
//        xkey: 'y',
//        ykeys: ['a', 'b'],
//        labels: ['Price', 'Test'],
//        lineColors: ['#3c8dbc', '#3c8dbc'],
//        hideHover: 'auto'
//    })
//        ;

//    });

//});
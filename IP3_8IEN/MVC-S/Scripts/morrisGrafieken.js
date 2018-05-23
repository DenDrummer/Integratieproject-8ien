$(document).ready(function() {


    function maakDonutAan(result) {

        Morris.Donut({
            // ID of the element in which to draw the chart.
            element: 'chart2',
            // Chart data records -- each entry in this array corresponds to a point on
            // the chart.
            data: result,
            // The name of the data record attribute that contains x-values.

            hideHover: 'auto',
            resize: true
        });
    }

    $.get('@Url.Action("GetData","Home",new {id = 170})',
        function(result) {

            Morris.Line({
                // ID of the element in which to draw the chart.
                element: 'chart1',
                // Chart data records -- each entry in this array corresponds to a point on
                // the chart.
                data: result,
                // The name of the data record attribute that contains x-values.
                xkey: 'label',
                ykeys: ['value'],
                labels: ['aantal tweets van Bart De Wever'],
                pointSize: 2,
                hideHover: 'auto',
                resize: true
            });


        });
    $.get('@Url.Action("GetData","Home",new {id = 231})',
        function(result) {

            window.Morris.Area({
                // ID of the element in which to draw the chart.
                element: 'chart4',
                // Chart data records -- each entry in this array corresponds to a point on
                // the chart.
                data: result,
                // The name of the data record attribute that contains x-values.
                xkey: 'label',
                ykeys: ['value'],
                labels: ['aantal tweets Van Verhofstad'],
                pointSize: 2,
                hideHover: 'auto',
                resize: true
            });


        });

    var bar = {
        element: "chart3",
        data: [
            {
                y: '2006',
                a: 42,
                b: 99
            }, {
                y: '1234',
                a: 75,
                b: 65
            }, {
                y: '2008',
                a: 50,
                b: 40
            }, {
                y: '2009',
                a: 75,
                b: 65
            }, {
                y: '2010',
                a: 50,
                b: 40
            }, {
                y: '2011',
                a: 75,
                b: 65
            }, {
                y: '2012',
                a: 100,
                b: 90
            }
        ],
        xkey: 'y',
        ykeys: ['a', 'b'],
        labels: ['Series A', 'Series B'],
        hideHover: 'auto',
        resize: true
    };
    Morris.Bar(bar);



        $.get('@Url.Action("GetRank","Home",new {aantal = 1})', function (result) {

            Morris.Donut({
                // ID of the element in which to draw the chart.
                element: 'chart2',
                // Chart data records -- each entry in this array corresponds to a point on
                // the chart.
                data: result,
                // The name of the data record attribute that contains x-values.

                hideHover: 'auto',
                resize: true
            });
        });
    console.log("kuro");



    $.get('@Url.Action("GetData","Home",new {id = 2})', function (result) {

        Morris.Line({
            // ID of the element in which to draw the chart.
            element: 'chart1',
            // Chart data records -- each entry in this array corresponds to a point on
            // the chart.
            data: result,
            // The name of the data record attribute that contains x-values.
            xkey: 'label',
            ykeys: ['value'],
            labels: ['aantal tweets van Bart De Wever'],
            pointSize: 2,
            hideHover: 'auto',
            resize: true
        });
    });

    });
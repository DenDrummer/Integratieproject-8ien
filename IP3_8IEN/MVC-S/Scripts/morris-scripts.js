$(document).ready(function () {
    
            $(window).resize(function () {
                    window.custChart.redraw();
                });
    });

    function custChart(dashItemid, elementId) {
        $.ajax({
 url: "/Home/GetJsonFromGraphData",
                data: { 'id': dashItemid },
                type: 'GET',
                success: function (result) {
                        //do the necessary updations
                            Morris.Area({
        // ID of the element in which to draw the chart.
                                    element: elementId,
                                // Chart data records - each entry in this array corresponds to a point on
                                // the chart.
                                data: result,
                                // The name of the data record attribute that contains x-values.
                                xkey: 'label',
                                ykeys: ['value'],
                                labels: ['aantal tweets Van Verhofstad'],
                                pointSize: 2,
                                hideHover: 'auto',
                                resize: true,
                                redraw: true
                            });
            },
        error: function (result) {
            }
});

    }
//function custChart(result) {
    //    //do the necessary updations
    //    Morris.Area({
    //        // ID of the element in which to draw the chart.
    //        element: "chart1",
    //        // Chart data records  each entry in this array corresponds to a point on
    //        // the chart.
    //        data: result,
    //        // The name of the data record attribute that contains xvalues.
    //        xkey: 'label',
    //        ykeys: ['value'],
    //        labels: ['aantal tweets Van Verhofstad'],
    //        pointSize: 2,
    //        hideHover: 'auto',
    //        resize: true,
    //        redraw: true
    //    });
    //}
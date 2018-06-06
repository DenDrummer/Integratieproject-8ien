$(document).ready(function () {
    $(window).resize(function () {
        for (var i = 0; i < morris.length; i++) {
            morris[i].redraw();
        }
    });
});
var morris = [];
var aantal = 0;

    function custLineChart(dashItemid, elementId, titel) {
        $.ajax({
 url: "/Home/GetJsonFromGraphData",
                data: { 'id': dashItemid },
                type: 'GET',
                success: function (result) {
                    morris[aantal++] = Morris.Line({
                                element: elementId,
                                data: result,
                                xkey: 'label',
                                ykeys: ['value'],
                                labels: [titel],
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
function custAreaChart(dashItemid, elementId, titel) {
    $.ajax({
        url: "/Home/GetJsonFromGraphData",
        data: { 'id': dashItemid },
        type: 'GET',
        success: function (result) {
            morris[aantal++] = Morris.Area({
                element: elementId,
                data: result,
                xkey: 'label',
                ykeys: ['value'],
                labels: [titel],
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
function custBarChart(dashItemid, elementId, titel) {
    $.ajax({
        url: "/Home/GetJsonFromGraphData",
        data: { 'id': dashItemid },
        type: 'GET',
        success: function (result) {
            morris[aantal++] = Morris.Bar({
                element: elementId,
                data: result,
                xkey: 'label',
                ykeys: ['value'],
                labels: [titel],
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
function custDonutChart(dashItemid, elementId, titel) {
    $.ajax({
        url: "/Home/GetJsonFromGraphData",
        data: { 'id': dashItemid },
        type: 'GET',
        success: function (result) {
            morris[aantal++] = Morris.Donut({
                element: elementId,
                data: result,
                hideHover: 'auto',
                resize: true,
                redraw: true
            });
        },
        error: function (result) {
        }
    });

}
function custCijfer(dashItemid, elementId, titel) {
    var plaats = elementId - 1;
    var id = "chart" + plaats;
    $.ajax({
        url: "/Home/GetJsonFromGraphData",
        data: { 'id': dashItemid },
        type: 'GET',
        success: function (result) {
            result.forEach(function (data) {
                $(`#${id}`).html(`<h6 style="font-size: 25px"> ${titel} heeft <span style="font-size: 50px; color: #00295C;">${data.value}</span>  tweets</h6>`);
            });
        },
        error: function (result) {
        }
    });

}
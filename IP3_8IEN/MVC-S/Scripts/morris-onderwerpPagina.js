$(document).ready(function () {

    $(window).resize(function () {
        window.morris.redraw();
    });
});

addEventListener("load", init);

var morris;
var persoonId = 170;
var aantaldagen = 10;
var morristype = 1;


function init(event) {
    //document.getElementById("morrisType")
    //    .addEventListener("click", mchart);
    document.getElementById("buttonReload").addEventListener("click", reload)
    document.getElementById("morrisType").addEventListener("click", changeType)
}
function changeType() {
    morristype = $("#aantaldagen").val();
    makeChart;
}

function reload() {
    setAantalDagen($("#aantaldagen").val());
    changeData();
}
function setOnderwerpId(id) {
    persoonId = id;
}
function setAantalDagen(aantal) {
    aantaldagen = aantal;
}
function clearBox(elementID) {
    document.getElementById(elementID).innerHTML = "";
}

//function mChart(persoonId, aantaldagen, elementId, message) {
//    $.ajax({
//        url: "/Home/GetTweets",
//        data: { 'persoonId': persoonId, 'aantaldagen': aantaldagen },
//        type: 'GET',
//        success: function (result) {

//            //do the necessary updations
//            morris = Morris.Area({
//                // ID of the element in which to draw the chart.
//                element: elementId,
//                // Chart data records -- each entry in this array corresponds to a point on
//                // the chart.
//                data: result,
//                // The name of the data record attribute that contains x-values.
//                xkey: 'label',
//                ykeys: ['value'],
//                labels: message,
//                pointSize: 2,
//                hideHover: 'auto',
//                resize: true,
//                redraw: true
//            });
//        },
//        error: function (result) {
//        }
//    });
//}
function makeChart() {
    clearBox("persoon-chart1");
        $.ajax({
            url: "/Home/GetTweets",
            data: { 'persoonId': persoonId, 'aantaldagen': aantaldagen },
            type: 'GET',
            success: function (result) {
                if (morristype === 1) {
                    //do the necessary updations
                    morris = Morris.Area({
                        // ID of the element in which to draw the chart.
                        element: "persoon-chart1",
                        // Chart data records -- each entry in this array corresponds to a point on
                        // the chart.
                        data: result,
                        // The name of the data record attribute that contains x-values.
                        xkey: 'label',
                        ykeys: ['value'],
                        labels: ["bla"],
                        pointSize: 2,
                        hideHover: 'auto',
                        resize: true,
                        redraw: true
                    });
                } else if (morristype === 2) {
                    //do the necessary updations
                    morris = Morris.Line({
                        // ID of the element in which to draw the chart.
                        element: "persoon-chart1",
                        // Chart data records -- each entry in this array corresponds to a point on
                        // the chart.
                        data: result,
                        // The name of the data record attribute that contains x-values.
                        xkey: 'label',
                        ykeys: ['value'],
                        labels: ["bla"],
                        pointSize: 2,
                        hideHover: 'auto',
                        resize: true,
                        redraw: true
                    });
                }
            },
            error: function (result) {
            }
        });
}

//function makeChart2() {
//    morris = Morris.Area({
//        // ID of the element in which to draw the chart.
//        element: "persoon-chart1",
//        // Chart data records -- each entry in this array corresponds to a point on
//        // the chart.
//        data: data,
//        // The name of the data record attribute that contains x-values.
//        xkey: 'label',
//        ykeys: ['value'],
//        labels: ["bla"],
//        pointSize: 2,
//        hideHover: 'auto',
//        resize: true,
//        redraw: true
//    });
//}
//function getData() {
//    $.ajax({
//        url: "/Home/GetTweets",
//        data: { 'persoonId': persoonId, 'aantaldagen': aantaldagen },
//        type: 'GET',
//        success: function (result) {

//            data = result
//        },
//        error: function (result) {
//            alert("getdata failed");
//        }
//    });
//}
function changeData() {
    $.ajax({
        url: "/Home/GetTweets",
        data: { 'persoonId': persoonId, 'aantaldagen': aantaldagen },
        type: 'GET',
        success: function (result) {

            morris.setData(result);
          
        }
    });
}

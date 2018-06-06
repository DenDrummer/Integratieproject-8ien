$(document).ready(function () {

    $(window).resize(function () {
        window.morris.redraw();
        window.morris2.redraw();
    });
});

addEventListener("load", init);

var morris;
var morris2;
var persoonId = 170;
var aantaldagen = 5;
var aantaldagen2 = 5;
var message = "aantal tweets: ";
var message2 = "aantal tweets: ";
var naam = "Bart De Wever";

function init(event) {
    //document.getElementById("morrisType")
    //    .addEventListener("click", mchart);

    document.getElementById("buttonReload2").addEventListener("click", reload2);
    document.getElementById("buttonSave2").addEventListener("click", save2);
    document.getElementById("buttonReload1").addEventListener("click", reload);
    document.getElementById("buttonSave1").addEventListener("click", save);
    document.getElementById("buttonAxe1").addEventListener("click", changeAxes);
}

//function changeType() {
//    morristype = $("#aantaldagen").val();
//    makeChart;
//}


function setMessage(mes) {
    message = mes;
}
function setMessage2(mes) {
    message = mes;
}
function setOnderwerpId(id) {
    persoonId = id;
}
function setAantalDagen(aantal) {
    aantaldagen = aantal;
}
function setAantalDagen2(aantal) {
    aantaldagen2 = aantal;
}

function setNaam(naam) {
    this.naam = naam;
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
    $.ajax({
        url: "/Home/GetTweets",
        data: { 'persoonId': persoonId, 'aantaldagen': aantaldagen },
        type: 'GET',
        success: function (result) {
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
                labels: [message],
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
function makeChart2() {
    $.ajax({
        url: "/Home/GetTweets",
        data: {
            'persoonId': persoonId, 'aantaldagen': aantaldagen2
        },
        type: 'GET',
        success: function (result) {
            var omgekeerd = result.reverse();
            //do the necessary updations
            morris2 = Morris.Bar({
                // ID of the element in which to draw the chart.
                element: "persoon-chart2",
                // Chart data records -- each entry in this array corresponds to a point on
                // the chart.
                data: omgekeerd,
                // The name of the data record attribute that contains x-values.
                xkey: 'label',
                ykeys: ['value'],
                labels: [message2],
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
function changeOptions() {
    morris.options.labels = [message];
    morris.redraw;
}
function changeAxes() {
    var x = morris.options.xkey;
    morris.options.xkey = morris.options.ykeys;
    morris.options.ykeys = x;
    morris.redraw;
    changeData();
}
function save() {
    createChart(naam, 'Line', aantaldagen);
}
function reload() {
    setMessage($("#message1").val());
    setAantalDagen($("#aantaldagen1").val());
    changeOptions();
    changeData();
}

function changeData2() {
    $.ajax({
        url: "/Home/GetTweets",
        data: { 'persoonId': persoonId, 'aantaldagen': aantaldagen2 },
        type: 'GET',
        success: function (result) {

            morris2.setData(result);

        }
    });
}
function changeOptions2() {
    morris2.options.labels = [message2];
    morris2.redraw;
}
function changeAxes2() {
    var x = morris2.options.xkey;
    morris2.options.xkey = morris2.options.ykeys;
    morris2.options.ykeys = x;
    morris2.redraw;
    changeData2();
}
function save2() {
    createChart(naam, 'Bar', aantaldagen2);
}
function reload2() {
    setMessage2($("#message2").val());
    setAantalDagen2($("#aantaldagen2").val());
    changeOptions2();
    changeData2();
}
//$('#convert').click(function () {
//    var svg = $("#myfirstchart").html();
//    canvg(document.getElementById('canvas'), svg.split("<div")[0]);
//    html2canvas($("#canvas"), {
//        onrendered: function (canvas) {
//            var imgData = canvas.toDataURL(
//                'image/png');
//            var doc = new jsPDF('p', 'mm');
//            doc.addImage(imgData, 'PNG', 10, 10);
//            doc.save('sample-file.pdf'); 
//            console.log(imgData);
//            $("#imgBinary").html(imgData);
//        }
//    });
//});

function createChart(politicus, selectedType, dagen) {
    $.ajax({
        url: "/Home/CreateChartAantalTweetsPerDag",
        data: { 'politicus': politicus, 'type': selectedType, 'aantalDagenTerug': dagen },
        type: "POST"
    });
}
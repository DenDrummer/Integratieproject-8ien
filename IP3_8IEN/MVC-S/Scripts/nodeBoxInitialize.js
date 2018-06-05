addEventListener("load", init);

function init(event) {
    $("#submitBtn1").click(loadContent);
}


var persoon = "";


var options = {
    userId: '18ien',
    projectId: 'barometerV1',
    functionId: 'main',
    canvasId: 'canvas-chart0',
    autoplay: true
};

function initializeNodeBox(id) {
    loadCanvas("chart" + id);
    loadExtra("extra" + id);
    //populateSelect();
}

function loadCanvas(id) {
    var canvas = document.createElement('canvas'),
        div = document.getElementById(id);
    canvas.id = "canvas-" + id;
    canvas.width = 500;
    canvas.height = 500;
    div.appendChild(canvas);
}

function loadExtra(id) {
    $(`#${id}`).html('<button type="button" class="btn btn-info btn-lg" data-toggle="modal" data-target="#myModal1">Verander Politieker</button>');
    //$(`#${id}`).html('<select id="persL" style="float:right" class="Input"></select><button style="float:right" type="button" id="button" onclick="loadContent()">load</button>');
    //$('<div>').appendTo(`#${id}`).load('@Html.RenderPartial("Zoeken", "Home");');
}

ndbx.embed(options, function (err, player) {
    if (err) {
        throw new Error(err);
    } else {
        window.player = player;
    }
});

//function loadCanvas2(id) {
//    var div = document.getElementById(id);
//    var html = '<canvas id="' + id + '" width="500" height="1000"></canvas>';

//    div.appendChild(html);
//}



//function addElement(id) {
//    var html = '<input type="range"  name="width' + counter + '" min="40" max="200" value="40" oninput="this.form.widthPlus' + counter + '.value=this.value" />' +
//        '<input type="number" name="widthPlus' + counter + '" min="40" max="200" value="40" oninput="this.form.width' + counter + '.value=this.value" />';
//    $("#id").append(html);

//}



function doeiets() {
    console.log("iets gedaan");
}

    //var options2 = {
    //    userId: 'JordenL',
    //    projectId: 'test',
    //    functionId: 'test3',
    //    canvasId: 'canvas-chart6',
    //    autoplay: true
    //};


    // Initialize the NodeBox player object

    //ndbx.embed(options2, function (err, player) {
    //    if (err) {
    //        throw new Error(err);
    //    } else {
    //        window.player = player;
    //    }
//});
function loadContent() {
    setPersonName($(".automplete-1").val());
    setWoorden($(".automplete-1").val());

    let myModal = $("#myModal1");
    $(".modal-backdrop").remove();
    myModal.hide();
    myModal.modal('toggle');
}
function setPersonName(id) {
    //this.persoon = name;
    window.player.setValue('text7', 'text', id);
    $("#heading0").html("Top frequente woorden van " + id);
}

function setWoorden(id) {
    var aantal = 5;
    var dashItemid = id;
    $.ajax({
        url: "/Home/GetFrequenteWoorden",
        data: { 'id': dashItemid , 'aantal' : aantal},
        type: 'GET',
        success: function (result) {
            var i = 1;
            var totaal = 0;
            result.forEach(function (data) {
                totaal += data.Value;
            });
            result.forEach(function (data) {
                window.player.setValue(`Text_Topic_${i}`, 'text', data.Label);
                i++;
            });
            var j = 1;
            result.forEach(function (data) {
                var perc = data.Value / totaal * 100; 
                window.player.setValue(`number${j}`, 'v', perc);
                j++;
            });
        }
    });
}
function populateSelect() {
    $.ajax({
        url: "/Home/GetAllPersonsList",
        type: 'GET',
        success: function (result) {
            console.log("succes");
            var options = '';
            result.forEach(function (data) {
                options += `<option value=${data.OnderwerpId}>${data.Naam}</option>`;
            });
            $('#pers').html(options);
        }, Error: function (result) {
            Console.log("fail");
        }
    });
}

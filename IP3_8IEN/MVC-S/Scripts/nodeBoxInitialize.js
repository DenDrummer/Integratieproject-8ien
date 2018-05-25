var options = {
    userId: '18ien',
    projectId: 'barometerV1',
    functionId: 'main',
    canvasId: 'canvas-chart0',
    autoplay: true
};

function initializeNodeBox(id) {
    loadCanvas(id);
}

function loadCanvas(id) {
        var canvas = document.createElement('canvas'),
        div = document.getElementById(id);
        canvas.id = "canvas-"+id;
        canvas.width = 500;
        canvas.height = 500;
        div.appendChild(canvas);
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

$(function () {
    
    function loadCanvas(id) {
        var canvas = document.createElement('canvas'),
        div = document.getElementById(id);
        canvas.id = "canvas-"+id;
        canvas.width = 500;
        canvas.height = 500;
        
        
        
        div.appendChild(canvas);
    }

    loadCanvas('chart5');
    loadCanvas('chart6');

    var options = {
        userId: 'JordenL',
        projectId: 'test',
        functionId: 'test3',
        canvasId: 'canvas-chart5',
        autoplay: true
    };
    var options2 = {
        userId: '18ien',
        projectId: 'barometerV1',
        functionId: 'main',
        canvasId: 'canvas-chart6',
        autoplay: true
    };

    // Initialize the NodeBox player object
    ndbx.embed(options, function(err, player) {
        if (err) {
            throw new Error(err);
        } else {
            window.player = player;
        }
    });
    ndbx.embed(options2, function (err, player) {
        if (err) {
            throw new Error(err);
        } else {
            window.player = player;
        }
    });

    function newNumber(number) {
        document.getElementById("chart7").innerHTML = number;
    }

    //Ivo 21/05
    var icon = document.createElement("i");
    icon.className = "glyphicon glyphicon-plus glyphicon-center";
    icon.style = "font-size:100px;color:green"
    icon.setAttribute("data-toggle", "modal");
    icon.setAttribute("data-target", "#myModal");
    $("#chart0").append(icon);


});

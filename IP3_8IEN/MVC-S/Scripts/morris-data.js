//$(function () {

   
//    //Morris.Area({
//    //    element: 'chart1',
//    //    data: [{
//    //        period: '2010 Q1',
//    //        Bartje: 2666,
//    //        Anouk: null,
//    //        Ivo: 2647
//    //    }, {
//    //        period: '2010 Q2',
//    //        Bartje: 2778,
//    //        Anouk: 2294,
//    //        Ivo: 2441
//    //    }, {
//    //        period: '2010 Q3',
//    //        Bartje: 4912,
//    //            Anouk: 1969,
//    //        Ivo: 2501
//    //    }, {
//    //        period: '2010 Q4',
//    //        Bartje: 3767,
//    //        Anouk: 3597,
//    //        Ivo: 5689
//    //    }, {
//    //        period: '2011 Q1',
//    //        Bartje: 6810,
//    //        Anouk: 1914,
//    //        Ivo: 2293
//    //    }, {
//    //        period: '2011 Q2',
//    //        Bartje: 5670,
//    //        Anouk: 4293,
//    //        Ivo: 1881
//    //    }, {
//    //        period: '2011 Q3',
//    //        Bartje: 4820,
//    //        Anouk: 3795,
//    //        Ivo: 1588
//    //    }, {
//    //        period: '2011 Q4',
//    //        Bartje: 15073,
//    //        Anouk: 5967,
//    //        Ivo: 5175
//    //    }, {
//    //        period: '2012 Q1',
//    //        Bartje: 10687,
//    //        Anouk: 4460,
//    //        Ivo: 2028
//    //    }, {
//    //        period: '2012 Q2',
//    //        Bartje: 8432,
//    //        Anouk: 5713,
//    //        Ivo: 1791
//    //    }],
//    //    xkey: 'period',
//    //    ykeys: ['Bartje', 'Anouk', 'Ivo'],
//    //    labels: ['Bartje', 'Anouk', 'Ivaylo'],
//    //    pointSize: 2,
//    //    hideHover: 'auto',
//    //    resize: true
//    //});

//    //Morris.Donut({
//    //    element: 'chart2',
//    //    data: [{
//    //        label: "Download Sales",
//    //        value: 12
//    //    }, {
//    //        label: "In-Store Sales",
//    //        value: 30
//    //    }, {
//    //        label: "Mail-Order Sales",
//    //        value: 20
//    //    }],
//    //    resize: true
//    //});

//    //Morris.Bar({
//    //    element: 'chart3',
//    //    data: [{
//    //        y: '2006',
//    //        a: 100,
//    //        b: 90
//    //    }, {
//    //        y: '2007',
//    //        a: 75,
//    //        b: 65
//    //    }, {
//    //        y: '2008',
//    //        a: 50,
//    //        b: 40
//    //    }, {
//    //        y: '2009',
//    //        a: 75,
//    //        b: 65
//    //    }, {
//    //        y: '2010',
//    //        a: 50,
//    //        b: 40
//    //    }, {
//    //        y: '2011',
//    //        a: 75,
//    //        b: 65
//    //    }, {
//    //        y: '2012',
//    //        a: 100,
//    //        b: 90
//    //    }],
//    //    xkey: 'y',
//    //    ykeys: ['a', 'b'],
//    //    labels: ['Series A', 'Series B'],
//    //    hideHover: 'auto',
//    //    resize: true
//    //});

//    function loadCanvas(id) {
//        var canvas = document.createElement('canvas'),
//        div = document.getElementById(id);
//        canvas.id = "canvas-"+id;
//        canvas.width = 500;
//        canvas.height = 500;

       
//        div.appendChild(canvas);
//    }

//    //function loadCanvas2(id) {
//    //    var div = document.getElementById(id);
//    //    var html = '<canvas id="' + id + '" width="500" height="1000"></canvas>';

//    //    div.appendChild(html);
//    //}

    

//    //function addElement(id) {
//    //    var html = '<input type="range"  name="width' + counter + '" min="40" max="200" value="40" oninput="this.form.widthPlus' + counter + '.value=this.value" />' +
//    //        '<input type="number" name="widthPlus' + counter + '" min="40" max="200" value="40" oninput="this.form.width' + counter + '.value=this.value" />';
//    //    $("#id").append(html);
        
//    //}

//    loadCanvas('chart5');
//    loadCanvas('chart6');

//    function doeiets(e) {
//        console.log("iets gedaan" + e.value);
//    }
    
//    var options = {
//        userId: 'JordenL',
//        projectId: 'test',
//        functionId: 'test3',
//        canvasId: 'canvas-chart5',
//        autoplay: true
//    };
//    var options2 = {
//        userId: '18ien',
//        projectId: 'barometerV1',
//        functionId: 'main',
//        canvasId: 'canvas-chart6',
//        autoplay: true
//    };

//    // Initialize the NodeBox player object
//    ndbx.embed(options, function(err, player) {
//        if (err) {
//            throw new Error(err);
//        } else {
//            window.player = player;
//        }
//    });
//    ndbx.embed(options2, function (err, player) {
//        if (err) {
//            throw new Error(err);
//        } else {
//            window.player = player;
//        }
//    });

//    document.getElementById("button").onclick = loadPerson()

//    function loadPersn() {
//        window.player.setValue('number1', 'v', e.value);
//    }
        

//    function newNumber(number) {
//        document.getElementById("chart7").innerHTML = number;
//    }

//    var icon = document.createElement("i");
//    icon.className = "fa fa-area-chart";
//    icon.style = "font-size:200px;color:green;"
//    $("#chart0").append(icon);
  



//});

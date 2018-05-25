addEventListener("load", init, false);
var chart;
var dagen;
var politicus;

function init() {
    let aanmakenBtn = document.getElementById("aanmakenBtn");
    aanmakenBtn.addEventListener("click", grafiekForm, false);
//    let type = document.getElementById("type");
//    type.addEventListener("click", selectDashItem, false);

    let type = document.getElementById("type");
    type.addEventListener("click", showType, false);

    let aantal = document.getElementById("aantal");
    aantal.addEventListener("click", showAantalPersonen, false);

    let grafiekTypeInfo = document.getElementById("grafiekTypeInfo");
    grafiekTypeInfo.addEventListener("click", showGrafiekTypeInfo, false);

    $(".ui-widget").children().hide();
    $('.save').hide();
    $(".automplete-1").show();


}


function showType() {
    $("#type").change(function () {
        if ($(this).val() === "donut" ||
            $(this).val() === "bar" ||
            $(this).val() === "area" ||
            $(this).val() === "line") {
            $("#grafiekTypeInfo").show();
        } else {
            $("#grafiekTypeInfo").hide();
        }
    });
}

function showGrafiekTypeInfo() {
    $(".grafiekTypeInfo").change(function() {
        if ($(this).val() === "GetTweetsPerDag") {
            $(".personenVergelijken").show();
            showAantalPersonen();
            $(".dagenTerug").show();
            $("#aantal").show();
            $(".aantalPolitici").hide();
            $(".urenTerug").hide();
        } else {
            $("#aantal").hide();
            $(".dagenTerug").hide();
            $(".naamVeld").hide();
            $(".personenVergelijken").hide();
            $(".aantalPolitici").show();
            $(".urenTerug").show();
        }
    });
}

function showAantalPersonen() {
     let aantal = parseInt($("#aantal option:selected").val());
    for (let i = 0; i < 5; i++) {
        if (i < aantal) {
            $(".naamVeld").eq(i).show();
        } else if (i >= aantal) {
            $(".naamVeld").eq(i).hide();
        }
        
    }
}

function grafiekForm() {
    let myModal = $("#myModal");
    $(".modal-backdrop").remove();
    myModal.hide();
    myModal.modal('toggle');
    var selectedType = $("#type option:selected").val();
    console.log(selectedType);
    selectType(selectedType);
}

function selectType(type) {
    switch (type) {
    case "bar":
        maakBarChartAan();
        break;
    case "line":
        maakLineChartAan();
        break;
    case "doughnut":
        maakDoughnutAan();
        break;
    case "area":
        maakAreaAan();
        break;
    }
}

function maakBarChartAan() {

}

function maakDoughnutAan() {
    alert("dougnut");
}

function maakLineChartAan() {
    politicus = $("#automplete-1").val();
    dagen = parseInt($("#aantalDagenTerug option:selected").val());
    $.ajax({
        url: "/Home/CreateLineChart",
        data: { 'politicus': politicus, 'aantalDagenTerug': dagen },
        type: "POST",
        error: function() {
            inloggenMsg();

        }
    });
}

function maakAreaAan() {
    politicus = $(".automplete-1").val();
    dagen = parseInt($("#aantalDagenTerug option:selected").val());
    $.ajax({
        url: "/Home/CreateAreaChart",
        data: { 'politicus': politicus, 'aantalDagenTerug': dagen },
        type: "POST",
        error: function() {
            inloggenMsg();
        }
    });
}

function inloggenMsg() {
    $("#inloggenForm").modal('show');
}
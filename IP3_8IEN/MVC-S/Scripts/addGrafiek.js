addEventListener("load", init, false);

function init() {

//    let type = document.getElementById("type");
//    type.addEventListener("click", selectDashItem, false);

    //let type = document.getElementById("type");
    //type.addEventListener("click", showType, false);

    let aantal = document.getElementById("aantal");
    aantal.addEventListener("click", showHideAantalPersonen, false);

    let grafiekTypeInfo = document.getElementById("grafiekTypeInfo");
    grafiekTypeInfo.addEventListener("click", showGrafiekTypeInfo, false);

    let naamInput = document.getElementsByClassName("automplete-1");
    for (let i = 0; i < naamInput.length; i++) {
        naamInput[i].addEventListener("keyup", showError, false);
    }

    var header = document.getElementById("type");
    var btns = header.getElementsByClassName("typeGrafiek");
    for (var i = 0; i < btns.length; i++) {
        btns[i].addEventListener("click", function() {
            var current = document.getElementsByClassName("active");
            current[0].className = current[0].className.replace(" active", "");
            this.className += " active";
        });
        btns[i].addEventListener("click", showType, false);
    }
}


function showType() {
 
        if ($(".active").val() === "") { 
            $("#grafiekTypeInfo").hide();
            $(".personenVergelijken").hide();
            $(".aantalPolitici").hide();;
            $(".dagenTerug").hide();
            $(".naamVeld").hide();
            $(".aantalPolitici").hide();
            $(".urenTerug").hide();
            $(".error").hide();
            $("#grafiekTypeInfo").val("");

        } else {
            $("#grafiekTypeInfo select").val("");
            $("#grafiekTypeInfo").show();
            showGrafiekTypeInfo();
        }    
  
}

function showGrafiekTypeInfo() {
    //var aantal = $("#aantal").val();
    $(".grafiekTypeInfo").change(function() {
        if ($(this).val() === "GetTweetsPerDag") {
            if ($(".active").val() === "Number") {
                $(".personenVergelijken").hide();
                showHideAantalPersonen(1);
            } else {
                $(".personenVergelijken").show();
                showHideAantalPersonen(0);
            }
            $(".dagenTerug").show();
            $(".aantalPolitici").hide();
            $(".urenTerug").hide();
        } else if ($(this).val() === "GetRanking") {
            $(".dagenTerug").hide();
            $(".naamVeld").hide();
            $(".personenVergelijken").hide();
            $(".aantalPolitici").show();
            $(".urenTerug").show();
        } else {
            $(".dagenTerug").hide();
            $(".naamVeld").hide();
            $(".personenVergelijken").hide();
            $(".urenTerug").hide();
            $(".dagenTerug").hide();
        }
    });
}

function showHideAantalPersonen(aantal = 1) {
    if (aantal !== 1) {
        aantal = parseInt($("#aantal option:selected").val());
    }
   
    for (let i = 0; i < 5; i++) {
        if (i < aantal) {
            $(".naamVeld").eq(i).show();
        } else if (i >= aantal) {
            $(".naamVeld").eq(i).hide();
        }
    }
}


//function grafiekForm() {
//    let myModal = $("#myModal");
//    $(".modal-backdrop").remove();
//    myModal.hide();
//    myModal.modal('toggle');
//    if (ingelogged) {
//        createChartAantalTweetsPerDag();
//    } else {
//        showError();
//    }
//   
//}


function createChartAantalTweetsPerDag2(politici) {
        
        $.ajax({
            url: "/Home/CreateComparisonPersonLine",
            data: { 'pers1': politici[0], 'pers2': politici[1], 'pers3': politici[2], 'pers4': politici[3], 'pers5': politici[4]},
            type: "POST"
        });
}

function createChartAantalTweetsPerDag3(politici,type) {

    $.ajax({
        url: "/Home/CreateComparisonPerson",
        data: { 'pers1': politici[0], 'pers2': politici[1], 'pers3': politici[2], 'pers4': politici[3], 'pers5': politici[4],type },
        type: "POST"
    });
}

function createChartAantalTweetsPerDag(politicus, selectedType, dagen) {
    $.ajax({
        url: "/Home/CreateChartAantalTweetsPerDag",
        data: { 'politicus': politicus, 'type': selectedType, 'aantalDagenTerug': dagen },
        type: "POST"
    });
}


function stuurFormulier() {
    var selectedType = $(".active").val();
    var politicus = $(".automplete-1").eq(0).val();
    var dagen = parseInt($("#aantalDagenTerug option:selected").val());
    console.log(selectedType, politicus, dagen);
    if (politicus === null || politicus === "") {
        $(".error").show();
    } else {
        $(".error").hide();
        createChartAantalTweetsPerDag(politicus,selectedType,dagen);
    }
}
//function stuurFormulier2() {
//    var selectedType = $(".active").val();
//    var politici = document.getElementsByClassName("autoplete-1");
//    console.log(politici);
//    var dagen = parseInt($("#aantalDagenTerug option:selected").val());
//    for (var i = 0; i < politici.length; i++) {
//        if (politici[i] === null || politici[i] === "") {
//            $(".error").eq(i).show();
//        } else {
//            $(".error").eq(i).hide();
//            createChartAantalTweetsPerDag(politici[i], selectedType, dagen);
//        }
//    }
//}

function inloggenMsg() {
    $("#inloggenForm").modal('show');
}


function showError() {
    let politici = $(".automplete-1");
    for (let i = 1; i < politici.length; i++) {
        if (politici.eq(i).val() === null || politici.eq(i).val() === "") {
            $(".error").eq(i).show();
        } else {
            $(".error").eq(i).hide();
        }
    }
}
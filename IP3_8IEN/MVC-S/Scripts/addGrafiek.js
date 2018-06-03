addEventListener("load", init, false);

function init() {
    let aanmakenBtn = document.getElementById("aanmakenBtn");
    aanmakenBtn.addEventListener("click", grafiekForm, false);
//    let type = document.getElementById("type");
//    type.addEventListener("click", selectDashItem, false);

    let type = document.getElementById("type");
    type.addEventListener("click", showType, false);

    let aantal = document.getElementById("aantal");
    aantal.addEventListener("click", showHideAantalPersonen, false);

    let grafiekTypeInfo = document.getElementById("grafiekTypeInfo");
    grafiekTypeInfo.addEventListener("click", showGrafiekTypeInfo, false);

    let naamInput = document.getElementsByClassName("automplete-1");
    for (let i = 0; i < naamInput.length; i++) {
        naamInput[i].addEventListener("keyup", showError, false);
    }

    $(".ui-widget").children().hide();
    $('.save').hide();
    $(".automplete-1").show();

   // addIcon();
}


function showType() {
    $("#type").change(function() {
        if ($(this).val() === "") { 
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
        
    });
}

function showGrafiekTypeInfo() {
    //var aantal = $("#aantal").val();
    $(".grafiekTypeInfo").change(function() {
        if ($(this).val() === "GetTweetsPerDag") {
            if ($("#type").val() === "number") {
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


function grafiekForm() {
    let myModal = $("#myModal");
    $(".modal-backdrop").remove();
    myModal.hide();
    myModal.modal('toggle');
    createChartAantalTweetsPerDag();
}


function createChartAantalTweetsPerDag() {
    var selectedType = $("#type option:selected").val();
    var politicus = $(".automplete-1").val();
    var dagen = parseInt($("#aantalDagenTerug option:selected").val());
    if (politicus === null || politicus === "") {
        $(".error").show();
    } else {
        $(".error").hide();
        $.ajax({
            url: "/Home/CreateChartAantalTweetsPerDag",
            data: { 'politicus': politicus, 'type': selectedType, 'aantalDagenTerug': dagen },
            type: "POST",
            error: function() {
                inloggenMsg();

            }
        });
    }
}

function inloggenMsg() {
    $("#inloggenForm").modal('show');
}


function showError() {
    var politicus = $(".automplete-1").val();
    if (politicus === null || politicus === "") {
        $(".error").show();
        return true;
    } else {
        $(".error").hide();
        return false;
    }

}
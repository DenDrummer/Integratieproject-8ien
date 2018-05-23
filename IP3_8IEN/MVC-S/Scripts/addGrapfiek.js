addEventListener("load", init, false);


function init() {
    var aanmakenBtn = document.getElementById("aanmakenBtn");
    aanmakenBtn.addEventListener("click", grafiekAanmaken, false);

}


function grafiekAanmaken() {
    let myModal = $("#myModal");
    $(".modal-backdrop").remove();
    myModal.hide();
    myModal.modal("toggle");

    $("#sortable").append($("#tmp").html());
    var selectedType = document.grafiekType.type.value;
    console.log(selectedType);
}
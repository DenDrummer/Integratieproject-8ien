function updateKendoZones(dashId, zonesorder) {
 
        $.ajax({
            url: "/Home/SaveTilezonesOrder",
            data: { 'dashId': dashId, 'zonesorder': zonesorder },
            type: 'POST',
            success: function (result) {

                //alert("updated zones");
                Console.log("saved");
            }
        });
} 
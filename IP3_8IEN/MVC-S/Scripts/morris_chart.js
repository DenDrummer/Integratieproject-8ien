function areaChart (elementId,result) {

    Morris.Area({
        // ID of the element in which to draw the chart.
        element: elementId,
        // Chart data records -- each entry in this array corresponds to a point on
        // the chart.
        data: result,
        // The name of the data record attribute that contains x-values.
        xkey: 'label',
        ykeys: ['value'],
        labels: ['aantal tweets Van Verhofstad'],
        pointSize: 2,
        hideHover: 'auto',
        resize: true
    });
}
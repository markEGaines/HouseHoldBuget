$(function () {

    if (!$('#bar-chart').length) { return false; }

    bar();

    $(window).resize(App.debounce(bar, 325));

});

function bar(barOpt) {
    $('#bar-chart').empty();
    $('#barLoader').show();
    $.post("/Home/GetChartData", { barOpt: barOpt }).then(function (response) {

        console.log(response)
        new Morris.Bar({
            element: 'bar-chart',
            data: response.barData,
            xkey: 'label',
            ykeys: ['actual', 'budget'],
            labels: ['Actual', 'Budget'],
            barColors: App.chartColors,
            xLabelMargin: 1
        });

        $('#barLoader').hide();
    })
}
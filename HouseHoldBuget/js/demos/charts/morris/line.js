$(function () {

    if (!$('#line-chart').length) { return false; }

    line();

    $(window).resize(App.debounce(line, 325));

});

function line(lineOpt) {
    $('#line-chart').empty();
    $('#lineLoader').show();
    $.post("/Home/GetLineChartData", { val: lineOpt }).then(function (data) {
        console.log(data)
        Morris.Line({
            element: 'line-chart',
            data: data,
            xkey: 'monthNum',
            ykeys: ['actual', 'budget'],
            labels: ['Actual', 'Budget'],
            xlabels: 'month',
            lineColors: App.chartColors,
            smooth: false,
            parseTime: false
        });
        $('#lineLoader').hide();
    })
}
$(function () {

    if (!$('#donut-chart').length) { return false; }

    donut();

    $(window).resize(App.debounce(donut, 325));

});

function donut(donutOpt) {
    $('#donut-chart').empty();
    $('#donutLoader').show();
    $.post("/Home/GetDonutChartData", {donutOpt: donutOpt}).then(function (response) {
        console.log(response)
        Morris.Donut({
            element: 'donut-chart',
            data: response.donutData,
            colors: App.chartColors1,
            hideHover: true,
            formatter: function (y) { return y }
        });

        $('#donutLoader').hide();
    })    
}

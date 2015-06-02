$(function () {

    if (!$('#donut-chart').length) { return false; }

    donut();

    $(window).resize(App.debounce(donut, 325));

});

function donut() {
    $('#donut-chart').empty();
    $.post("/Home/GetChartData").then(function (response) {
        console.log(response)
        Morris.Donut({
            element: 'donut-chart',
            data: response.donutData,
            colors: App.chartColors1,
            hideHover: true,
            formatter: function (y) { return y }
        });
    })

    
}

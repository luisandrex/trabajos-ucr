/**
 * Funciton that creates the Incidents vs Time Graph
 * Params: result
 * [results] only contains an array the the data to include in the graph
 *
 * NO SENSITIVE DATA SHOULD BE TREATED IN THIS CODE
 */

function CreateIncidentsVsTimeChartComponent(results) {
    am4core.ready(function () {
        var chartData = [];

        for (var i = 0; i < results.length; i += 2) {
            var date = results[i];
            var quantity = results[i + 1];


            chartData.push({
                "date": date,
                "value": quantity
            });
        }

        // Themes begin
        am4core.useTheme(am4themes_frozen);
        am4core.useTheme(am4themes_animated);
        // Themes end

        // Create chart instance
        var chart = am4core.create("IncidentsVsTimeChartComponentJS", am4charts.XYChart);

        //Change language 
        chart.language.locale = am4lang_es_ES;

        // Enable chart cursor
        chart.cursor = new am4charts.XYCursor();
        chart.cursor.lineX.disabled = true;
        chart.cursor.lineY.disabled = true;

        // Enable scrollbar
        chart.scrollbarX = new am4core.Scrollbar();

        // Add data
        chart.data = chartData;

        // Create axes
        var dateAxis = chart.xAxes.push(new am4charts.DateAxis());
        dateAxis.title.text = "Fecha";
        dateAxis.title.fontWeight = "bold";
        dateAxis.renderer.grid.template.location = 0.5;
        dateAxis.dateFormatter.inputDateFormat = "yyyy-MM-dd";
        dateAxis.renderer.minGridDistance = 40;
        dateAxis.tooltipDateFormat = "MMM dd, yyyy";
        dateAxis.dateFormats.setKey("day", "dd");

        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
        //Chart Scale
        valueAxis.min = 0;
        valueAxis.max = 30;


        valueAxis.maxPrecision = 0;
        valueAxis.title.text = "Cantidad de Incidentes";
        valueAxis.title.fontWeight = "bold";


        // Create series
        var series = chart.series.push(new am4charts.LineSeries());
        series.tooltipText = "{date}\n[bold font-size: 17px]# Incidentes: {valueY}[/]";
        series.dataFields.valueY = "value";
        series.dataFields.dateX = "date";
        series.strokeDasharray = 3;
        series.strokeWidth = 2;
        series.strokeDasharray = "3,3";
      


        var bullet = series.bullets.push(new am4charts.CircleBullet());
        bullet.strokeWidth = 2;
        bullet.stroke = am4core.color("#fff");
        bullet.setStateOnChildren = true;
        //bullet.propertyFields.fillOpacity = "opacity";
        //bullet.propertyFields.strokeOpacity = "opacity";

        var hoverState = bullet.states.create("hover");
        hoverState.properties.scale = 1.7;

        function createTrendLine(data) {
            var trend = chart.series.push(new am4charts.LineSeries());
            trend.dataFields.valueY = "value";
            trend.dataFields.dateX = "date";
            trend.strokeWidth = 2
            trend.stroke = trend.fill = am4core.color("#c00");
            trend.data = data;

            var bullet = trend.bullets.push(new am4charts.CircleBullet());
            bullet.tooltipText = "{date}\n[bold font-size: 17px]value: {valueY}[/]";
            bullet.strokeWidth = 2;
            bullet.stroke = am4core.color("#fff")
            bullet.circle.fill = trend.stroke;

            var hoverState = bullet.states.create("hover");
            hoverState.properties.scale = 1.7;

            return trend;
        };

    }); // end am4core.ready()
}
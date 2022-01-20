/**
 * Funciton that creates the Incidents vs Destination Graph
 * Params: result
 * [results] only contains an array the the data to include in the graph
 * 
 * NO SENSITIVE DATA SHOULD BE TREATED IN THIS CODE
 */

function CreateIncidentsVsDestinationLocationComponentJS(results) {
    am4core.ready(function () {


        var chartData = [];

        for (var i = 0; i < results.length; i+=2) {
            var destination = results[i];
            var quantity = results[i+1];


            chartData.push({
                "destination": destination,
                "quantity": quantity
            });
        }


        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        var chart = am4core.create("IncidentsVsDestinationLocationComponentJS", am4charts.XYChart);
        chart.padding(40, 40, 40, 40);

        //Change language 
        chart.language.locale = am4lang_es_ES;

        var categoryAxis = chart.yAxes.push(new am4charts.CategoryAxis());
        categoryAxis.title.text = "Destino";
        categoryAxis.title.fontWeight = "bold";
        categoryAxis.renderer.grid.template.location = 0;
        categoryAxis.dataFields.category = "destination";
        categoryAxis.renderer.minGridDistance = 1;
        categoryAxis.renderer.inversed = true;
        categoryAxis.renderer.grid.template.disabled = true;

        var valueAxis = chart.xAxes.push(new am4charts.ValueAxis());
        //Chart Scale
        valueAxis.min = 0;
        valueAxis.max = 50;

        valueAxis.min = 0;
        valueAxis.title.text = "Cantidad de Incidentes";
        valueAxis.title.fontWeight = "bold";


        var series = chart.series.push(new am4charts.ColumnSeries());
        series.dataFields.categoryY = "destination";
        series.dataFields.valueX = "quantity";
        //series.tooltipText = "{valueX.value}"
        series.columns.template.strokeOpacity = 0;
        series.columns.template.column.cornerRadiusBottomRight = 5;
        series.columns.template.column.cornerRadiusTopRight = 5;
        series.columns.template.tooltipText = "{categoryY}: [bold]{valueX}";


        var labelBullet = series.bullets.push(new am4charts.LabelBullet())
        labelBullet.label.horizontalCenter = "left";
        labelBullet.label.dx = 10;
        labelBullet.label.text = "{values.valueX.workingValue.formatNumber('#')}";
        labelBullet.locationX = 1;

        // as by default columns of the same series are of the same color, we add adapter which takes colors from chart.colors color set
        series.columns.template.adapter.add("fill", function (fill, target) {
            return chart.colors.getIndex(target.dataItem.index);
        });

        categoryAxis.sortBySeries = series;
        chart.data = chartData;


    }); // end am4core.ready()
}
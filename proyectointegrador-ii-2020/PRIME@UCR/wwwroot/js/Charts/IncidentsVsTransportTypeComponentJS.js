/**
 * Funciton that creates the Incidents vs Transport Type Graph
 * Params: result
 * [results] only contains an array the the data to include in the graph
 *
 * NO SENSITIVE DATA SHOULD BE TREATED IN THIS CODE
 */
function CreateIncidentsVsTransportTypeComponent(results) {

    am4core.ready(function () {
        // Themes begin
        am4core.useTheme(am4themes_frozen);
        am4core.useTheme(am4themes_animated);
        // Themes end

        // Create chart instance
        var chart = am4core.create("chartdiv", am4charts.XYChart3D);

        //Change language 
        chart.language.locale = am4lang_es_ES;

        var dataArray = [];

        for (var i = 0; i < results.length; i += 2) {
            dataArray.push({
                "incidentType": results[i],
                "quantity": results[i+1],
            })
        }

        chart.data = dataArray;



        // Create axes
        let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
        categoryAxis.dataFields.category = "incidentType";
        categoryAxis.renderer.labels.template.rotation = 0;
        categoryAxis.renderer.labels.template.hideOversized = false;
        categoryAxis.renderer.minGridDistance = 20;
        categoryAxis.renderer.labels.template.horizontalCenter = "middle";
        categoryAxis.renderer.labels.template.verticalCenter = "middle";
        categoryAxis.tooltip.label.rotation = 270;
        categoryAxis.tooltip.label.horizontalCenter = "right";
        categoryAxis.tooltip.label.verticalCenter = "middle";

        let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
        //Chart Scale
        valueAxis.min = 0;
        valueAxis.max = 30;

        valueAxis.title.text = "Cantidad de Incidentes";
        valueAxis.title.fontWeight = "bold";

        // Create series
        var series = chart.series.push(new am4charts.ColumnSeries3D());
        series.dataFields.valueY = "quantity";
        series.dataFields.categoryX = "incidentType";
        series.name = "Modalidad de transporte";
        series.tooltipText = "{categoryX}: [bold]{valueY}[/]";
        series.columns.template.fillOpacity = .8;
        series.maxPrecision = 0;

        var columnTemplate = series.columns.template;
        columnTemplate.strokeWidth = 2;
        columnTemplate.strokeOpacity = 1;
        columnTemplate.stroke = am4core.color("#FFFFFF");

        columnTemplate.adapter.add("fill", function (fill, target) {
            return chart.colors.getIndex(target.dataItem.index);
        })

        columnTemplate.adapter.add("stroke", function (stroke, target) {
            return chart.colors.getIndex(target.dataItem.index);
        })

        chart.cursor = new am4charts.XYCursor();
        chart.cursor.lineX.strokeOpacity = 0;
        chart.cursor.lineY.strokeOpacity = 0;

    }); // end am4core.ready()
}
/**
 * Funciton that creates the Appointment VS medical center component
 * Params: result
 * [results] only contains an array the the data to include in the graph
 * 
 * NO SENSITIVE DATA SHOULD BE TREATED IN THIS CODE
 */

function CreateAppointmentsVsMedicalRecordsHeightComponentJS(results) {
    am4core.ready(function () {
        
        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        // Create chart instance
        var chart = am4core.create("AppointmentsVsMedicalRecordsHeightComponent", am4charts.XYChart);

        //

        // Increase contrast by taking evey second color
        chart.colors.step = 2;

        // Create axes
        var dateAxis = chart.xAxes.push(new am4charts.DateAxis());
        dateAxis.renderer.minGridDistance = 50;

        var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

        // Create series
        function createAxisAndSeries(field, dateName, name, opposite) {

            var series = chart.series.push(new am4charts.LineSeries());
            series.dataFields.valueY = field;
            series.dataFields.dateX = dateName;
            series.strokeWidth = 2;
            series.yAxis = valueAxis;
            series.name = name;

            series.tooltipText = "{name}: [bold]{valueY}[/]";
            series.tensionX = 0.8;
            series.showOnInit = true;

            var interfaceColors = new am4core.InterfaceColorSet();

            var bullet = series.bullets.push(new am4charts.CircleBullet());
            bullet.circle.stroke = interfaceColors.getFor("background");
            bullet.circle.strokeWidth = 2;
        }

        
        var chartData = [];
        for (var i = 0; i < results.length; i += 2) {
            var dateName = "date" + i.toString();
            var valueName = "value" + i.toString();
            for (var j = 0; j < results[i+1].length; j += 2) {
                var date = results[i+1][j];
                var value = results[i + 1][j + 1];
                var object = {};
                object[dateName] = date;
                object[valueName] = value;
                chartData.push(object);
            }
        }
        chart.data = chartData;

        for (var i = 0; i < results.length; i += 2) {
            var dateName = "date" + i.toString();
            var valueName = "value" + i.toString();
            createAxisAndSeries(valueName, dateName, results[i][0], false);
        }


        // Add legend
        chart.legend = new am4charts.Legend();

        // Add cursor
        chart.cursor = new am4charts.XYCursor();

    }); // end am4core.ready()
}
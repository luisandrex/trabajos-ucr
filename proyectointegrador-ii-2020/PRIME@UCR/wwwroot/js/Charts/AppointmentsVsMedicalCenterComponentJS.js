/**
 * Funciton that creates the Appointment VS medical center component
 * Params: result
 * [results] only contains an array the the data to include in the graph
 * 
 * NO SENSITIVE DATA SHOULD BE TREATED IN THIS CODE
 */

function CreateAppointmentsVsMedicalCenterComponentJS(results) {
    am4core.ready(function () {


        var chartData = [];

        for (var i = 0; i < results.length; i += 2) {
            var destination = results[i];
            var quantity = results[i + 1];


            chartData.push({
                "destination": destination,
                "quantity": quantity
            });
        }


        // Themes begin
        am4core.useTheme(am4themes_animated);
        // Themes end

        var chart = am4core.create("AppointmentsVsMedicalCenterComponent", am4charts.PieChart);
        chart.padding(40, 40, 40, 40);

        //Change language 
        chart.language.locale = am4lang_es_ES;

        chart.data = chartData;

        var pieSeries = chart.series.push(new am4charts.PieSeries());
        pieSeries.dataFields.value = "quantity";
        pieSeries.dataFields.category = "destination";
        pieSeries.slices.template.stroke = am4core.color("#fff");
        pieSeries.slices.template.strokeWidth = 2;
        pieSeries.slices.template.strokeOpacity = 1;

        // This creates initial animation
        pieSeries.hiddenState.properties.opacity = 1;
        pieSeries.hiddenState.properties.endAngle = -90;
        pieSeries.hiddenState.properties.startAngle = -90;


    }); // end am4core.ready()
}
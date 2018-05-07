using System.Web.Optimization;

namespace MVC_S
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/scripts/bootstrap.js",
                      "~/scripts/respond.js",
                      "~/DataTables/datatables.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css",
                      "~/Content/sb-admin-2.css",
                      "~/DataTables/datatables.css",
                      "~/Content/sortable.css",
                      "~/Content/morris.css"));
            
            //Sam 24/4
            //bundles.Add(new StyleBundle("~/content/smartadmin").IncludeDirectory("~/content/css", "*.min.css"));

            bundles.Add(new ScriptBundle("~/scripts/smartadmin").Include(
                //"~/scripts/app.config.js",
                //"~/scripts/plugin/jquery-touch/jquery.ui.touch-punch.min.js",
                //"~/scripts/bootstrap/bootstrap.min.js",
                //"~/scripts/notification/SmartNotification.min.js",
                "~/scripts/smartwidgets/jarvis.widget.min.js"
                //,
                //"~/scripts/plugin/jquery-validate/jquery.validate.min.js",
                //"~/scripts/plugin/masked-input/jquery.maskedinput.min.js",
                //"~/scripts/plugin/select2/select2.min.js",
                //"~/scripts/plugin/bootstrap-slider/bootstrap-slider.min.js",
                //"~/scripts/plugin/bootstrap-progressbar/bootstrap-progressbar.min.js",
                //"~/scripts/plugin/msie-fix/jquery.mb.browser.min.js",
                //"~/scripts/plugin/fastclick/fastclick.min.js",
                //"~/scripts/app.min.js"
                ));

            //bundles.Add(new ScriptBundle("~/scripts/full-calendar").Include(
            //    "~/scripts/plugin/moment/moment.min.js",
            //    "~/scripts/plugin/fullcalendar/jquery.fullcalendar.min.js"
            //    ));

            bundles.Add(new ScriptBundle("~/scripts/charts").Include(
                //    "~/scripts/plugin/easy-pie-chart/jquery.easy-pie-chart.min.js",
                //"~/scripts/plugin/sparkline/jquery.sparkline.min.js"
                //,
                "~/scripts/sparkline/jquery.sparkline.min.js",
                "~/scripts/morris/morris.min.js",
                "~/scripts/morris/raphael.min.js",
                "~/scripts/morris/morris-chart-settings.min.js"
            //    "~/scripts/plugin/flot/jquery.flot.cust.min.js",
            //    "~/scripts/plugin/flot/jquery.flot.resize.min.js",
            //    "~/scripts/plugin/flot/jquery.flot.time.min.js",
            //    "~/scripts/plugin/flot/jquery.flot.fillbetween.min.js",
            //    "~/scripts/plugin/flot/jquery.flot.orderBar.min.js",
            //    "~/scripts/plugin/flot/jquery.flot.pie.min.js",
            //    "~/scripts/plugin/flot/jquery.flot.tooltip.min.js",
            //    "~/scripts/plugin/dygraphs/dygraph-combined.min.js",
            //    "~/scripts/plugin/chartjs/chart.min.js",
            //    "~/scripts/plugin/highChartCore/highcharts-custom.min.js",
            //    "~/scripts/plugin/highchartTable/jquery.highchartTable.min.js"
                ));

            //bundles.Add(new ScriptBundle("~/scripts/datatables").Include(
            //    "~/scripts/plugin/datatables/jquery.dataTables.min.js",
            //    "~/scripts/plugin/datatables/dataTables.colVis.min.js",
            //    "~/scripts/plugin/datatables/dataTables.tableTools.min.js",
            //    "~/scripts/plugin/datatables/dataTables.bootstrap.min.js",
            //    "~/scripts/plugin/datatable-responsive/datatables.responsive.min.js"
            //    ));

            //bundles.Add(new ScriptBundle("~/scripts/jq-grid").Include(
            //    "~/scripts/plugin/jqgrid/jquery.jqGrid.min.js",
            //    "~/scripts/plugin/jqgrid/grid.locale-en.min.js"
            //    ));

            //bundles.Add(new ScriptBundle("~/scripts/forms").Include(
            //    "~/scripts/plugin/jquery-form/jquery-form.min.js"
            //    ));

            //bundles.Add(new ScriptBundle("~/scripts/smart-chat").Include(
            //    "~/scripts/smart-chat-ui/smart.chat.ui.min.js",
            //    "~/scripts/smart-chat-ui/smart.chat.manager.min.js"
            //    ));

            //bundles.Add(new ScriptBundle("~/scripts/vector-map").Include(
            //    "~/scripts/plugin/vectormap/jquery-jvectormap-1.2.2.min.js",
            //    "~/scripts/plugin/vectormap/jquery-jvectormap-world-mill-en.js"
            //    ));

            //BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                      "~/scripts/kendo.all.min.js",
                      "~/scripts/kendo_ui.js"));
           
        }
    }
}

using System.Web;
using System.Web.Optimization;
using Tienda.App_Start;

namespace Tienda
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/Scripts/jquery-*"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Content/Scripts/jquery.validate*",
                        "~/Content/Scripts/error-jquery.js"));
   
            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/Scripts/bootstrap.min.js",
                      "~/Content/Scripts/respond.js"));
                        

            ScriptBundle LteJS = new ScriptBundle("~/bundles/AdminLteJs");
            LteJS.Orderer = new DefinedBundleOrderer();
            LteJS.Include(
                "~/Content/Scripts/AdminLte/raphael.min.js",              
                "~/Content/Scripts/AdminLte/jquery.sparkline.min.js",
                "~/Content/Scripts/AdminLte/jquery-jvectormap-1.2.2.min.js",
                "~/Content/Scripts/AdminLte/jquery.knob.min.js",
                "~/Content/Scripts/AdminLte/moment.min.js",              
                "~/Content/Scripts/AdminLte/bootstrap3-wysihtml5.all.min.js",
                "~/Content/Scripts/AdminLte/jquery.slimscroll.min.js",
                "~/Content/Scripts/AdminLte/fastclick.js",
                "~/Content/Scripts/AdminLte/adminlte.min.js",
                "~/Content/Scripts/AdminLte/demo.js");
            bundles.Add(LteJS);
            
            
        }

    }

   
}


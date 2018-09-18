using System.Web.Optimization;

namespace GigHub
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                        "~/Scripts/app/services/attendanceService.js",
                        "~/Scripts/app/controllers/gigsController.js",
                        "~/scripts/app/app.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/underscore-min.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootbox.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        //"~/Scripts/bootstrap.js", // this packages have been moved to the lib bundle
                        //"~/Scripts/respond.js",   // this packages have been moved to the lib bundle
                        //"~/Scripts/bootbox.min.js"   // this packages have been moved to the lib bundle
                        //          ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/animate.css"));
        }
    }
}

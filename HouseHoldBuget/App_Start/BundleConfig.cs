using System.Web;
using System.Web.Optimization;

namespace HouseHoldBuget
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/js/libs/css/ui-lightness/jquery-ui-1.9.2.custom.css",  // for Canvas theme
                      "~/js/plugins/icheck/skins/minimal/blue.css",             // for Canvas theme
                      "~/js/plugins/select2/select2.css",                       // for Canvas theme
                      "~/js/plugins/fullcalendar/fullcalendar.css",             // for Canvas theme
                      "~/Content/css/App.css",                                          // for Canvas theme
                      "~/Content/css/custom.css",                                       // for Canvas theme
                      "~/Content/site.css"));
        }
    }
}

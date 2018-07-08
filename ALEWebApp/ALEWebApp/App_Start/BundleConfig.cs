using System.Web;
using System.Web.Optimization;

namespace ALEWebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Styles

            // CSS style (bootstrap/style)
            bundles.Add(new StyleBundle("~/bundles/maincss").Include(
                "~/Content/lib/bootstrap/dist/css/bootstrap.min.css",
                "~/Content/lib/bootstrap/dist/css/bootstrap-reboot.min.css",
                "~/Content/lib/bootstrap/dist/css/bootstrap-grid.min.css",
                "~/Content/lib/metisMenu/dist/metisMenu.min.css",
                "~/Content/sass/master.min.css"));

            // Animate css
            bundles.Add(new StyleBundle("~/bundles/animatecss").Include(
                "~/Content/lib/animate.css/animate.min.css"));

            // Font Awesome icons
            bundles.Add(new StyleBundle("~/bundles/font-awesome-css").Include(
                "~/Content/lib/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            // Jquery UI css
            bundles.Add(new StyleBundle("~/bundles/jqueryuicss").Include(
                "~/Content/lib/jquery-ui/themes/smoothness/jquery-ui.min.css"));

            // ** PAGES

            #endregion

            #region Scripts


            // Main website scripts
            bundles.Add(new ScriptBundle("~/bundles/mainjs").Include(
                "~/Content/lib/jQuery/dist/jquery.min.js",
                "~/Content/lib/popper.js/dist/umd/popper.min.js",
                "~/Content/lib/popper.js/dist/umd/popper-utils.min.js",
                "~/Content/lib/bootstrap/dist/js/bootstrap.min.js",
                "~/Content/lib/bootstrap/js/dist/alert.js",
                "~/Content/lib/bootstrap/js/dist/button.js",
                "~/Content/lib/bootstrap/js/dist/carousel.js",
                "~/Content/lib/bootstrap/js/dist/collapse.js",
                "~/Content/lib/bootstrap/js/dist/dropdown.js",
                "~/Content/lib/bootstrap/js/dist/modal.js",
                "~/Content/lib/bootstrap/js/dist/tooltip.js",
                "~/Content/lib/bootstrap/js/dist/popover.js",
                "~/Content/lib/bootstrap/js/dist/scrollspy.js",
                "~/Content/lib/bootstrap/js/dist/tab.js",
                "~/Content/lib/bootstrap/js/dist/util.js",
                "~/Content/lib/PACE/pace.min.js",
                "~/Content/lib/metisMenu/dist/metisMenu.min.js",
                "~/Content/lib/jquery-slimscroll/jquery.slimscroll.min.js",
                "~/Scripts/header.js",
                "~/Scripts/main.js"));



            // Jquery validation
            bundles.Add(new ScriptBundle("~/bundles/jqueryValidation").Include(
                "~/Content/lib/jquery-validation/dist/jquery.validate.min.js",
                "~/Content/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"));

            // Jquery UI JS
            bundles.Add(new ScriptBundle("~/bundles/jqueryuijs").Include(
                "~/Content/lib/jquery-ui/jquery-ui.min.js"
                ));


            

            #endregion
        }
    }
}

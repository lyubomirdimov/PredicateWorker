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
                "~/bower_components/bootstrap/dist/css/bootstrap.min.css",
                "~/bower_components/bootstrap/dist/css/bootstrap-reboot.min.css",
                "~/bower_components/bootstrap/dist/css/bootstrap-grid.min.css",
                "~/bower_components/metisMenu/dist/metisMenu.min.css",
                "~/Content/sass/master.min.css"));

            // Animate css
            bundles.Add(new StyleBundle("~/bundles/animatecss").Include(
                "~/bower_components/animate.css/animate.min.css"));

            // Font Awesome icons
            bundles.Add(new StyleBundle("~/bundles/font-awesome-css").Include(
                "~/bower_components/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            // Jquery UI css
            bundles.Add(new StyleBundle("~/bundles/jqueryuicss").Include(
                "~/bower_components/jquery-ui/themes/smoothness/jquery-ui.min.css"));

            // ** PAGES

            #endregion

            #region Scripts


            // Main website scripts
            bundles.Add(new ScriptBundle("~/bundles/mainjs").Include(
                "~/bower_components/jQuery/dist/jquery.min.js",
                "~/bower_components/popper.js/dist/umd/popper.min.js",
                "~/bower_components/popper.js/dist/umd/popper-utils.min.js",
                "~/bower_components/bootstrap/dist/js/bootstrap.min.js",
                "~/bower_components/bootstrap/js/dist/alert.js",
                "~/bower_components/bootstrap/js/dist/button.js",
                "~/bower_components/bootstrap/js/dist/carousel.js",
                "~/bower_components/bootstrap/js/dist/collapse.js",
                "~/bower_components/bootstrap/js/dist/dropdown.js",
                "~/bower_components/bootstrap/js/dist/modal.js",
                "~/bower_components/bootstrap/js/dist/tooltip.js",
                "~/bower_components/bootstrap/js/dist/popover.js",
                "~/bower_components/bootstrap/js/dist/scrollspy.js",
                "~/bower_components/bootstrap/js/dist/tab.js",
                "~/bower_components/bootstrap/js/dist/util.js",
                "~/bower_components/PACE/pace.min.js",
                "~/bower_components/metisMenu/dist/metisMenu.min.js",
                "~/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                "~/Scripts/header.js",
                "~/Scripts/main.js"));



            // Jquery validation
            bundles.Add(new ScriptBundle("~/bundles/jqueryValidation").Include(
                "~/bower_components/jquery-validation/dist/jquery.validate.min.js",
                "~/bower_components/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"));

            // Jquery UI JS
            bundles.Add(new ScriptBundle("~/bundles/jqueryuijs").Include(
                "~/bower_components/jquery-ui/jquery-ui.min.js"
                ));


            

            #endregion
        }
    }
}

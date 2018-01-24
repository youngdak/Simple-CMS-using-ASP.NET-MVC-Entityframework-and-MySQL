using System.Web.Optimization;

namespace NCCFOhaukwu.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/tinymce/tinymce.min.js",
                "~/Scripts/tinymce.init.js"));

            bundles.Add(new ScriptBundle("~/bundles/tinymce").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/slider").Include(
                "~/Wow_Slider/engine1/wowslider.js",
                "~/Wow_Slider/engine1/script.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryFormValidator").Include(
                "~/Scripts/jquery.form-validator.js",
                "~/Scripts/html5.js",
                "~/Scripts/file.js",
                "~/Scripts/security.js"));

            bundles.Add(new ScriptBundle("~/bundles/chosen").Include(
                      "~/Scripts/chosen.jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                "~/Scripts/custom.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/font-awesome.min.css",
                "~/Content/jquery-ui.min.css",
                "~/Content/chosen.min.css",
                "~/Content/custom.css"));

            //jQuery fancybox plugin css
            bundles.Add(new StyleBundle("~/Content/fancybox").Include(
                "~/Content/jquery.fancybox.css"));

            //jQuery fancybox plugin js
            bundles.Add(new ScriptBundle("~/bundles/fancybox").Include(
                "~/Scripts/jquery.fancybox.pack.js"));

            //jQuery fullcalendar plugin css
            bundles.Add(new StyleBundle("~/Content/fullcalendar").Include(
                "~/Content/fullcalendar.css"));

            //jQuery fullcalendar plugin js
            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include(
                "~/Scripts/fullcalendar.js"));
        }
    }
}
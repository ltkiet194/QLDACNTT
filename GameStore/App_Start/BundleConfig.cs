using System.Web;
using System.Web.Optimization;

namespace GameStore
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Theme/css").Include(
                      "~/Theme/vendor/bootstrap/dist/css/bootstrap.min.css",
                      "~/Theme/vendor/ionicons/css/ionicons.min.css",
                      "~/Theme/vendor/flickity/dist/flickity.min.css",
                      "~/Theme/vendor/photoswipe/dist/photoswipe.css",
                      "~/Theme/vendor/photoswipe/dist/default-skin/default-skin.css",
                      "~/Theme/vendor/bootstrap-slider/dist/css/bootstrap-slider.min.css",
                      "~/Theme/vendor/summernote/dist/summernote-bs4.css",
                      "~/Theme/css/goodgames.css",
                      "~/Theme/css/custom.css"));
            bundles.Add(new ScriptBundle("~/Theme/jquery").Include(
            "~/Theme/vendor/object-fit-images/dist/ofi.min.js",
            "~/Theme/vendor/gsap/src/minified/TweenMax.min.js",
            "~/Theme/vendor/gsap/src/minified/plugins/ScrollToPlugin.min.js",
            "~/Theme/vendor/popper.js/dist/umd/popper.min.js",
            "~/Theme/vendor/bootstrap/dist/js/bootstrap.min.js",
            "~/Theme/vendor/sticky-kit/dist/sticky-kit.min.js",
            "~/Theme/vendor/jarallax/dist/jarallax.min.js",
            "~/Theme/vendor/jarallax/dist/jarallax-video.min.js",
            "~/Theme/vendor/imagesloaded/imagesloaded.pkgd.min.js",
            "~/Theme/vendor/flickity/dist/flickity.pkgd.min.js",
            "~/Theme/vendor/photoswipe/dist/photoswipe.min.js",
            "~/Theme/vendor/photoswipe/dist/photoswipe-ui-default.min.js",
            "~/Theme/vendor/jquery-validation/dist/jquery.validate.min.js",
            "~/Theme/vendor/jquery-countdown/dist/jquery.countdown.min.js",
            "~/Theme/vendor/moment/min/moment.min.js",
            "~/Theme/vendor/moment-timezone/builds/moment-timezone-with-data.min.js",
            "~/Theme/vendor/hammerjs/hammer.min.js",
            "~/Theme/vendor/nanoscroller/bin/javascripts/jquery.nanoscroller.js",
            "~/Theme/vendor/soundmanager2/script/soundmanager2-nodebug-jsmin.js",
            "~/Theme/vendor/bootstrap-slider/dist/bootstrap-slider.min.js",
            "~/Theme/vendor/summernote/dist/summernote-bs4.min.js",
            "~/Theme/plugins/nk-share/nk-share.js",
            "~/Theme/js/goodgames.min.js",
            "~/Theme/js/goodgames-init.js"         
            ));
        }
    }
}

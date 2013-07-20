using System.Web;
using System.Web.Optimization;

namespace CoderBattle
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/shared")
                .Include("~/Scripts/jquery-2.0.2.js")
                .Include("~/Scripts/angular.js")
                .Include("~/Scripts/jquery.signalR-1.1.2.js")
                .Include("~/Scripts/bootstrap.js")
                .Include("~/Scripts/underscore.js")
                .Include("~/Scripts/coderbattle.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/bootstrap-responsive.css")
                .Include("~/Content/font-awesome.css")
                .Include("~/Content/animate.css")
                .Include("~/Content/site.css"));
        }
    }
}
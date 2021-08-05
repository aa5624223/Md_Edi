using System.Web;
using System.Web.Optimization;

namespace Md_Edi
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-confirm.min.js",
                        "~/Scripts/umd/popper.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/Datatable").Include(
                      "~/Scripts/jquery.dataTables.min.js",
                      "~/Scripts/dataTables.bootstrap4.min.js",
                      "~/Scripts/buttons.min.js",
                      "~/Scripts/datatable/jszip.min.js",
                      "~/Scripts/datatable/html5.min.js",
                       "~/Scripts/datatable/buttons.print.min.js"
                      ));


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/jquery-confirm.min.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/Datatable").Include(
                        "~/Content/datatable/dataTables.bs4.css",
                        "~/Content/datatable/dataTables.bs4-custom.css",
                        "~/Content/datatable/buttons.bs.css"
                      ));

        }
    }
}

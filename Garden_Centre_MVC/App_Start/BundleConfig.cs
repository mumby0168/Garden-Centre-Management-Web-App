﻿using System.Web;
using System.Web.Optimization;

namespace Garden_Centre_MVC
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

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/Paging*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/Employees").Include("~/Scripts/Employee/Employee.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js", "~/Scripts/bootbox.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/flatly.css",
                      "~/Content/site.css",
                "~/Content/font-awesome.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/SiteScripts").Include("~/Scripts/Account/Account.js", 
                "~/Scripts/Employee/Employee.js", 
                "~/Scripts/Home/Home.js"));

            bundles.Add(new ScriptBundle("~/bundles/Customers").Include("~/Scripts/Customers/Customers.js"));


        }
    }
}

using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PostcardsManager
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            i18n.UrlLocalizer.IncomingUrlFilters += delegate(Uri url)
            {
                if (url.LocalPath.Contains("/Content/"))
                {
                    return false;
                }
                return true;
            };
            i18n.UrlLocalizer.OutgoingUrlFilters += delegate(string url, Uri currentRequestUrl)
            {
                Uri uri;
                if (Uri.TryCreate(url, UriKind.Absolute, out uri)
                    || Uri.TryCreate(currentRequestUrl, url, out uri))
                {
                    if (uri.LocalPath.Contains("/Content/"))
                    {
                        return false;
                    }
                }
                return true;
            };
        }
    }
}
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Newtonsoft.Json.Serialization;

namespace PostcardsManager
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            i18n.LocalizedApplication.Current.DefaultLanguage = "ru";

            i18n.UrlLocalizer.IncomingUrlFilters += delegate(Uri url)
            {
                if (url.LocalPath.Contains("/Content/") || url.LocalPath.Contains("/api/"))
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
                    if (uri.LocalPath.Contains("/Content/") || uri.LocalPath.Contains("/api/"))
                    {
                        return false;
                    }
                }
                return true;
            };

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();


            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
        }
    }
}
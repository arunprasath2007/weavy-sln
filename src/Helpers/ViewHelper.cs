using Doplace.Constants;
using System.Web.Routing;
using Weavy.Core;
using Weavy.Web;

namespace Wvy.Helpers
{
    public static class ViewHelper
    {
        public static string IsActiveTab(this RouteData routeData, string action, string controller)
        {
            return (routeData.Values["Action"].ToString() == action || string.IsNullOrEmpty(action))
                && routeData.Values["Controller"].ToString() == controller
                ? "active"
                : string.Empty;
        }

        public static string IsActiveHub(this RouteData routeData)
        {
            return WeavyRequestContext.Current != null
                && WeavyRequestContext.Current.Space != null
                && WeavyRequestContext.Current.Space.Id == ConfigurationConstants.DOP_GLOBAL_SPACE_ID
                ? "active"
                : string.Empty;
        }

        public static string IsActiveSpace(this RouteData routeData)
        {
            return (WeavyRequestContext.Current != null
                && WeavyRequestContext.Current.Space != null
                && WeavyRequestContext.Current.Space.Id != ConfigurationConstants.DOP_GLOBAL_SPACE_ID)
                || routeData.Values["Controller"].ToString() == "Space"
                ? "active"
                : string.Empty;
        }
    }
}
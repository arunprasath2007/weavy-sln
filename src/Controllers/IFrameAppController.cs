using System.Web.Mvc;
using Weavy.Core.Models;
using Weavy.Web.Controllers;
using Wvy.Models;

namespace Wvy.Controllers
{
    [RoutePrefix("apps/{id:int}/98871544-5eaf-4d5b-bc03-ee7af41f8106")]
    public class IFrameAppController : AppController<IFrameApp>
    {

        /// <summary>
        /// Display the specified url in an iframe.
        /// </summary>
        /// <param name="app">The app to display.</param>
        /// <param name="query">An object with query parameters for search, paging etc.</param>
        public override ActionResult Get(IFrameApp app, Query query)
        {
            // add you custom logic here...
            return View(app);
        }
    }
}
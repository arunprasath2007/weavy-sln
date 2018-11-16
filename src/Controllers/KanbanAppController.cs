using System.Web.Mvc;
using Weavy.Core.Models;
using Weavy.Web.Controllers;
using Wvy.Models;

namespace Wvy.Controllers
{
    [RoutePrefix("apps/{id:int}/7e5fb812-71c7-4e1e-be0d-567e06f6dae9")]
    public class KanbanAppController : AppController<KanbanApp>
    {
        /// <summary>
        /// Display the Task status page.
        /// </summary>
        /// <param name="app">The app to display.</param>
        /// <param name="query">An object with query parameters for search, paging etc.</param>
        public override ActionResult Get(KanbanApp app, Query query)
        {
            return View(app);
        }
    }
}
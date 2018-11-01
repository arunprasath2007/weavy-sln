using Doplace.Services.Lucene;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using Weavy.Core.Models;
using Weavy.Web.Controllers;
using Wvy.Models;

namespace Wvy.Controllers
{
    [RoutePrefix("apps/{id:int}/df8beb98-7dd5-4c40-93bc-3da22d03d616")]
    public class AdvancedSearchAppController : AppController<AdvancedSearchApp>
    {
        public override ActionResult Get(AdvancedSearchApp app, Query query)
        {
            // add you custom logic here...
            return View(app);
        }

        [HttpGet]
        [Route("search")]
        public ActionResult Search(int id, string q)
        {
            var url = Request.UrlReferrer?.AbsoluteUri ?? "/";
            var searchService = new LuceneSearchService(HostingEnvironment.MapPath("/"));
            var results = searchService.Search(q).ToList();

            return View(new AdvancedSearch
            {
                AppId = id,
                Query = q,
                Referrer = url,
                SearchResults = results
            });
        }
    }
}
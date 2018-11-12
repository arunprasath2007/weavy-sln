using Doplace.Services.Lucene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Weavy.Core;
using Weavy.Core.Models;
using Weavy.Core.Services;

namespace Wvy.Controllers
{
    [RoutePrefix("dop/search")]
    public class SearchController : Controller
    {
        [HttpGet]
        [Route("{tab:vals(everything|spaces|posts|files|comments)?}")]
        public ActionResult Index(string tab = null, Query query = null)
        {
            query.SpaceId = null;
            if (tab == "spaces")
            {
                query.EntityTypes = new EntityType[1]
                {
                EntityType.Space
                };
            }
            else if (tab == "posts")
            {
                query.EntityTypes = new EntityType[1]
                {
                EntityType.Post
                };
            }
            else if (tab == "files")
            {
                query.EntityTypes = new EntityType[1]
                {
                EntityType.Content
                };
            }
            else if (tab == "comments")
            {
                query.EntityTypes = new EntityType[1]
                {
                EntityType.Comment
                };
            }
            else
            {
                query.EntityTypes = new EntityType[4]
                {
                EntityType.Space,
                EntityType.Post,
                EntityType.Content,
                EntityType.Comment
                };
            }
            IndexResult model = IndexService.Search(query);
            //var ls = new LuceneSearchService(WeavyContext.Current.ApplicationDirectory);
            //var rslt = ls.Search(new Doplace.Dto.SearchIndexDto
            //{
            //    SearchQuery = "text"
            //});
            //model.AddRange(rslt);
            if (base.Request.IsAjaxRequest())
            {
                return PartialView("_SearchResult", model);
            }
            return View(model);
        }
    }
}
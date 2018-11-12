﻿using Doplace.Constants;
using Doplace.Repository;
using Doplace.Services.Lucene;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using Weavy.Core;
using Weavy.Core.Models;
using Weavy.Core.Services;
using Weavy.Web;
using Weavy.Web.Controllers;
using Wvy.Models;

namespace Wvy.Controllers
{
    [RoutePrefix("apps/{id:int}/df8beb98-7dd5-4c40-93bc-3da22d03d616")]
    public class AdvancedSearchAppController : AppController<AdvancedSearchApp>
    {
        private readonly SpaceRepository _spaceRepository;

        public AdvancedSearchAppController()
        {
            _spaceRepository = new SpaceRepository();
        }

        public override ActionResult Get(AdvancedSearchApp app, Query query)
        {
            return Search(app.Id, null, query);
        }

        [HttpGet]
        [Route("{tab:vals(posts|files|comments)?}")]
        public ActionResult Search(int id, string tab = null, Query query = null)
        {
            App app = GetApp(id);
            query.SpaceId = app.SpaceId;
            if (tab == "posts")
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
                query.EntityTypes = new EntityType[3]
                {
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
            return View("Get", model);
        }
    }
}
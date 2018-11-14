using Doplace.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Weavy.Core.Models;
using Weavy.Core.Services;
using Weavy.Web.Controllers;
using Weavy.Web.Models;

namespace Wvy.Controllers
{
    [RoutePrefix("doplace/hub")]
    public class HubController : WeavyController
    {
        [Route("")]
        public ActionResult Index(Query query = null)
        {
            var space = GetSpace(ConfigurationConstants.DOP_GLOBAL_SPACE_ID);
            PostsModel postsModel = new PostsModel();
            postsModel.Members = SpaceService.GetMembers(postsModel.Space.Id, new MemberQuery
            {
                Top = new int?(6),
                OrderBy = "Random",
                Count = true
            });
            query = query ?? new Query
            {
                Top = PageSizes[0] / 5
            };
            postsModel.Posts = PostService.GetPosts(postsModel.Space.Id, query);

            if (base.Request.IsAjaxRequest())
            {
                return PartialView("_Posts", postsModel.Posts);
            }

            return View(postsModel);
        }
        
        [Route("files")]
        public ActionResult Files(Query query = null)
        {
            var app = GetApp(ConfigurationConstants.DOP_GLOBAL_FILES_APP_ID);
            FilesModel filesModel = new FilesModel();
            filesModel.Items = ContentService.Search(new ContentQuery(query)
            {
                AppId = new int?(app.Id),
                Depth = new int?(1),
                TransientBy = new int?(base.User.Id),
                Count = true
            });
            filesModel.Layout = Layout.Table;
            if (base.Request.IsAjaxRequest())
            {
                if (filesModel.Layout == Layout.Card)
                {
                    return PartialView("~/Views/Content/_Cards.cshtml", null, filesModel.Items);
                }
                if (filesModel.Layout == Layout.List)
                {
                    return PartialView("~/Views/Content/_ListItems.cshtml", null, filesModel.Items);
                }
                return PartialView("~/Views/Content/_Rows.cshtml", null, filesModel.Items);
            }
            return View(filesModel);
        }
    }
}
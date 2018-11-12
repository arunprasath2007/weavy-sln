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
    [RoutePrefix("doplace/newsfeed")]
    public class NewsFeedController : WeavyController
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
        

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReviewWeb.Models;

namespace ReviewWeb.Controllers
{
    public class HomeController : Controller
    {
        private ReviewWebEntities db = new ReviewWebEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PostDetail(int p)
        {
            Post post = db.Posts.FirstOrDefault(x => x.ID == p);
            if (post == null || p == null)
            {
                return HttpNotFound();
            }
            ViewBag.Comments = db.Comments.Where(x => x.PostID == p).OrderByDescending(x => x.AddDate).ToList();
            return View(post);
        }
        public ActionResult Comment(int PostId, string commentContent)
        {
            ViewBag.Comments = db.Comments.Where(x => x.PostID == PostId).OrderByDescending(x => x.AddDate).ToList();
            if (db.Posts.FirstOrDefault(x => x.ID == PostId) == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }else if (commentContent.Equals("")||commentContent==null)
            {
                return PartialView("CommentForm");
            }
            else
            {
                try
                {
                    Comment comment = new Comment();
                    comment.AddDate = DateTime.Now;
                    comment.UserID = 1;
                    comment.Content = commentContent;
                    comment.PostID = PostId;
                    comment.IsDeleted = false;

                    db.Comments.Add(comment);
                    db.SaveChanges();

                    return PartialView("CommentForm");
                }
                catch
                {
                    return Content("Có cl đòi comment");
                }
            }
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
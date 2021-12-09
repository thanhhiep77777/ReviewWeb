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
            ViewBag.Comments = db.Comments.Where(x => x.PostID == p).OrderByDescending(x => x.AddDate).Take(5).ToList();
            return View(post);
        }
        public JsonResult Comment(int PostId, string commentContent)
        {
            int userID = 1;
            if (db.Posts.FirstOrDefault(x => x.ID == PostId) != null && !(commentContent.Equals("") || commentContent == null))
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
                    return Json(new
                    {
                        Fullname = db.Users.First(x => x.ID == userID).Fullname,
                        AddDate = comment.AddDate.ToString(),
                        Content = comment.Content,

                    }) ;
                }
                catch
                {
                }
            }
            return Json("");
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
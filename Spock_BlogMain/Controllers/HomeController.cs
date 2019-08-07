using Spock_BlogMain.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace Spock_BlogMain.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);
         
            var publishedPosts = db.BlogMains.Where(b => b.Published).OrderByDescending(b => b.Created);
            return View(publishedPosts.ToPagedList(pageNumber, pageSize));
        }

        private ActionResult PagedList(object p)
        {
            throw new NotImplementedException();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();
            return View(model);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)

        {
            if (ModelState.IsValid)
            {
                try
                {
                    //var body = model.Body;
                    var from = $"{model.FromEmail}<{WebConfigurationManager.AppSettings["emailto"]}>";

                   // model.Body = "This is a message from your portfolio site.  The name and the email of the contacting person is above.";

                    var email = new MailMessage(from,
                        WebConfigurationManager.AppSettings["emailto"])

                    {

                        Subject = model.Subject,
                        Body = $"You have an email from{model.FromName}<br/>{model.Body}",
                        //string.Format(body, model.FromName, model.FromEmail, model.Body),
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);
                    return View(new EmailModel());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }

            }

            return View(model);

        }
    }
    }

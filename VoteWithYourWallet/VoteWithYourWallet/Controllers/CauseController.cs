using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VoteWithYourWallet.Database;
using VoteWithYourWallet.Models;

namespace VoteWithYourWallet.Controllers
{
    public class CauseController : Controller
    {
        private readonly AuthDbContext db;
        private readonly UserManager<IdentityUser> userManager;

        public CauseController(AuthDbContext db, UserManager<IdentityUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            List<Cause> causes = db.Causes.ToList();
            foreach (var cause in causes)
            {
                db.Entry(cause).Reference("IdentityUser").Load();
            }
            
            return View(causes);
        }

        public IActionResult Details(int id)
        {
            var cause = db.Causes.First(a => a.Id == (int)id);
            if (cause == null)
            {
                return NotFound();
            }

            db.Entry(cause).Reference("IdentityUser").Load();            

            return View(cause);
        }

        [Authorize(Roles = "Admin, Member")]
        [HttpGet]
        [Route("Cause/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Member")]
        [HttpPost]
        [Route("Cause/Create")]
        public async Task<IActionResult> Create(Cause model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(User.Identity.Name);
                model.IdentityUserId = user.Id;
                db.Causes.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        [Route("Cause/AddSignature")]
        public JsonResult AddSignature(string causeId, string userId, string userName)
        {
            Cause cause = db.Causes.First(a => a.Id == int.Parse(causeId));
            JsonResponse model = null;
            if (cause == null)
            {
                model = new JsonResponse() { status = false, msg = "Failed Due To Cause Does Not Exist" };
                return new JsonResult(model);
            }

            if (db.Signatures.Any(a => a.CauseId == cause.Id && a.IdentityUserId == userId))
            {
                model = new JsonResponse() { status = false, msg = "Signature Already Exists" };
                return new JsonResult(model);
            }

            Signature signature = new Signature() { CauseId = cause.Id, IdentityUserId = userId, Name = userName };
            db.Signatures.Add(signature);
            db.SaveChanges();

            model = new JsonResponse() { status = true, msg = "Successfully Signed Up To Cause" };
            return new JsonResult(model);
        }

        [HttpGet]
        [Route("Cause/GetSignatures")]
        public JsonResult GetSignatures(string causeId)
        {
            JsonResponse model = null;
            List<Signature> allSignatures = db.Signatures.Where(a => a.CauseId == int.Parse(causeId)).ToList();
            if (allSignatures.Count == 0)
            {
                model = new JsonResponse() { status = false, msg = "No Signatures" };
                return new JsonResult(model);
            }
            Cause cause = db.Causes.First(a => a.Id == int.Parse(causeId));
            if (cause != null)
            {
                List<JsonResponse> list = new List<JsonResponse>();
                foreach (var signature in allSignatures)
                {
                    list.Add(new JsonResponse() { msg = userManager.FindByIdAsync(signature.IdentityUserId).Result.UserName });
                }
                model = new JsonResponse() { status = true, msg = JsonConvert.SerializeObject(list) };
                return new JsonResult(model);
            }
            else
            {
                model = new JsonResponse() { status = false, msg = "No Signatures" };
                return new JsonResult(model);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Cause/Delete/{id}")]
        public IActionResult Delete(int? id)
        {
            var cause = db.Causes.First(a => a.Id == id);

            if (cause == null)
            {
                return NotFound();
            }

            db.Causes.Remove(cause);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //[Route("Cause/ShareFacebook/{id")]
        //public async Task<IActionResult> ShareFacebook(int? id)
        //{
        //    //HttpClient client = new HttpClient();

        //    //HttpRequestMessage response = await client.GetAsync("http://www.facebook.com/sharer/sharer.php?s=100&p[url]=http://www.c-sharpcorner.com/conference2014/#Register&p[images]=&p[title] = &p[summary] =");

        //    return View();
        //}

        //[Route("Cause/ShareFacebook/{id")]
        //public IActionResult ShareTwitter(int? id)
        //{
        //    return View();
        //}

        //[Route("Cause/ShareFacebook/{id")]
        //public IActionResult ShareReddit(int? id)
        //{
        //    return View();
        //}
    }
}

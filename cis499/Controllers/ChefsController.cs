using cis499.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace cis499.Controllers
{
    public class ChefsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;

        public ChefsController(ModelContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var categories = _context.Useraccounts.Where(x => x.Roleid == 2).ToList();

            return View(categories);
        }
        public IActionResult ShowRecibeByChef(int id)
        {
            ViewBag.Categories = new SelectList(_context.CategoryRecipes.ToList(), "Id", "Name");
            var chefName = _context.Useraccounts.ToList();
            ViewBag.ChefName = new SelectList(chefName, "Id", "FullName");
            var recipe = _context.Recipes.Where(x => x.UserAccountId == id).ToList();
            return View(recipe);
        }

        public IActionResult CreateRecipe()
        {
            var categories = _context.CategoryRecipes.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult CreateRecipe(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                recipe.State = "Waiting";
                recipe.AddedDate = DateTime.Now;
                recipe.UserAccountId = HttpContext.Session.GetInt32("ChefId");
                _context.Recipes.Add(recipe);
                _context.SaveChanges();
                return RedirectToAction("Index", "Chefs");
            }
            return View(recipe);
        }
        public IActionResult ShowAllRecipe()
        {
            var accepterRecipe = _context.Recipes.Include(x => x.CategoryRecipe)
                .Include(x => x.UserAccount).Where(x => x.State == "Accepted").ToList();
            return View(accepterRecipe);
        }
        [HttpPost]
        public IActionResult ShowAllRecipe(string name)
        {
            var accepterRecipe = _context.Recipes.Include(x => x.CategoryRecipe)
                .Include(x => x.UserAccount).Where(x => x.State == "Accepted").ToList();
            if (name != null)
            {
                var recipeName = accepterRecipe.Where(x => x.Name.ToLower() == name
                || x.Name.ToUpper() == name).ToList();
                return View(recipeName);
            }
            else
            {
                return View(accepterRecipe);

            }

        }

        public IActionResult ShowCategory()
        {
            var category = _context.CategoryRecipes.ToList();
            return View(category);
        }

        public IActionResult ShowRequestFromUser()
        {
            ViewBag.UserRequest = new SelectList(_context.Useraccounts.ToList(), "Id", "FullName");
            int? chefSession = HttpContext.Session.GetInt32("ChefId");
            var requestRecipe = _context.RequestRecipes.Where(x => x.State == "Waiting"
            && x.ChefAccountId == chefSession).ToList();
            return View(requestRecipe);
        }

        public async Task<IActionResult> AcceptRequest(int id)
        {
            var acceptRequest = await _context.RequestRecipes.FindAsync(id);
            var userAccount = await _context.Useraccounts.FindAsync(acceptRequest.UserAccountId);
            var userEmail = userAccount.Email;
            if (acceptRequest != null)
            {
                acceptRequest.State = "Accepted";
            }
            await _context.SaveChangesAsync();

            SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp-mail.outlook.com", 587);
            mail.From = new MailAddress("smtp.service.asp@outlook.com"); // outlook email here
            mail.To.Add(userEmail);
            mail.Subject = "Purchase Invoice";
            mail.Body = "The Recipe that you Request is accepted, Please visit the website to complete";
            smtp.Credentials = new NetworkCredential("smtp.service.asp@outlook.com", "shgh9999"); // authenticated
            smtp.EnableSsl = true;
            smtp.Send(mail);

            return RedirectToAction("ShowRequestFromUser");
        }

        public async Task<IActionResult> RejectRecuest(int id)
        {
            var rejectRequest = await _context.RequestRecipes.FindAsync(id);
            var userAccount = await _context.Useraccounts.FindAsync(rejectRequest.UserAccountId);
            var userEmail = userAccount.Email;
            if (rejectRequest != null)
            {
                rejectRequest.State = "Rejected";
            }
            await _context.SaveChangesAsync();

            SmtpClient smtp = new SmtpClient("smtp-mail.outlook.com", 587);
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp-mail.outlook.com", 587);
            mail.From = new MailAddress("smtp.service.asp@outlook.com"); // outlook email here
            mail.To.Add(userEmail);
            mail.Subject = "Purchase Invoice";
            mail.Body = "The Recipe that you Request is rejected.";
            smtp.Credentials = new NetworkCredential("smtp.service.asp@outlook.com", "shgh9999"); // authenticated
            smtp.EnableSsl = true;
            smtp.Send(mail);
            return RedirectToAction("ShowRequestFromUser");

        }

        public IActionResult ViewProfile()
        {
            var sessionUser = HttpContext.Session.GetInt32("ChefId");

            if (sessionUser != null)
            {
                var existUser = _context.Useraccounts.Where(x => x.Id == sessionUser).FirstOrDefault();
                if (existUser == null)
                {
                    return RedirectToAction("Login", "RegisterLogin");
                }
                return View(existUser);
            }

            //if the session Id is null
            return RedirectToAction("Login", "RegisterLogin");
        }

        public IActionResult EditProfile(decimal? id)
        {
            var Ids = _context.Useraccounts.Where(x => x.Id == id).FirstOrDefault();
            if (Ids == null || id == null)
            {
                return RedirectToAction("Login", "RegisterLogin");
            }
            return View(Ids);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(decimal? id, [Bind("Id, Fname, Lname, Email, Password, ImageFile, Roleid")] Useraccount useraccount)
        {
            if (ModelState.IsValid)
            {
                if (useraccount.ImageFile != null)
                {
                    string w3Root = _environment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + useraccount.ImageFile.FileName;
                    string path = Path.Combine(w3Root, "/images/" + imageName);
                    //using(var fileStream = new FileStream(path, FileMode.Create))
                    //{
                    //    await useraccount.ImageFile.CopyToAsync(fileStream);
                    //}
                    useraccount.Imagepath = imageName;
                }
                _context.Update(useraccount);
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewProfile", "Chefs");
            }
            return View(useraccount);
        }


    }
}

using cis499.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace cis499.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;

        public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _environment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.numberOfUser = _context.Useraccounts.Count();
            ViewBag.numberOfRecipe = _context.Recipes.Count();
            ViewBag.numberOfRequestRecipe = _context.RequestRecipes.Count();
            return View();
        }

        public IActionResult Search()
        {
            var recipeSearch = _context.Recipes.Include(x => x.CategoryRecipe).Include(x => x.UserAccount).ToList();
            return View(recipeSearch);
        }
        [HttpPost]
        public IActionResult Search(DateTime? startDate, DateTime? endDate)
        {
            var recipeSearch = _context.Recipes.Include(x => x.CategoryRecipe)
                .Include(x => x.UserAccount).ToList();
            if (startDate == null && endDate == null)
            {
                return View(recipeSearch);
            }
            else if (startDate != null && endDate == null)
            {
                var recipeSearchStartDate = _context.Recipes.Where(x => x.AddedDate.Value.Date >= startDate)
                    .Include(x => x.CategoryRecipe).Include(x => x.UserAccount).ToList();
                return View(recipeSearchStartDate);
            }
            else if (startDate == null && endDate != null)
            {
                var recipeSearchEndDate = _context.Recipes.Where(x => x.AddedDate.Value.Date <= endDate)
                    .Include(x => x.CategoryRecipe).Include(x => x.UserAccount).ToList();
                return View(recipeSearchEndDate);
            }
            else
            {
                var recipeSearchBothDate = _context.Recipes.Where(x => x.AddedDate.Value.Date >= startDate &&
                x.AddedDate.Value.Date <= endDate).Include(x => x.CategoryRecipe).Include(x => x.UserAccount).ToList();
                return View(recipeSearchBothDate);

            }
        }

        public IActionResult ReviewRecipes()
        {
            var waitingRecipes = _context.Recipes.Where(r => r.State == "Waiting").ToList();
            return View(waitingRecipes);
        }

        public IActionResult AcceptRecipe(decimal id)
        {
            var recipe = _context.Recipes.Find(id);
            if (recipe != null)
            {
                recipe.State = "Accepted";
                _context.SaveChanges();
            }
            return RedirectToAction("ReviewRecipes");
        }

        public IActionResult RejectRecipe(decimal id)
        {
            var recipe = _context.Recipes.Find(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                _context.SaveChanges();
            }
            return RedirectToAction("ReviewRecipes");
        }

        public IActionResult ReviewTestimonial()
        {
            var waitingTestimonials = _context.Testimonials.Where(r => r.State == "waiting").ToList();
            var showName = _context.Useraccounts.ToList();
            ViewBag.UserFname = new SelectList(showName, "Id", "Fname");
            return View(waitingTestimonials);
        }

        public IActionResult AcceptTestimonial(decimal id)
        {
            var testimonial = _context.Testimonials.Find(id);
            if (testimonial != null)
            {
                testimonial.State = "Accepted";
                _context.SaveChanges();
            }
            return RedirectToAction("ReviewTestimonial");
        }

        public IActionResult RejectTestimonial(decimal id)
        {
            var testimonial = _context.Testimonials.Find(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
                _context.SaveChanges();
            }
            return RedirectToAction("ReviewTestimonial");
        }

        public IActionResult IndexHomePage()
        {
            var homePages = _context.HomePages.ToList();
            return View(homePages);
        }

        public IActionResult EditHomePage(decimal id)
        {
            var homePage = _context.HomePages.Find(id);
            if (homePage == null)
            {
                return NotFound();
            }
            return View(homePage);
        }

        [HttpPost]
        public async Task<IActionResult> EditHomePage(HomePage homepage)
        {
            if (ModelState.IsValid)
            {
                _context.HomePages.Update(homepage);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(homepage);
        }

        public IActionResult ViewProfile()
        {
            var sessionUser = HttpContext.Session.GetInt32("AdminId");

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
                return RedirectToAction("ViewProfile", "Admin");
            }
            return View(useraccount);
        }




    }
}

using cis499.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace cis499.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, ModelContext context, IWebHostEnvironment environment)
        {
            _logger = logger;
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var viewHomePage = _context.HomePages.ToList();
            var viewTestimonial = _context.Testimonials.Where(x => x.State == "Accepted").Include(x => x.UserAccount).Include(x => x.UserAccount.Role).ToList();
            var contactForm = _context.ContactUs.ToList();
            var categories = _context.CategoryRecipes.ToList();
            var collectionModel = Tuple.Create<IEnumerable<HomePage>, IEnumerable<Testimonial>,
                IEnumerable<ContactU>, IEnumerable<CategoryRecipe>>(viewHomePage, viewTestimonial, contactForm, categories);
            //Contactu contactUs = new Contactu();
            //HomePageAndContact homePageAndCntact = new HomePageAndContact
            //{
            //    homepage = viewHomePage,
            //    contact = contactUs
            //};
            //var showTestimonial = _context.Testimonials.Where(x=>x.State == "Accepted").Include(x=>x.Useraccount).ToList();
            //var collectionModel = new CollectionHomePage
            //{
            //    homepageView = viewHomePage,
            //    testimonialView = showTestimonial
            //};
            var sessionUser = HttpContext.Session.GetInt32("UserId");
            ViewBag.NameOfUser = HttpContext.Session.GetString("NameOfUser");
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("IsLoggedIn");
            ViewBag.Email = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text2).FirstOrDefault();
            ViewBag.PhoneNumber = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text3).FirstOrDefault();
            ViewBag.Address = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text4).FirstOrDefault();
            return View(collectionModel);
        }

        public IActionResult AddTestimonial()
        {
            ViewBag.Email = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text2).FirstOrDefault();
            ViewBag.PhoneNumber = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text3).FirstOrDefault();
            ViewBag.Address = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text4).FirstOrDefault();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTestimonial(Testimonial testimonial)
        {
            string NameOfUser = HttpContext.Session.GetString("NameOfUser");
            int? Id = HttpContext.Session.GetInt32("UserId");
            testimonial.UserAccountId = Id;
            testimonial.State = "waiting";
            _context.Add(testimonial);
            await _context.SaveChangesAsync();
            return View(testimonial);
        }

        public IActionResult ShowChefs()
        {
            ViewBag.Email = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text2).FirstOrDefault();
            ViewBag.PhoneNumber = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text3).FirstOrDefault();
            ViewBag.Address = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text4).FirstOrDefault();
            var chef = _context.Useraccounts.Where(x => x.Roleid == 2).ToList();
            return View(chef);
        }

        public IActionResult ShowRecipeByChef(decimal id)
        {
            ViewBag.Email = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text2).FirstOrDefault();
            ViewBag.PhoneNumber = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text3).FirstOrDefault();
            ViewBag.Address = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text4).FirstOrDefault();
            ViewBag.Categories = new SelectList(_context.CategoryRecipes.ToList(), "Id", "Name");
            ViewBag.ChefName = new SelectList(_context.Useraccounts.ToList(), "Id", "FullName");
            var recipe = _context.Recipes.Where(x => x.UserAccountId == id && x.State == "Accepted").ToList();
            return View(recipe);
        }

        public IActionResult ShowAllRecipe()
        {
            ViewBag.Email = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text2).FirstOrDefault();
            ViewBag.PhoneNumber = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text3).FirstOrDefault();
            ViewBag.Address = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text4).FirstOrDefault();
            ViewBag.Categories = new SelectList(_context.CategoryRecipes.ToList(), "Id", "Name");
            ViewBag.ChefName = new SelectList(_context.Useraccounts.ToList(), "Id", "FullName");
            var recipe = _context.Recipes.Where(x => x.State == "Accepted").ToList();
            return View(recipe);
        }

        [HttpPost]
        public IActionResult ShowAllRecipe(string name)
        {
            //var recipeNames = from m in _context.Recipes.Where(x => x.State == "Accepted")
            //                 select m;
            //if (!string.IsNullOrEmpty(name))
            //{
            //    recipeNames = recipeNames.Where(n => n.Name!.Contains(name));
            //}
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

        public IActionResult ViewProfile()
        {
            ViewBag.Email = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text2).FirstOrDefault();
            ViewBag.PhoneNumber = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text3).FirstOrDefault();
            ViewBag.Address = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text4).FirstOrDefault();
            var sessionUser = HttpContext.Session.GetInt32("UserId");

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
            ViewBag.Email = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text2).FirstOrDefault();
            ViewBag.PhoneNumber = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text3).FirstOrDefault();
            ViewBag.Address = _context.HomePages.Where(x => x.Id == 2).Select(x => x.Text4).FirstOrDefault();
            var Ids = _context.Useraccounts.Where(x => x.Id == id).FirstOrDefault();
            if (Ids == null || id == null)
            {
                return RedirectToAction("Login", "RegisterLogin");
            }
            return View(Ids);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(decimal? id, [Bind("Id, Fname, Lname, Email, Password, ImageFile, Roleid")] Useraccount useraccount, string currentPassword, string newPassword)
        {
            var sessionUser = HttpContext.Session.GetInt32("UserId");
            var currentUser = await _context.Useraccounts.FindAsync(id);

            if (ModelState.IsValid)
            {
                if (currentUser != null)
                {
                    if (useraccount.ImageFile != null)
                    {
                        string w3Root = _environment.WebRootPath;
                        string imageName = Guid.NewGuid().ToString() + "_" + useraccount.ImageFile.FileName;
                        string path = Path.Combine(w3Root, "/images/" + imageName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await useraccount.ImageFile.CopyToAsync(fileStream);
                        }
                        useraccount.Imagepath = imageName;
                    }
                    else
                    {
                        useraccount.Imagepath = currentUser.Imagepath;
                    }

                    if (currentUser.Password == currentPassword)
                    {
                        currentUser.Password = newPassword;
                    }
                    else
                    {
                        ViewData["PasswordError"] = "Current Password Not Matched.";
                    }
                }
                currentUser.Fname = useraccount.Fname;
                currentUser.Lname = useraccount.Lname;
                currentUser.Email = useraccount.Email;
                currentUser.Roleid = useraccount.Roleid;
                currentUser.Imagepath = useraccount.Imagepath;
                _context.Update(currentUser);
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewProfile", "Home");
            }
            return View(useraccount);
        }

        public IActionResult ShowRecipeRequested()
        {
            var userSession = HttpContext.Session.GetInt32("UserId");
            var request = _context.RequestRecipes.Where(x => x.UserAccountId == userSession);
            return View(request);

        }
               
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
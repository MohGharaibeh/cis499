using cis499.Models;
using Microsoft.AspNetCore.Mvc;

namespace cis499.Controllers
{
	public class RegisterLoginController : Controller
	{
		private readonly ModelContext _context;
		private readonly IWebHostEnvironment _hostEnvironment;
		public RegisterLoginController(ModelContext context, IWebHostEnvironment _hostEnvironment)
		{
			this._hostEnvironment = _hostEnvironment;
			_context = context;
		}
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register([Bind("Id,Fname,Lname, ImageFile, Email, Password")] Useraccount useraccount)
		{
			if (ModelState.IsValid)
			{
				string w3rootPath = _hostEnvironment.WebRootPath;

				string imageName = Guid.NewGuid().ToString() + "_" + useraccount.ImageFile.FileName;
				// dvfhfhdvfsvhfsvjfhh_raghad.png

				string path = Path.Combine(w3rootPath + "/images/", imageName);

				using (var fileStream = new FileStream(path, FileMode.Create))
				{
					await useraccount.ImageFile.CopyToAsync(fileStream);
				}
				useraccount.Imagepath = imageName;
				bool existEmail = _context.Useraccounts.Any(u => u.Email == useraccount.Email);

				if (existEmail)
				{
					ViewData["RegisterError"] = "This email is already in use.";
					return View(useraccount);
				}

				useraccount.Roleid = 3;
				// Save record/row to the DB (rcustomer table)
				_context.Add(useraccount); // Add the customer object to the DB
				await _context.SaveChangesAsync(); // Add the customer object to the DB

				return RedirectToAction("Login");
			}
			return View(useraccount);
		}

		public IActionResult Login()
		{

			return View();

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Login([Bind("Email,Password")] Useraccount useraccount)
		{

			var verifyEmailAndPassword = _context.Useraccounts.Where(u => u.Email == useraccount.Email
			&& u.Password == useraccount.Password).SingleOrDefault();
			if (verifyEmailAndPassword != null)
			{

				switch (verifyEmailAndPassword.Roleid)
				{

					// Admin
					case 1:
						HttpContext.Session.SetInt32("AdminId", (int)verifyEmailAndPassword.Id);
						return RedirectToAction("Index", "Admin");
					// Chef
					case 2:
						HttpContext.Session.SetInt32("ChefId", (int)verifyEmailAndPassword.Id);
						return RedirectToAction("Index", "Chefs");
					// user
					case 3:
						HttpContext.Session.SetInt32("UserId", (int)verifyEmailAndPassword.Id);
						HttpContext.Session.SetString("NameOfUser", verifyEmailAndPassword.Fname);
						HttpContext.Session.SetString("IsLoggedIn", "true");
						return RedirectToAction("Index", "Home");
				}
			}
			else
			{
				ViewData["LoginError"] = "Email or Password is Wrong!";
				return View(useraccount);
			}
			return View();


		}
		public IActionResult RegisterChef()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> RegisterChef([Bind("Id,Fname,Lname, ImageFile, Email, Password, CvChef")] Useraccount useraccount)
		{
			if (ModelState.IsValid)
			{
				string w3rootPath = _hostEnvironment.WebRootPath;

				string imageName = Guid.NewGuid().ToString() + "_" + useraccount.ImageFile.FileName;

				string path = Path.Combine(w3rootPath + "/images/", imageName);

				using (var fileStream = new FileStream(path, FileMode.Create))
				{
					await useraccount.ImageFile.CopyToAsync(fileStream);
				}
				useraccount.Imagepath = imageName;
				string wwwrootPath = _hostEnvironment.WebRootPath;

				string cveName = Guid.NewGuid().ToString() + "_" + useraccount.CvChef.FileName;

				string paths = Path.Combine(wwwrootPath + "/images/", cveName);

				using (var fileStream = new FileStream(paths, FileMode.Create))
				{
					await useraccount.CvChef.CopyToAsync(fileStream);
				}
				bool existEmail = _context.Useraccounts.Any(u => u.Email == useraccount.Email);

				if (existEmail)
				{
					ViewData["RegisterError"] = "This email is already in use.";
					return View(useraccount);
				}

				useraccount.Roleid = 2;
				// Save record/row to the DB (rcustomer table)
				_context.Add(useraccount); // Add the customer object to the DB
				await _context.SaveChangesAsync(); // Add the customer object to the DB

				return RedirectToAction("Login");
			}
			return View(useraccount);
		}
		public IActionResult Logout()
		{

			HttpContext.Session.Clear();
			return View("Login");
		}
	}
}

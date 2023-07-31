using cis499.Models;
using Microsoft.AspNetCore.Mvc;

namespace cis499.Controllers
{
    public class SearchController : Controller
    {
        private readonly ModelContext _context;

        public SearchController(ModelContext context)
        {
            _context = context;
        }

        public IActionResult Search(string searchField)
        {
            searchField = searchField.Trim();
            var userAccount = _context.Useraccounts.Where(u=>u.Fname.Contains(searchField) 
            || u.Lname.Contains(searchField)).ToList();

            var recipe = _context.Recipes.Where(r => r.Name.Contains(searchField)).ToList();

            var categoryRecipe = _context.CategoryRecipes.Where(c => c.Name.Contains(searchField)).ToList();

            var requestRecipe = _context.RequestRecipes.Where(r => r.RecipeName.Contains(searchField)).ToList();

            var searchAll = new SearchAll()
            {
                Useraccounts = userAccount,
                Recipes = recipe,
                CategoryRecipes = categoryRecipe,
                RequestRecipes = requestRecipe
            };
            return View(searchAll);
        }
    }
}

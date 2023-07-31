using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cis499.Models;

public partial class CategoryRecipe
{
    public int Id { get; set; }

    public string? ImagePath { get; set; }

    public string? Name { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}

using System;
using System.Collections.Generic;

namespace cis499.Models;

public partial class Recipe
{
    public int Id { get; set; }

    public string? State { get; set; }

    public DateTime? AddedDate { get; set; }

    public string? Name { get; set; }

    public string? Ingredients { get; set; }

    public string? Steps { get; set; }

    public int? CategoryRecipeId { get; set; }

    public int? UserAccountId { get; set; }

    public virtual CategoryRecipe? CategoryRecipe { get; set; }

    public virtual Useraccount? UserAccount { get; set; }
}

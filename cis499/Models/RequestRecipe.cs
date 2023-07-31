using System;
using System.Collections.Generic;

namespace cis499.Models;

public partial class RequestRecipe
{
    public int Id { get; set; }

    public string? State { get; set; }

    public string? RecipeName { get; set; }

    public string? Ingredient { get; set; }

    public string? PreparationTime { get; set; }

    public string? Price { get; set; }

    public string? Steps { get; set; }

    public DateTime? RequestDate { get; set; }

    public int? UserAccountId { get; set; }

    public int? ChefAccountId { get; set; }

    public virtual Useraccount? ChefAccount { get; set; }

    public virtual ICollection<Invoive> Invoives { get; set; } = new List<Invoive>();

    public virtual Useraccount? UserAccount { get; set; }
}

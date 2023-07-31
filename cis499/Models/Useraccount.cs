using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cis499.Models;

public partial class Useraccount
{
    public int Id { get; set; }

    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }
    [NotMapped]
    public string FullName => $"{Fname} {Lname}";

    public string? Imagepath { get; set; }
	[NotMapped]
	public IFormFile? ImageFile { get; set; }
	[NotMapped]
	public IFormFile? CvChef { get; set; }

	public int? Roleid { get; set; }

    public virtual ICollection<Invoive> Invoives { get; set; } = new List<Invoive>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual ICollection<RequestRecipe> RequestRecipeChefAccounts { get; set; } = new List<RequestRecipe>();

    public virtual ICollection<RequestRecipe> RequestRecipeUserAccounts { get; set; } = new List<RequestRecipe>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}

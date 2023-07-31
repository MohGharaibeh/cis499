using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace cis499.Models;

public partial class HomePage
{
    public int Id { get; set; }

    public string? ImagePath { get; set; }
    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    public string? Description { get; set; }

    public string? Text1 { get; set; }

    public string? Text2 { get; set; }

    public string? Text3 { get; set; }

    public string? Text4 { get; set; }

    public string? Text5 { get; set; }
}
